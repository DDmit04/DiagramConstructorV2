using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.commonPatterns;

namespace DiagramConstructorV3.app.tokenPattern.builders
{
    public class TokenSequencePatternBuilder
    {
        protected List<TokenType> Seq = new List<TokenType>();

        public TokenSequencePatternBuilder Reset()
        {
            Seq = new List<TokenType>();
            return this;
        }
        
        public TokenSequencePatternBuilder Then(TokenType newToken)
        {
            Seq.Add(newToken);
            return this;
        }

        public TokenSequencePattern Build()
        {
            var res = new TokenSequencePattern(Seq);
            Reset();
            return res;
        }
    }
}