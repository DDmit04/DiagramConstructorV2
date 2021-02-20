using System.Text.RegularExpressions;

namespace DiagramConstructorV2.src.lang.langConfig
{
    public class CppLanguageConfig : LanguageConfig
    {

        protected Regex unimportantOutputRegex = new Regex(@"\'\S*\'\,*");
        protected Regex methodSingleCallRegex = new Regex(@"(\S*)\((\S*)\)");
        protected Regex methodReturnCallRegex = new Regex(@"(\S*)(\=)(\S*)\((\S*)\)");
        protected Regex methodCallOnObjectRegex = new Regex(@"\S*\=\S*\.\S*\(\S*\)");

        protected string inputV2Statement = "cout«";
        protected string outputV2Statement = "cin»";

        public CppLanguageConfig()
        {
            this.ifStatement = "if(";
            this.elseStatement = "else{";
            this.elseCloseStatement = "}else";
            this.elseIfStatement = "elseif(";
            this.forStatement = "for(";
            this.whileStatement = "while(";
            this.doWhileStatement = "do{";
            this.inputStatement = "cin>>";
            this.outputStatement = "cout<<";
            this.inputReplacement = "Ввод ";
            this.outputReplacement = "Вывод ";
            this.methodHead = "";

            this.ifStatementRegex = new Regex(this.ifStatement + @"\(\S*\))");
            this.forStatementRegex = new Regex(this.forStatement + @"\(\S*\))");
            this.whileStatementRegex = new Regex(this.whileStatement + @"\(\S*\))");
            this.unimportantOutput = new Regex(@"^" + this.outputStatement + @"\s*(\'\s*\S*\s*\'|\s*)$");
        }

        public override bool isLineStartWithInOutPut(string codeLine)
        {
            return base.isLineStartWithInOutPut(codeLine)
                || codeLine.IndexOf(this.inputV2Statement) == 0
                || codeLine.IndexOf(this.outputV2Statement) == 0;
        }

        public override bool isLineStartWithProgram(string codeLine)
        {
            return methodSingleCallRegex.IsMatch(codeLine)
                || methodReturnCallRegex.IsMatch(codeLine)
                || methodCallOnObjectRegex.IsMatch(codeLine);
        }
    }
}
