using System;
using System.Collections.Generic;
using DiagramConstructorV3.app.exceptions;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.patternMatch;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.tokenPattern.boundaryPatterns
{
    public class TokensBlockPattern : TokenPattern
    {
        public static readonly TokensBlockPattern BracketBlock =
            new TokensBlockPattern(TokenType.BRACKET_OPEN, TokenType.BRACKET_CLOSE);

        public TokenType BlockStartToken { get; }
        public TokenType BlockEndToken { get; }

        public override int ConditionsCount => 1;

        public TokensBlockPattern(TokenType blockStartToken, TokenType blockEndToken)
        {
            if (blockStartToken == blockEndToken)
            {
                throw new PatternException("Block can't starts and ends with the same token! Use StartEndPattern instead!");
            }
            BlockStartToken = blockStartToken;
            BlockEndToken = blockEndToken;
        }

        public override PatternMatchResult GetMatch(List<Token> tokens, int @from = 0)
        {
            var openTokenMatch = TokenSearchUtils.FindToken(tokens, BlockStartToken, from);
            if (openTokenMatch.IsFullMatch)
            {
                var blockBeginIndex = openTokenMatch.Start;
                var openTokenCount = 0;
                var closeTokenCount = 0;
                for (var i = blockBeginIndex; i < tokens.Count; i++)
                {
                    if (tokens[i].TokenType == BlockStartToken)
                    {
                        openTokenCount++;
                    }
                    else if (tokens[i].TokenType == BlockEndToken)
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

        public override string ToString()
        {
            var res = "Block: [ " + BlockStartToken + " <any tokens> " + BlockEndToken + " ]";
            return res;
        }
    }
}