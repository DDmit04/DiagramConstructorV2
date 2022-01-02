using System.Text.RegularExpressions;
using DiagramConstructorV3.app.builder.data;
using DiagramConstructorV3.app.parser.data;

namespace DiagramConstructorV3.app.threeController.structureController
{
    public class CppThreeStructureController : ThreeStructureController
    {
        public CppThreeStructureController()
        {
            UnnecessaryNodesRules.Add(IsUnImportantNode);
        }
        
        protected bool IsUnImportantNode(Node node)
        {
            return false;
        }
    }
}
