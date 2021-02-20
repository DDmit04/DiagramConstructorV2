using DiagramConsructorV2.src.actor.codeFormatter;
using DiagramConsructorV2.src.actor.codeParser;
using DiagramConstructorV2.src.nodeThreeAnylizer;
using System.Collections.Generic;
using System.Linq;

namespace DiagramConsructorV2.src.lang
{
    public abstract class Language
    {
        public readonly List<string> fileExtensions;
        public readonly string displayName;
        public readonly string langFileDicription;

        public readonly CodeFormatter codeFormatter;
        public readonly NodeThreeAnalyzer nodeThreeAnalyzer;
        public readonly CodeParser codeParser;

        public Language(string displayName, List<string> fileExtensions, string langFileDicription,
            CodeFormatter codeFormatter, NodeThreeAnalyzer nodeThreeAnalyzer, CodeParser codeParser)
        {
            this.displayName = displayName;
            this.fileExtensions = fileExtensions;
            this.langFileDicription = langFileDicription;
            this.codeFormatter = codeFormatter;
            this.nodeThreeAnalyzer = nodeThreeAnalyzer;
            this.codeParser = codeParser;
        }

        public virtual bool checkExtantions(string extantionToCheck)
        {
            return fileExtensions.Count(extantion => extantion.ToUpperInvariant() == extantionToCheck.ToUpperInvariant()) > 0;
        }

        public string getExtantionsFileConditions()
        {
            string result = "";
            for(int i = 0; i < fileExtensions.Count; i++)
            {
                string extantion = fileExtensions[i];
                result += $"{langFileDicription} (*{extantion})|*{extantion}";
                if(i != fileExtensions.Count - 1)
                {
                    result += "|";
                }
            }
            return result;
        }

    }
}
