using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.patternMatch;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.tokenPattern.commonPatterns
{
    public class SingleTokenPattern : TokenPattern
    {
        public TokenType Token { get; }
        public override int ConditionsCount => 1;

        public SingleTokenPattern(TokenType token)
        {
            Token = token;
        }

        public override PatternMatchResult GetMatch(List<Token> tokens, int @from = 0)
        {
            return TokenSearchUtils.FindToken(tokens, Token, from);
        }

        public override string ToString()
        {
            return "Single: [ " + Token + " ]";
        }
    }
}