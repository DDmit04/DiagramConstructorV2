using System;
using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.patternMatch;

namespace DiagramConstructorV3.app.utils
{
    public static class TokenSearchUtils
    {
        public static PatternMatchResult FindNextTokenIndex(List<Token> tokens, TokenType tokenType, int from = 0)
        {
            var tmp = tokens.GetRange(from, tokens.Count - from);
            var tokenIndex = tmp.FindIndex(tk => tk.TokenType == tokenType);
            if (tokenIndex == -1)
            {
                return PatternMatchResult.Empty;
            }
            var trueTokenIndex = tokenIndex + from;
            return new PatternMatchResult(trueTokenIndex, trueTokenIndex + 1);
        }

        public static PatternMatchResult FindFirstOneTokenIndex(List<Token> tokens, List<TokenType> searchingTokensTypes, int from = 0)
        {
            if (searchingTokensTypes.Count == 1)
            {
                return FindNextTokenIndex(tokens, searchingTokensTypes[0], from);
            }
            var firstTokenIndex = int.MaxValue;
            foreach (var tokenType in searchingTokensTypes)
            {
                var lexMatchResult = FindNextTokenIndex(tokens, tokenType, from);
                if (lexMatchResult.IsFullMatch && lexMatchResult.Start < firstTokenIndex)
                {
                    firstTokenIndex = lexMatchResult.Start;
                }
            }

            if (firstTokenIndex == int.MaxValue)
            {
                return PatternMatchResult.Empty;
            }
            return new PatternMatchResult(firstTokenIndex, firstTokenIndex + 1);
        }
        
        
        public static PatternMatchResult FindNextTokenBlock(List<Token> tokens, TokenType openTokenType = TokenType.BRACKET_OPEN,
            TokenType closeTokenType = TokenType.BRACKET_CLOSE, int from = 0)
        {
            if (openTokenType == closeTokenType)
            {
                throw new Exception("Block can't starts and ends with the same token! Use StartEndPattern instead!");
            }

            var openTokenMatch = FindNextTokenIndex(tokens, openTokenType, from);
            if (openTokenMatch.IsFullMatch)
            {
                var blockBeginIndex = openTokenMatch.Start;
                var openTokenCount = 0;
                var closeTokenCount = 0;
                for (var i = blockBeginIndex; i < tokens.Count; i++)
                {
                    if (tokens[i].TokenType == openTokenType)
                    {
                        openTokenCount++;
                    }
                    else if (tokens[i].TokenType == closeTokenType)
                    {
                        closeTokenCount++;
                    }

                    if (openTokenCount == closeTokenCount && openTokenCount != 0 && closeTokenCount != 0)
                    {
                        return new PatternMatchResult(blockBeginIndex, i + 1);
                    }
                }
            }

            return PatternMatchResult.Empty;
        }

        public static PatternMatchResult FindTokensSequence(List<Token> tokens, List<TokenType> sequence, int from = 0)
        {
            var firstToken = FindNextTokenIndex(tokens, sequence[0], from);
            if (sequence.Count == 1)
            {
                return firstToken;
            }
            var seqStartIndex = firstToken.Start;
            while (seqStartIndex + sequence.Count <= tokens.Count)
            {
                var allMatches = true;
                // from 1 !!!
                for (var i = 1; i < sequence.Count; i++)
                {
                    if (sequence[i] != tokens[seqStartIndex + i].TokenType)
                    {
                        allMatches = false;
                        break;
                    }
                }

                if (allMatches)
                {
                    return new PatternMatchResult(seqStartIndex, seqStartIndex + sequence.Count);
                }
                seqStartIndex++;
                seqStartIndex = FindNextTokenIndex(tokens, sequence[0], seqStartIndex).Start;
                if (seqStartIndex == -1)
                {
                    return PatternMatchResult.Empty;
                }
            }
            return PatternMatchResult.Empty;
        }
    }
}