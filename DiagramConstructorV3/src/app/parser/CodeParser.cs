using System;
using System.Collections.Generic;
using System.Linq;
using DiagramConstructorV3.app.exceptions;
using DiagramConstructorV3.app.parser.data;
using DiagramConstructorV3.app.threeController.textController;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern;
using DiagramConstructorV3.app.tokenPattern.boundaryPatterns;
using DiagramConstructorV3.app.tokenPattern.builders;
using DiagramConstructorV3.app.tokenPattern.commonPatterns;
using DiagramConstructorV3.app.tokenPattern.patternMatch;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.parser
{
    public abstract class CodeParser
    {
        protected readonly TokenSequencePatternBuilder TokenSequencePatternBuilder =
            new TokenSequencePatternBuilder();

        protected readonly StrictComboPatternBuilder StrictPatternBuilder =
            new StrictComboPatternBuilder();

        protected readonly ComboPatternByFirstMatchBuilder PatternByFirstMatchBuilder =
            new ComboPatternByFirstMatchBuilder();

        protected readonly TokensBlockWithExcludePatternBuilder TokensBlockWithExcludePatternBuilder =
            new TokensBlockWithExcludePatternBuilder();

        protected TokenPattern ArgsPattern;
        protected TokenPattern MethodDefPattern;

        protected List<ParseRule> ParseRules;

        public CodeParser()
        {
            ArgsPattern = TokensBlockWithExcludePatternBuilder
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
            MethodDefPattern = StrictPatternBuilder
                .Reset()
                .NextPattern(new SingleTokenPattern(TokenType.IDENTIFIER))
                .NextPattern(ArgsPattern)
                .NextPattern(TokensBlockPattern.BracketBlock)
                .Build();
        }

        public List<Method> ParseCode(List<Token> tokensToParse)
        {
            var methods = new List<Method>();
            var pos = 0;
            while (pos < tokensToParse.Count)
            {
                var methodNodes = new List<Node>();
                var methodSignature = "main()";
                var nexMethodMatch = MethodDefPattern.GetMatch(tokensToParse, pos);
                List<Token> methodBlockTokens;
                if (nexMethodMatch.IsFullMatch)
                {
                    var globalNodes = new List<Node>();
                    if (nexMethodMatch.Start > pos)
                    {
                        var globalTokens = tokensToParse.GetRange(pos, nexMethodMatch.Start - pos);
                        globalNodes = ParseBlock(globalTokens);
                        pos = nexMethodMatch.Start;
                    }

                    pos += nexMethodMatch.Length;

                    var methodSignatureTokens = GetMethodSignatureTokens(tokensToParse, nexMethodMatch);
                    methodSignature = TokenUtils.TokensToString(methodSignatureTokens);
                    if (globalNodes.Count > 0)
                    {
                        methodNodes.AddRange(globalNodes);
                    }

                    methodBlockTokens = GetMethodBlockTokens(tokensToParse, nexMethodMatch);
                }
                else
                {
                    methodBlockTokens = tokensToParse;
                }

                methodNodes = ParseBlock(methodBlockTokens);
                var newMethod = new Method(methodSignature, methodNodes);
                methods.Add(newMethod);
            }

            return methods;
        }

        protected List<Node> ParseBlock(List<Token> methodTokens)
        {
            var nodes = new List<Node>();
            var pos = 0;
            ParseRules.Sort((fRule, sRule) => sRule.RuleConditionLength.CompareTo(fRule.RuleConditionLength));
            while (pos < methodTokens.Count)
            {
                var parsed = false;
                foreach (var syntaxRule in ParseRules)
                {
                    var newNode = syntaxRule.TryParseTokens(methodTokens, out var ruleLength, pos);
                    if (ruleLength > 0)
                    {
                        nodes.Add(newNode);
                        pos += ruleLength;
                        parsed = true;
                        break;
                    }
                }

                if (!parsed)
                {
                    throw new ParseRuleNotFoundException(methodTokens, pos);
                }
            }

            return nodes;
        }

        protected List<Token> GetMethodSignatureTokens(List<Token> tokens, PatternMatchResult methodMatch)
        {
            if (methodMatch.MatchesCount > 1)
            {
                var methodIdMatch = methodMatch.GetMatch(0);
                var methodArgsMatch = methodMatch.GetMatch(1);
                var methodSignature = TokenUtils.GetMatchResultTokens(tokens, methodIdMatch);
                methodSignature.AddRange(TokenUtils.GetMatchResultTokens(tokens, methodArgsMatch));
                return methodSignature;
            }
            throw new ParseException(tokens, methodMatch.Start);
        }

        protected List<Token> GetMethodBlockTokens(List<Token> tokens, PatternMatchResult methodMatch)
        {
            if (methodMatch.MatchesCount > 2)
            {
                var methodBlockMatch = methodMatch.GetMatch(2);
                var methodBlockTokens = TokenUtils.GetMatchResultTokens(tokens, methodBlockMatch);
                return methodBlockTokens;
            }
            throw new ParseException(tokens, methodMatch.Start);
        }

        protected TokenPattern BuildFullOperatorPattern(TokenType operatorType)
        {
            return PatternByFirstMatchBuilder
                .Reset()
                .NextPattern(new SingleTokenPattern(operatorType))
                .NextPattern(ArgsPattern)
                .NextPattern(TokensBlockPattern.BracketBlock)
                .Build();
        }
        
        protected ParseRule GetElseParseRule(TokenPattern pattern)
        {
            var parseRuleDelegate = GetElseParseFunc(pattern);
            return new ParseRule(pattern, parseRuleDelegate);
        }

        protected ParseRule.ParseRuleDelegate GetElseParseFunc(TokenPattern pattern)
        {
            return (tokens, matchResult) =>
            {
                if (matchResult.MatchesCount == 2)
                {
                    var blockMatch = matchResult.GetMatch(1);
                    if (matchResult.IsFullMatch)
                    {
                        var blockTokens = TokenUtils.GetMatchResultBlockTokens(tokens, blockMatch);
                        var childNodes = ParseBlock(blockTokens);

                        var newNode = new Node(NodeType.ELSE);
                        newNode.ChildNodes = childNodes;
                        return newNode;
                    }
                    else if (!blockMatch.IsFullMatch)
                    {
                        throw new ParseRuleException(pattern, tokens, matchResult.Start);
                    }
                }
                throw new ParseException(tokens, matchResult.Start);
            };
        }

        protected ParseRule GetDoWhileParseRule(TokenPattern pattern)
        {
            var parseRuleDelegate = GetDoWhileParseFunc(pattern);
            return new ParseRule(pattern, parseRuleDelegate);
        }
        protected ParseRule.ParseRuleDelegate GetDoWhileParseFunc(TokenPattern pattern)
        {
            return (tokens, matchResult) =>
            {
                if (matchResult.MatchesCount == 5)
                {
                    var blockMatch = matchResult.GetMatch(1);
                    var whileOperatorMatch = matchResult.GetMatch(2);
                    var argsMatch = matchResult.GetMatch(3);
                    var lineEndMatch = matchResult.GetMatch(4);
                    if (matchResult.IsFullMatch)
                    {
                        var blockTokens = TokenUtils.GetMatchResultBlockTokens(tokens, blockMatch);
                        var childNodes = ParseBlock(blockTokens);

                        var argsTokens = TokenUtils.GetMatchResultTokens(tokens, argsMatch);

                        var newNode = new Node(NodeType.DO_WHILE, argsTokens);
                        newNode.ChildNodes = childNodes;
                        return newNode;
                    }
                    else if (!blockMatch.IsFullMatch || !whileOperatorMatch.IsFullMatch || !argsMatch.IsFullMatch ||
                             !lineEndMatch.IsFullMatch)
                    {
                        throw new ParseRuleException(pattern, tokens, matchResult.Start);
                    }
                }

                throw new ParseException(tokens, matchResult.Start);
            };
        }

        protected ParseRule GetCommonOperatorParseRule(NodeType nodeType, TokenPattern pattern)
        {
            var parseRuleDelegate = GetCommonOperatorParseFunc(nodeType, pattern);
            return new ParseRule(pattern, parseRuleDelegate);
        }

        protected ParseRule.ParseRuleDelegate GetCommonOperatorParseFunc(NodeType nodeType, TokenPattern pattern)
        {
            return (tokens, matchResult) =>
            {
                if (matchResult.MatchesCount == 3)
                {
                    var argsMatch = matchResult.GetMatch(1);
                    var blockMatch = matchResult.GetMatch(2);
                    if (matchResult.IsFullMatch)
                    {
                        var argsTokens = TokenUtils.GetMatchResultTokens(tokens, argsMatch);
                        var blockTokens = TokenUtils.GetMatchResultBlockTokens(tokens, blockMatch);
                        var childNodes = ParseBlock(blockTokens);

                        var newNode = new Node(nodeType, argsTokens);
                        newNode.ChildNodes = childNodes;
                        return newNode;
                    }
                    else if (!argsMatch.IsFullMatch || !blockMatch.IsFullMatch)
                    {
                        throw new ParseRuleException(pattern, tokens, matchResult.Start);
                    }
                }
                throw new ParseException(tokens, matchResult.Start);
            };
        }

        protected ParseRule GetSimpleOperatorParseRule(NodeType nodeType, TokenPattern pattern)
        {
            var parseRuleDelegate = GetSimpleOperatorParseFunc(nodeType, pattern);
            return new ParseRule(pattern, parseRuleDelegate);
        }
        protected ParseRule.ParseRuleDelegate GetSimpleOperatorParseFunc(NodeType nodeType, TokenPattern pattern)
        {
            return (tokens, matchResult) =>
            {
                if (matchResult.IsFullMatch)
                {
                    var nodeTokens = TokenUtils.GetMatchResultTokens(tokens, matchResult);
                    var newNode = new Node(nodeType, nodeTokens);
                    return newNode;
                } 
                throw new ParseRuleException(pattern, tokens, matchResult.Start);
            };
        }
        
    }
}