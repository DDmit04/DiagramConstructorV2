using System.Collections.Generic;
using DiagramConstructorV3.app.parser.data;
using DiagramConstructorV3.app.parser.parseConfig;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.boundaryPatterns;
using DiagramConstructorV3.app.tokenPattern.commonPatterns;

namespace DiagramConstructorV3.app.parser
{
    public class CppCodeParser : CodeParser
    {
        public CppCodeParser(ParseConfig parseConfig) : base(parseConfig)
        {
            var ifPattern = BuildFullOperatorPattern(TokenType.IF_OPERATOR);
            var elseIfPattern = BuildFullOperatorPattern(TokenType.ELSE_IF_OPERATOR);
            var forPattern = BuildFullOperatorPattern(TokenType.FOR_OPERATOR);
            var whilePattern = BuildFullOperatorPattern(TokenType.WHILE_OPERATOR);
            var elsePattern = StrictPatternBuilder
                .Reset()
                .NextPattern(new SingleTokenPattern(TokenType.ELSE_OPERATOR))
                .NextPattern(TokensBlockPattern.BracketBlock)
                .Build();
            var doWhilePattern = StrictPatternBuilder
                .Reset()
                .NextPattern(new SingleTokenPattern(TokenType.DO_WHILE_OPERATOR))
                .NextPattern(TokensBlockPattern.BracketBlock)
                .NextPattern(new SingleTokenPattern(TokenType.WHILE_OPERATOR))
                .NextPattern(parseConfig.ArgsPattern)
                .NextPattern(new SingleTokenPattern(TokenType.LINE_END))
                .Build();
            var inputPattern = TokenSequencePatternBuilder.Reset()
                .Then(TokenType.INPUT_OPERATOR)
                .Then(TokenType.TEXT)
                .Then(TokenType.LINE_END)
                .Build();
            var outputPattern = TokenSequencePatternBuilder.Reset()
                .Then(TokenType.OUTPUT_OPERATOR)
                .Then(TokenType.TEXT)
                .Then(TokenType.LINE_END)
                .Build();
            var processPattern = new StartEndTokensPattern(TokenType.IDENTIFIER, TokenType.LINE_END);
            var programPattern = new StartEndTokensPattern(
                StrictPatternBuilder
                    .Reset()
                    .NextPattern(TokenSequencePatternBuilder
                        .Reset()
                        .Then(TokenType.IDENTIFIER)
                        .Then(TokenType.DOT)
                        .Then(TokenType.IDENTIFIER)
                        .Build())
                    .NextPattern(parseConfig.ArgsPattern)
                    .Build(),
                new SingleTokenPattern(TokenType.LINE_END));

            var programPattern1 = new StartEndTokensPattern(
                StrictPatternBuilder
                    .Reset()
                    .NextPattern(new SingleTokenPattern(TokenType.IDENTIFIER))
                    .NextPattern(parseConfig.ArgsPattern)
                    .Build(),
                new SingleTokenPattern(TokenType.LINE_END));

            var ifSyntaxRule = GetCommonOperatorParseRule(NodeType.IF, ifPattern);
            var elseIfSyntaxRule = GetCommonOperatorParseRule(NodeType.ELSE_IF, elseIfPattern);
            var forSyntaxRule = GetCommonOperatorParseRule(NodeType.FOR, forPattern);
            var whileSyntaxRule = GetCommonOperatorParseRule(NodeType.WHILE, whilePattern);
            var elseSyntaxRule = GetElseParseRule(elsePattern);
            var doWhileSyntaxRule = GetDoWhileParseRule(doWhilePattern);
            var inputSyntaxRule = GetSimpleOperatorParseRule(NodeType.INPUT, inputPattern);
            var outputSyntaxRule = GetSimpleOperatorParseRule(NodeType.OUTPUT, outputPattern);
            var processSyntaxRule = GetSimpleOperatorParseRule(NodeType.PROCESS, processPattern);
            var programSyntaxRule = GetSimpleOperatorParseRule(NodeType.PROGRAM, programPattern);
            var programSyntaxRule1 = GetSimpleOperatorParseRule(NodeType.PROGRAM, programPattern1);
            
            ParseRules = new List<ParseRule>()
            {
                ifSyntaxRule,
                elseIfSyntaxRule,
                elseSyntaxRule,
                forSyntaxRule,
                whileSyntaxRule,
                doWhileSyntaxRule,
                inputSyntaxRule,
                outputSyntaxRule,
                processSyntaxRule,
                programSyntaxRule,
                programSyntaxRule1,
            };
        }
    }
}