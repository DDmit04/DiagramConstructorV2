using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.patternMatch;

namespace DiagramConstructorV3.app.tokenPattern.comboPatterns
{
    public class StrictComboPattern : ComboPattern
    {
        public StrictComboPattern(List<TokenPattern> patterns) : base(patterns)
        {
        }

        public override PatternMatchResult GetMatch(List<Token> tokens, int @from = 0)
        {
            if (Patterns.Count > 0)
            {
                var matchList = new List<AbstractPatternMatch>() {PatternMatchResult.Empty};
                var firstPattern = Patterns[0];
                var firstMatch = firstPattern.GetMatch(tokens, from);
                while (firstMatch.IsFullMatch)
                {
                    matchList = new List<AbstractPatternMatch>();
                    matchList.Add(firstMatch);
                    from = firstMatch.End;
                    for (var i = 1; i < Patterns.Count; i++)
                    {
                        var nextPattern = Patterns[i];
                        var nextMatch = nextPattern.GetMatch(tokens, from);
                        if (nextMatch.Start != from)
                        {
                            break;
                        }
                        else if (nextMatch.IsFullMatch)
                        {
                            matchList.Add(nextMatch);
                            from += nextMatch.Length;
                        }
                    }
                    if (matchList.Count == Patterns.Count || from >= tokens.Count)
                    {
                        return new PatternMatchResult(matchList);
                    }
                    from += firstMatch.Length;
                    firstMatch = firstPattern.GetMatch(tokens, from);
                }

                if (matchList.Count == Patterns.Count)
                {
                    return new PatternMatchResult(matchList);
                }
                return PatternMatchResult.Empty;
            }
            return PatternMatchResult.Empty;
        }
    }
}