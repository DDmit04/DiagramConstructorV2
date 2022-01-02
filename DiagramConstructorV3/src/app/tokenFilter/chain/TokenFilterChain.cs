using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.boundaryPatterns;
using DiagramConstructorV3.app.tokenPattern.builders;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.tokenFilter.chain
{
    public class TokenFilterChain
    {
        protected List<TokenFilter> Filters { get; } = new List<TokenFilter>();
        
        protected readonly TokenSequencePatternBuilder TokenSequencePatternBuilder = new TokenSequencePatternBuilder();
        protected readonly StrictComboPatternBuilder StrictPatternBuilder = new StrictComboPatternBuilder();
        protected readonly AnyOfTokensPatternBuilder AnyOfTokensPatternBuilder = new AnyOfTokensPatternBuilder();
        protected readonly ComboPatternByFirstMatchBuilder ComboPatternByFirstMatchBuilder = new ComboPatternByFirstMatchBuilder();

        public TokenFilterChain()
        {
            Filters.AddRange(new List<TokenFilter>()
            {
                new EditTokensFilter(StringUniteFilter, TokenFilter.HighestPriority),

                new EditTokensFilter(ElseIfStatementsFilter, TokenFilter.HighestPriority),
                new EditTokensFilter(InputFilter, TokenFilter.HighestPriority),
                new EditTokensFilter(OutputFilter, TokenFilter.HighestPriority),
                
                new EditTokensFilter(TextUniteFilter, TokenFilter.LowestPriority)
            });
        }

        protected List<Token> ElseIfStatementsFilter(List<Token> tokens)
        {
            var elseIfPattern = TokenSequencePatternBuilder
                .Reset()
                .Then(TokenType.ELSE_OPERATOR)
                .Then(TokenType.IF_OPERATOR)
                .Build();
            var elseIfPatternResult = elseIfPattern.GetMatch(tokens);
            while (elseIfPatternResult.IsFullMatch)
            {
                tokens.RemoveRange(elseIfPatternResult.Start, elseIfPatternResult.Length);
                var newTokenText = new Token(TokenType.ELSE_IF_OPERATOR, "else if");
                tokens.Insert(elseIfPatternResult.Start, newTokenText);
                elseIfPatternResult = elseIfPattern.GetMatch(tokens, elseIfPatternResult.Start);
            }

            return tokens;
        }
        
        protected List<Token> StringUniteFilter(List<Token> lexes)
        {
            var strStartPattern = AnyOfTokensPatternBuilder
                .Reset()
                .Or(TokenType.DOUBLE_QUOTE)
                .Or(TokenType.SINGLE_QUOTE)
                .Build();
            var strStartPatternResult = strStartPattern.GetMatch(lexes);
            while (strStartPatternResult.IsFullMatch)
            {
                var quoteType = lexes[strStartPatternResult.Start].TokenType;
                var quotePattern = new StartEndTokensPattern(quoteType, quoteType);
                var quotePatternRes = quotePattern.GetMatch(lexes, strStartPatternResult.Start);
                var textLexes = lexes.GetRange(strStartPatternResult.Start + 1, quotePatternRes.Length - 2);
                lexes.RemoveRange(strStartPatternResult.Start, quotePatternRes.Length);
                var text = TokenUtils.TokensToString(textLexes);
                var newTextLex = new Token(TokenType.TEXT, text);
                lexes.Insert(strStartPatternResult.Start, newTextLex);
                strStartPatternResult = strStartPattern.GetMatch(lexes, strStartPatternResult.Start);
            }

            return lexes;
        }

        protected List<Token> TextUniteFilter(List<Token> lexes)
        {
            var twoTextsPattern = TokenSequencePatternBuilder
                .Reset()
                .Then(TokenType.TEXT)
                .Then(TokenType.TEXT)
                .Build();
            var textsPatternResult = twoTextsPattern.GetMatch(lexes);
            while (textsPatternResult.IsFullMatch)
            {
                var textLexes = lexes.GetRange(textsPatternResult.Start, textsPatternResult.Length);
                lexes.RemoveRange(textsPatternResult.Start, textsPatternResult.Length);
                var text = TokenUtils.TokensToString(textLexes);
                var newTextLex = new Token(TokenType.TEXT, text);
                lexes.Insert(textsPatternResult.Start, newTextLex);
                textsPatternResult = twoTextsPattern.GetMatch(lexes, textsPatternResult.Start);
            }

            return lexes;
        }

        protected List<Token> InputFilter(List<Token> lexes)
        {
            var inputPattern = new StartEndTokensPattern(
                TokenType.INPUT_OPERATOR,
                TokenType.LINE_END);
            var inputPatternResult = inputPattern.GetMatch(lexes);
            while (inputPatternResult.IsFullMatch)
            {
                var inputLexes = lexes.GetRange(inputPatternResult.Start + 1, inputPatternResult.Length - 2);
                lexes.RemoveRange(inputPatternResult.Start + 1, inputPatternResult.Length - 2);
                var inputText = TokenUtils.TokensToString(inputLexes);
                var newTextLex = new Token(TokenType.TEXT, inputText);
                lexes.Insert(inputPatternResult.Start + 1, newTextLex);
                inputPatternResult = inputPattern.GetMatch(lexes, inputPatternResult.Start + 1);
            }

            return lexes;
        }

        protected List<Token> OutputFilter(List<Token> lexes)
        {
            var outputPattern = new StartEndTokensPattern(
                TokenType.OUTPUT_OPERATOR,
                TokenType.LINE_END);
            var outputPatternResult = outputPattern.GetMatch(lexes);
            while (outputPatternResult.IsFullMatch)
            {
                var outputLexes = lexes.GetRange(outputPatternResult.Start + 1, outputPatternResult.Length - 2);
                lexes.RemoveRange(outputPatternResult.Start + 1, outputPatternResult.Length - 2);
                var inputText = TokenUtils.TokensToString(outputLexes);
                var newTextLex = new Token(TokenType.TEXT, inputText);
                lexes.Insert(outputPatternResult.Start + 1, newTextLex);
                outputPatternResult = outputPattern.GetMatch(lexes, outputPatternResult.Start + 1);
            }

            return lexes;
        }

        public List<Token> DoFilters(List<Token> lexes)
        {
            Filters.Sort((fFilter, sFiler) => sFiler.Priority.CompareTo(fFilter.Priority));
            foreach (var filterBefore in Filters)
            {
                lexes = filterBefore.ApplyFilter(lexes);
            }
            return lexes;
        }
    }
}