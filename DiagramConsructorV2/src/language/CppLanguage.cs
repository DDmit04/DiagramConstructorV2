using DiagramConsructorV2.src.actor.codeFormatter;
using DiagramConsructorV2.src.actor.codeParser;
using DiagramConstructorV2.src.nodeThreeAnylizer;
using System.Collections.Generic;

namespace DiagramConsructorV2.src.lang
{
    public class CppLanguage : Language
    {

        public CppLanguage() : base("C++",
            new List<string>() { ".cpp", ".h" }, 
            "C++ file",
            new CppCodeFormatter(), 
            new CppNodeThreeAnalyzer(), 
            new CppCodeParser()) {}

    }
}
