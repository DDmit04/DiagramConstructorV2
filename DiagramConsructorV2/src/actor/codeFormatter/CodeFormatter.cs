using DiagramConsructorV2.src.utills;
using DiagramConstructorV2.src.lang.langConfig;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DiagramConsructorV2.src.actor.codeFormatter
{
    public abstract class CodeFormatter
    {
        protected LanguageConfig languageConfig;
        protected List<Regex> replaceRegexps = new List<Regex>();

        public CodeFormatter(LanguageConfig languageConfig)
        {
            this.languageConfig = languageConfig;
        }

        public virtual string prepareCodeBeforeParse(string code)
        {
            foreach (var regexp in replaceRegexps)
            {
                code = regexp.Replace(code, "");
            }
            return code; 
        }

        public virtual string formatMethodHead(string codeLine)
        {
            codeLine = codeLine.Replace(languageConfig.methodHead, "");
            return codeLine;
        }

        /// <summary>
        /// Format 'for' statement
        /// </summary>
        /// <param name="codeLine">line to format</param>
        /// <returns>formated line (without statement string)</returns>
        public virtual string formatFor(string codeLine)
        {
            Match match = languageConfig.forStatementRegex.Match(codeLine);
            codeLine = match.Value;
            codeLine = CodeUtils.replaceFirst(codeLine, languageConfig.forStatement, "");
            return codeLine;
        }

        /// <summary>
        /// Format 'if' statement
        /// </summary>
        /// <param name="codeLine">line to format</param>
        /// <returns>formated line (without statement string)</returns>
        public virtual string formatIf(string codeLine)
        {
            Match match = languageConfig.ifStatementRegex.Match(codeLine);
            codeLine = match.Value;
            codeLine = CodeUtils.replaceFirst(codeLine, languageConfig.ifStatement, "");
            return codeLine;
        }

        /// <summary>
        /// Format 'while' statement
        /// </summary>
        /// <param name="codeLine">line to format</param>
        /// <returns>formated line (without statement string)</returns>
        public virtual string formatWhile(string codeLine)
        {
            Match match = languageConfig.whileStatementRegex.Match(codeLine);
            codeLine = match.Value;
            codeLine = CodeUtils.replaceFirst(codeLine, languageConfig.whileStatement, "");
            return codeLine;
        }

        /// <summary>
        /// Format 'inOutPut' statement
        /// </summary>
        /// <param name="codeLine">line to format</param>
        /// <returns>formated line (without statement string)</returns>
        public virtual string formatInOutPut(string codeLine)
        {
            codeLine = codeLine.Replace(languageConfig.inputStatement, languageConfig.inputReplacement);
            codeLine = codeLine.Replace(languageConfig.outputStatement, languageConfig.outputReplacement);
            return codeLine;
        }
    }
}
