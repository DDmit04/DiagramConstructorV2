using System.Windows;
using Microsoft.Office.Interop.Visio;

namespace DiagramConstructorV3.app.builder.data
{
    public class ShapeWrapper
    {
        public Shape Shape { get; }
        public string ShapeText { get; }
        public ShapeForm ShapeForm { get; }
        public Point CurrentPos { get; }

        public bool IsCommonShape =>
            ShapeForm == ShapeForm.PROCESS
            || ShapeForm == ShapeForm.IN_OUT_PUT
            || ShapeForm == ShapeForm.PROGRAM
            || ShapeForm == ShapeForm.BEGIN_END
            || ShapeForm == ShapeForm.LOOP_END
            || ShapeForm == ShapeForm.LOOP_START
            || ShapeForm == ShapeForm.INIT_SHAPE;

        public ShapeWrapper(Shape shape, ShapeForm shapeForm, string shapeText, Point currentPos)
        {
            Shape = shape;
            ShapeText = shapeText;
            ShapeForm = shapeForm;
            CurrentPos = currentPos;
        }
    }
}
