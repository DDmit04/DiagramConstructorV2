using System;
using System.Collections.Generic;
using System.Linq;
using DiagramConstructorV3.app.exceptions;
using DiagramConstructorV3.app.parser.data;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern;
using DiagramConstructorV3.app.tokenPattern.boundaryPatterns;
using DiagramConstructorV3.app.tokenPattern.builders;
using DiagramConstructorV3.app.tokenPattern.commonPatterns;
using DiagramConstructorV3.app.tokenPattern.patternMatch;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.tokenFilter.chain
{
    public class CppTokenFilterChain : TokenFilterChain
    {
        public CppTokenFilterChain()
        {
            var argsBlock = new TokensBlockWithExcludePatternBuilder()
                .Reset()
                .StartWith(TokenType.ARGS_OPEN)
                .ExceptRange(
                    Enum.GetValues(typeof(TokenType))
                        .Cast<TokenType>()
                        .Where(t => t.IsOperatorToken())
                        .ToList())
                .Except(TokenType.ACCESS_MODIFICATOR)
                .EndWith(TokenType.ARGS_CLOSE)
                .Build();

            var gotoLabelPattern = TokenSequencePatternBuilder
                .Reset()
                .Then(TokenType.IDENTIFIER)
                .Then(TokenType.DOUBLE_DOT)
                .Build();
            var gotoConstructionPattern = new StartEndTokensPattern(
                TokenType.RESERVE_5,
                TokenType.LINE_END);
            var accessModificatorPattern = TokenSequencePatternBuilder
                .Reset()
                .Then(TokenType.ACCESS_MODIFICATOR)
                .Then(TokenType.DOUBLE_DOT)
                .Build();
            var classQualifierPattern = TokenSequencePatternBuilder
                .Reset()
                .Then(TokenType.IDENTIFIER)
                .Then(TokenType.DOUBLE_DOT)
                .Then(TokenType.DOUBLE_DOT)
                .Build();
            var throwLinePattern = new StartEndTokensPattern(
                TokenType.THROW_OPERATOR,
                TokenType.LINE_END);
            var emptyObjVarsPattern = TokenSequencePatternBuilder
                .Reset()
                .Then(TokenType.IDENTIFIER)
                .Then(TokenType.IDENTIFIER)
                .Then(TokenType.LINE_END)
                .Build();
            var varDefPattern = TokenSequencePatternBuilder
                .Reset()
                .Then(TokenType.IDENTIFIER)
                .Then(TokenType.LINE_END)
                .Build();
            var castPattern = TokenSequencePatternBuilder
                .Reset()
                .Then(TokenType.ARGS_OPEN)
                .Then(TokenType.IDENTIFIER)
                .Then(TokenType.ARGS_CLOSE)
                .Build();

            var catchBlockPattern = StrictPatternBuilder
                .Reset()
                .NextPattern(new SingleTokenPattern(TokenType.CATCH_OPERATOR))
                .NextPattern(argsBlock)
                .NextPattern(TokensBlockPattern.BracketBlock)
                .Build();

            var structDefPattern = StrictPatternBuilder
                .Reset()
                .NextPattern(TokenSequencePatternBuilder
                    .Reset()
                    .Then(TokenType.RESERVE_1)
                    .Then(TokenType.IDENTIFIER)
                    .Build())
                .NextPattern(TokensBlockPattern.BracketBlock)
                .Build();

            var structVarInitComboPattern = StrictPatternBuilder
                .Reset()
                .NextPattern(TokenSequencePatternBuilder
                    .Reset()
                    .Then(TokenType.IDENTIFIER)
                    .Then(TokenType.IDENTIFIER)
                    .Then(TokenType.EQUAL_ACTION)
                    .Build())
                .NextPattern(TokensBlockPattern.BracketBlock)
                .Build();
            
            var tryPattern = ComboPatternByFirstMatchBuilder
                .Reset()
                .NextPattern(new SingleTokenPattern(TokenType.TRY_OPERATOR))
                .NextPattern(TokensBlockPattern.BracketBlock)
                .Build();
            
            var classDefPattern = ComboPatternByFirstMatchBuilder
                .Reset()
                .NextPattern(new SingleTokenPattern(TokenType.CLASS_OPERATOR))
                .NextPattern(new SingleTokenPattern(TokenType.IDENTIFIER))
                .NextPattern(TokensBlockPattern.BracketBlock)
                .Build();

            var lineBreakFilter = new SingleTokenPattern(TokenType.NEW_LINE);
            var tabFilter = new SingleTokenPattern(TokenType.TABULATION);
            var carriageRetFilter = new SingleTokenPattern(TokenType.CARRIAGE_RETURN);
            
            Filters.AddRange(new List<TokenFilter>()
            {
                new RemoveTokensFilter(lineBreakFilter, TokenFilter.HighestPriority),
                new RemoveTokensFilter(tabFilter, TokenFilter.HighestPriority),
                new RemoveTokensFilter(carriageRetFilter, TokenFilter.HighestPriority),

                new RemoveTokensFilter(gotoLabelPattern, TokenFilter.PreHighestPriority),
                new RemoveTokensFilter(gotoConstructionPattern, TokenFilter.PreHighestPriority),
                new RemoveTokensFilter(catchBlockPattern, TokenFilter.PreHighestPriority),
                new RemoveTokensFilter(structDefPattern, TokenFilter.PreHighestPriority),
                new RemoveTokensFilter(structVarInitComboPattern, TokenFilter.PreHighestPriority),
                new RemoveTokensFilter(accessModificatorPattern, TokenFilter.PreHighestPriority),
                new RemoveTokensFilter(classQualifierPattern, TokenFilter.PreHighestPriority),

                new StrictEditTokensFilter(FilterTryBlock, tryPattern),
                new StrictEditTokensFilter(ClassFilter, classDefPattern),

                new RemoveTokensFilter(throwLinePattern, TokenFilter.PreLowestPriority),
                new RemoveTokensFilter(castPattern, TokenFilter.PreLowestPriority),

                new RemoveTokensFilter(emptyObjVarsPattern, TokenFilter.LowestPriority),
                new RemoveTokensFilter(varDefPattern, TokenFilter.LowestPriority)
            });
        }

        protected List<Token> ClassFilter(TokenPattern classDefPattern, List<Token> tokens)
        {
            var classDef = classDefPattern.GetMatch(tokens);
            while (classDef.IsPartMatch)
            {
                var classOperatorMatch = classDef.GetMatch(0);
                var classArgsMatch = classDef.GetMatch(1);
                var codeBlockMatch = classDef.GetMatch(2);
                if (codeBlockMatch.IsFullMatch)
                {
                    var classBlockLex = TokenUtils.GetMatchResultBlockTokens(tokens, codeBlockMatch);
                    tokens.RemoveRange(classDef.Start, classDef.Length);
                    tokens.InsertRange(classDef.Start, classBlockLex);
                    classDef = classDefPattern.GetMatch(tokens, classDef.End - 4);
                }
                else if (classOperatorMatch.IsFullMatch && (!classArgsMatch.IsFullMatch | classArgsMatch.IsFullMatch))
                {
                    throw new ParseRuleException(classDefPattern, tokens, classDef.Start);
                }
            }

            return tokens;
        }

        protected List<Token> FilterTryBlock(TokenPattern tryPattern, List<Token> tokens)
        {
            var tryMatch = tryPattern.GetMatch(tokens);
            while (tryMatch.IsPartMatch)
            {
                var tryOpMatch = tryMatch.GetMatch(0);
                var codeBlockMatch = tryMatch.GetMatch(1);
                if (tryMatch.IsFullMatch)
                {
                    var tryBlockTokens = tokens.GetRange(codeBlockMatch.Start + 1, codeBlockMatch.Length - 2);
                    tokens.RemoveRange(tryMatch.Start, tryMatch.Length);
                    tokens.InsertRange(tryMatch.Start, tryBlockTokens);
                    tryMatch = tryPattern.GetMatch(tokens, tryMatch.Start + codeBlockMatch.Length);
                }
                else if (tryOpMatch.IsFullMatch && !codeBlockMatch.IsFullMatch)
                {
                    throw new ParseRuleException(tryPattern, tokens, tryMatch.Start);
                }
            }

            return tokens;
        }
    }
}