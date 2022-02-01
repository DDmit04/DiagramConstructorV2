using System;

namespace DiagramConstructorV3.app.exceptions
{
    public class LexException : Exception
    {
        public string ErrorToken { get; }
        public int ErrorLineNumber { get; }

        public LexException(string errorToken, int errorLineNumber)
        {
            ErrorLineNumber = errorLineNumber;
            ErrorToken = errorToken;
        }
    }
}