
using System.Text.RegularExpressions;

namespace DiagramConstructor.Config
{
    class PytonLanguageConfig : LanguageConfig
    {

        public PytonLanguageConfig()
        {
            this.ifStatement = "if";
            this.elseStatement = "else";
            this.elseIfStatement = "elif";
            this.forStatement = "for";
            this.whileStatement = "while";
            this.inputStatement = "input(";
            this.outputStatement = "print(";
            this.methodHead = "def ";

            this.ifStatementRegex = new Regex(this.ifStatement + @"\(\S*\)");
            this.forStatementRegex = new Regex(this.forStatement + @"\(\S*\)");
            this.whileStatementRegex = new Regex(this.whileStatement + @"\(\S*\)");
        }

        public override bool isLineStartWithInOutPut(string codeLine)
        {
            return codeLine.Contains(this.inputStatement) || codeLine.Contains(this.outputStatement );
        }

        public override bool isLineStartWithDoWhile(string codeLine)
        {
            return false;
        }

        public override bool isLineStartWithElse(string codeLine)
        {
            return codeLine.IndexOf(this.elseStatement) == 0;
        }

        public override bool isLineStartWithProgram(string codeLine)
        {
            return false;
        }

    }


}
