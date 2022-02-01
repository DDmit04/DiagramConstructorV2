using System.Windows;

namespace DiagramConstructorV3.app.builder.data
{
    public class BranchContext
    {
        public Point BranchPos => LastBranchShape.CurrentPos;

        public ShapeWrapper BranchParent { get; protected set; }

        public Point BranchOffset { get; protected set; }

        public ShapeWrapper LastBranchShape { get; protected set;}
        public ShapeWrapper ShapeToContinueThree { get; protected set;}
        public NodesBranchRelation BranchRelation { get; protected set; }
        public BranchContext(ShapeWrapper branchParent, ShapeWrapper lastBranchShape, ShapeWrapper shapeToContinueThree,
            NodesBranchRelation branchRelation = NodesBranchRelation.SAME_BRANCH, Point branchOffset = default)
        {
            BranchParent = branchParent;
            ShapeToContinueThree = shapeToContinueThree;
            BranchRelation = branchRelation;
            LastBranchShape = lastBranchShape;
            BranchOffset = branchOffset;
        }
        
        public BranchContext(ShapeWrapper branchParent,
            NodesBranchRelation branchRelation = NodesBranchRelation.SAME_BRANCH, Point branchOffset = default)
        {
            BranchParent = branchParent;
            ShapeToContinueThree = branchParent;
            BranchRelation = branchRelation;
            LastBranchShape = branchParent;
            BranchOffset = branchOffset;
        }

        public void ResetOffset()
        {
            BranchOffset = new Point(0, 0);
        }

        public void SetLastShape(ShapeWrapper shape)
        {
            LastBranchShape = shape;
        }
        public void ContinueTreeWithShape(ShapeWrapper shape)
        {
            ShapeToContinueThree = shape;
        }
        public void ChangeBranchParent(ShapeWrapper shape)
        {
            BranchParent = shape;
        }
    }
}