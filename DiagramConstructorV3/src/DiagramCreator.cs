using DiagramConstructorV3.app.builder;
using DiagramConstructorV3.language;

namespace DiagramConstructorV3
{
    public class DiagramCreator
    {

        protected readonly DiagramBuilder DiagramBuilder = new DiagramBuilder();
        protected readonly Language Language;

        public DiagramCreator(Language language)
        {
            Language = language;
        }

        public string CreateDiagram(string code, string diagramFilepath, bool closeAfterBuild)
        {
            code = Language.CodeCleaner.CleanCodeBeforeParse(code);
            var tokens = Language.CodeTokenizer.TokenizeCode(code);
            tokens = Language.TokenFilterChain.DoFilters(tokens);
            var codeAst = Language.CodeParser.ParseCode(tokens);
            codeAst = Language.ThreeStructureController.OptimizeStructure(codeAst);
            codeAst = Language.TextController.ApplyTextRules(codeAst);
            var diagramFilename = DiagramBuilder.BuildDiagram(codeAst, closeAfterBuild, diagramFilepath);
            return diagramFilename;
        }

    }
}
