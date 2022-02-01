using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.patternMatch;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.tokenPattern.commonPatterns
{
    public class AnyOfTokensPattern : TokenPattern
    {
        protected readonly List<TokenType> TokensToSearch;

        public override int ConditionsCount => 1;

        public AnyOfTokensPattern(List<TokenType> tokensToSearch)
        {
            TokensToSearch = tokensToSearch;
        }

        public override PatternMatchResult GetMatch(List<Token> tokens, int @from = 0)
        {
            if (TokensToSearch.Count == 1)
            {
                return TokenSearchUtils.FindToken(tokens, TokensToSearch[0], from);
            }
            var firstTokenIndex = int.MaxValue;
            foreach (var tokenType in TokensToSearch)
            {
                var lexMatchResult = TokenSearchUtils.FindToken(tokens, tokenType, from);
                if (lexMatchResult.IsFullMatch && lexMatchResult.Start < firstTokenIndex)
                {
                    firstTokenIndex = lexMatchResult.Start;
                }
            }

            if (firstTokenIndex == int.MaxValue)
            {
                return PatternMatchResult.Empty;
            }
            return new PatternMatchResult(firstTokenIndex, firstTokenIndex + 1);        }

        public override string ToString()
        {
            var res = "Any of: [ " + string.Join(", ", TokensToSearch) + " ]";
            return res;
        }
    }
}