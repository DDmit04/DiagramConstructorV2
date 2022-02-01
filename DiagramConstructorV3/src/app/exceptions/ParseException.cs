using System;
using System.Collections.Generic;
using DiagramConstructorV3.app.parser.data;
using DiagramConstructorV3.app.tokenizer.data;

namespace DiagramConstructorV3.app.exceptions
{
    public class ParseException : Exception
    {
        public NodeType ParsedNodeType { get; }
        public int ErrorLineNumber { get; }
        public List<Token> ErrorTokens { get; }
        public ParseException(List<Token> errorTokens, int errorStart, NodeType parsedNodeType = NodeType.OTHER)
        {
            ParsedNodeType = parsedNodeType;
            var errorTokensCount = errorTokens.Count - errorStart;
            List<Token> errorStartTokens;
            if (errorTokensCount - errorStart > 10)
            {
                errorStartTokens = errorTokens.GetRange(errorStart, 10);
            }
            else
            {
                errorStartTokens = errorTokens.GetRange(errorStart, errorTokensCount);
            }
            ErrorTokens = errorStartTokens;
            ErrorLineNumber = ErrorTokens[0].LineNumber;
        }
    }
}