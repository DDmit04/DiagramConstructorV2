using DiagramConstructor.Config;
using DiagramConstructor.utills;
using System.Text.RegularExpressions;

namespace DiagramConsructorV2.src.actor.codeFormatter
{
    abstract class CodeFormatter
    {
        protected LanguageConfig languageConfig;

        public CodeFormatter(LanguageConfig languageConfig)
        {
            this.languageConfig = languageConfig;
        }

        public abstract string prepareCodeBeforeParse(string code);

        public virtual string formatMethodHead(string codeLine)
        {
            return codeLine;
        }

        /// <summary>
        /// Format 'for' statement
        /// </summary>
        /// <param name="codeLine">line to format</param>
        /// <returns>formated line (without statement string)</returns>
        public virtual string formatFor(string codeLine)
        {
            Match match = this.languageConfig.forStatementRegex.Match(codeLine);
            codeLine = match.Value;
            codeLine = CodeUtils.replaceFirst(codeLine, this.languageConfig.forStatement, "");
            return codeLine;
        }

        /// <summary>
        /// Format 'if' statement
        /// </summary>
        /// <param name="codeLine">line to format</param>
        /// <returns>formated line (without statement string)</returns>
        public virtual string formatIf(string codeLine)
        {
            Match match = this.languageConfig.ifStatementRegex.Match(codeLine);
            codeLine = match.Value;
            codeLine = CodeUtils.replaceFirst(codeLine, this.languageConfig.ifStatement, "");
            return codeLine;
        }

        /// <summary>
        /// Format 'while' statement
        /// </summary>
        /// <param name="codeLine">line to format</param>
        /// <returns>formated line (without statement string)</returns>
        public virtual string formatWhile(string codeLine)
        {
            Match match = this.languageConfig.whileStatementRegex.Match(codeLine);
            codeLine = match.Value;
            codeLine = CodeUtils.replaceFirst(codeLine, this.languageConfig.whileStatement, "");
            return codeLine;
        }

        /// <summary>
        /// Format 'inOutPut' statement
        /// </summary>
        /// <param name="codeLine">line to format</param>
        /// <returns>formated line (without statement string)</returns>
        public virtual string formatInOutPut(string codeLine)
        {
            codeLine = codeLine.Replace(this.languageConfig.inputStatement, this.languageConfig.inputReplacement);
            codeLine = codeLine.Replace(this.languageConfig.outputStatement, this.languageConfig.outputReplacement);
            return codeLine;
        }
    }
}
