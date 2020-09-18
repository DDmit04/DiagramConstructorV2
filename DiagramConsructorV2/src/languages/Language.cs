using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramConsructorV2.fileTypes
{
    class Language
    {
        public List<string> fileExtensions { get; }
        public string displayName { get; }
        public string langFileDicription { get; }

        public Language(string displayName, List<string> extensions, string fileDicription)
        {
            this.displayName = displayName;
            this.fileExtensions = extensions;
            this.langFileDicription = fileDicription;
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
