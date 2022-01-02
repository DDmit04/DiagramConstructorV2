using System.Collections;
using System.Collections.Generic;
using DiagramConstructorV3.app.builder.data;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.parser.data
{
    public class Node
    {
        public List<Token> NodeTokens { get; set; } = new List<Token>();
        public string NodeText => TokenUtils.TokensToString(NodeTokens);
        public List<Node> ChildNodes { get; set; } = new List<Node>();
        public List<Node> ChildIfNodes { get; set; } = new List<Node>();
        public List<Node> ChildElseNodes { get; set; } = new List<Node>();

        public NodeType NodeType { get; set; }

        public ShapeForm NodeShapeForm
        {
            get => NodeType.MapToShapeForm();
        }

        public bool NoNeedText => NodeShapeForm == ShapeForm.INVISIBLE_BLOCK && NodeShapeForm == ShapeForm.DO;

        public bool IsSimpleNode => NodeShapeForm == ShapeForm.PROCESS 
                                    || NodeShapeForm == ShapeForm.PROGRAM 
                                    || NodeShapeForm == ShapeForm.IN_OUT_PUT;

        public Node(NodeType nodeType, List<Token> nodeTokens)
        {
            NodeType = nodeType;
            NodeTokens = nodeTokens;
        }
        
        public Node(NodeType nodeType)
        {
            NodeType = nodeType;
        }
        
    }
}