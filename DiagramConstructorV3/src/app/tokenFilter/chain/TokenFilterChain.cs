using System.Collections.Generic;
using System.Linq;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern;
using DiagramConstructorV3.app.tokenPattern.boundaryPatterns;
using DiagramConstructorV3.app.tokenPattern.builders;
using DiagramConstructorV3.app.tokenPattern.commonPatterns;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.tokenFilter.chain
{
    public class TokenFilterChain
    {
        protected List<TokenFilter> Filters { get; } = new List<TokenFilter>();

        protected readonly TokenSequencePatternBuilder TokenSequencePatternBuilder = new TokenSequencePatternBuilder();
        protected readonly StrictComboPatternBuilder StrictPatternBuilder = new StrictComboPatternBuilder();
        protected readonly AnyOfTokensPatternBuilder AnyOfTokensPatternBuilder = new AnyOfTokensPatternBuilder();

        protected readonly ComboPatternByFirstMatchBuilder ComboPatternByFirstMatchBuilder =
            new ComboPatternByFirstMatchBuilder();

        public TokenFilterChain()
        {
            var langSpecificPattern = new SingleTokenPattern(TokenType.LANG_SPECIFIC);
            var strDoubleQuotePattern = new StartEndTokensPattern(TokenType.DOUBLE_QUOTE, TokenType.DOUBLE_QUOTE);
            var strSingleQuotePattern = new StartEndTokensPattern(TokenType.SINGLE_QUOTE, TokenType.SINGLE_QUOTE);

            var elseIfPattern = TokenSequencePatternBuilder
                .Reset()
                .Then(TokenType.ELSE_OPERATOR)
                .Then(TokenType.IF_OPERATOR)
                .Build();
            
            var inputPattern = new StartEndTokensPattern(
                TokenType.INPUT_OPERATOR,
                TokenType.LINE_END);
            
            var outputPattern = new StartEndTokensPattern(
                TokenType.OUTPUT_OPERATOR,
                TokenType.LINE_END);
            
            var twoTextsPattern = TokenSequencePatternBuilder
                .Reset()
                .Then(TokenType.TEXT)
                .Then(TokenType.TEXT)
                .Build();
            Filters.AddRange(new List<TokenFilter>()
            {
                new RemoveTokensFilter(langSpecificPattern, TokenFilter.HighestPriority),

                new EditTokensFilter(TokensToTextTokenFilter, strDoubleQuotePattern, TokenFilter.PreHighestPriority),
                new EditTokensFilter(TokensToTextTokenFilter, strSingleQuotePattern, TokenFilter.PreHighestPriority),

                new EditTokensFilter(ElseIfMergeFilter, elseIfPattern, TokenFilter.PreHighestPriority),
                new EditTokensFilter(InOutPutTokenFilter, inputPattern, TokenFilter.PreHighestPriority),
                new EditTokensFilter(InOutPutTokenFilter, outputPattern, TokenFilter.PreHighestPriority),

                new EditTokensFilter(TokensToTextTokenFilter, twoTextsPattern, TokenFilter.LowestPriority)
            });
        }

        public List<Token> DoFilters(List<Token> tokens)
        {
            var orderedFilters = Filters
                .OrderByDescending(f => f.Priority)
                .ThenByDescending(f => f.FilterConditionsCount)
                .ToList();
            foreach (var filterBefore in orderedFilters)
            {
                tokens = filterBefore.ApplyFilter(tokens);
            }

            return tokens;
        }

        protected List<Token> ElseIfMergeFilter(List<Token> tokens)
        {
            var tokensStartLineNum = tokens[0].LineNumber;
            var newToken = new Token(TokenType.ELSE_IF_OPERATOR, "", tokensStartLineNum);
            return new List<Token>() { newToken };
        }
        protected List<Token> InOutPutTokenFilter(List<Token> tokens)
        {
            var inOutPutToken = tokens[0];
            var lineEndToken = tokens[tokens.Count - 1];
            var tokensStartLineNum = inOutPutToken.LineNumber;
            var text = TokenUtils.TokensToString(tokens.GetRange(1, tokens.Count - 2));
            var newTextToken = new Token(TokenType.TEXT, text, tokensStartLineNum);
            return new List<Token>() { inOutPutToken, newTextToken, lineEndToken};
        }
        protected List<Token> TokensToTextTokenFilter(List<Token> tokens)
        {
            var tokensStartLineNum = tokens[0].LineNumber;
            var text = TokenUtils.TokensToString(tokens);
            var newTextToken = new Token(TokenType.TEXT, text, tokensStartLineNum);
            return new List<Token>() { newTextToken };
        }
    }
}