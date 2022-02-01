using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern;
using DiagramConstructorV3.app.tokenPattern.commonPatterns;

namespace DiagramConstructorV3.app.tokenFilter
{
    public abstract class TokenFilter
    {
        public int Priority { get; }
        
        protected TokenPattern FilterPattern;

        public int FilterConditionsCount => FilterPattern.ConditionsCount;

        public static readonly int HighestPriority = int.MaxValue;
        public static readonly int PreHighestPriority = int.MaxValue / 2;
        public static readonly int CommonPriority = 0;
        public static readonly int PreLowestPriority = -int.MaxValue / 2;
        public static readonly int LowestPriority = -int.MaxValue;

        protected TokenFilter(TokenPattern filterPattern, int priority)
        {
            FilterPattern = filterPattern;
            Priority = priority;
        }

        protected TokenFilter(TokenPattern filterPattern)
        {
            FilterPattern = filterPattern;
            Priority = CommonPriority;
        }

        public abstract List<Token> ApplyFilter(List<Token> tokens);
    }
}