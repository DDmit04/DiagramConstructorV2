using DiagramConstructor.Config;
using DiagramConstructor.utills;
using System.Text.RegularExpressions;

namespace DiagramConsructorV2.src.actor.codeFormatter
{
    class PytonCodeFormatter : CodeFormatter
    {

        protected string rangeStatement = "range(";
        public PytonCodeFormatter(LanguageConfig languageConfig) : base(languageConfig) { }

        public override string prepareCodeBeforeParse(string code)
        {
            Regex witespacesRegex = new Regex(@"[' ']{4}");
            Regex fromImportRegex = new Regex(@"from(.)*\n");
            Regex importRegex = new Regex(@"import(.)*\n");
            Regex singleLineComment = new Regex(@"^#.*$");
            Regex multiLineComment = new Regex("\"\"\"(.|[\r\n])*\"\"\"");
            Regex empltyLinesRegex = new Regex(@"^\s*$");

            code = witespacesRegex.Replace(code, "\t");
            code = fromImportRegex.Replace(code, "");
            code = importRegex.Replace(code, "");
            code = singleLineComment.Replace(code, "");
            code = multiLineComment.Replace(code, "");
            code = empltyLinesRegex.Replace(code, "");

            code = code.Replace("\r", "");
            code = code.Replace("\"", "'");
            code = code.Trim();

            return code;
        }

        public override string formatMethodHead(string codeLine)
        {
            codeLine = codeLine.Replace(this.languageConfig.methodHead, "");
            codeLine = CodeUtils.removeLastOcur(codeLine, ":");
            return codeLine;
        }
        public override string formatInOutPut(string codeLine)
        {
            int inputIndex = codeLine.IndexOf(this.languageConfig.inputStatement);
            if (inputIndex != -1)
            {
                codeLine = codeLine.Trim().Substring(0, codeLine.IndexOf("="));
                codeLine = codeLine.Insert(0, this.languageConfig.inputReplacement);
            }
            codeLine = codeLine.Replace(this.languageConfig.outputStatement, this.languageConfig.outputReplacement);
            codeLine = CodeUtils.removeLastOcur(codeLine, ")");
            return codeLine;
        }

        public override string formatFor(string codeLine)
        {
            codeLine = codeLine.Replace(this.languageConfig.forStatement, "");
            codeLine = CodeUtils.removeLastOcur(codeLine, ":");
            if(codeLine.IndexOf(this.rangeStatement) != -1)
            {
                codeLine = codeLine.Replace(this.rangeStatement, "");
                codeLine = CodeUtils.removeLastOcur(codeLine, ")");
            }
            return codeLine;
        }

        public override string formatIf(string codeLine)
        {
            codeLine = codeLine.Replace(this.languageConfig.elseIfStatement, "");
            codeLine = codeLine.Replace(this.languageConfig.ifStatement, "");
            codeLine = CodeUtils.removeLastOcur(codeLine, ":");
            return codeLine;
        }

        public override string formatWhile(string codeLine)
        {
            codeLine = codeLine.Replace(this.languageConfig.whileStatement, "");
            codeLine = CodeUtils.removeLastOcur(codeLine, ":");
            return codeLine;
        }

    }
}
