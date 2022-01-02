namespace DiagramConstructorV3.app.tokenizer.data
{
    public class Token
    {
        public TokenType TokenType { get; }
        public string TokenText { get; }

        public Token(TokenType tokenType, string tokenText)
        {
            TokenType = tokenType;
            TokenText = tokenText;
        }
    }
}