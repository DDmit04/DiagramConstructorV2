using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DiagramConstructorV3.app.builder.data;
using DiagramConstructorV3.app.parser.data;
using DiagramConstructorV3.app.tokenizer.data;

namespace DiagramConstructorV3.app.threeController.structureController
{
    public class ThreeStructureController
    {
        protected readonly List<Predicate<Node>> UnnecessaryNodesRules = new List<Predicate<Node>>(); 
        public List<Method> OptimizeStructure(List<Method> methodsToAnalyze)
        {
            foreach(var method in methodsToAnalyze)
            {
                CompareElseIfNodes(method.MethodNodes);
                CompareNodes(method.MethodNodes);
                FormatNodeTree(method.MethodNodes);
            }
            return methodsToAnalyze;
        }
        
        protected List<Node> CompareElseIfNodes(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.ChildNodes.Count > 0)
                {
                    node.ChildNodes = CompareElseIfNodes(node.ChildNodes);
                }
                if (node.ChildIfNodes.Count > 0)
                {
                    node.ChildIfNodes = CompareElseIfNodes(node.ChildIfNodes);
                }
                if (node.ChildElseNodes.Count > 0)
                {
                    node.ChildElseNodes = CompareElseIfNodes(node.ChildElseNodes);
                }
            }
            var index = nodes.FindLastIndex(nde => nde.NodeType == NodeType.ELSE || nde.NodeType == NodeType.ELSE_IF);
            while (index > 0)
            {
                var prevNode = nodes[index - 1];
                while (index > 0 && (prevNode.NodeType != NodeType.IF))
                {
                    if (prevNode.NodeType == NodeType.ELSE_IF)
                    {
                        prevNode.ChildElseNodes = nodes[index].ChildNodes;
                        nodes.RemoveAt(index);
                    }
                    else
                    {
                        throw new Exception();
                    }
                    index--;
                    prevNode = nodes[index - 1];
                }

                prevNode.ChildElseNodes = new List<Node>() { nodes[index] };
                nodes.RemoveAt(index);
                index = nodes.FindLastIndex(nde => nde.NodeType == NodeType.ELSE);
            }
            return nodes;
        }

        protected List<Node> CompareNodes(List<Node> nodesToAnalyze)
        {
            foreach (var node in nodesToAnalyze)
            {
                if (NodeBranchNeedsAnalyze(node.ChildNodes))
                {
                    node.ChildNodes = CompareNodes(node.ChildNodes);
                }
                if (NodeBranchNeedsAnalyze(node.ChildIfNodes))
                {
                    node.ChildIfNodes = CompareNodes(node.ChildIfNodes);
                }
                if (NodeBranchNeedsAnalyze(node.ChildElseNodes))
                {
                    node.ChildElseNodes = CompareNodes(node.ChildElseNodes);
                }
            }
            var pervNode = nodesToAnalyze[nodesToAnalyze.Count - 1];
            // nodesToAnalyze.Count - 2 !!!
            for (var i = nodesToAnalyze.Count - 2; i >= 0; i--)
            {
                var currentNode = nodesToAnalyze[i];
                var canCompare = CanCompareNodes(currentNode, pervNode);
                if (canCompare)
                {
                    pervNode.NodeTokens = CompareNodeTexts(currentNode, pervNode);
                    nodesToAnalyze.RemoveAt(i);
                }
                else
                {
                    pervNode = currentNode;
                }
            }
            return nodesToAnalyze;
        }

        protected void FormatNodeTree(List<Node> methodNodes)
        {
            foreach (var node in methodNodes)
            {
                if (NodeBranchNeedsAnalyze(node.ChildIfNodes))
                {
                    FormatNodeTree(node.ChildIfNodes);
                }
                if (NodeBranchNeedsAnalyze(node.ChildElseNodes))
                {
                    FormatNodeTree(node.ChildElseNodes);
                }
                if (NodeBranchNeedsAnalyze(node.ChildNodes))
                {
                    FormatNodeTree(node.ChildNodes);
                }
            }
            if (methodNodes.Count > 1)
            {
                methodNodes.RemoveAll(node => UnnecessaryNodesRules.Any(rule => rule(node)));
            }
        }
        
        protected bool NodeBranchNeedsAnalyze(List<Node> nodes)
        {
            return nodes.Count > 1 || (nodes.Count > 0 && nodes[0].NodeShapeForm != ShapeForm.IN_OUT_PUT);
        }

        protected bool CanCompareNodes(Node firstNode, Node secNode)
        {
            var bothNodesTextLength = firstNode.NodeText.Length + secNode.NodeText.Length;
            var bothNodesLineBreaksCount = CountLineBreaks(firstNode.NodeText) + 
                                           CountLineBreaks(secNode.NodeText);
            var lineBreaksCountIsOk = bothNodesLineBreaksCount < 5;

            var canCompareProcessNodes =
                firstNode.NodeType == NodeType.PROCESS
                && secNode.NodeType == NodeType.PROCESS
                && bothNodesTextLength < 50
                && lineBreaksCountIsOk;

            var canCompareProgramNodes =
                firstNode.NodeType == NodeType.PROGRAM
                && secNode.NodeType == NodeType.PROGRAM
                && bothNodesTextLength < 35
                && lineBreaksCountIsOk;

            var canCompareInPutNodes =
                firstNode.NodeType == NodeType.INPUT
                && secNode.NodeType == NodeType.INPUT
                && bothNodesTextLength < 50
                && lineBreaksCountIsOk;
            
            var canCompareOutPutNodes =
                firstNode.NodeType == NodeType.OUTPUT
                && secNode.NodeType == NodeType.OUTPUT
                && bothNodesTextLength < 50
                && lineBreaksCountIsOk;

            return canCompareProcessNodes || canCompareProgramNodes || canCompareOutPutNodes || canCompareInPutNodes;
        }

        protected List<Token> CompareNodeTexts(Node firstText, Node secText)
        {
            var res = new List<Token>();
            res.AddRange(firstText.NodeTokens);
            res.Add(new Token(TokenType.COMMA, ","));
            if (firstText.NodeText.Length > 15 || secText.NodeText.Length > 15)
            {
                res.Add(new Token(TokenType.LINE_END, "\n"));
            }
            res.AddRange(secText.NodeTokens);
            return res;
        }
        
        public static int CountLineBreaks(string text)
        {
            return new Regex("\n").Matches(text).Count;
        }
    }
}
