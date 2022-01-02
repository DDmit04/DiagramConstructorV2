using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DiagramConstructorV3.app.tokenizer.data
{
    public class LexRule
    {
        public TokenType TokenRuleType { get; }
        protected List<Regex> RuleRegexs { get; }

        public LexRule(TokenType tokenRuleType, Regex ruleRegexs)
        {
            TokenRuleType = tokenRuleType;
            RuleRegexs = new List<Regex> { ruleRegexs };
        }

        public Match TestPartMatch(string str)
        {
            var match = RuleRegexs.Select(rgx => rgx.Match(str))
                .FirstOrDefault(mch => mch.Success && str.IndexOf(mch.Value, StringComparison.Ordinal) == 0);
            return match ?? Match.Empty;
        }

        public Match TestWholeMatch(string str)
        {
            var match = RuleRegexs.Select(rgx => rgx.Match(str))
                .FirstOrDefault(mch =>
                    mch.Success && mch.Value.Length == str.Length &&
                    str.IndexOf(mch.Value, StringComparison.Ordinal) == 0);
            return match ?? Match.Empty;
        }
    }
}