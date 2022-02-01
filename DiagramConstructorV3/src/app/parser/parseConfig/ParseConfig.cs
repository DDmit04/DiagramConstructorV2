using DiagramConstructorV3.app.tokenPattern;

namespace DiagramConstructorV3.app.parser.parseConfig
{
    public class ParseConfig
    {
        public string MainMethodName { get; set; }
        public TokenPattern ArgsPattern { get; set; }
        public TokenPattern MethodDefPattern { get; set; }
    }
}