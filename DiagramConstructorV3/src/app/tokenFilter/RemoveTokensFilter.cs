using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern;

namespace DiagramConstructorV3.app.tokenFilter
{
    public class RemoveTokensFilter : TokenFilter
    {
        protected readonly TokenPattern PatternToRemove;

        public RemoveTokensFilter(TokenPattern pattern, int priority) : base(priority)
        {
            PatternToRemove = pattern;
        }

        public RemoveTokensFilter(TokenPattern pattern)
        {
            PatternToRemove = pattern;
        }

        public override List<Token> ApplyFilter(List<Token> tokens)
        {
            var from = 0;
            var patternRes = PatternToRemove.GetMatch(tokens, from);
            while (patternRes.IsFullMatch)
            {
                tokens.RemoveRange(patternRes.Start, patternRes.Length);
                from = patternRes.Start;
                patternRes = PatternToRemove.GetMatch(tokens, from);
            }

            return tokens;
        }
    }
}