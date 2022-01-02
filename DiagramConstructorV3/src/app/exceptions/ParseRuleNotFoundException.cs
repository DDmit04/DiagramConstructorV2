using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;

namespace DiagramConstructorV3.app.exceptions
{
    public class ParseRuleNotFoundException : ParseException
    {
        public ParseRuleNotFoundException(List<Token> errorTokens, int errorStart) : base(errorTokens, errorStart)
        {
        }
    }
}