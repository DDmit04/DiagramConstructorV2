using System;
using System.IO;
using System.Linq;

namespace DiagramConstructorV3.app.exceptions
{
    static class ExceptionLogger
    {
        private static readonly string LogFolder = AppConfiguration.LogFolder;
        private static readonly string LogFileExt = AppConfiguration.LogFileExt;

        public static string LogException(Exception exception, string codeToBuildDiagram, string additionalInfo = "")
        {
            var newLogFilePath = LogFolder + "exception_" + GetLogDate() + LogFileExt;
            newLogFilePath = WriteToLog(newLogFilePath, exception, codeToBuildDiagram, additionalInfo);
            return newLogFilePath;
        }

        public static string LogParseException(ParseException exception, string codeToBuildDiagram,
            string additionalInfo = "")
        {
            additionalInfo +=
                $"Node parse error: {exception.ParsedNodeType}! Error tokens start - [{string.Join(", ", exception.ErrorTokens.Select(t => t.TokenType))}]";
            return LogException(exception, codeToBuildDiagram, additionalInfo);
        }

        public static string LogWarning(Exception exception, string codeToBuildDiagram, string additionalInfo = "")
        {
            var newLogFilePath = LogFolder + "warning_" + GetLogDate() + LogFileExt;
            newLogFilePath = WriteToLog(newLogFilePath, exception, codeToBuildDiagram, additionalInfo);
            return newLogFilePath;
        }

        private static string WriteToLog(string filepath, Exception exception, string codeToBuildDiagram,
            string additionalInfo)
        {
            try
            {
                if (!Directory.Exists(LogFolder))
                {
                    Directory.CreateDirectory(LogFolder);
                }

                File.Create(filepath).Dispose();
                using (var writer = File.AppendText(filepath))
                {
                    if (additionalInfo != "")
                    {
                        writer.WriteLine("Additional Info: " + additionalInfo);
                    }

                    writer.WriteLine("Date: " + DateTime.Now);
                    writer.WriteLine("Type: " + exception.GetType());
                    writer.WriteLine("Message: " + exception.Message);
                    writer.WriteLine("Stacktrace: \n" + exception.StackTrace);
                    writer.WriteLine("Code:\n" + codeToBuildDiagram);
                }
            }
            catch (Exception ex)
            {
                return "";
            }

            return filepath;
        }

        private static string GetLogDate()
        {
            return DateTime.Now.ToString("MM_dd_yyyy-HH_mm_ss");
        }
    }
}