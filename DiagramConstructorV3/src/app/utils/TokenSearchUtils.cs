using System;
using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.patternMatch;

namespace DiagramConstructorV3.app.utils
{
    public static class TokenSearchUtils
    {
        public static PatternMatchResult FindToken(List<Token> tokens, TokenType tokenType, int from = 0)
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
    }
}