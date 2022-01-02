using System.Collections.Generic;
using System.Windows;
using DiagramConstructorV3.app.builder.data;
using DiagramConstructorV3.app.parser.data;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.builder
{
    public class DiagramBuilder 
    {
        protected readonly VisioApi VisioManipulator;

        protected static readonly double START_X = 1.25;
        protected static readonly double START_Y = 10.75;

        protected Point StartPoint = new Point(START_X, START_Y);

        public DiagramBuilder()
        {
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

            var currentPos = StartPoint;
            foreach (var method in allMethods)
            {
                if (!method.MethodSignature.Equals("main()"))
                {
                    PlaceTextField(method.MethodSignature, currentPos);
                    currentPos.Offset(0, -0.4);
                }

                var treeContext = PlaceBeginShape(currentPos);
                foreach (var node in method.MethodNodes)
                {
                    treeContext = BuildTree(node, treeContext);
                    treeContext.BranchPos = default;
                    treeContext.BranchParent = null;
                    treeContext.BranchRelation = NodesBranchRelation.OTHER_BRANCH;
                }
                PlaceEndShape(treeContext);
                MoveCordsToNewMethod(currentPos);
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

        private BranchContext BuildTree(Node node, BranchContext thisBranchContext)
        {
            var prevShape = thisBranchContext.ShapeToContinueThree;
            var newShapePos = MoveToNextShapePos(thisBranchContext.LastBranchShape, node, thisBranchContext.BranchPos);
            var lastDroppedShape = VisioManipulator.DropShape(node, newShapePos);
            var newBranchContext = new BranchContext(lastDroppedShape, lastDroppedShape, lastDroppedShape);
            VisioManipulator.ConnectShapes(lastDroppedShape, prevShape, thisBranchContext.BranchRelation);
            if (!node.IsSimpleNode)
            {
                var nodeType = node.NodeType;
                if (nodeType == NodeType.DO_WHILE)
                {
                    var branchHeight = BuilderUtils.CalcThreeHeight(node);
                    var yesTextPoint = new Point(newShapePos.X - 0.7, newShapePos.Y - branchHeight + 0.2);
                    var noTextPoint = new Point(newShapePos.X + 0.3, newShapePos.Y - branchHeight - 0.4);
                    VisioManipulator.DropSmallTextField("Да", yesTextPoint);
                    VisioManipulator.DropSmallTextField("Нет", noTextPoint);
                    newBranchContext = BuildSubTree(node.ChildNodes, newBranchContext);
                    newShapePos = MoveToNextShapePos(newBranchContext.LastBranchShape);
                    var lastDoWhileShape = VisioManipulator.DropSimpleShape(node.NodeText, newShapePos, ShapeForm.DO_WHILE);
                    VisioManipulator.ConnectShapes(lastDoWhileShape, newBranchContext.ShapeToContinueThree);
                    newBranchContext = new BranchContext(lastDroppedShape, lastDoWhileShape, lastDoWhileShape);
                }
                else if (nodeType == NodeType.WHILE)
                {
                    var yesTextPoint = new Point(newShapePos.X + 0.28, newShapePos.Y - 0.7);
                    var noTextPoint = new Point(newShapePos.X + 0.7, newShapePos.Y + 0.2);
                    VisioManipulator.DropSmallTextField("Да", yesTextPoint);
                    VisioManipulator.DropSmallTextField("Нет", noTextPoint);
                    var invisibleShapePos = new Point(newShapePos.X, newShapePos.Y + 0.5);
                    var invisibleBlock = VisioManipulator.DropInvisibleShape(invisibleShapePos);
                    newBranchContext.BranchParent = invisibleBlock;
                    newBranchContext.BranchRelation = NodesBranchRelation.SAME_BRANCH;
                    var tmpContext = BuildSubTree(node.ChildNodes, newBranchContext);
                    newBranchContext.LastBranchShape = tmpContext.LastBranchShape;
                }
                else if (nodeType == NodeType.IF || nodeType == NodeType.ELSE_IF)
                {
                    var yesTextPoint = new Point(newShapePos.X - 0.7, newShapePos.Y + 0.2);
                    var noTextPoint = new Point(newShapePos.X + 0.7, newShapePos.Y + 0.2);
                    VisioManipulator.DropSmallTextField("Да", yesTextPoint);
                    VisioManipulator.DropSmallTextField("Нет", noTextPoint);

                    var branchHeight = BuilderUtils.CalcThreeHeight(node);
                    var invisibleBlockPos = new Point(newShapePos.X, newShapePos.Y - branchHeight);
                    var invisibleBlock = VisioManipulator.DropInvisibleShape(invisibleBlockPos);

                    var ifBranchContainsIf = IsBranchContainsIf(node.ChildNodes);
                    var ifBranchPos = new Point(newShapePos.X - 1.2, newShapePos.Y);
                    if (ifBranchContainsIf)
                    {
                        ifBranchPos.Offset(-1.5, 0);
                    }
                    var tmpContext = newBranchContext;
                    tmpContext.BranchRelation = NodesBranchRelation.IF_BRANCH;
                    tmpContext.BranchPos = ifBranchPos;
                    tmpContext = BuildSubTree(node.ChildNodes, tmpContext);
                    VisioManipulator.ConnectShapes(invisibleBlock, tmpContext.ShapeToContinueThree);

                    newBranchContext.BranchParent = null;
                    var elseBranchContainsIf = IsBranchContainsIf(node.ChildElseNodes);
                    var elseBranchPos = new Point(newShapePos.X + 1.2, newShapePos.Y);
                    if (elseBranchContainsIf)
                    {
                        elseBranchPos.Offset(1.5, 0);
                    }
                    tmpContext = newBranchContext;
                    tmpContext.BranchPos = elseBranchPos;
                    tmpContext.BranchRelation = NodesBranchRelation.ELSE_BRANCH;
                    tmpContext = BuildSubTree(node.ChildElseNodes, tmpContext);
                    VisioManipulator.ConnectShapes(invisibleBlock, tmpContext.ShapeToContinueThree);

                    newBranchContext = new BranchContext(thisBranchContext.BranchParent, invisibleBlock, invisibleBlock);
                }
                else
                {
                    var tmpContext = BuildSubTree(node.ChildNodes, newBranchContext);
                    newBranchContext.LastBranchShape = tmpContext.LastBranchShape;
                }

                if (newBranchContext.BranchParent != null)
                {
                    VisioManipulator.ConnectShapes(newBranchContext.LastBranchShape, newBranchContext.BranchParent, NodesBranchRelation.PARENT);
                }
            }
            return newBranchContext;
        }

        private BranchContext BuildSubTree(List<Node> nodes, BranchContext context)
        {
            var tmpContext = context;
            foreach (var node in nodes)
            {
                tmpContext = BuildTree(node, tmpContext);
            }
            return tmpContext;
        }

        private void PlaceEndShape(BranchContext context)
        {
            var lastShape = context.LastBranchShape;
            var endShapePos = new Point(lastShape.CurrentPos.X, lastShape.CurrentPos.Y);
            endShapePos.Offset(0, -0.7);
            var endShapeText = "Конец";
            var endShape = VisioManipulator.DropSimpleShape(endShapeText, endShapePos, ShapeForm.BEGIN_END);
            VisioManipulator.ConnectShapes(endShape, context.ShapeToContinueThree, NodesBranchRelation.OTHER_BRANCH);
        }

        private BranchContext PlaceBeginShape(Point beginShapePos)
        {
            var beginShapeText = "Начало";
            var beginShape = VisioManipulator.DropSimpleShape(beginShapeText, beginShapePos, ShapeForm.BEGIN_END);
            return new BranchContext(null, beginShape, beginShape);
        }

        private void PlaceTextField(string text, Point point)
        {
            VisioManipulator.DropTextField(text, point);
        }

        private void MoveCordsToNewMethod(Point point)
        {
            point.Offset(4.5, 0);
        }

        private static Point MoveToNextShapePos(ShapeWrapper lastShape, Node newShapeNode = null, Point pos = default)
        {
            if (pos == default)
            {
                pos = lastShape.CurrentPos;
            }
            var resOffsetPoint = GetNextShapeOffsetWithPrevShape(lastShape);
            if (newShapeNode != null)
            {
                var nextShapeOffsetPoint = GetNextShapeOffsetWithNextShape(newShapeNode);
                resOffsetPoint.Offset(nextShapeOffsetPoint.X, nextShapeOffsetPoint.Y);
            }
            return new Point(pos.X + resOffsetPoint.X,  pos.Y + resOffsetPoint.Y);
        }

        private static Point GetNextShapeOffsetWithPrevShape(ShapeWrapper lastShape)
        {
            var offsetPoint = new Point(0, 0);
            var lastShapeType = lastShape.ShapeForm;
            if (lastShapeType == ShapeForm.BEGIN_END)
            {
                offsetPoint.Offset(0, -0.75);
            }
            else if (lastShapeType == ShapeForm.IF)
            {
                offsetPoint.Offset(0, -0.8);
            }
            else if (lastShapeType == ShapeForm.INVISIBLE_BLOCK || lastShapeType == ShapeForm.DO)
            {
                offsetPoint.Offset(0, -0.5);
            }
            else
            {
                offsetPoint.Offset(0, -1);
            }
            return offsetPoint;
        }
        
        private static Point GetNextShapeOffsetWithNextShape(Node newShapeNode)
        {
            var offsetPoint = new Point(0, 0);
            var newShapeForm = newShapeNode.NodeShapeForm;
            if (newShapeForm == ShapeForm.INVISIBLE_BLOCK || newShapeForm == ShapeForm.DO)
            {
                offsetPoint.Offset(0, +0.5);
            }
            return offsetPoint;
        }

        private bool IsBranchContainsIf(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.NodeShapeForm == ShapeForm.IF)
                {
                    return true;
                }
                return IsBranchContainsIf(node.ChildNodes) || IsBranchContainsIf(node.ChildIfNodes) ||
                       IsBranchContainsIf(node.ChildElseNodes);
            }
            return false;
        }
    }
}