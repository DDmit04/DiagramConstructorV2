using DiagramConsructorV2.src.data;
using DiagramConsructorV2.src.lang;
using DiagramConstructorV2.actor;
using System.Collections.Generic;

namespace DiagramConsructorV2.src
{
    public class DiagramCreator
    {

        private readonly DiagramBuilder diagramBuilder = new DiagramBuilder();
        private readonly Language language;

        public DiagramCreator(Language language)
        {
            this.language = language;
        }

        public string createDiagram(string code, string diagramFilepath, bool closeAfterBuild)
        {
            code = language.codeFormatter.prepareCodeBeforeParse(code);
            List<Method> codeAST = language.codeParser.ParseCode(code);
            codeAST = language.nodeThreeAnalyzer.analyzeMethods(codeAST);
            string diagramFilename = diagramBuilder.buildDiagram(codeAST, closeAfterBuild, diagramFilepath);
            return diagramFilename;
        }

    }
}
