using System;
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

        public string NodeText
        {
            get
            {
                if (string.IsNullOrEmpty(_NodeText))
                {
                    return TokenUtils.TokensToString(NodeTokens);
                }
                return _NodeText;
            }
        }

        public List<Node> PrimaryChildNodes { get; set; } = new List<Node>();
        public List<Node> SecondaryChildNodes { get; set; } = new List<Node>();

        public NodeType NodeType { get; }

        public ShapeForm NodeShapeForm => NodeType.MapToShapeForm();

        private string _NodeText = "";

        public bool NoNeedText => NodeShapeForm == ShapeForm.INVISIBLE_BLOCK;

        public bool IsSimpleNode => NodeShapeForm == ShapeForm.PROCESS 
                                    || NodeShapeForm == ShapeForm.PROGRAM 
                                    || NodeShapeForm == ShapeForm.IN_OUT_PUT;

        public Node(NodeType nodeType, string nodeText, List<Token> nodeTokens)
        {
            _NodeText = nodeText;
            NodeType = nodeType;
            NodeTokens = nodeTokens;
        }
        
        public Node(NodeType nodeType, string nodeText)
        {
            _NodeText = nodeText;
            NodeType = nodeType;
            NodeTokens = new List<Token>();
        }
        
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