using DiagramConsructorV2.src.actor.codeFormatter;
using DiagramConsructorV2.src.data;
using DiagramConsructorV2.src.enumerated;
using System.Text.RegularExpressions;

namespace DiagramConstructorV2.src.nodeThreeAnylizer
{
    public class CppNodeThreeAnalyzer : NodeThreeAnalyzer
    {
        public CppNodeThreeAnalyzer() : base(new CppCodeFormatter()){}

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