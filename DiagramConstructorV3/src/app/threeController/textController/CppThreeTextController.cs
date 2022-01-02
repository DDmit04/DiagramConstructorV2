using System.Collections.Generic;
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
            FormatRules.Add(new NodeTextFormatRule(NodeType.FOR, new EditTokensFilter(FormatCommonFor)));
            FormatRules.Add(new NodeTextFormatRule(NodeType.ANY, new RemoveTokensFilter(new SingleTokenPattern(TokenType.LINE_END))));
            FormatRules.Add(new NodeTextFormatRule(NodeType.ANY, new RemoveTokensFilter(new SingleTokenPattern(TokenType.INPUT_OPERATOR))));
            FormatRules.Add(new NodeTextFormatRule(NodeType.ANY, new RemoveTokensFilter(new SingleTokenPattern(TokenType.OUTPUT_OPERATOR))));
        }

        public List<Token> FormatCommonFor(List<Token> tokens)
        {
            return tokens;
        }
    }
}