using System;

namespace DiagramConstructorV3.app.exceptions
{
    public class LexException : Exception
    {
        public string ErrorToken { get; }

        public LexException(string errorToken)
        {
            ErrorToken = errorToken;
        }
    }
}