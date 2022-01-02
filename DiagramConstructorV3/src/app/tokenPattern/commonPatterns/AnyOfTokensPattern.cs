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
            return TokenSearchUtils.FindFirstOneTokenIndex(tokens, TokensToSearch, from);
        }

        public void AddToken(TokenType newToken)
        {
            TokensToSearch.Add(newToken);
        }
    }
}