using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern;
using DiagramConstructorV3.app.tokenPattern.patternMatch;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.tokenFilter
{
    public class StrictEditTokensFilter : TokenFilter
    {
        public delegate List<Token> StrictTokenEditFunc(TokenPattern pattern, List<Token> tokens);

        protected readonly StrictTokenEditFunc EditFunc;

        public StrictEditTokensFilter(StrictTokenEditFunc editFunc, TokenPattern tokenPattern, int priority) : base(tokenPattern, priority)
        {
            EditFunc = editFunc;
        }
        
        public StrictEditTokensFilter(StrictTokenEditFunc editFunc, TokenPattern tokenPattern) : base(tokenPattern)
        {
            EditFunc = editFunc;
        }

        public override List<Token> ApplyFilter(List<Token> tokens)
        {
            return EditFunc(FilterPattern, tokens);
        }

    }
}