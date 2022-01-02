using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.commonPatterns;
using DiagramConstructorV3.app.tokenPattern.patternMatch;

namespace DiagramConstructorV3.app.tokenPattern.boundaryPatterns
{
    public class StartEndTokensPattern : TokenPattern
    {
        public TokenPattern StartPattern { get; }
        public TokenPattern EndPattern { get; }

        public override int ConditionsCount => StartPattern.ConditionsCount + EndPattern.ConditionsCount;

        public StartEndTokensPattern(TokenPattern startPattern, TokenPattern endPattern)
        {
            StartPattern = startPattern;
            EndPattern = endPattern;
        }

        public StartEndTokensPattern(TokenType startToken, TokenType endToken)
        {
            StartPattern = new SingleTokenPattern(startToken);
            EndPattern = new SingleTokenPattern(endToken);
        }


        public override PatternMatchResult GetMatch(List<Token> tokens, int @from = 0)
        {
            var beginRes = StartPattern.GetMatch(tokens, from);
            if (beginRes.IsFullMatch)
            {
                var endRes = EndPattern.GetMatch(tokens, beginRes.End);
                return new PatternMatchResult(beginRes.Start, endRes.End);
            }
            return PatternMatchResult.Empty;
        }
    }
}