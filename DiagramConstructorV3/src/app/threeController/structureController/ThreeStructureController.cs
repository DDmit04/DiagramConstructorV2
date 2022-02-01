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
                AddBeginEndNodes(method.MethodNodes);
            }
            return methodsToAnalyze;
        }

        protected void AddBeginEndNodes(List<Node> nodes)
        {
            var beginNode = new Node(NodeType.BEGIN, "Начало");
            var endNode = new Node(NodeType.END, "Конец");
            nodes.Insert(0, beginNode);
            nodes.Add(endNode);
        }
        
        protected List<Node> CompareElseIfNodes(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.PrimaryChildNodes.Count > 0)
                {
                    node.PrimaryChildNodes = CompareElseIfNodes(node.PrimaryChildNodes);
                }
                if (node.SecondaryChildNodes.Count > 0)
                {
                    node.SecondaryChildNodes = CompareElseIfNodes(node.SecondaryChildNodes);
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
                        prevNode.SecondaryChildNodes = nodes[index].PrimaryChildNodes;
                        nodes.RemoveAt(index);
                    }
                    else
                    {
                        //TODO
                        throw new Exception();
                    }
                    index--;
                    prevNode = nodes[index - 1];
                }

                if (nodes[index].NodeType == NodeType.ELSE)
                {
                    prevNode.SecondaryChildNodes = nodes[index].PrimaryChildNodes;
                }
                else
                {
                    prevNode.SecondaryChildNodes = new List<Node>() { nodes[index] };
                }
                nodes.RemoveAt(index);
                index = nodes.FindLastIndex(nde => nde.NodeType == NodeType.ELSE);
            }
            return nodes;
        }

        protected List<Node> CompareNodes(List<Node> nodesToAnalyze)
        {
            foreach (var node in nodesToAnalyze)
            {
                if (NodeBranchNeedsAnalyze(node.PrimaryChildNodes))
                {
                    node.PrimaryChildNodes = CompareNodes(node.PrimaryChildNodes);
                }
                if (NodeBranchNeedsAnalyze(node.SecondaryChildNodes))
                {
                    node.SecondaryChildNodes = CompareNodes(node.SecondaryChildNodes);
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
                if (NodeBranchNeedsAnalyze(node.PrimaryChildNodes))
                {
                    FormatNodeTree(node.PrimaryChildNodes);
                }
                if (NodeBranchNeedsAnalyze(node.SecondaryChildNodes))
                {
                    FormatNodeTree(node.SecondaryChildNodes);
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
            res.Add(new Token(TokenType.COMMA, ",", -1));
            if (firstText.NodeText.Length > 15 || secText.NodeText.Length > 15)
            {
                res.Add(new Token(TokenType.LINE_END, "\n", -1));
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
