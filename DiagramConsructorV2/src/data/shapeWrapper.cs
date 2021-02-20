using DiagramConsructorV2.src.enumerated;
using Microsoft.Office.Interop.Visio;

namespace DiagramConsructorV2.src.data
{
    public class ShapeWrapper
    {
        public ShapeWrapper(Shape shape, ShapeForm shapeType)
        {
            this.shape = shape;
            this.shapeType = shapeType;
        }
        public Shape shape { get; }
        public ShapeForm shapeType {get;}

        public bool isCommonShape()
        {
            return this.shapeType == ShapeForm.PROCESS 
                || this.shapeType == ShapeForm.IN_OUT_PUT
                || this.shapeType == ShapeForm.PROGRAM 
                || this.shapeType == ShapeForm.BEGIN;
        }

    }
}
