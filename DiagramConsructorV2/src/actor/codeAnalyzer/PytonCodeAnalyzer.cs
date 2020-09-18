using DiagramConsructorV2.src.actor.codeFormatter;
using DiagramConstructor.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiagramConstructor.actor.codeAnalyzer
{
    class PytonCodeAnalyzer : CodeAnalyzer
    {
        public PytonCodeAnalyzer(CodeFormatter codeFormatter) : base(codeFormatter)
        {
        }

        protected override bool isUnimportantNode(Node node)
        {
            Regex unimportantOutput = new Regex(@"\s*(\'\s*\S*\s*\')+\s*$");

            if (node.shapeForm == ShapeForm.IN_OUT_PUT
                && unimportantOutput.IsMatch(node.nodeText)
                && node.nodeText.IndexOf("f'") == -1
                && node.nodeText.IndexOf(",") == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override string formatNodeText(Node node)
        {
            string nodeCode = node.nodeText;
            nodeCode = base.formatNodeText(node);
            nodeCode = nodeCode.Replace("\n", "").Replace("{", "").Replace("}", "");
            return nodeCode;
        }

    }
}
