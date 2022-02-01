using System;
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
                var firstTokenMatch = TokenSearchUtils.FindToken(tokens, Seq[0], from);
                var seqStartIndex = firstTokenMatch.Start;
                while (seqStartIndex + Seq.Count <= tokens.Count)
                {
                    var allMatches = true;
                    // from 1 !!!
                    for (var i = 1; i < Seq.Count; i++)
                    {
                        if (Seq[i] != tokens[seqStartIndex + i].TokenType)
                        {
                            allMatches = false;
                            break;
                        }
                    }

                    if (allMatches)
                    {
                        return new PatternMatchResult(seqStartIndex, seqStartIndex + Seq.Count);
                    }
                    seqStartIndex++;
                    seqStartIndex = TokenSearchUtils.FindToken(tokens, Seq[0], seqStartIndex).Start;
                    if (seqStartIndex == -1)
                    {
                        return PatternMatchResult.Empty;
                    }
                }
            }
                return PatternMatchResult.Empty;
        }

        public override string ToString()
        {
            var res = "Sequence: [ " + string.Join(" -> ", Seq) + " ]";
            return res;
        }
    }
}