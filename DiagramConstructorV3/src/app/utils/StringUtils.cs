using System.Text.RegularExpressions;

namespace DiagramConstructorV3.app.utils
{
    public static class StringUtils
    {
        public static string ReplaceFirst(string text, string textToReplace, string replace)
        {
            var regex = new Regex(Regex.Escape(textToReplace));
            text = regex.Replace(text, replace, 1);
            return text;
        }
    }
}
