using DiagramConsructorV2.src.data;
using DiagramConsructorV2.src.enumerated;
using DiagramConsructorV2.src.utills;
using DiagramConstructorV2.src.lang.langConfig;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DiagramConsructorV2.src.actor.codeParser
{
    public class CppCodeParser : CodeParser
    {
        public CppCodeParser() : base(new CppLanguageConfig()) {}

        /// <summary>
        /// Class main method convert code from string to AST
        /// </summary>
        /// <param name="codeToParse">code as string to convert</param>
        /// <returns>AST (list of methods)</returns>
        public override List<Method> ParseCode(string codeToParse)
        {
            List<Method> methods = new List<Method>();
            List<Node> globalVars = new List<Node>();
            Method newMethod = new Method("no method!", new List<Node>());
            while (!codeIsEmptyMarcks(codeToParse))
            {
                string nextMethodCode = getNextCodeBlock(codeToParse);
                int methodCodeBegin = nextMethodCode.IndexOf('{');
                string methodBlock = nextMethodCode.Substring(methodCodeBegin);
                string methodSignature = nextMethodCode.Substring(0, methodCodeBegin);
                codeToParse = codeToParse.Replace(nextMethodCode, "");
                methodBlock = methodBlock.Remove(methodBlock.IndexOf('{'), 1);
                methodBlock = methodBlock.Remove(methodBlock.LastIndexOf('}'), 1);
                methodNodes = parseNode(methodBlock);
                newMethod = new Method(methodSignature, methodNodes);
                if (methodSignature.LastIndexOf(';') != -1)
                {
                    globalVars.AddRange(extractGlobalVarsFromMethodSignature(methodSignature));
                }
                if (methodSignature.IndexOf("mian(") != 0)
                {
                    methods.Insert(0, newMethod);
                }
                else
                {
                    methods.Add(newMethod);
                }
            }
            foreach (Node globalVarDeclaration in globalVars)
            {
                methods[0].methodNodes.Insert(0, globalVarDeclaration);
            }
            return methods;
        }

        /// <summary>
        /// Convert global varibles to PROCESS blocks
        /// </summary>
        /// <param name="methodName">method signature to search global varibles</param>
        /// <returns>list of nodes - PROCESS blocks</returns>
        public List<Node> extractGlobalVarsFromMethodSignature(string methodName)
        {
            List<Node> extractedNodes = new List<Node>();
            Node newNode;
            int lastGlobalVaribleEndIndex = methodName.LastIndexOf(';');
            while (lastGlobalVaribleEndIndex != -1)
            {
                newNode = new Node();
                newNode.nodeText = methodName.Substring(0, lastGlobalVaribleEndIndex);
                newNode.shapeForm = ShapeForm.PROCESS;
                extractedNodes.Add(newNode);

                methodName = methodName.Substring(lastGlobalVaribleEndIndex + 1);
                lastGlobalVaribleEndIndex = methodName.LastIndexOf(';');
            }
            return extractedNodes;
        }

        /// <summary>
        /// Returns code block (code detween first {}) extracted from string
        /// </summary>
        /// <param name="code">code to search blocks</param>
        /// <returns>string from begin to index of closing } (count of { and } in result string is equal)</returns>
        private string getNextCodeBlock(string code)
        {
            if (code[0].Equals('{') && code[code.Length - 1].Equals('}'))
            {
                code = code.Remove(0, 1);
                code = code.Remove(code.Length - 1, 1);
            }
            string codeBlock = "";
            int openMarksCount = 0;
            int closeMarksCount = 0;
            for (int endIndex = 0; endIndex < code.Length; endIndex++)
            {
                if (code[endIndex].Equals('}'))
                {
                    openMarksCount++;
                }
                if (code[endIndex].Equals('{'))
                {
                    closeMarksCount++;
                }
                if (openMarksCount == closeMarksCount && openMarksCount != 0 && closeMarksCount != 0)
                {
                    codeBlock = code.Substring(0, endIndex + 1);
                    break;
                }
            }
            if (codeBlock.Equals(""))
            {
                codeBlock = code;
            }
            return codeBlock;
        }

        /// <summary>
        /// Get operator text from begin of code block
        /// </summary>
        /// <param name="nextCodeBlock">string to extract operator text</param>
        /// <returns>operator text</returns>
        string extractOperatorTextFromCodeBlock(string nextCodeBlock)
        {
            int nextLineDivider = nextCodeBlock.IndexOf('{');
            if (nextLineDivider == -1)
            {
                nextLineDivider = nextCodeBlock.Length;
            }
            nextCodeBlock = nextCodeBlock.Substring(0, nextLineDivider);
            return nextCodeBlock;
        }

        /// <summary>
        /// Recursive method which convert code lines to AST nodes 
        /// (if it meets if | else | while | ... it create node and put in it's child nodes result of itself call)
        /// </summary>
        /// <param name="nodeCode">code to convert</param>
        /// <returns>list of code ASTs</returns>
        private List<Node> parseNode(string nodeCode)
        {
            List<Node> resultNodes = new List<Node>();
            while (!codeIsEmptyMarcks(nodeCode))
            {
                Node newNode = new Node();
                string nextCodeBlock = getNextCodeBlock(nodeCode);
                int operatorAndCodeDivider = nextCodeBlock.IndexOf('{');
                if (languageConfig.isLineStartWithIf(nextCodeBlock))
                {
                    newNode.shapeForm = ShapeForm.IF;
                    newNode.nodeText = extractOperatorTextFromCodeBlock(nextCodeBlock);
                    newNode.childIfNodes = parseNode(nextCodeBlock.Substring(operatorAndCodeDivider));

                    string localCode = CodeUtils.replaceFirst(nodeCode, nextCodeBlock, "");

                    if (!codeIsEmptyMarcks(localCode))
                    {
                        //TO DO refactor this
                        string onotherNextBlock = getNextCodeBlock(localCode);
                        if (languageConfig.isLineStartWithElseIf(onotherNextBlock))
                        {
                            Regex elseifRegex = new Regex(@"(elseif\(\S+\){\S+;})+(else{\S+;})*");
                            Match match = elseifRegex.Match(localCode);
                            if (match.Success)
                            {
                                onotherNextBlock = match.Value;
                                nodeCode = CodeUtils.replaceFirst(nodeCode, onotherNextBlock, "");
                            }
                        }
                        if (languageConfig.isLineStartWithElse(onotherNextBlock))
                        {
                            nodeCode = nodeCode.Replace(onotherNextBlock, "");
                            onotherNextBlock = CodeUtils.replaceFirst(onotherNextBlock, "else", "");
                            newNode.childElseNodes = parseNode(onotherNextBlock);
                        }
                    }
                    resultNodes.Add(newNode);
                }
                else if (languageConfig.isLineStartWithFor(nextCodeBlock))
                {
                    newNode.shapeForm = ShapeForm.FOR;
                    newNode.nodeText = extractOperatorTextFromCodeBlock(nextCodeBlock);
                    newNode.childNodes = parseNode(nextCodeBlock.Substring(operatorAndCodeDivider));
                    resultNodes.Add(newNode);
                }
                else if (languageConfig.isLineStartWithWhile(nextCodeBlock))
                {
                    newNode.shapeForm = ShapeForm.WHILE;
                    newNode.nodeText = extractOperatorTextFromCodeBlock(nextCodeBlock);
                    newNode.childNodes = parseNode(nextCodeBlock.Substring(operatorAndCodeDivider));
                    resultNodes.Add(newNode);
                }
                else if (languageConfig.isLineStartWithDoWhile(nextCodeBlock))
                {
                    //find first 'while' after 'do{}'
                    int whileOperatorTextEndIndex = nodeCode.Substring(nextCodeBlock.Length).IndexOf(';');
                    string whileOperatorText = nodeCode.Substring(nextCodeBlock.Length, whileOperatorTextEndIndex);

                    //get only 'while' statement which belong to 'do-while'
                    Match match = languageConfig.whileStatementRegex.Match(whileOperatorText);
                    whileOperatorText = match.Value;

                    newNode.childNodes = parseNode(nextCodeBlock.Substring(operatorAndCodeDivider));

                    Node lastDoWhileNode = new Node(whileOperatorText, ShapeForm.WHILE);

                    newNode.childNodes.Add(lastDoWhileNode);
                    newNode.shapeForm = ShapeForm.DO;
                    resultNodes.Add(newNode);

                    nextCodeBlock += whileOperatorText;
                }
                else
                {
                    string nextCodeBlockCopy = nextCodeBlock;
                    while (!codeIsEmptyMarcks(nextCodeBlockCopy))
                    {
                        newNode = new Node();
                        operatorAndCodeDivider = nextCodeBlockCopy.IndexOf(';');
                        string nodeCodeLine = nextCodeBlockCopy.Substring(0, operatorAndCodeDivider + 1);
                        if (!lineIsSimple(nodeCodeLine))
                        {
                            break;
                        }
                        nextCodeBlockCopy = CodeUtils.replaceFirst(nextCodeBlockCopy, nodeCodeLine, "");
                        if (languageConfig.isLineStartWithInOutPut(nodeCodeLine))
                        {
                            newNode.shapeForm = ShapeForm.IN_OUT_PUT;
                        }
                        else if (languageConfig.isLineStartWithProgram(nodeCodeLine))
                        {
                            newNode.shapeForm = ShapeForm.PROGRAM;
                        }
                        else
                        {
                            newNode.shapeForm = ShapeForm.PROCESS;
                        }

                        newNode.nodeText = nodeCodeLine.Replace(";", "");
                        resultNodes.Add(newNode);
                    }
                    if (nextCodeBlockCopy.Length > 0)
                    {
                        nextCodeBlock = CodeUtils.replaceFirst(nextCodeBlock, nextCodeBlockCopy, "");
                    }
                }
                nodeCode = CodeUtils.replaceFirst(nodeCode, nextCodeBlock, "");
            }
            return resultNodes;
        }

        /// <summary>
        /// Check is text contains code operators
        /// </summary>
        /// <param name="text">text to check</param>
        /// <returns>Is code contains only { and } chars</returns>
        private bool codeIsEmptyMarcks(string text)
        {
            Regex regex = new Regex(@"({;*})");
            return regex.IsMatch(text) || text.Equals("");
        }

    }
}
