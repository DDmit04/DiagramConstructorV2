using System.Collections.Generic;
using DiagramConstructorV3.app.parser.data;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern;

namespace DiagramConstructorV3.app.exceptions
{
    public class ParseRuleException : ParseException
    {
        public TokenPattern Pattern { get; }

        public ParseRuleException(TokenPattern tokenPattern, List<Token> errorTokens, int errorStart, NodeType parsedNodeType) : base(errorTokens, errorStart, parsedNodeType)
        {
            Pattern = tokenPattern;
        }
        
        public ParseRuleException(TokenPattern tokenPattern, List<Token> errorTokens, int errorStart) : base(errorTokens, errorStart)
        {
            Pattern = tokenPattern;
        }
    }
}