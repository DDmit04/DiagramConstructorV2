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
        public string NodeText { get; set; }
        public List<Node> PrimaryChildNodes { get; set; } = new List<Node>();
        public List<Node> SecondaryChildNodes { get; set; } = new List<Node>();

        public NodeType NodeType { get; set; }

        public ShapeForm NodeShapeForm
        {
            get => NodeType.MapToShapeForm();
        }

        public bool NoNeedText => NodeShapeForm == ShapeForm.INVISIBLE_BLOCK;

        public bool IsSimpleNode => NodeShapeForm == ShapeForm.PROCESS 
                                    || NodeShapeForm == ShapeForm.PROGRAM 
                                    || NodeShapeForm == ShapeForm.IN_OUT_PUT;

        public Node(NodeType nodeType, string nodeText, List<Token> nodeTokens)
        {
            NodeText = nodeText;
            NodeType = nodeType;
            NodeTokens = nodeTokens;
        }
        
        public Node(NodeType nodeType, string nodeText)
        {
            NodeText = nodeText;
            NodeType = nodeType;
            NodeTokens = new List<Token>();
        }
        
        public Node(NodeType nodeType, List<Token> nodeTokens)
        {
            NodeText = TokenUtils.TokensToString(NodeTokens);;
            NodeType = nodeType;
            NodeTokens = nodeTokens;
        }
        
        public Node(NodeType nodeType)
        {
            NodeType = nodeType;
        }
        
    }
}