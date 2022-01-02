using System.Collections.Generic;
using System.Linq;
using DiagramConstructorV3.app.codeCleaner;
using DiagramConstructorV3.app.lexer;
using DiagramConstructorV3.app.parser;
using DiagramConstructorV3.app.threeController;
using DiagramConstructorV3.app.threeController.structureController;
using DiagramConstructorV3.app.threeController.textController;
using DiagramConstructorV3.app.tokenFilter.chain;
using DiagramConstructorV3.app.tokenizer;

namespace DiagramConstructorV3.language
{
    public abstract class Language
    {
        public readonly string DisplayName;
        protected readonly List<string> FileExtensions;
        protected readonly string LangFileDescription;

        public readonly CodeCleaner CodeCleaner;
        public readonly CodeTokenizer CodeTokenizer;
        public readonly TokenFilterChain TokenFilterChain;
        public readonly CodeParser CodeParser;
        public readonly ThreeTextController TextController;
        public readonly ThreeStructureController ThreeStructureController;

        protected Language(string displayName, List<string> fileExtensions, string langFileDescription,
            CodeCleaner codeCleaner, CodeTokenizer codeTokenizer, TokenFilterChain tokenFilterChain, CodeParser codeParser,
            ThreeTextController textController, ThreeStructureController threeStructureController)
        {
            DisplayName = displayName;
            FileExtensions = fileExtensions;
            LangFileDescription = langFileDescription;
            CodeCleaner = codeCleaner;
            CodeTokenizer = codeTokenizer;
            TokenFilterChain = tokenFilterChain;
            CodeParser = codeParser;
            TextController = textController;
            ThreeStructureController = threeStructureController;
        }

        public bool CheckExtensions(string extensionToCheck)
        {
            return FileExtensions.Count(extensions =>
                extensions.ToUpperInvariant() == extensionToCheck.ToUpperInvariant()) > 0;
        }

        public string GetExtensionsFileConditions()
        {
            var result = "";
            for (var i = 0; i < FileExtensions.Count; i++)
            {
                var extensions = FileExtensions[i];
                result += $"{LangFileDescription} (*{extensions})|*{extensions}";
                if (i != FileExtensions.Count - 1)
                {
                    result += "|";
                }
            }

            return result;
        }
    }
}