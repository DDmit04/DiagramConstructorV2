using System.Collections.Generic;
using DiagramConstructorV3.app.tokenizer.data;
using DiagramConstructorV3.app.tokenPattern.patternMatch;
using DiagramConstructorV3.app.utils;

namespace DiagramConstructorV3.app.tokenPattern.commonPatterns
{
    public class TokenSequencePattern: TokenPattern
    {
        protected List<TokenType> Seq { get; }
        public override int ConditionsCount => Seq.Count;

        public TokenSequencePattern(List<TokenType> seq)
        {
            Seq = seq;
        }

        public override PatternMatchResult GetMatch(List<Token> tokens, int @from = 0)
        {
            if (Seq.Count > 0)
            {
                return TokenSearchUtils.FindTokensSequence(tokens, Seq, from);
            }
            else
            {
                return PatternMatchResult.Empty;
            }
        }

        public void AddToNexToken(TokenType newToken)
        {
            Seq.Add(newToken);
        }
    }
}