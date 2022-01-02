using DiagramConstructorV3.app.parser.data;
using DiagramConstructorV3.app.tokenFilter;

namespace DiagramConstructorV3.app.threeController.textController
{
    public class NodeTextFormatRule
    {
        public NodeType ApplyToNodeType { get; }
        protected TokenFilter Filter { get; }

        public NodeTextFormatRule(NodeType applyToNodeType, TokenFilter filter)
        {
            ApplyToNodeType = applyToNodeType;
            Filter = filter;
        }

        public void TryApplyRule(Node node)
        {
            if (ApplyToNodeType == NodeType.ANY || ApplyToNodeType == node.NodeType)
            {
                node.NodeTokens = Filter.ApplyFilter(node.NodeTokens);
            }
        }
    }
}