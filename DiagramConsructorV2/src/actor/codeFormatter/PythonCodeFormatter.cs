using DiagramConsructorV2.src.utills;
using DiagramConstructorV2.src.lang.langConfig;
using System.Text.RegularExpressions;

namespace DiagramConsructorV2.src.actor.codeFormatter
{
    public class PythonCodeFormatter : CodeFormatter
    {

        protected string rangeStatement = "range(";
        public PythonCodeFormatter() : base(new PytonLanguageConfig()) 
        {
            //from Import Regex 
            replaceRegexps.Add(new Regex(@"from(.)*\n"));
            //import Regex
            replaceRegexps.Add(new Regex(@"import(.)*\n"));
            //single Line Comment 
            replaceRegexps.Add(new Regex(@"^#.*$"));
            //multi Line Comment 
            replaceRegexps.Add(new Regex("\"\"\"(.|[\r\n])*\"\"\""));
            //emplty Lines 
            replaceRegexps.Add(new Regex(@"^\s*?$"));
        }

        public override string prepareCodeBeforeParse(string code)
        {
            code = base.prepareCodeBeforeParse(code);
            Regex witespacesRegex = new Regex(@"[' ']{4}");
            code = witespacesRegex.Replace(code, "\t");
            code = code.Replace("\r", "");
            code = code.Replace("\"", "'");
            code = code.Trim();
            return code;
        }

        public override string formatMethodHead(string codeLine)
        {
            codeLine = base.formatMethodHead(codeLine);
            codeLine = CodeUtils.removeLastOcur(codeLine, ":") ;
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
            codeLine = CodeUtils.replaceFirst(codeLine, this.languageConfig.outputStatement, this.languageConfig.outputReplacement);
            codeLine = codeLine.Replace(this.languageConfig.outputStatement, "");
            codeLine = CodeUtils.removeLastOcur(codeLine, ")");
            return codeLine;
        }

        public override string formatFor(string codeLine)
        {
            codeLine = base.formatFor(codeLine);
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
            codeLine = base.formatIf(codeLine);
            codeLine = CodeUtils.removeLastOcur(codeLine, ":");
            return codeLine;
        }

        public override string formatWhile(string codeLine)
        {
            codeLine = base.formatWhile(codeLine);
            codeLine = CodeUtils.removeLastOcur(codeLine, ":");
            return codeLine;
        }

    }
}
