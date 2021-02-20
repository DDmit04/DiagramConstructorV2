using DiagramConsructorV2.src.enumerated;
using System;
using System.Text.RegularExpressions;

namespace DiagramConstructorV2.src.lang.langConfig
{
    public abstract class LanguageConfig
    {

        public string ifStatement;
        public string elseStatement;
        public string elseCloseStatement;
        public string elseIfStatement;
        public string forStatement;
        public string whileStatement;
        public string doWhileStatement;
        public string inputStatement;
        public string outputStatement;
        public string methodHead;

        public string inputReplacement = "Ввод ";
        public string outputReplacement = "Вывод ";

        public Regex ifStatementRegex;
        public Regex forStatementRegex;
        public Regex whileStatementRegex;
        public Regex unimportantOutput;


        /// <summary>
        /// Check is line starts with 'InOutPut' statement
        /// </summary>
        /// <param name="codeLine">line to check</param>
        /// <returns>bool</returns>
        public virtual bool isLineStartWithInOutPut(string codeLine)
        {
            return codeLine.IndexOf(this.inputStatement) == 0 || codeLine.IndexOf(this.outputStatement) == 0;
        }

        /// <summary>
        /// Check is line starts with 'for' statement
        /// </summary>
        /// <param name="codeLine">line to check</param>
        /// <returns>bool</returns>
        public virtual bool isLineStartWithFor(string codeLine)
        {
            return codeLine.IndexOf(this.forStatement) == 0;
        }

        /// <summary>
        /// Check is line starts with 'while' statement
        /// </summary>
        /// <param name="codeLine">line to check</param>
        /// <returns>bool</returns>
        public virtual bool isLineStartWithWhile(string codeLine)
        {
            return codeLine.IndexOf(this.whileStatement) == 0;
        }

        /// <summary>
        /// Check is line starts with 'if' statement
        /// </summary>
        /// <param name="codeLine">line to check</param>
        /// <returns>bool</returns>
        public virtual bool isLineStartWithIf(string codeLine)
        {
            return codeLine.IndexOf(this.ifStatement) == 0;
        }

        /// <summary>
        /// Check is line starts with 'do-while' statement
        /// </summary>
        /// <param name="codeLine">line to check</param>
        /// <returns>bool</returns>
        public virtual bool isLineStartWithDoWhile(string codeLine)
        {
            return codeLine.IndexOf(this.doWhileStatement) == 0;
        }

        /// <summary>
        /// Check is line starts with 'else' statement [else{ | }else | elseif(]
        /// </summary>
        /// <param name="codeLine">line to check</param>
        /// <returns>bool</returns>
        public virtual bool isLineStartWithElse(string codeLine)
        {
            return codeLine.IndexOf(this.elseStatement) == 0  
                || codeLine.IndexOf(this.elseCloseStatement) == 0 
                || codeLine.IndexOf(this.elseIfStatement) == 0;
        }

        /// <summary>
        /// Check is line starts with 'elseif' statement
        /// </summary>
        /// <param name="codeLine">line to check</param>
        /// <returns>bool</returns>
        public virtual bool isLineStartWithElseIf(string codeLine)
        {
            return codeLine.IndexOf(this.elseIfStatement) == 0;
        }

        /// <summary>
        /// Check is line starts with program statement (call lib, call method)
        /// </summary>
        /// <param name="codeLine">line to check</param>
        /// <returns>bool</returns>
        public virtual bool isLineStartWithProgram(string codeLine)
        {
            return false;
        }

        public virtual ShapeForm GetShapeFormFromLine(string line)
        {
            if (isLineStartWithIf(line))
            {
                return ShapeForm.IF;
            }
            else if (isLineStartWithFor(line))
            {
                return ShapeForm.FOR;
            }
            else if (isLineStartWithWhile(line))
            {
                return ShapeForm.WHILE;
            }
            else if (isLineStartWithInOutPut(line))
            {
                return ShapeForm.IN_OUT_PUT;
            }
            else
            {
                return ShapeForm.PROCESS;
            }
        }

    }
}
