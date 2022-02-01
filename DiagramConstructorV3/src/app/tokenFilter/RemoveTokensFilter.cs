using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern;

namespace DiagramConstructorV3.app.tokenFilter
{
    public class RemoveTokensFilter : TokenFilter
    {
        public RemoveTokensFilter(TokenPattern pattern, int priority) : base(pattern, priority)
        {
            FilterPattern = pattern;
        }

        public RemoveTokensFilter(TokenPattern pattern): base(pattern)
        {
            FilterPattern = pattern;
        }

        public override List<Token> ApplyFilter(List<Token> tokens)
        {
            var from = 0;
            var patternRes = FilterPattern.GetMatch(tokens, from);
            while (patternRes.IsFullMatch)
            {
                tokens.RemoveRange(patternRes.Start, patternRes.Length);
                from = patternRes.Start;
                patternRes = FilterPattern.GetMatch(tokens, from);
            }

            return tokens;
        }
    }
}