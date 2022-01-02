using System.Collections.Generic;
using DiagramConstructorV3.app.tokenPattern.comboPatterns;

namespace DiagramConstructorV3.app.tokenPattern.builders
{
    public class ComboPatternByFirstMatchBuilder
    {
        protected List<TokenPattern> Patterns = new List<TokenPattern>();

        public ComboPatternByFirstMatchBuilder Reset()
        {
            Patterns = new List<TokenPattern>();
            return this;
        }
        public ComboPatternByFirstMatchBuilder NextPattern(TokenPattern newPattern)
        {
            Patterns.Add(newPattern);
            return this;
        }

        public ComboPattern Build()
        {
            var res = new ComboPatternByFirstMatch(Patterns);
            Reset();
            return res;
        }
    }
}