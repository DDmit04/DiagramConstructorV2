using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern;
using DiagramConstructorV3.app.tokenPattern.patternMatch;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.tokenFilter
{
    public class EditTokensFilter : TokenFilter
    {
        public delegate List<Token> TokenEditFunc(List<Token> tokens);

        protected readonly TokenEditFunc EditFunc;

        public EditTokensFilter(TokenEditFunc editFunc, TokenPattern tokenPattern, int priority) : base(tokenPattern, priority)
        {
            EditFunc = editFunc;
        }

        public EditTokensFilter(TokenPattern tokenPattern, TokenEditFunc editFunc): base(tokenPattern)
        {
            EditFunc = editFunc;
        }
        public override List<Token> ApplyFilter(List<Token> tokens)
        {
            var matchResult = FilterPattern.GetMatch(tokens);
            while (matchResult.IsPartMatch)
            {
                var tokensToApplyRule = TokenUtils.GetMatchResultTokens(tokens, matchResult);
                tokensToApplyRule = EditFunc(tokensToApplyRule);
                tokens.RemoveRange(matchResult.Start, matchResult.Length);
                tokens.InsertRange(matchResult.Start, tokensToApplyRule);
                matchResult = FilterPattern.GetMatch(tokens, matchResult.Start + tokensToApplyRule.Count);
            }
            return tokens;
        }

    }
}