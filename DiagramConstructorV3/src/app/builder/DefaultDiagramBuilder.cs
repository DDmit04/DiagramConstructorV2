using System.Collections.Generic;
using System.Windows;
using DiagramConstructorV3.app.builder.data;
using DiagramConstructorV3.app.parser.data;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.builder
{
    public class DefaultDiagramBuilder : DiagramBuilder
    {
        public DefaultDiagramBuilder()
        {
            BuildRules = new Dictionary<NodeType, NodeBuildDelegate>()
            {
                { NodeType.IF, BuildIf },
                { NodeType.ELSE, BuildIf },
                { NodeType.ELSE_IF, BuildIf },
                { NodeType.FOR, BuildFor },
                { NodeType.WHILE, BuildWhile },
                { NodeType.DO_WHILE, BuildDoWhile }
            };
        }

        public BranchContext BuildDoWhile(Node nextShapeNode, BranchContext thisBranchContext)
        {
            var nextLoopName = LoopNameGenerator.GetNextName();
            return BuildAnyWhileLoop(nextShapeNode, thisBranchContext, nextLoopName, 
                $"{nextShapeNode.NodeText}\n{nextLoopName}");
        }

        public BranchContext BuildWhile(Node nextShapeNode, BranchContext thisBranchContext)
        {
            var nextLoopName = LoopNameGenerator.GetNextName();
            return BuildAnyWhileLoop(nextShapeNode, thisBranchContext, $"{nextShapeNode.NodeText}\n{nextLoopName}",
                nextLoopName);
        }

        protected BranchContext BuildAnyWhileLoop(Node whileNode, BranchContext thisBranchContext, string firstBlockText, string lastBlockText)
        {
            var newShapePos = GetNextShapePos(thisBranchContext);
            var firstLoopShape = VisioManipulator.DropSimpleShape(firstBlockText, newShapePos, ShapeForm.LOOP_START);
            VisioManipulator.ConnectShapes(firstLoopShape, thisBranchContext.ShapeToContinueThree, thisBranchContext.BranchRelation);
            var newBranchContext = new BranchContext(thisBranchContext.BranchParent, firstLoopShape, firstLoopShape);
            
            newBranchContext = BuildSubTreeNodes(whileNode.PrimaryChildNodes, newBranchContext);
            
            newShapePos = GetNextShapePos(newBranchContext);
            var lastLoopShape = VisioManipulator.DropSimpleShape(lastBlockText, newShapePos, ShapeForm.LOOP_END);
            VisioManipulator.ConnectShapes(lastLoopShape, newBranchContext.ShapeToContinueThree, newBranchContext.BranchRelation);
            newBranchContext = new BranchContext(thisBranchContext.BranchParent, lastLoopShape, lastLoopShape);
            
            ConnectToParent(newBranchContext);
            return newBranchContext;
        }

        public BranchContext BuildIf(Node nextShapeNode, BranchContext thisBranchContext)
        {
            var newBranchContext = BuildSimpleNode(nextShapeNode, thisBranchContext);
            var ifShape = newBranchContext.LastBranchShape;
            var ifShapePos = newBranchContext.BranchPos;
            var yesTextPoint = new Point(ifShapePos.X - 0.7, ifShapePos.Y + 0.2);
            var noTextPoint = new Point(ifShapePos.X + 0.7, ifShapePos.Y + 0.2);
            VisioManipulator.DropSmallTextField("Да", yesTextPoint);
            VisioManipulator.DropSmallTextField("Нет", noTextPoint);

            var branchHeight = BuilderUtils.CalcThreeHeight(nextShapeNode);
            var invisibleBlockPos = new Point(ifShapePos.X, ifShapePos.Y - branchHeight);
            var invisibleBlock = VisioManipulator.DropInvisibleShape(invisibleBlockPos);

            var ifBranchContainsIf = IsBranchContainsIf(nextShapeNode.PrimaryChildNodes);
            var ifBranchOffset = new Point(-1.2, 0);
            if (ifBranchContainsIf)
            {
                ifBranchOffset.Offset(-1.5, 0);
            }

            var ifBranchContext = new BranchContext(null, ifShape, ifShape, 
                NodesBranchRelation.IF_BRANCH, ifBranchOffset);
            ifBranchContext = BuildSubTreeNodes(nextShapeNode.PrimaryChildNodes, ifBranchContext);
            VisioManipulator.ConnectShapes(invisibleBlock, ifBranchContext.ShapeToContinueThree);

            var elseBranchContainsIf = IsBranchContainsIf(nextShapeNode.SecondaryChildNodes);
            var elseBranchOffset = new Point(1.2, 0);
            if (elseBranchContainsIf)
            {
                elseBranchOffset.Offset(1.5, 0);
            }

            var elseBranchContext = new BranchContext(null, ifShape, ifShape, 
                NodesBranchRelation.ELSE_BRANCH, elseBranchOffset);
            elseBranchContext = BuildSubTreeNodes(nextShapeNode.SecondaryChildNodes, elseBranchContext);
            VisioManipulator.ConnectShapes(invisibleBlock, elseBranchContext.ShapeToContinueThree);

            newBranchContext = new BranchContext(thisBranchContext.BranchParent, invisibleBlock, invisibleBlock);
            ConnectToParent(thisBranchContext);
            return newBranchContext;
        }

        public BranchContext BuildFor(Node nextShapeNode, BranchContext thisBranchContext)
        {
            var newShapePos = GetNextShapePos(thisBranchContext);
            var firstLoopShape = VisioManipulator.DropSimpleShape(nextShapeNode.NodeText, newShapePos, ShapeForm.FOR);
            VisioManipulator.ConnectShapes(firstLoopShape, thisBranchContext.ShapeToContinueThree, thisBranchContext.BranchRelation);
            var newBranchContext = new BranchContext(firstLoopShape);
            newBranchContext = BuildSubTreeNodes(nextShapeNode.PrimaryChildNodes, newBranchContext);
            ConnectToParent(newBranchContext);
            newBranchContext.ContinueTreeWithShape(firstLoopShape);
            return newBranchContext;
        }
    }
}