﻿using DiagramConstructorV3.app.builder.data;

namespace DiagramConstructorV3.app.parser.data
{
    public enum NodeType
    {
        IF,
        ELSE,
        ELSE_IF,
        FOR,
        WHILE,
        DO_WHILE,
        INPUT,
        OUTPUT,
        SWITCH,
        CASE,
        SWITCH_DEFAULT,
        PROGRAM,
        PROCESS,
        ANY
    }
    static class NodeTypeMethods
    {
        public static ShapeForm MapToShapeForm(this NodeType type)
        {
            switch (type)
            {
                case NodeType.IF:
                case NodeType.ELSE_IF:
                case NodeType.ELSE:
                case NodeType.SWITCH:
                    return ShapeForm.IF;
                case NodeType.FOR:
                    return ShapeForm.FOR;
                case NodeType.WHILE:
                    return ShapeForm.WHILE;
                case NodeType.DO_WHILE:
                    return ShapeForm.DO;
                case NodeType.INPUT:
                case NodeType.OUTPUT:
                    return ShapeForm.IN_OUT_PUT;
                case NodeType.PROGRAM:
                    return ShapeForm.PROGRAM;
                default:
                    return ShapeForm.PROCESS;
            }
        }
    }
    
}