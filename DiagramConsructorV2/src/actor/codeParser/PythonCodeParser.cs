using DiagramConsructorV2.src.data;
using DiagramConsructorV2.src.enumerated;
using DiagramConsructorV2.src.utills;
using DiagramConstructorV2.src.lang.langConfig;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DiagramConsructorV2.src.actor.codeParser
{
    public class PytonCodeParser : CodeParser
    {
        public PytonCodeParser() : base(new PytonLanguageConfig()) { }

        public override List<Method> ParseCode(string codeToParse)
        {
            List<Method> allMethods = new List<Method>();
            if (getNextCodeBlock(0, codeToParse) != codeToParse)
            {
                while (codeToParse != "")
                {
                    string methodHeadLine = CodeUtils.getNextLine(codeToParse);
                    while (methodHeadLine == "\n")
                    {
                        methodHeadLine = CodeUtils.getNextLine(codeToParse);
                        codeToParse = CodeUtils.replaceFirst(codeToParse, methodHeadLine, "");
                    }
                    string methodCode;
                    if (methodHeadLine.IndexOf(languageConfig.methodHead) == -1
                        && codeToParse.IndexOf(languageConfig.methodHead) == -1)
                    {
                        methodHeadLine = "main";
                        methodCode = getNextCodeBlock(-1, codeToParse);
                        int nextMethodIndex = methodHeadLine.IndexOf(languageConfig.methodHead);
                        if (nextMethodIndex == -1)
                        {
                            methodCode = methodCode.Substring(0, nextMethodIndex);
                        }
                    }
                    else
                    {
                        methodHeadLine = formatLine(methodHeadLine);
                        codeToParse = CodeUtils.replaceFirst(codeToParse, methodHeadLine, "");
                        methodCode = getNextCodeBlock(0, codeToParse);
                    }
                    codeToParse = CodeUtils.replaceFirst(codeToParse, methodCode, "");

                    List<Node> methodNodes = parse(1, methodCode);
                    Method newMethod = new Method(methodHeadLine, methodNodes);
                    allMethods.Add(newMethod);
                }
            } 
            else
            {
                List<Node> methodNodes = parse(0, codeToParse);
                Method newMethod = new Method("main", methodNodes);
                allMethods.Add(newMethod);
            }
            return allMethods;
        }

        private string getNextCodeBlock(int currentLevel, string code)
        {
            string resultBlock = "";
            int level = -1;
            string nextLine = CodeUtils.getNextLine(code);
            while (code != "")
            {
                if (level == -1 || level > currentLevel)
                {
                    resultBlock += nextLine;
                    code = CodeUtils.replaceFirst(code, nextLine, "");
                }
                else
                {
                    break;
                }
                nextLine = CodeUtils.getNextLine(code);
                level = Regex.Matches(nextLine, "\t").Count;
            }

            return resultBlock;
        }

        private List<Node> parse(int currentLevel, string codeToParse)
        {
            List<Node> resNodes = new List<Node>();
            while (codeToParse != "")
            {
                string nextLine = CodeUtils.getNextLine(codeToParse);
                codeToParse = CodeUtils.replaceFirst(codeToParse, nextLine, "");
                nextLine = formatLineWithCurrentLevel(currentLevel, nextLine);

                Node newNode = new Node();
                newNode.nodeText = formatLine(nextLine);
                newNode.shapeForm = languageConfig.GetShapeFormFromLine(nextLine);

                if (languageConfig.isLineStartWithIf(nextLine))
                {
                    string nextCodeBlock = getNextCodeBlock(currentLevel, codeToParse);
                    codeToParse = CodeUtils.replaceFirst(codeToParse, nextCodeBlock, "");

                    newNode.childIfNodes = parse(currentLevel + 1, nextCodeBlock);

                    nextLine = getNextLineFormattedByLevel(currentLevel, codeToParse);

                    Node currentNode = newNode;

                    while (languageConfig.isLineStartWithElse(nextLine) || languageConfig.isLineStartWithElseIf(nextLine))
                    {

                        if (languageConfig.isLineStartWithElse(nextLine))
                        {
                            codeToParse = CodeUtils.replaceFirst(codeToParse, nextLine, "");
                            nextCodeBlock = getNextCodeBlock(currentLevel, codeToParse);
                            codeToParse = CodeUtils.replaceFirst(codeToParse, nextCodeBlock, "");

                            currentNode.childElseNodes = parse(currentLevel + 1, nextCodeBlock);
                        }

                        nextLine = getNextLineFormattedByLevel(currentLevel, codeToParse);

                        if (languageConfig.isLineStartWithElseIf(nextLine))
                        {
                            Node elseIfNode = new Node();
                            elseIfNode.nodeText = nextLine;
                            elseIfNode.shapeForm = ShapeForm.IF;

                            codeToParse = CodeUtils.replaceFirst(codeToParse, nextLine, "");
                            nextCodeBlock = getNextCodeBlock(currentLevel, codeToParse);
                            codeToParse = CodeUtils.replaceFirst(codeToParse, nextCodeBlock, "");

                            elseIfNode.childIfNodes = parse(currentLevel + 1, nextCodeBlock);

                            currentNode.childElseNodes.Add(elseIfNode);
                            currentNode = elseIfNode;
                        }
                        nextLine = getNextLineFormattedByLevel(currentLevel, codeToParse);
                    }

                }
                else if (!lineIsSimple(nextLine))
                {
                    string nextCodeBlock = getNextCodeBlock(currentLevel, codeToParse);
                    codeToParse = CodeUtils.replaceFirst(codeToParse, nextCodeBlock, "");
                    newNode.childNodes = parse(currentLevel + 1, nextCodeBlock);
                }
                resNodes.Add(newNode);
            }
            return resNodes;
        }

        private string formatLine(string line)
        {
            return line.Replace("\t", "").Replace("\n", "").Replace("\r", "");
        }

        private string formatLineWithCurrentLevel(int currentLevel, string codeLine)
        {
            Regex pattern = new Regex("[\\t]");
            return pattern.Replace(codeLine, "", currentLevel);
        }

        private string getNextLineFormattedByLevel(int currentLevel, string code)
        {
            return formatLineWithCurrentLevel(currentLevel, CodeUtils.getNextLine(code));
        }

    }
}
