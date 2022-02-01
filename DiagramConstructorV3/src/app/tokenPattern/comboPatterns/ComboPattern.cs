using System.Collections.Generic;
using System.Linq;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.patternMatch;

namespace DiagramConstructorV3.app.tokenPattern.comboPatterns
{
    public abstract class ComboPattern : TokenPattern
    {
        protected List<TokenPattern> Patterns { get; }

        public override int ConditionsCount => Patterns.Sum(pattern => pattern.ConditionsCount);
        public ComboPattern(List<TokenPattern> patterns)
        {
            Patterns = patterns;
        }
        
        public abstract override PatternMatchResult GetMatch(List<Token> tokens, int @from = 0);
        
        public override string ToString()
        {
            var res = "Combo: [ " + string.Join(" -> ", Patterns) + " ]";
            return res;
        }
    }
}