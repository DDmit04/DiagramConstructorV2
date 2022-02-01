using System.Collections.Generic;
using DiagramConstructorV3.app.codeCleaner;
using DiagramConstructorV3.app.parser;
using DiagramConstructorV3.app.parser.parseConfig;
using DiagramConstructorV3.app.threeController.structureController;
using DiagramConstructorV3.app.threeController.textController;
using DiagramConstructorV3.app.tokenFilter.chain;
using DiagramConstructorV3.app.tokenizer;

namespace DiagramConstructorV3.language
{
    public class CppLanguage : Language
    {
        public CppLanguage() : base(
            "C++",
            new List<string>() { ".cpp", ".h" },
            "C++ file",
            new CppCodeCleaner(),
            new CppCodeTokenizer(),
            new CppTokenFilterChain(),
            new CppCodeParser(new DefaultParseConfig()),
            new CppThreeTextController(),
            new CppThreeStructureController())
        { }

    }
}
