using DiagramConstructorV3.app.builder;
using DiagramConstructorV3.language;

namespace DiagramConstructorV3
{
    public class DiagramCreator
    {
        protected readonly DiagramBuilder DiagramBuilder = new DefaultDiagramBuilder();
        protected readonly Language Language;

        public DiagramCreator(Language language)
        {
            Language = language;
        }

        public string CreateDiagram(DiagramCreateProps diagramCreateProps)
        {
            var code = Language.CleanCodeBeforeParse(diagramCreateProps.Code);
            var tokens = Language.TokenizeCode(code);
            tokens = Language.FilterTokens(tokens);
            var codeAst = Language.ParseTokens(tokens);
            codeAst = Language.ApplyNodeTextRules(codeAst);
            codeAst = Language.OptimizeMethodsStructure(codeAst);
            var diagramFilename = DiagramBuilder.BuildDiagram(codeAst, diagramCreateProps.CloseDiagramAfterBuild,
                diagramCreateProps.DiagramPath);
            return diagramFilename;
        }
    }
}