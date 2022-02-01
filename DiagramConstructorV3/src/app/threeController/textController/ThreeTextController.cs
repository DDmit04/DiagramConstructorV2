using System.Collections.Generic;
using DiagramConstructorV3.app.parser.data;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.threeController.textController
{
    public class ThreeTextController
    {
        protected readonly List<NodeTokensFormatRule> FormatRules = new List<NodeTokensFormatRule>();

        public List<Method> ApplyTextRules(List<Method> methods)
        {
            foreach (var method in methods)
            {
                ApplyTextRules(method.MethodNodes);
            }
            foreach (var method in methods)
            {
                SetNodeTexts(method.MethodNodes);
            }

            return methods;
        }

        public void ApplyTextRules(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if(node.SecondaryChildNodes.Count != 0)
                {
                    ApplyTextRules(node.SecondaryChildNodes);
                }
                if(node.PrimaryChildNodes.Count != 0)
                {
                    ApplyTextRules(node.PrimaryChildNodes);
                }
                foreach (var rule in FormatRules)
                {
                    rule.TryApplyRule(node);
                }
            }
        }
        
        public void SetNodeTexts(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if(node.SecondaryChildNodes.Count != 0)
                {
                    ApplyTextRules(node.SecondaryChildNodes);
                }
                if(node.PrimaryChildNodes.Count != 0)
                {
                    ApplyTextRules(node.PrimaryChildNodes);
                }

                node.NodeText = TokenUtils.TokensToString(node.NodeTokens);
            }
        }
    }
}