using System.Collections.Generic;
using DiagramConstructorV3.app.tokenPattern.comboPatterns;

namespace DiagramConstructorV3.app.tokenPattern.builders
{
    public class StrictComboPatternBuilder
    {
        protected List<TokenPattern> Patterns = new List<TokenPattern>();

        public StrictComboPatternBuilder Reset()
        {
            Patterns = new List<TokenPattern>();
            return this;
        }
        public StrictComboPatternBuilder NextPattern(TokenPattern newPattern)
        {
            Patterns.Add(newPattern);
            return this;
        }

        public ComboPattern Build()
        {
            var res = new StrictComboPattern(Patterns);
            Reset();
            return res;
        }
    }
}