using System.Text.RegularExpressions;
using DiagramConstructorV3.app.tokenizer.data;

namespace DiagramConstructorV3.app.tokenizer
{
    public class CppCodeTokenizer : CodeTokenizer
    {
        public CppCodeTokenizer()
        {
            LexRules.Add(new LexRule(TokenType.IF_OPERATOR, new Regex("if")));
            LexRules.Add(new LexRule(TokenType.ELSE_OPERATOR, new Regex("else")));
            LexRules.Add(new LexRule(TokenType.FOR_OPERATOR, new Regex("for")));
            LexRules.Add(new LexRule(TokenType.WHILE_OPERATOR, new Regex("while")));
            LexRules.Add(new LexRule(TokenType.DO_WHILE_OPERATOR, new Regex("do")));
            LexRules.Add(new LexRule(TokenType.INPUT_OPERATOR, new Regex("cin")));
            LexRules.Add(new LexRule(TokenType.OUTPUT_OPERATOR, new Regex("cout")));
            LexRules.Add(new LexRule(TokenType.SWITCH_OPERATOR, new Regex("switch")));
            LexRules.Add(new LexRule(TokenType.CASE_OPERATOR, new Regex("case")));
            LexRules.Add(new LexRule(TokenType.SWITCH_DEFAULT_OPERATOR, new Regex("default")));
            LexRules.Add(new LexRule(TokenType.BREAK_OPERATOR, new Regex("break")));
            LexRules.Add(new LexRule(TokenType.CONTINUE_OPERATOR, new Regex("continue")));
            LexRules.Add(new LexRule(TokenType.THIS_OPERATOR, new Regex("this")));
            LexRules.Add(new LexRule(TokenType.TRY_OPERATOR, new Regex("try")));
            LexRules.Add(new LexRule(TokenType.CATCH_OPERATOR, new Regex("catch")));
            LexRules.Add(new LexRule(TokenType.THROW_OPERATOR, new Regex("throw")));
            LexRules.Add(new LexRule(TokenType.CLASS_OPERATOR, new Regex("class")));
            LexRules.Add(new LexRule(TokenType.SQRT, new Regex("sqrt")));
            LexRules.Add(new LexRule(TokenType.ABS, new Regex("abs")));
            LexRules.Add(new LexRule(TokenType.POW, new Regex("pow")));
            LexRules.Add(new LexRule(TokenType.RETURN_OPERATOR, new Regex("return")));

            LexRules.Add(new LexRule(TokenType.ACCESS_MODIFICATOR, new Regex("public")));
            LexRules.Add(new LexRule(TokenType.ACCESS_MODIFICATOR, new Regex("private")));
            LexRules.Add(new LexRule(TokenType.ACCESS_MODIFICATOR, new Regex("protected")));

            LexRules.Add(new LexRule(TokenType.ARGS_OPEN, new Regex("\\(")));
            LexRules.Add(new LexRule(TokenType.ARGS_CLOSE, new Regex("\\)")));
            LexRules.Add(new LexRule(TokenType.BRACKET_OPEN, new Regex("{")));
            LexRules.Add(new LexRule(TokenType.BRACKET_CLOSE, new Regex("}")));
            LexRules.Add(new LexRule(TokenType.ARRAY_OPEN, new Regex("\\[")));
            LexRules.Add(new LexRule(TokenType.ARRAY_CLOSE, new Regex("\\]")));

            LexRules.Add(new LexRule(TokenType.LINE_END, new Regex(";")));
            LexRules.Add(new LexRule(TokenType.DOT, new Regex("\\.")));
            LexRules.Add(new LexRule(TokenType.DOT, new Regex("->")));
            LexRules.Add(new LexRule(TokenType.DOUBLE_DOT, new Regex(":")));
            
            LexRules.Add(new LexRule(TokenType.LOGIC_OR, new Regex("\\|\\||\\|")));
            LexRules.Add(new LexRule(TokenType.LOGIC_AND, new Regex("&&|&")));
            LexRules.Add(new LexRule(TokenType.LOGIC_OR, new Regex("or")));
            LexRules.Add(new LexRule(TokenType.LOGIC_AND, new Regex("and")));
            
            LexRules.Add(new LexRule(TokenType.COMMA, new Regex(",")));
            LexRules.Add(new LexRule(TokenType.SHARP, new Regex("#")));
            LexRules.Add(new LexRule(TokenType.ESCAPE_CHAR, new Regex(@"\\")));
            
            LexRules.Add(new LexRule(TokenType.NEW_LINE, new Regex(@"\n")));
            LexRules.Add(new LexRule(TokenType.TABULATION, new Regex(@"\t")));
            LexRules.Add(new LexRule(TokenType.CARRIAGE_RETURN, new Regex(@"\r")));

            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("<<")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("«")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex(">>")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("»")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("\\*|\\*\\*")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("int")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("double")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("float")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("string")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("String")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("char")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("long")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("short")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("void")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("new")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("const")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("/")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("typedef")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("auto")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("static")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("signed")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("unsigned")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("virtual")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("volatile")));
            LexRules.Add(new LexRule(TokenType.LANG_SPECIFIC, new Regex("endl")));

            LexRules.Add(new LexRule(TokenType.RESERVE_1, new Regex("struct")));
            LexRules.Add(new LexRule(TokenType.RESERVE_2, new Regex("malloc")));
            LexRules.Add(new LexRule(TokenType.RESERVE_3, new Regex("free")));
            LexRules.Add(new LexRule(TokenType.RESERVE_4, new Regex("sizeof")));
            LexRules.Add(new LexRule(TokenType.RESERVE_5, new Regex("goto")));
            
            LexRules.Add(new LexRule(TokenType.TEXT, new Regex("\\!|\\?|\\$")));

            LexRules.Add(new LexRule(TokenType.NUMBER, new Regex("[0-9]+")));
            LexRules.Add(new LexRule(TokenType.IDENTIFIER, new Regex("\\w+|\\d+")));
            LexRules.Add(new LexRule(TokenType.EQUAL_ACTION, new Regex("!=|<=|>=|<>|<|>|=")));
            LexRules.Add(new LexRule(TokenType.MATH_ACTION, new Regex("\\+=|-=|\\*=|/=|%=|/%|\\+\\+|--|\\+|-|\\*")));
            
            LexRules.Add(new LexRule(TokenType.NEW_LINE, new Regex("\r\n")));
            LexRules.Add(new LexRule(TokenType.SINGLE_QUOTE, new Regex("'")));
            LexRules.Add(new LexRule(TokenType.DOUBLE_QUOTE, new Regex("\"")));
            LexRules.Add(new LexRule(TokenType.ESCAPED_SINGLE_QUOTE, new Regex("\\\'")));
            LexRules.Add(new LexRule(TokenType.ESCAPED_DOUBLE_QUOTE, new Regex("\\\"")));
        }
    }
}