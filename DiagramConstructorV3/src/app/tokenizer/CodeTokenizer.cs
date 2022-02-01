using System.Collections.Generic;
using System.Text.RegularExpressions;
using DiagramConstructorV3.app.exceptions;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.tokenizer
{
    public class CodeTokenizer
    {
        protected HashSet<LexRule> LexRules { get; } = new HashSet<LexRule>();

        public List<Token> TokenizeCode(string text)
        {
            var currentLineBreaksCount = 1;
            text = text.TrimStart();
            var result = new List<Token>();
            while (text != string.Empty)
            {
                var nextToken = GetNextLex(text, out var lineBreaksCount);
                currentLineBreaksCount += lineBreaksCount;
                if (nextToken != "")
                {
                    result.AddRange(LexToToken(nextToken, currentLineBreaksCount));
                    text = StringUtils.ReplaceFirst(text, nextToken, "");
                    text = text.TrimStart(' ');
                }
            }
            return result;
        }

        protected List<Token> LexToToken(string str, int lineBreaksCount)
        {
            var resultTokensList = TestToken(str, lineBreaksCount).Key;
            if (resultTokensList.Count == 0)
            {
                while (str != string.Empty)
                {
                    var tokenTestResult = TestToken(str, lineBreaksCount, false);
                    resultTokensList.AddRange(tokenTestResult.Key);
                    str = tokenTestResult.Value;
                }
                if (resultTokensList.Count == 0)
                {
                    throw new LexException(str, lineBreaksCount);
                }
            }
            return resultTokensList;
        }

        protected KeyValuePair<List<Token>, string> TestToken(string token, int lineBreaksCount, bool testFullMatch = true)
        {
            var result = new List<Token>();
            foreach (var lexRule in LexRules)
            {
                Match ruleMatch;
                if (testFullMatch)
                {
                    ruleMatch = lexRule.TestWholeMatch(token);
                }
                else
                {
                    ruleMatch = lexRule.TestPartMatch(token);
                }
                var matchValue = ruleMatch.Value;
                if (matchValue != string.Empty)
                {
                    var newToken = new Token(lexRule.TokenRuleType, matchValue, lineBreaksCount);
                    token = StringUtils.ReplaceFirst(token, matchValue, "");
                    result.Add(newToken);
                    break;
                }
            }
            return new KeyValuePair<List<Token>, string>(result, token);
        }

        protected string GetNextLex(string text, out int lineBreaksCount)
        {
            lineBreaksCount = 0;
            var strEndIndex = 0;
            if(text[strEndIndex] == '\n')
            {
                lineBreaksCount++;
                strEndIndex++;
            }
            else if (strEndIndex < text.Length)
            {
                var nextChar = text[strEndIndex];
                var dijitCharRegex = new Regex("\\d");
                var nonWordCharRegex = new Regex("\\W");
                var identifierRegex = new Regex("[a-zA-Z0-9_]+");
                var identifierStartRegex = new Regex("[a-zA-Z_]+");
                Regex regexpToUse;
                if (identifierStartRegex.IsMatch(nextChar.ToString()) && !char.IsDigit(nextChar))
                {
                    regexpToUse = identifierRegex;
                }
                else if (dijitCharRegex.IsMatch(nextChar.ToString()))
                {
                    regexpToUse = dijitCharRegex;
                }
                else
                {
                    regexpToUse = nonWordCharRegex;
                }

                while (regexpToUse.IsMatch(nextChar.ToString()) && nextChar != ' ' && nextChar != '\n')
                {
                    strEndIndex++;
                    if (strEndIndex >= text.Length)
                    {
                        break;
                    }
                    nextChar = text[strEndIndex];
                }
            }
            var res = text.Substring(0, strEndIndex);
            return res;
        }
    }
}