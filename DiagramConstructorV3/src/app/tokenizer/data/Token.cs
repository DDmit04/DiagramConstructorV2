namespace DiagramConstructorV3.app.tokenizer.data
{
    public class Token
    {
        public TokenType TokenType { get; }
        public string TokenText { get; }

        public int LineNumber { get; }

        public Token(TokenType tokenType, string tokenText, int lineNumber)
        {
            TokenType = tokenType;
            TokenText = tokenText;
            LineNumber = lineNumber;
        }
    }
}