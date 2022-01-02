using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.commonPatterns;

namespace DiagramConstructorV3.app.tokenPattern.builders
{
    public class AnyOfTokensPatternBuilder
    {
        protected List<TokenType> TokensToSearch = new List<TokenType>();

        public AnyOfTokensPatternBuilder Reset()
        {
            TokensToSearch = new List<TokenType>();
            return this;
        }
        
        public AnyOfTokensPatternBuilder Or(TokenType newToken)
        {
            TokensToSearch.Add(newToken);
            return this;
        }

        public AnyOfTokensPattern Build()
        {
            var res = new AnyOfTokensPattern(TokensToSearch);
            Reset();
            return res;
        }
    }
}