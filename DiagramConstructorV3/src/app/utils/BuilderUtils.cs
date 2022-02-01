using System;
using DiagramConstructorV3.app.builder.data;
using DiagramConstructorV3.app.parser.data;
using Microsoft.Office.Interop.Visio;

namespace DiagramConstructorV3.app.utils
{
    public class BuilderUtils
    {
        /// <summary>
        /// Define connection type between two shapes
        /// </summary>
        /// <param name="lastPlacedShape">shape from connection</param>
        /// <param name="oldShape">shape to connection (or parent shape)</param>
        /// <param name="nodesBranchRelation">shapes ('from shape' for 'to shape') relations</param>
        /// <returns>connection type</returns>
        public static ShapeConnectionType DefineConnectionType(ShapeWrapper lastPlacedShape, ShapeWrapper oldShape,
            NodesBranchRelation nodesBranchRelation = NodesBranchRelation.SAME_BRANCH)
        {
            if (nodesBranchRelation == NodesBranchRelation.SAME_BRANCH)
            {
                return ShapeConnectionType.FROM_TOP_TO_BOT;
            }
            else if (nodesBranchRelation == NodesBranchRelation.IF_BRANCH)
            {
                return ShapeConnectionType.FROM_TOP_TO_LEFT;
            }
            else if (nodesBranchRelation == NodesBranchRelation.ELSE_BRANCH)
            {
                return ShapeConnectionType.FROM_TOP_TO_RIGHT;
            }
            else if (nodesBranchRelation == NodesBranchRelation.OTHER_BRANCH)
            {
                if (oldShape.IsCommonShape)
                {
                    return ShapeConnectionType.FROM_TOP_TO_BOT;
                }
                return ShapeConnectionType.FROM_TOP_TO_RIGHT;
            }
            else if (nodesBranchRelation == NodesBranchRelation.PARENT)
            {
                if (!lastPlacedShape.IsCommonShape)
                {
                    if (oldShape.IsCommonShape)
                    {
                        return ShapeConnectionType.FROM_BOT_TO_LEFT;
                    }
                    else
                    {
                        return ShapeConnectionType.FROM_RIGHT_TO_LEFT;
                    }

                    throw new Exception("Parent shape can't be common!");
                }
                else if (lastPlacedShape.IsCommonShape &&
                         (!oldShape.IsCommonShape || oldShape.ShapeForm == ShapeForm.INVISIBLE_BLOCK))
                {
                    return ShapeConnectionType.FROM_BOT_TO_LEFT;
                }
            }

            return ShapeConnectionType.FROM_TOP_TO_BOT;
        }

        /// <summary>
        /// Out return cells align from given connection type
        /// </summary>
        /// <param name="connectionFromType">Out cell alignment for connection from shape</param>
        /// <param name="connectionToType">Out cell alignment for connection to shape</param>
        /// <param name="connectionType">type of shape connection</param>
        public static void GetCellsAlignsFromConnectionType(out VisCellIndices connectionFromType,
            out VisCellIndices connectionToType, ShapeConnectionType connectionType)
        {
            connectionToType = VisCellIndices.visAlignBottom;
            connectionFromType = VisCellIndices.visAlignTop;
            switch (connectionType)
            {
                case ShapeConnectionType.FROM_TOP_TO_BOT:
                    connectionFromType = VisCellIndices.visAlignTop;
                    connectionToType = VisCellIndices.visAlignBottom;
                    break;
                case ShapeConnectionType.FROM_TOP_TO_RIGHT:
                    connectionFromType = VisCellIndices.visAlignTop;
                    connectionToType = VisCellIndices.visAlignRight;
                    break;
                case ShapeConnectionType.FROM_RIGHT_TO_LEFT:
                    connectionFromType = VisCellIndices.visAlignRight;
                    connectionToType = VisCellIndices.visAlignLeft;
                    break;
                case ShapeConnectionType.FROM_TOP_TO_LEFT:
                    connectionFromType = VisCellIndices.visAlignTop;
                    connectionToType = VisCellIndices.visAlignLeft;
                    break;
                case ShapeConnectionType.FROM_LEFT_TO_LEFT:
                    connectionFromType = VisCellIndices.visAlignLeft;
                    connectionToType = VisCellIndices.visAlignLeft;
                    break;
                case ShapeConnectionType.FROM_BOT_TO_LEFT:
                    connectionFromType = VisCellIndices.visAlignBottom;
                    connectionToType = VisCellIndices.visAlignLeft;
                    break;
                default:
                    connectionToType = VisCellIndices.visAlignBottom;
                    connectionFromType = VisCellIndices.visAlignTop;
                    break;
            }
        }

        /// <summary>
        /// Returns max child branch length (num of nodes)
        /// </summary>
        /// <param name="node">parent node</param>
        /// <returns>max num of nodes</returns>
        public static double CalcThreeHeight(Node node)
        {
            if (node.IsSimpleNode)
            {
                return 1;
            }

            ShapeForm nodeShapeForm;
            double primaryBranchMaxLength = 0;
            double secondaryBranchMaxLength = 0;
            foreach (var childNode in node.PrimaryChildNodes)
            {
                primaryBranchMaxLength++;
                primaryBranchMaxLength += CalcThreeHeight(childNode);
                nodeShapeForm = childNode.NodeShapeForm;
                if (nodeShapeForm == ShapeForm.IF)
                {
                    primaryBranchMaxLength += 0.3;
                }
            }

            foreach (var childNode in node.SecondaryChildNodes)
            {
                secondaryBranchMaxLength++;
                secondaryBranchMaxLength += CalcThreeHeight(childNode);
                nodeShapeForm = childNode.NodeShapeForm;
                if (nodeShapeForm == ShapeForm.IF)
                {
                    secondaryBranchMaxLength += 0.3;
                }
            }
            var result = Math.Max(primaryBranchMaxLength, secondaryBranchMaxLength);
            return result;
        }
    }
}