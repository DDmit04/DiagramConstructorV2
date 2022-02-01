using DiagramConstructorV3.app.parser.data;
using DiagramConstructorV3.app.tokenFilter;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.commonPatterns;

namespace DiagramConstructorV3.app.threeController.textController
{
    public class CppThreeTextController : ThreeTextController
    {
        public CppThreeTextController()
        {
            FormatRules.Add(new NodeTokensFormatRule(NodeType.ANY, new RemoveTokensFilter(new SingleTokenPattern(TokenType.LINE_END))));
            FormatRules.Add(new NodeTokensFormatRule(NodeType.ANY, new RemoveTokensFilter(new SingleTokenPattern(TokenType.INPUT_OPERATOR))));
            FormatRules.Add(new NodeTokensFormatRule(NodeType.ANY, new RemoveTokensFilter(new SingleTokenPattern(TokenType.OUTPUT_OPERATOR))));
        }
    }
}