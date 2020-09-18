using DiagramConsructorV2.src.actor.codeFormatter;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DiagramConstructor.actor.codeAnalyzer
{
    class CppCodeAnalyzer : CodeAnalyzer
    {
        public CppCodeAnalyzer(CodeFormatter codeFormatter) : base(codeFormatter){}

        protected override bool isUnimportantNode(Node node)
        {
            Regex unimportantOutput = new Regex(@"\s*(\'\s*\S*\s*\'|\s*)$");

            if (node.shapeForm == ShapeForm.IN_OUT_PUT 
                && unimportantOutput.IsMatch(node.nodeText) 
                && node.nodeText.IndexOf(',') == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}