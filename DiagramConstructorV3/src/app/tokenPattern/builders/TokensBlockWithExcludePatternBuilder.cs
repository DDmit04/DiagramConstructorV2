using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.boundaryPatterns;

namespace DiagramConstructorV3.app.tokenPattern.builders
{
    public class TokensBlockWithExcludePatternBuilder
    {
        protected TokenType StartToken;
        protected TokenType EndToken;
        protected readonly List<TokenType> ExcludeList = new List<TokenType>();

        public TokensBlockWithExcludePatternBuilder Reset()
        {
            StartToken = TokenType.BRACKET_OPEN;
            EndToken = TokenType.BRACKET_CLOSE;
            return this;
        }
        
        public TokensBlockWithExcludePatternBuilder StartWith(TokenType startToken)
        {
            StartToken = startToken;
            return this;
        }
        
        public TokensBlockWithExcludePatternBuilder EndWith(TokenType endToken)
        {
            EndToken = endToken;
            return this;
        }
        
        public TokensBlockWithExcludePatternBuilder Except(TokenType exceptToken)
        {
            ExcludeList.Add(exceptToken);
            return this;
        }
        
        public TokensBlockWithExcludePatternBuilder ExceptRange(List<TokenType> exceptTokens)
        {
            ExcludeList.AddRange(exceptTokens);
            return this;
        }

        public TokensBlockWithExcludePattern Build()
        {
            var res = new TokensBlockWithExcludePattern(StartToken, EndToken, ExcludeList);
            Reset();
            return res;
        }
    }
}