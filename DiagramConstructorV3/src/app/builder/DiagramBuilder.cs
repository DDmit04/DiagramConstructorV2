using System.Collections.Generic;
using System.IO;
using System.Windows;
using DiagramConstructorV3.app.builder.data;
using DiagramConstructorV3.app.parser.data;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.builder
{
    public abstract class DiagramBuilder
    {
        protected VisioApi VisioManipulator;
        protected LoopNameGenerator LoopNameGenerator;

        protected const double StartX = 1.25;
        protected const double StartY = 10.75;
        
        protected const double MethodsXDist = 4.5;
        protected const double DefaultShapesYOffset = -1;
        protected const double InvisibleBlockYOffset = -0.3;

        public delegate BranchContext NodeBuildDelegate(Node nextShapeNode, BranchContext thisBranchContext);

        protected Dictionary<NodeType, NodeBuildDelegate> BuildRules = new Dictionary<NodeType, NodeBuildDelegate>();

        public DiagramBuilder()
        {
            LoopNameGenerator = new LoopNameGenerator();
            VisioManipulator = new VisioApi();
        }

        public string BuildDiagram(List<Method> allMethods, bool closeAfterBuild, string diagramFilepath)
        {
            try
            {
                VisioManipulator.OpenDocument();
            }
            catch
            {
                VisioManipulator.AbortOpenDocument();
                throw;
            }

            var currentPos = new Point(StartX, StartY);
            foreach (var method in allMethods)
            {
                if (method.MethodType == MethodType.COMMON)
                {
                    VisioManipulator.DropTextField(method.MethodSignature, currentPos);
                    currentPos.Offset(0, -0.4);
                }

                var initShape = VisioManipulator.DropSimpleShape("", new Point(currentPos.X, currentPos.Y - 0.25), ShapeForm.INIT_SHAPE);
                var treeContext = new BranchContext(null, initShape, initShape);
                foreach (var node in method.MethodNodes)
                {
                    treeContext = BuildTree(node, treeContext);
                    treeContext = new BranchContext(null, treeContext.LastBranchShape, treeContext.ShapeToContinueThree,
                        NodesBranchRelation.OTHER_BRANCH);
                }
                MoveCordsToNextMethod(currentPos);
            }

            string diagramFilename;
            if (closeAfterBuild)
            {
                diagramFilename = VisioManipulator.CloseDiagramDocument(diagramFilepath);
            }
            else
            {
                diagramFilename = VisioManipulator.SaveDiagramDocument(diagramFilepath);
            }

            return diagramFilename;
        }
        
        protected BranchContext BuildTree(Node node, BranchContext thisBranchContext)
        {
            BranchContext newBranchContext;
            if (BuildRules.ContainsKey(node.NodeType))
            {
                newBranchContext = BuildRules[node.NodeType].Invoke(node, thisBranchContext);
            }
            else
            {
                newBranchContext = BuildSimpleNode(node, thisBranchContext);
            }

            return newBranchContext;
        }

        protected BranchContext BuildSubTreeNodes(List<Node> nodes, BranchContext context)
        {
            var lastParent = context.BranchParent;
            var tmpContext = context;
            foreach (var node in nodes)
            {
                tmpContext = BuildTree(node, tmpContext);
            }
            tmpContext.ChangeBranchParent(lastParent);
            return tmpContext;
        }

        protected BranchContext BuildSimpleNode(Node nextShapeNode, BranchContext thisBranchContext)
        {
            var newBranchContext = DropNextShape(nextShapeNode, thisBranchContext);
            ConnectLastDroppedShape(newBranchContext);
            return newBranchContext;
        }
        
        protected BranchContext DropNextShape(Node node, BranchContext thisBranchContext)
        {
            var newShapePos = GetNextShapePos(thisBranchContext, node);
            if (thisBranchContext.BranchOffset != default)
            {
                newShapePos.Offset(thisBranchContext.BranchOffset.X, thisBranchContext.BranchOffset.Y);
                thisBranchContext.ResetOffset();
            }

            var lastDroppedShape = VisioManipulator.DropShape(node, newShapePos);
            thisBranchContext.SetLastShape(lastDroppedShape);
            return thisBranchContext;
        }
        
        protected void ConnectLastDroppedShape(BranchContext thisBranchContext)
        {
            var prevShape = thisBranchContext.ShapeToContinueThree;
            var lastDroppedShape = thisBranchContext.LastBranchShape;
            if (prevShape != null && lastDroppedShape != null)
            {
                VisioManipulator.ConnectShapes(lastDroppedShape, prevShape, thisBranchContext.BranchRelation);
                thisBranchContext.ContinueTreeWithShape(lastDroppedShape);
            }
        }

        protected void ConnectToParent(BranchContext branchContext)
        {
            var lastDroppedShape = branchContext.ShapeToContinueThree;
            if (branchContext.BranchParent != null && lastDroppedShape != null)
            {
                var branchParent = branchContext.BranchParent;
                VisioManipulator.ConnectShapes(lastDroppedShape, branchParent, NodesBranchRelation.PARENT);
            }
        }

        protected void MoveCordsToNextMethod(Point point)
        {
            point.Offset(MethodsXDist, 0);
        }

        protected Point GetNextShapePos(BranchContext context, Node newShapeNode = null)
        {
            var pos = context.BranchPos;
            var lastShape = context.LastBranchShape;
            var resOffsetPoint = new Point(0, 0);
            if (lastShape.ShapeForm != ShapeForm.INIT_SHAPE)
            {
                resOffsetPoint.Offset(0, DefaultShapesYOffset);
            }
            else if (lastShape.ShapeForm == ShapeForm.INVISIBLE_BLOCK)
            {
                resOffsetPoint.Offset(0, InvisibleBlockYOffset);
            }
            return new Point(pos.X + resOffsetPoint.X, pos.Y + resOffsetPoint.Y);
        }

        protected bool IsBranchContainsIf(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.NodeShapeForm == ShapeForm.IF)
                {
                    return true;
                }

                return IsBranchContainsIf(node.PrimaryChildNodes) ||
                       IsBranchContainsIf(node.SecondaryChildNodes);
            }

            return false;
        }
    }
}