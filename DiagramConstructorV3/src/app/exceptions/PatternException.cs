using System;

namespace DiagramConstructorV3.app.exceptions
{
    public class PatternException : Exception
    {
        public PatternException(string message) : base(message)
        {
        }
    }
}