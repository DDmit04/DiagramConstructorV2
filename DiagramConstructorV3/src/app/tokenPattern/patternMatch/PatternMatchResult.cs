using System.Collections.Generic;
using System.Linq;

namespace DiagramConstructorV3.app.tokenPattern.patternMatch
{
    public class PatternMatchResult : AbstractPatternMatch
    {
        public static readonly PatternMatchResult Empty = new PatternMatchResult(-1, -1);
        public override int Start
        {
            get
            {
                var firstSuccessMatch = MatchList.FirstOrDefault(match => match.IsFullMatch);
                if (firstSuccessMatch == null)
                {
                    return -1;
                }
                return firstSuccessMatch.Start;
            }
        }

        public override int End
        {
            get
            {
                var lastSuccessMatch = MatchList.LastOrDefault(match => match.IsFullMatch);
                if (lastSuccessMatch == null)
                {
                    return -1;
                }
                return lastSuccessMatch.End;
            }
        }

        public List<AbstractPatternMatch> MatchList { get; }
        
        public override int MatchesCount => MatchList.Sum(match => match.MatchesCount);

        public override bool IsFullMatch => MatchList.All(match => match.IsFullMatch);
        public bool IsPartMatch => MatchList.Any(match => match.IsFullMatch);

        public PatternMatchResult(int start, int end)
        {
            MatchList = new List<AbstractPatternMatch>() { new PatternMatch(start, end) };
        }

        public PatternMatchResult(List<AbstractPatternMatch> matchList)
        {
            MatchList = matchList;
        }

        public AbstractPatternMatch GetMatch(int index)
        {
            if (index >= MatchList.Count)
            {
                return Empty;
            }
            else
            {
                return MatchList[index];
            }
        }
    }
}