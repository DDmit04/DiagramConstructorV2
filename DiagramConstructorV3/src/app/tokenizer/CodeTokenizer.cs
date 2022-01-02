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
            text = text.TrimStart();
            var result = new List<Token>();
            while (text != string.Empty)
            {
                var nextToken = GetNextStr(text);
                result.AddRange(LexToToken(nextToken));
                text = StringUtils.ReplaceFirst(text, nextToken, "");
                text = text.TrimStart(' ');
            }
            result.RemoveAll(lex => lex.TokenType == TokenType.LANG_SPECIFIC);
            return result;
        }

        protected List<Token> LexToToken(string str)
        {
            var resultTokensList = TestToken(str).Key;
            if (resultTokensList.Count == 0)
            {
                while (str != string.Empty)
                {
                    var tokenTestResult = TestToken(str, false);
                    resultTokensList.AddRange(tokenTestResult.Key);
                    str = tokenTestResult.Value;
                }
                if (resultTokensList.Count == 0)
                {
                    throw new LexException(str);
                }
            }
            return resultTokensList;
        }

        protected KeyValuePair<List<Token>, string> TestToken(string token, bool testFullMatch = true)
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
                    var newToken = new Token(lexRule.TokenRuleType, matchValue);
                    token = StringUtils.ReplaceFirst(token, matchValue, "");
                    result.Add(newToken);
                    break;
                }
            }
            return new KeyValuePair<List<Token>, string>(result, token);
        }

        protected string GetNextStr(string text)
        {
            var strEndIndex = 0;
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
            
            while (regexpToUse.IsMatch(nextChar.ToString()) && nextChar != ' ' && strEndIndex < text.Length)
            {
                strEndIndex++;
                if (strEndIndex >= text.Length)
                {
                    break;
                }

                nextChar = text[strEndIndex];
            }

            return text.Substring(0, strEndIndex);
        }
    }
}