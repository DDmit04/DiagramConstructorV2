using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.patternMatch;

namespace DiagramConstructorV3.app.tokenPattern
{
    public abstract class TokenPattern
    {
        public abstract int ConditionsCount { get; }
        public abstract PatternMatchResult GetMatch(List<Token> tokens, int from = 0);

    }
}