using DiagramConstructorV2.src.lang.langConfig;
using System;
using System.Text.RegularExpressions;

namespace DiagramConsructorV2.src.actor.codeFormatter
{
    public class CppCodeFormatter : CodeFormatter
    {
        protected string inputV2Statement = "cout«";
        protected string outputV2Statement = "cin»";
        public CppCodeFormatter() : base(new CppLanguageConfig())
        {
            //namespaces 
            replaceRegexps.Add(new Regex(@"using\s*namespace\s*\w*\;"));
            //prepocessor
            replaceRegexps.Add(new Regex(@"#\w*\s*[<|']\w*\.*\w?[\>|']"));
            //struct
            replaceRegexps.Add(new Regex(@"(struct)\s*\S*\s*\{\s*\S*\s*(\};)"));
            //comments
            replaceRegexps.Add(new Regex(@"\/\*[\s\S]*?\*\/|([^\\:]|^)\/\/.*$"));
            //varible Declaration
            replaceRegexps.Add(new Regex(@"^(const)*\s*(void|char|int|double|bool|long|short|string|String|float)+\s*\**\s*\S*\s*;"));
            //arr And Struct Declaration With Init
            replaceRegexps.Add(new Regex("^((const)*(\\s*\\S*\\s*)\\s*\\**\\s*)(\\s*\\S*\\s*)(\\[\\d*\\])*\\s*= +\\s*{({*((\\d*|\\w*|'|\"),*\\s*)*}*\\,*)*}\\s*;"));
            //arr Declaration
            replaceRegexps.Add(new Regex(@"^((const)*(\s*\S*\s*)\s*\**\s*)(\s*\S*\s*)(\[\d*\])+\s*;"));
            //constructior Call
            replaceRegexps.Add(new Regex(@"^(const)*(\s*[\d*|\w*]\s*)\s*\**\s*(\s*\S*\s*)(\(\s*\S*\s*\))+;"));
            //service Words
            replaceRegexps.Add(new Regex(@"(const)*(return|void|int|char|double|long|short|string|String|float)"));
            //special Symbols
            replaceRegexps.Add(new Regex(@"\r|\n|\t"));
        }


        public override string prepareCodeBeforeParse(string code)
        {
            code = code.Replace("\"", "'");
            code = code.Replace(" ", "");
            code = code.Replace("==", "=");
            code = code.Replace("->", ".");
            code = code.Replace("\"", "'");
            code = code.Replace("\\n", "'");
            code = base.prepareCodeBeforeParse(code);
            return code;
        }

        public override string formatMethodHead(string codeLine)
        {
            Regex arraySizeRegexp = new Regex(@"(\[\d*\])*");
            Regex classQualifierRegexp = new Regex(@".*::");
            codeLine = arraySizeRegexp.Replace(codeLine, "", 1);
            codeLine = classQualifierRegexp.Replace(codeLine, "", 1);
            int methodArgsIndex = codeLine.IndexOf('(');
            if (methodArgsIndex != -1)
            {
                codeLine = codeLine.Insert(methodArgsIndex, " ");
            }
            if (codeLine.LastIndexOf(';') != -1)
            {
                codeLine = clearMethodSignature(codeLine);
            }
            return codeLine;
        }

        public override string formatFor(string codeLine)
        {
            codeLine = base.formatFor(codeLine);
            //exampe - startText = 'i=0;i<10;i++'
            int index = codeLine.LastIndexOf(';');
            // i++
            String incrementText = codeLine.Substring(index + 1);
            // i
            Regex incrementName = new Regex(@"(\d*|\w*)[^\W*]");
            // ++
            incrementText = incrementName.Replace(incrementText, "", 1);
            // ++ (in case of incrementText - '+=10' incrementAction - '+=')
            String incrementAction = incrementText.Substring(0, 2);
            // '' (in case of incrementText - '+=10' incrementArg - '10')
            String incrementArg = incrementText.Replace(incrementAction, "");

            if (incrementAction.Equals("++"))
            {
                incrementText = "1";
            }
            else if (incrementAction.Equals("--"))
            {
                incrementText = "-1";
            }
            else if (incrementAction.Equals("+="))
            {
                incrementText = incrementArg;
            }
            else if (incrementAction.Equals("-="))
            {
                incrementText = "-" + incrementArg;
            }
            else if (incrementAction.Equals("*="))
            {
                incrementText = "*" + incrementArg;
            }
            else if (incrementAction.Equals("/="))
            {
                incrementText = "/" + incrementArg;
            }

            codeLine = codeLine.Substring(0, index);
            codeLine = codeLine.Replace(";", " (" + incrementText + ") ");
            return codeLine;
        }

        public override string formatIf(string codeLine)
        {
            codeLine = base.formatIf(codeLine);
            codeLine = codeLine.Replace("||", " or ").Replace("|", " or ");
            codeLine = codeLine.Replace("&&", " and ").Replace("&", " and ");
            return codeLine;
        }

        public override string formatInOutPut(string codeLine)
        {
            codeLine = base.formatInOutPut(codeLine);
            codeLine = codeLine.Replace(this.inputV2Statement, this.languageConfig.inputReplacement);
            codeLine = codeLine.Replace(this.outputV2Statement, this.languageConfig.outputReplacement);
            return codeLine;
        }

        /// <summary>
        /// Delete global args from method signature
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public String clearMethodSignature(String methodName)
        {
            int lastGlobalVaribleEndIndex = methodName.LastIndexOf(';');
            while (lastGlobalVaribleEndIndex != -1)
            {
                methodName = methodName.Substring(lastGlobalVaribleEndIndex + 1);
                lastGlobalVaribleEndIndex = methodName.LastIndexOf(';');
            }
            return methodName;
        }

    }
}
