using System.Windows;

namespace DiagramConstructorV3.app.builder.data
{
    public class BranchContext
    {
        private Point _branchPos;
        public ShapeWrapper BranchParent { get; set; }

        public Point BranchPos
        {
            get => GetBranchPos();
            set => _branchPos = value;
        }

        public ShapeWrapper LastBranchShape { get; set; }
        public ShapeWrapper ShapeToContinueThree { get; set; }
        public NodesBranchRelation BranchRelation { get; set; }
        public BranchContext(ShapeWrapper branchParent, ShapeWrapper lastBranchShape, ShapeWrapper shapeToContinueThree,
            NodesBranchRelation branchRelation = NodesBranchRelation.SAME_BRANCH, Point branchPos = default)
        {
            BranchParent = branchParent;
            _branchPos = branchPos;
            ShapeToContinueThree = shapeToContinueThree;
            BranchRelation = branchRelation;
            LastBranchShape = lastBranchShape;
        }

        public Point GetBranchPos()
        {
            if (_branchPos == default)
            {
                _branchPos = LastBranchShape.CurrentPos;
            }

            return _branchPos;
        }
    }
}