namespace DiagramConstructorV3.app.tokenPattern.patternMatch
{
    public abstract class AbstractPatternMatch
    {
        public abstract int Start { get; }
        public abstract int End { get; }
        
        public abstract bool IsFullMatch { get; }
        
        public int Length => End - Start;

        public abstract int MatchesCount { get; } 

        }
}