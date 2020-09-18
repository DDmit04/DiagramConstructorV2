using DiagramConstructor.Config;
using DiagramConstructor.utills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiagramConsructorV2.src.actor.codeFormatter
{
    class CppCodeFormatter : CodeFormatter
    {

        protected string inputV2Statement = "cout«";
        protected string outputV2Statement = "cin»";
        public CppCodeFormatter(LanguageConfig languageConfig) : base(languageConfig) { }

        public override string prepareCodeBeforeParse(string code)
        {
            Regex namespacesRegex = new Regex(@"using\s*namespace\s*\w*\;");
            Regex prepocessorRegex = new Regex(@"#\w*\s*[<|']\w*\.*\w?[\>|']");
            Regex structRegex = new Regex(@"(struct)\s*\S*\s*\{\s*\S*\s*(\};)");
            Regex commentsRegex = new Regex(@"\/\*[\s\S]*?\*\/|([^\\:]|^)\/\/.*$");
            Regex varibleDeclarationRegex = new Regex(@"^(const)*\s*(void|char|int|double|bool|long|short|string|String|float)+\s*\**\s*\S*\s*;");
            Regex arrAndStructDeclarationWithInitRegex = new Regex("^((const)*(\\s*\\S*\\s*)\\s*\\**\\s*)(\\s*\\S*\\s*)(\\[\\d*\\])*\\s*= +\\s*{({*((\\d*|\\w*|'|\"),*\\s*)*}*\\,*)*}\\s*;");
            Regex arrDeclarationRegex = new Regex(@"^((const)*(\s*\S*\s*)\s*\**\s*)(\s*\S*\s*)(\[\d*\])+\s*;");
            Regex constructiorCallRegex = new Regex(@"^(const)*(\s*[\d*|\w*]\s*)\s*\**\s*(\s*\S*\s*)(\(\s*\S*\s*\))+;");
            Regex servoceWordsRegex = new Regex(@"(const)*(return|void|int|char|double|long|short|string|String|float)");
            Regex specialSymbolRegex = new Regex(@"\r|\n|\t");

            code = namespacesRegex.Replace(code, "");
            code = prepocessorRegex.Replace(code, "");
            code = structRegex.Replace(code, "");
            code = namespacesRegex.Replace(code, "");
            code = commentsRegex.Replace(code, "");
            code = varibleDeclarationRegex.Replace(code, "");
            code = arrAndStructDeclarationWithInitRegex.Replace(code, "");
            code = arrDeclarationRegex.Replace(code, "");
            code = constructiorCallRegex.Replace(code, "");
            code = servoceWordsRegex.Replace(code, "");
            code = specialSymbolRegex.Replace(code, "");

            code = code.Replace(" ", "");
            code = code.Replace("==", "=");
            code = code.Replace("->", ".");
            code = code.Replace("\"", "'");
            code = code.Replace("\\n", "'");

            return code;
        }

        public override string formatMethodHead(string codeLine)
        {
            Regex arraySizeRegexp = new Regex(@"(\[\d*\])*");
            codeLine = arraySizeRegexp.Replace(codeLine, "", 1);
            int methodArgsIndex = codeLine.IndexOf('(');
            if (methodArgsIndex != -1)
            {
                codeLine = codeLine.Insert(methodArgsIndex, " ");
            }
            if (codeLine.LastIndexOf(';') != -1)
            {
                codeLine = clearMethodSignature(codeLine);
            }
            return base.formatMethodHead(codeLine);
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
        /// Delete globar args from method signature
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
