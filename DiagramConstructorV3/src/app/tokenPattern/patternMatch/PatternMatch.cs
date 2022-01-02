namespace DiagramConstructorV3.app.tokenPattern.patternMatch
{
    public class PatternMatch : AbstractPatternMatch
    {
        public override int Start { get; }
        public override int End { get; }
        
        public override bool IsFullMatch => Start > -1 && End > -1 && Length > 0;

        public override int MatchesCount => 1;

        public PatternMatch(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}