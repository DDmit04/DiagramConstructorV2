using System.Collections.Generic;
using System.Linq;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.patternMatch;

namespace DiagramConstructorV3.app.utils
{
    public static class TokenUtils
    {
        public static string TokensToString(List<Token> tokens)
        {
            if (tokens.Count > 0)
            {
                return string.Join(" ", tokens.Select(tk => tk.TokenText));
            }

            return "";
        }
        public static string MatchToString(List<Token> tokens, AbstractPatternMatch matchResult)
        {
            var tokensBlock = GetMatchResultTokens(tokens, matchResult);
            return TokensToString(tokensBlock);
        }
        public static List<Token> GetMatchResultTokens(List<Token> tokens, AbstractPatternMatch matchResult)
        {
            return tokens.GetRange(matchResult.Start, matchResult.Length);
        }
        public static List<Token> GetMatchResultBlockTokens(List<Token> tokens, AbstractPatternMatch matchResult)
        {
            return tokens.GetRange(matchResult.Start + 1, matchResult.Length - 2);
        }
    }
}