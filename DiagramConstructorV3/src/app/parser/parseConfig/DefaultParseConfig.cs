using System;
using System.Linq;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.boundaryPatterns;
using DiagramConstructorV3.app.tokenPattern.builders;
using DiagramConstructorV3.app.tokenPattern.commonPatterns;

namespace DiagramConstructorV3.app.parser.parseConfig
{
    public class DefaultParseConfig : ParseConfig
    {
        public DefaultParseConfig()
        {
            MainMethodName = "main";
            ArgsPattern = new TokensBlockWithExcludePatternBuilder()
                .Reset()
                .StartWith(TokenType.ARGS_OPEN)
                .ExceptRange(
                    Enum.GetValues(typeof(TokenType))
                        .Cast<TokenType>()
                        .Where(t => t.IsOperatorToken())
                        .ToList())
                .Except(TokenType.ACCESS_MODIFICATOR)
                .EndWith(TokenType.ARGS_CLOSE)
                .Build();
            MethodDefPattern = new StrictComboPatternBuilder()
                .Reset()
                .NextPattern(new SingleTokenPattern(TokenType.IDENTIFIER))
                .NextPattern(ArgsPattern)
                .NextPattern(TokensBlockPattern.BracketBlock)
                .Build();
        }
    }
}