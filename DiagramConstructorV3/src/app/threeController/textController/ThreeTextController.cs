using System.Collections.Generic;
using DiagramConstructorV3.app.parser.data;

namespace DiagramConstructorV3.app.threeController.textController
{
    public class ThreeTextController
    {
        protected readonly List<NodeTextFormatRule> FormatRules = new List<NodeTextFormatRule>();

        public List<Method> ApplyTextRules(List<Method> methods)
        {
            foreach (var method in methods)
            {
                ApplyTextRules(method.MethodNodes);
            }

            return methods;
        }

        public void ApplyTextRules(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if(node.ChildNodes.Count != 0)
                {
                    ApplyTextRules(node.ChildNodes);
                }
                if(node.ChildElseNodes.Count != 0)
                {
                    ApplyTextRules(node.ChildElseNodes);
                }
                if(node.ChildIfNodes.Count != 0)
                {
                    ApplyTextRules(node.ChildIfNodes);
                }
                foreach (var rule in FormatRules)
                {
                    rule.TryApplyRule(node);
                }
            }
        }
    }
}