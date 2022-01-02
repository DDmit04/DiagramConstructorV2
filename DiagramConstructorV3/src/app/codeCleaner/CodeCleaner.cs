using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DiagramConstructorV3.app.codeCleaner
{
    public class CodeCleaner
    {
        protected readonly List<Regex> ReplaceRegexps = new List<Regex>();

        public virtual string CleanCodeBeforeParse(string code)
        {
            foreach (var regexp in ReplaceRegexps)
            {
                code = regexp.Replace(code, "");
            }
            return code;
        }

    }
}
