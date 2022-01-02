using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;

namespace DiagramConstructorV3.app.tokenFilter
{
    public class EditTokensFilter : TokenFilter
    {
        public delegate List<Token> TokenFilterRule(List<Token> tokens);

        protected readonly TokenFilterRule FilterRule;

        public EditTokensFilter(TokenFilterRule filterRule, int priority) : base(priority)
        {
            FilterRule = filterRule;
        }

        public EditTokensFilter(TokenFilterRule filterRule)
        {
            FilterRule = filterRule;
        }
        public override List<Token> ApplyFilter(List<Token> tokens)
        {
            return FilterRule(tokens);
        }

    }
}