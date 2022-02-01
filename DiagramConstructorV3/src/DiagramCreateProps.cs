namespace DiagramConstructorV3
{
    public class DiagramCreateProps
    {
        public string Code { get; }
        public string DiagramPath { get; }
        public bool CloseDiagramAfterBuild { get; }

        public DiagramCreateProps(string code, string diagramPath, bool closeDiagramAfterBuild)
        {
            Code = code;
            DiagramPath = diagramPath;
            CloseDiagramAfterBuild = closeDiagramAfterBuild;
        }
    }
}