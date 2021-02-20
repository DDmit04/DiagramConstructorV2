using DiagramConsructorV2.src.data;
using DiagramConstructorV2.src.lang.langConfig;
using System;
using System.Collections.Generic;

namespace DiagramConsructorV2.src.actor.codeParser
{
    public abstract class CodeParser
    {
        protected List<Node> methodNodes = new List<Node>();
        protected LanguageConfig languageConfig;

        public CodeParser(LanguageConfig languageConfig)
        {
            this.languageConfig = languageConfig;
        }

        /// <summary>
        /// Class main method convert code from string to AST
        /// </summary>
        /// <param name="codeToParse">code as string to convert</param>
        /// <returns>AST (list of methods)</returns>
        public abstract List<Method> ParseCode(string codeToParse);

        /// <summary>
        /// Check is code not started with (if | for | while | etc...)
        /// </summary>
        /// <param name="line">code to check</param>
        /// <returns>bool</returns>
        protected virtual bool lineIsSimple(String line)
        {
            return !(languageConfig.isLineStartWithIf(line) 
                || languageConfig.isLineStartWithElseIf(line)
                || languageConfig.isLineStartWithFor(line)
                || languageConfig.isLineStartWithWhile(line) 
                || languageConfig.isLineStartWithDoWhile(line));
        }

    }
}
