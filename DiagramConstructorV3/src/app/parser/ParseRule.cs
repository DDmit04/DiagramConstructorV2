using System.Collections.Generic;
using DiagramConstructorV3.app.parser.data;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern;
using DiagramConstructorV3.app.tokenPattern.patternMatch;

namespace DiagramConstructorV3.app.parser
{
    public class ParseRule
    {
        public delegate Node ParseRuleDelegate(List<Token> tokens, PatternMatchResult matchResult);
        protected TokenPattern RulePattern { get; }
        protected ParseRuleDelegate ParseFunction { get; }

        public int RuleConditionLength => RulePattern.ConditionsCount;

        public ParseRule(TokenPattern rulePattern, ParseRuleDelegate parseFunction)
        {
            RulePattern = rulePattern;
            ParseFunction = parseFunction;
        }

        protected PatternMatchResult TestRule(List<Token> tokens, int from = 0)
        {
            var matchResult = RulePattern.GetMatch(tokens, from);
            if (matchResult.Start == from)
            {
                return matchResult;
            }

            return PatternMatchResult.Empty;
        }

        public Node TryParseTokens(List<Token> tokens, out int ruleLength, int from = 0)
        {
            ruleLength = -1;
            var ruleMatchResult = TestRule(tokens, from);
            if (ruleMatchResult.IsPartMatch)
            {
                ruleLength = ruleMatchResult.Length;
                return ParseFunction(tokens, ruleMatchResult);
            }
            return null;
        }
    }
}