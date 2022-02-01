namespace DiagramConstructorV3.app.threeController.structureController.config
{
    public class ThreeStructureControllerConfig
    {
        public string BeginNodeText { get; }
        public string EndNodeText { get; }
        public int LineCharsCount { get; }
        public int MaxCharsToMergeNodes { get; }
        public int MaxCharsToMergeNodesInShortShape { get; }
        public int MaxLineBreaksCount { get; }

        public ThreeStructureControllerConfig()
        {
            BeginNodeText = "Начало";
            EndNodeText = "Конец";
            LineCharsCount = 15;
            MaxCharsToMergeNodes = 50;
            MaxCharsToMergeNodesInShortShape = 35;
            MaxLineBreaksCount = 5;
        }
    }
}