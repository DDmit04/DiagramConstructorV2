using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;

namespace DiagramConstructorV3.app.tokenFilter
{
    public abstract class TokenFilter
    {
        public int Priority { get; }

        public static readonly int HighestPriority = int.MaxValue;
        public static readonly int PreHighestPriority = int.MaxValue / 2;
        public static readonly int CommonPriority = 0;
        public static readonly int PreLowestPriority = -int.MaxValue / 2;
        public static readonly int LowestPriority = -int.MaxValue;
        
        public TokenFilter(int priority)
        {
            Priority = priority;
        }
        public TokenFilter()
        {
            Priority = CommonPriority;
        }
        
        public abstract List<Token> ApplyFilter(List<Token> tokens);
    }
}