using DiagramConsructorV2.fileTypes;
using DiagramConsructorV2.src.actor.codeFormatter;
using DiagramConstructor;
using DiagramConstructor.actor;
using DiagramConstructor.actor.codeAnalyzer;
using DiagramConstructor.actor.codeParser;
using DiagramConstructor.Config;
using System;
using System.Collections.Generic;

namespace DiagramConsructorV2.src
{
    class DiagramCreator
    {

        private LanguageConfig languageConfig;
        private CodeParser codeParser;
        private CodeAnalyzer codeAnalyzer;
        private CodeFormatter codeFormatter;
        private DiagramBuilder diagramBuilder = new DiagramBuilder();

        public DiagramCreator(Language language)
        {
            Type langType = language.GetType();
            if (langType == typeof(CppLanguage))
            {
                this.languageConfig = new CppLanguageConfig();
                this.codeFormatter = new CppCodeFormatter(languageConfig);
                this.codeParser = new CppCodeParser(languageConfig);
                this.codeAnalyzer = new CppCodeAnalyzer(codeFormatter);
            }
            else if (langType == typeof(PytonLanguage))
            {
                this.languageConfig = new PytonLanguageConfig();
                this.codeFormatter = new PytonCodeFormatter(languageConfig);
                this.codeParser = new PytonCodeParser(languageConfig);
                this.codeAnalyzer = new PytonCodeAnalyzer(codeFormatter);
            }
            else
            {
                throw new Exception($"Unknown language class type - {langType}");
            }
        }

        public string createDiagram(string code, string diagramFilepath, bool closeAfterBuild)
        {
            code = codeFormatter.prepareCodeBeforeParse(code);
            List<Method> codeAST = codeParser.ParseCode(code);
            codeAST = codeAnalyzer.analyzeMethods(codeAST);
            string diagramFilename = diagramBuilder.buildDiagram(codeAST, closeAfterBuild, diagramFilepath);
            return diagramFilename;
        }

    }
}
