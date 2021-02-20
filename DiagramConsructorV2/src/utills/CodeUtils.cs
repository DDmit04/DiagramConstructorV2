using System;
using System.Text.RegularExpressions;

namespace DiagramConsructorV2.src.utills
{
    public class CodeUtils
    {

        /// <summary>
        /// Replace fist ocurence in some string of some string 
        /// </summary>
        /// <param name="text">text to search ocurence</param>
        /// <param name="textToReplace">text for replace</param>
        /// <param name="replace">text to replace </param>
        /// <returns>modifyed text</returns>
        public static String replaceFirst(string text, string textToReplace, string replace)
        {
            Regex regex = new Regex(Regex.Escape(textToReplace));
            text = regex.Replace(text, replace, 1);
            return text;
        }

        public static string removeLastOcur(string text, string replace)
        {
            int index = text.LastIndexOf(replace);
            if (index != -1)
            {
                text = text.Remove(index, 1);
            }
            return text;
        }

        public static string getNextLine(string code)
        {
            int lineBreakIndex = code.IndexOf('\n');
            if (lineBreakIndex != -1)
            {
                return code.Substring(0, lineBreakIndex + 1);
            }
            else
            {
                return code;
            }
        }

    }
}
