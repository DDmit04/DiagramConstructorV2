using DiagramConsructorV2.src.actor.codeFormatter;
using DiagramConstructor.Config;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DiagramConstructor.actor
{
    abstract class CodeAnalyzer
    {

        protected CodeFormatter codeFormatter;

        public CodeAnalyzer(CodeFormatter codeFormatter)
        {
            this.codeFormatter = codeFormatter;
        }

        /// <summary>
        /// Modificate all method nodes
        /// </summary>
        /// <param name="methodsToAnylize">list of methods to modificate</param>
        /// <returns>list of modificated methods</returns>
        public virtual List<Method> analyzeMethods(List<Method> methodsToAnylize)
        {
            foreach(Method method in methodsToAnylize)
            {
                method.methodSignature = codeFormatter.formatMethodHead(method.methodSignature).Trim();
                formatNodeTree(method.methodNodes);
                compareNodes(method.methodNodes);
            }
            return methodsToAnylize;
        }

        /// <summary>
        /// Compare short text (PROCESS and IN_OUT_PUT) blocks
        /// </summary>
        /// <param name="nodesToAnylize">nodes for compare</param>
        /// <returns>list of compared nodes</returns>
        protected virtual List<Node> compareNodes(List<Node> nodesToAnylize)
        {
            foreach (Node node in nodesToAnylize)
            {
                if (nodeBranchNeedAnylize(node.childNodes))
                {
                    node.childNodes = compareNodes(node.childNodes);
                }
                if (nodeBranchNeedAnylize(node.childIfNodes))
                {
                    node.childIfNodes = compareNodes(node.childIfNodes);
                }
                if (nodeBranchNeedAnylize(node.childElseNodes))
                {
                    node.childElseNodes = compareNodes(node.childElseNodes);
                }
            }
            Node pervNode = nodesToAnylize[nodesToAnylize.Count - 1];
            // nodesToAnylize.Count - 2 !!!
            for (int i = nodesToAnylize.Count - 2; i >= 0; i--)
            {
                Node currentNode = nodesToAnylize[i];
                if (canCompareNodes(currentNode, pervNode))
                {
                    pervNode.nodeText = compareNodeTexsts(currentNode.nodeText, pervNode.nodeText);
                    nodesToAnylize.RemoveAt(i);
                }
                else
                {
                    pervNode = currentNode;
                }
            }
            return nodesToAnylize;
        }

        protected virtual void formatNodeTree(List<Node> methodNodes)
        {
            foreach (Node node in methodNodes)
            {
                if (node.childIfNodes.Count != 0)
                {
                    formatNodeTree(node.childIfNodes);
                }
                if (node.childElseNodes.Count != 0)
                {
                    formatNodeTree(node.childElseNodes);
                }
                if (node.childNodes.Count != 0)
                {
                    formatNodeTree(node.childNodes);
                }
                node.nodeText = formatNodeText(node);
            }
            if (methodNodes.Count > 1)
            {
                methodNodes.RemoveAll(node => isUnimportantNode(node));
            }
        }

        /// Check is console output just constant string
        /// </summary>
        /// <param name="node">node to anylize</param>
        /// <returns>is console output just constant string</returns>
        protected abstract bool isUnimportantNode(Node node);

        /// <summary>
        /// Check is node branch need to be anylized
        /// </summary>
        /// <param name="nodes">nodes to check</param>
        /// <returns>is neccesary to analyze node branch</returns>
        protected bool nodeBranchNeedAnylize(List<Node> nodes)
        {
            return nodes.Count > 1 || (nodes.Count > 0 && nodes[0].shapeForm != ShapeForm.IN_OUT_PUT);
        }

        /// <summary>
        /// Gets operator text from code line
        /// </summary>
        /// <param name="node">node to extract text</param>
        /// <returns>extracted operator text</returns>
        protected virtual string formatNodeText(Node node)
        {
            ShapeForm nodeType = node.shapeForm;
            string nodeCode = node.nodeText;
            if (nodeType == ShapeForm.IF)
            {
                nodeCode = this.codeFormatter.formatIf(nodeCode);
            }
            else if (nodeType == ShapeForm.FOR)
            {
                nodeCode = this.codeFormatter.formatFor(nodeCode);
            }
            else if (nodeType == ShapeForm.WHILE)
            {
                nodeCode = this.codeFormatter.formatWhile(nodeCode);
            }
            else if(nodeType == ShapeForm.IN_OUT_PUT)
            {
                nodeCode = this.codeFormatter.formatInOutPut(nodeCode);
            }
            return nodeCode;
        }

        protected virtual bool canCompareNodes(Node firstNode, Node secNode)
        {
            int bothNodesTextLength = firstNode.nodeText.Length + secNode.nodeText.Length;
            int bothNodesLineBrakersCount = getLineBraksCount(firstNode.nodeText) + getLineBraksCount(secNode.nodeText);
            bool lineBreaksCountIsOk = bothNodesLineBrakersCount < 5;

            bool canCompareProcessNodes =
                firstNode.shapeForm == ShapeForm.PROCESS
                && secNode.shapeForm == ShapeForm.PROCESS
                && bothNodesTextLength < 50
                && lineBreaksCountIsOk;

            bool canCompareProgramNodes =
                firstNode.shapeForm == ShapeForm.PROGRAM
                && secNode.shapeForm == ShapeForm.PROGRAM
                && bothNodesTextLength < 35
                && lineBreaksCountIsOk;

            bool canCompareInOutPutNodes =
                firstNode.shapeForm == ShapeForm.IN_OUT_PUT
                && secNode.shapeForm == ShapeForm.IN_OUT_PUT
                && bothNodesTextLength < 50
                && lineBreaksCountIsOk;

            return canCompareProcessNodes || canCompareProgramNodes || canCompareInOutPutNodes;
        }

        protected virtual string compareNodeTexsts(string firstText, string secText)
        {
            string result = "";
            if (firstText.Length > 15 || secText.Length > 15)
            {
                result = firstText + ",\n" + secText;
            }
            else
            {
                result = firstText + ", " + secText;
            }
            return result;
        }

        private int getLineBraksCount(string text)
        {
            return new Regex("\n").Matches(text).Count;
        }

    }
}
