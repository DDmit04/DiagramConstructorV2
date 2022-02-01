using System.Collections.Generic;
using System.Linq;
using DiagramConstructorV3.app.codeCleaner;
using DiagramConstructorV3.app.parser;
using DiagramConstructorV3.app.parser.data;
using DiagramConstructorV3.app.threeController.structureController;
using DiagramConstructorV3.app.threeController.textController;
using DiagramConstructorV3.app.tokenFilter.chain;
using DiagramConstructorV3.app.tokenizer;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.language
{
    public abstract class Language
    {
        public readonly string DisplayName;
        protected readonly List<string> FileExtensions;
        protected readonly string LangFileDescription;

        protected readonly CodeCleaner CodeCleaner;
        protected readonly CodeTokenizer CodeTokenizer;
        protected readonly TokenFilterChain TokenFilterChain;
        protected readonly CodeParser CodeParser;
        protected readonly ThreeTextController TextController;
        protected readonly ThreeStructureController ThreeStructureController;

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

        public string CleanCodeBeforeParse(string code)
        {
            return CodeCleaner.CleanCodeBeforeParse(code);
        }
        public List<Token> TokenizeCode(string code)
        {
            return CodeTokenizer.TokenizeCode(code);
        }
        public List<Token> FilterTokens(List<Token> tokens)
        {
            return TokenFilterChain.DoFilters(tokens);
        }
        public List<Method> ParseTokens(List<Token> tokens)
        {
            return CodeParser.ParseCode(tokens);
        }
        public List<Method> OptimizeMethodsStructure(List<Method> methods)
        {
            return ThreeStructureController.OptimizeStructure(methods);
        }
        public List<Method> ApplyNodeTextRules(List<Method> methods)
        {
            return TextController.ApplyTextRules(methods);
        }
    }
}