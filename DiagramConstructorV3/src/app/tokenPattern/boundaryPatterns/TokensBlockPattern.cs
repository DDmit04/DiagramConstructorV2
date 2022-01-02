using System;
using System.Collections.Generic;
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
            BlockStartToken = blockStartToken;
            BlockEndToken = blockEndToken;
            if (BlockEndToken == BlockStartToken)
            {
                throw new Exception("Block can't starts and ends with the same token! Use StartEndPattern instead!");
            }
        }

        public override PatternMatchResult GetMatch(List<Token> tokens, int @from = 0)
        {
            return TokenSearchUtils.FindNextTokenBlock(tokens, BlockStartToken, BlockEndToken, from);
        }
    }
}