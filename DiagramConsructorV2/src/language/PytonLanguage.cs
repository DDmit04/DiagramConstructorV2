using DiagramConsructorV2.src.actor.codeFormatter;
using DiagramConsructorV2.src.actor.codeParser;
using DiagramConstructorV2.src.nodeThreeAnylizer;
using System.Collections.Generic;


namespace DiagramConsructorV2.src.lang
{
    public class PytonLanguage : Language
    {
        public PytonLanguage(): base("Pyton", 
            new List<string>() { ".py" }, 
            "Pyton file",
            new PythonCodeFormatter(),
            new PytonNodeThreeAnalyzer(), 
            new PytonCodeParser()) {}
    }
}
