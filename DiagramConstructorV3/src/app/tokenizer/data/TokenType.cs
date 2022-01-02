namespace DiagramConstructorV3.app.tokenizer.data
{
    public enum TokenType
    {
        IF_OPERATOR,
        ELSE_OPERATOR,
        ELSE_IF_OPERATOR,
        FOR_OPERATOR,
        WHILE_OPERATOR,
        DO_WHILE_OPERATOR,
        INPUT_OPERATOR,
        OUTPUT_OPERATOR,
        BREAK_OPERATOR,
        CONTINUE_OPERATOR,
        SWITCH_OPERATOR,
        CASE_OPERATOR,
        SWITCH_DEFAULT_OPERATOR,
        THIS_OPERATOR,
        TRY_OPERATOR,
        CATCH_OPERATOR,
        FINALLY_OPERATOR,
        THROW_OPERATOR,
        CLASS_OPERATOR,
        RETURN_OPERATOR,
        
        ACCESS_MODIFICATOR,
        METHOD_DEF,
        
        IDENTIFIER,
        MATH_ACTION,
        EQUAL_ACTION,
        SQRT,
        ABS,
        POW,
        NUMBER,
        LANG_SPECIFIC,

        LOGIC_AND,
        LOGIC_OR,
        
        DOT,
        DOUBLE_DOT,
        SHARP,
        COMMA,
        LINE_END,
        TAB,
        NEW_LINE,
        ESCAPED_DOUBLE_QUOTE,
        ESCAPED_SINGLE_QUOTE,
        DOUBLE_QUOTE,
        SINGLE_QUOTE,
        ESCAPE_CHAR,
        
        ARRAY_OPEN,
        ARRAY_CLOSE,
        ARGS_OPEN,
        ARGS_CLOSE,
        BRACKET_OPEN,
        BRACKET_CLOSE,

        TEXT,
        
        RESERVE_1,
        RESERVE_2,
        RESERVE_3,
        RESERVE_4,
        RESERVE_5,
        
        UNKNOWN,
        ANY
    }
    
    static class TokenTypeMethods
    {
        public static bool IsOperatorToken(this TokenType type)
        {
            return type.ToString().EndsWith("_OPERATOR");
        }
    }
}