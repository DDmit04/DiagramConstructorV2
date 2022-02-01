using System;
using System.Collections.Generic;
using System.Linq;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.patternMatch;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.tokenPattern.boundaryPatterns
{
    public class TokensBlockWithExcludePattern : TokensBlockPattern
    {
        public override int ConditionsCount => 1;

        protected readonly List<TokenType> ExcludeList;

        public TokensBlockWithExcludePattern(TokenType blockStartToken, TokenType blockEndToken,
            List<TokenType> excludeList) : base(blockStartToken, blockEndToken)
        {
            ExcludeList = excludeList;
        }

        public override PatternMatchResult GetMatch(List<Token> tokens, int @from = 0)
        {
            var matchBlock = base.GetMatch(tokens, from);
            if (matchBlock.IsFullMatch)
            {
                var matchTokens = TokenUtils.GetMatchResultTokens(tokens, matchBlock);
                if (matchTokens.Any(t => ExcludeList.Contains(t.TokenType)))
                {
                    return PatternMatchResult.Empty;
                }
            }
            return matchBlock;
        }
        public override string ToString()
        {
            var res = "Block: [ " + BlockStartToken + " <any tokens except: (" + string.Join(" ,", ExcludeList) + ")> " + BlockEndToken + " ]";
            return res;
        }
    }
}