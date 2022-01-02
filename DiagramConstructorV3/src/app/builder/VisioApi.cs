using System;
using System.Windows;
using DiagramConstructorV3.app.builder.data;
using DiagramConstructorV3.app.parser.data;
using DiagramConstructorV3.app.utils;
using Microsoft.Office.Interop.Visio;

namespace DiagramConstructorV3.app.builder
{
    public class VisioApi
    {

        protected Microsoft.Office.Interop.Visio.Application VisioApp;
        protected Document VisioStencil;

        protected Master Begin;
        protected Master Process;
        protected Master InOutPut;
        protected Master IfState;
        protected Master ForState;
        protected Master Program;
        protected Master Connector;
        protected Master PageConnector;
        protected Master Line;
        protected Master TextField;
        protected Master SmallTextField;
        protected Master Arrow;
        protected Master LittleInvisibleBlock;

        public void OpenDocument()
        {
            VisioApp = new Microsoft.Office.Interop.Visio.Application();
            VisioStencil = VisioApp.Documents.OpenEx(
               AppConfiguration.ShapesMastersFilePath,
               (short)VisOpenSaveArgs.visOpenDocked
            );
            Begin = VisioStencil.Masters.get_ItemU(@"Begin");
            Process = VisioStencil.Masters.get_ItemU(@"Process");
            InOutPut = VisioStencil.Masters.get_ItemU(@"inoutput");
            IfState = VisioStencil.Masters.get_ItemU(@"if");
            ForState = VisioStencil.Masters.get_ItemU(@"for");
            Program = VisioStencil.Masters.get_ItemU(@"program");
            Connector = VisioStencil.Masters.get_ItemU(@"Connector");
            PageConnector = VisioStencil.Masters.get_ItemU(@"pageConnector");
            Line = VisioStencil.Masters.get_ItemU(@"line");
            TextField = VisioStencil.Masters.get_ItemU(@"textField");
            SmallTextField = VisioStencil.Masters.get_ItemU(@"yesNo");
            Arrow = VisioStencil.Masters.get_ItemU(@"arrowRight");
            LittleInvisibleBlock = VisioStencil.Masters.get_ItemU(@"LittleInvisibleBlock");
            VisioApp.Documents.Add("");
        }

        public void AbortOpenDocument()
        {
            VisioApp.ActiveDocument.Close();
            VisioApp.Quit();
        }

        public string CloseDiagramDocument(string diagramFilePath)
        {
            var diagramFilename = SaveDiagramDocument(diagramFilePath);
            VisioStencil.Close();
            VisioApp.ActiveDocument.Close();
            VisioApp.Quit();
            return diagramFilename;
        }

        public string SaveDiagramDocument(string diagramFilePath)
        {
            var currentDate = DateTime.Now.ToString("dd-mm-yyyy__HH_mm_ss_fff");
            var resultFilePath = diagramFilePath + @"\result " + currentDate + AppConfiguration.DiagramFileExtension; 
            VisioApp.ActiveDocument.SaveAs(resultFilePath);
            return resultFilePath;
        }
        
        public ShapeWrapper DropInvisibleShape(Point point)
        {
            return DropSimpleShape("", point, ShapeForm.INVISIBLE_BLOCK);
        }
        
        public ShapeWrapper DropShape(Node node, Point shapePos)
        {
            if (node != null)
            {
                if (node.NoNeedText)
                {
                    return DropInvisibleShape(shapePos);
                }
                return DropSimpleShape(node.NodeText, shapePos, node.NodeShapeForm);
            }
            throw new NullReferenceException("Node to place shape can't be null!");

        }
        public ShapeWrapper DropSimpleShape(string text, Point point, ShapeForm shapeForm = ShapeForm.PROCESS)
        {
            var visioPage = VisioApp.ActivePage;
            var shapeMaster = GetShapeMasterByShapeForm(shapeForm);
            var droppedShape = visioPage.Drop(shapeMaster, point.X, point.Y);
            droppedShape.Text = text;
            return new ShapeWrapper(droppedShape, shapeForm, text, point);;
        }

        public void DropSmallTextField(string text, Point point)
        {
            DropTextField(text, point, ShapeForm.SMALL_TEXT_FIELD);
        }

        public void DropTextField(string text, Point point)
        {
            DropTextField(text, point, ShapeForm.TEXT_FIELD);
        }

        private void DropTextField(string text, Point point, ShapeForm textFieldType)
        {
            DropSimpleShape(text, point, textFieldType);
        }

        public void ConnectShapes(ShapeWrapper shapeFrom, ShapeWrapper shapeTo, NodesBranchRelation nodesBranchRelation = NodesBranchRelation.SAME_BRANCH)
        {
            var connectionType = BuilderUtils.DefineConnectionType(shapeFrom, shapeTo, nodesBranchRelation);
            var connectorMaster = GetConnectionMasterFromConnectionType(connectionType, nodesBranchRelation);
            BuilderUtils.GetCellsAlignsFromConnectionType(out var connectionFromType, out var connectionToType, connectionType);
            ConnectWithDynamicGlueAndConnector(shapeFrom, shapeTo, connectorMaster, connectionFromType, connectionToType);
        }
        protected Master GetConnectionMasterFromConnectionType(ShapeConnectionType connectionType, NodesBranchRelation nodesBranchRelation)
        {
            if (nodesBranchRelation == NodesBranchRelation.SAME_BRANCH
                || nodesBranchRelation == NodesBranchRelation.ELSE_BRANCH
                || nodesBranchRelation == NodesBranchRelation.IF_BRANCH)
            {
                return Line;
            } 
            else if (nodesBranchRelation == NodesBranchRelation.PARENT)
            {
                switch (connectionType)
                {
                    case ShapeConnectionType.FROM_RIGHT_TO_LEFT:
                    case ShapeConnectionType.FROM_BOT_TO_LEFT:
                    case ShapeConnectionType.FROM_LEFT_TO_LEFT:
                        return Arrow;
                    default:
                        return Line;
                }
            }
            switch (connectionType)
            {
                case ShapeConnectionType.FROM_TOP_TO_BOT:
                case ShapeConnectionType.FROM_TOP_TO_RIGHT:
                    return Line;
                case ShapeConnectionType.FROM_RIGHT_TO_LEFT:
                    return Arrow;
                default:
                    return Line;
            }
        }

        /// <summary>
        /// Get figure master from visio shape form
        /// </summary>
        /// <param name="shapeForm">form to get master</param>
        /// <returns>Shape master</returns>
        protected Master GetShapeMasterByShapeForm(ShapeForm shapeForm)
        {
            Master resultFigure = Begin;
            switch (shapeForm)
            {
                case ShapeForm.BEGIN_END:
                    resultFigure = Begin;
                    break;
                case ShapeForm.FOR:
                    resultFigure = ForState;
                    break;
                case ShapeForm.WHILE:
                case ShapeForm.DO_WHILE:
                case ShapeForm.IF:
                    resultFigure = IfState;
                    break;
                case ShapeForm.PROCESS:
                    resultFigure = Process;
                    break;
                case ShapeForm.PROGRAM:
                    resultFigure = Program;
                    break;
                case ShapeForm.ARROW_RIGHT:
                    resultFigure = Arrow;
                    break;
                case ShapeForm.LINE:
                    resultFigure = Line;
                    break;
                case ShapeForm.IN_OUT_PUT:
                    resultFigure = InOutPut;
                    break;
                case ShapeForm.CONNECTOR:
                    resultFigure = Connector;
                    break;
                case ShapeForm.PAGE_CONNECTOR:
                    resultFigure = PageConnector;
                    break;
                case ShapeForm.TEXT_FIELD:
                    resultFigure = TextField;
                    break;
                case ShapeForm.SMALL_TEXT_FIELD:
                    resultFigure = SmallTextField;
                    break;
                case ShapeForm.INVISIBLE_BLOCK:
                case ShapeForm.DO:
                    resultFigure = LittleInvisibleBlock;
                    break;
            }
            return resultFigure;
        }

        /// <summary>
        /// Actually connect two shapes using Visio API
        /// </summary>
        /// <param name="shapeFrom">first shape</param>
        /// <param name="shapeTo">sec shape</param>
        /// <param name="shapeToWrap"></param>
        /// <param name="connectorMaster">shape wich connect shapes</param>
        /// <param name="fromPoint">point to connect on first shape</param>
        /// <param name="toPoint">point to connect on sec shape</param>
        /// <param name="shapeFromWrap"></param>
        protected void ConnectWithDynamicGlueAndConnector(
            ShapeWrapper shapeFromWrap,
            ShapeWrapper shapeToWrap,
            Master connectorMaster,
            VisCellIndices fromPoint,
            VisCellIndices toPoint,
            double x = 10, double y = 10)
        {
            Shape shapeFrom = shapeFromWrap.Shape;
            Shape shapeTo = shapeToWrap.Shape;

            Page visioPage = VisioApp.ActivePage;
            var connector = visioPage.Drop(connectorMaster, x, y);

            var beginXCell = connector.get_CellsSRC(
                (short)VisSectionIndices.visSectionObject,
                (short)VisRowIndices.visRowXForm1D,
                (short)VisCellIndices.vis1DBeginX);

            beginXCell.GlueTo(shapeFrom.get_CellsSRC(
                (short)VisSectionIndices.visSectionObject,
                (short)VisRowIndices.visRowAlign,
                (short)fromPoint));

            var endXCell = connector.get_CellsSRC(
                (short)VisSectionIndices.visSectionObject,
                (short)VisRowIndices.visRowXForm1D,
                (short)VisCellIndices.vis1DEndX);

            endXCell.GlueTo(shapeTo.get_CellsSRC(
                (short)VisSectionIndices.visSectionObject,
                (short)VisRowIndices.visRowAlign,
                (short)toPoint));
        }

    }
}
