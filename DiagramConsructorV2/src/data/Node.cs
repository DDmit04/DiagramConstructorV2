using DiagramConsructorV2.src.enumerated;
using System;
using System.Collections.Generic;

namespace DiagramConsructorV2.src.data
{
    public class Node
    {
        public ShapeForm shapeForm { get; set; } = ShapeForm.PROCESS;
        public String nodeText { get; set; } = "";
        public List<Node> childNodes { get; set; } = new List<Node>();
        public List<Node> childIfNodes { get; set; } = new List<Node>();
        public List<Node> childElseNodes { get; set; } = new List<Node>();

        public Node(){}

        public Node(String nodeText, ShapeForm shapeForm)
        {
            this.nodeText = nodeText;
            this.shapeForm = shapeForm;
        }

        public bool isSimpleNode()
        {
            return shapeForm == ShapeForm.PROCESS || shapeForm == ShapeForm.PROGRAM || shapeForm == ShapeForm.IN_OUT_PUT;
        }

        public bool isNoChileNodes()
        {
            return childNodes.Count == 0 && childIfNodes.Count == 0 && childElseNodes.Count == 0;
        }

    }
}
