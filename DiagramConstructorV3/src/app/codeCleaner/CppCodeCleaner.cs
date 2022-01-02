using System.Text.RegularExpressions;

namespace DiagramConstructorV3.app.codeCleaner
{
    public class CppCodeCleaner : CodeCleaner
    {

        public CppCodeCleaner()
        {
            //namespaces 
            ReplaceRegexps.Add(new Regex(@"using\s+namespace\s+\w*\;"));
            //preprocessor
            ReplaceRegexps.Add(new Regex("#include\\s*(<|\")+\\S+(>|\")+"));
            //define
            ReplaceRegexps.Add(new Regex("#define\\s+\\S+\\s+\\S+"));
            //multi-line comments
            ReplaceRegexps.Add(new Regex(@"\/\*[\s\S]*?\*\/|([^\\:]|^)\/\/.*$"));
            //single-line comments
            ReplaceRegexps.Add(new Regex(@"(\/\/)(.+?)(?=[\n\r]|\*\))"));
            
            ReplaceRegexps.Add(new Regex(@"\r|\n|\t"));
        }

        public override string CleanCodeBeforeParse(string code)
        {
            code = base.CleanCodeBeforeParse(code);
            code = code.Replace("==", "=");
            code = code.Replace("\\n", "");
            code = code.Replace("||", " or ").Replace("|", " or ");
            code = code.Replace("&&", " and ").Replace("&", " and ");
            return code;
        }

    }
}
