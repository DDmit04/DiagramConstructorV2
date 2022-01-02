using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.patternMatch;

namespace DiagramConstructorV3.app.tokenPattern.comboPatterns
{
    public class ComboPatternByFirstMatch : ComboPattern
    {
        public ComboPatternByFirstMatch(List<TokenPattern> patterns) : base(patterns)
        {
        }

        public override PatternMatchResult GetMatch(List<Token> tokens, int @from = 0)
        {
            if (Patterns.Count > 0)
            {
                var firstPattern = Patterns[0];
                var firstMatch = firstPattern.GetMatch(tokens, from);
                var matchList = new List<AbstractPatternMatch>();
                if (firstMatch.IsFullMatch)
                {
                    var failed = false;
                    matchList.Add(firstMatch);
                    from = firstMatch.End;
                    for (var i = 1; i < Patterns.Count; i++)
                    {
                        if (failed)
                        {
                            matchList.Add(PatternMatchResult.Empty);
                            continue;
                        }
                        var nextPattern = Patterns[i];
                        var nextMatch = nextPattern.GetMatch(tokens, from);
                        if (nextMatch.Start != from)
                        {
                            failed = true;
                            matchList.Add(PatternMatchResult.Empty);
                        }
                        else if (nextMatch.IsFullMatch)
                        {
                            matchList.Add(nextMatch);
                            from += nextMatch.Length;
                        }
                    }
                    return new PatternMatchResult(matchList);
                }
                return PatternMatchResult.Empty;
            }
            return PatternMatchResult.Empty;
        }
    }
}