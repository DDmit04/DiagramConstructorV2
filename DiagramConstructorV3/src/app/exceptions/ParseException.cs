using System;
using System.Collections.Generic;
using System.Linq;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern;

namespace DiagramConstructorV3.app.exceptions
{
    public class ParseException : Exception
    {
        public List<Token> ErrorTokens { get; }
        public ParseException(List<Token> errorTokens, int errorStart)
        {
            var errorTokensCount = errorTokens.Count - errorStart;
            List<Token> errorStartTokens;
            if (errorTokensCount >= 3)
            {
                errorStartTokens = errorTokens.GetRange(errorStart, 10);
            }
            else
            {
                errorStartTokens = errorTokens.GetRange(errorStart, errorTokensCount);
            }
            ErrorTokens = errorStartTokens;
        }
    }
}