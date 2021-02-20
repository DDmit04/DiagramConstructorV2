using System;
using System.IO;

namespace DiagramConsructorV2.src.excaptions
{
    class ExceptionLogger
    {
        private static readonly string logFolder = AppDomain.CurrentDomain.BaseDirectory + "logs\\";
        private static readonly string logFileExtantion = ".txt";

        public static string logException(Exception exception, string codeToBuildDiagram, string additionalInfo = "")
        {
            string newLogFilePath = logFolder + "exception_" + getLogDate() + logFileExtantion;
            newLogFilePath = writeToLog(newLogFilePath, exception, codeToBuildDiagram, additionalInfo);
            return newLogFilePath;
        }

        public static string logWarning(Exception exception, string codeToBuildDiagram, string additionalInfo = "")
        { 
            string newLogFilePath = logFolder + "warning_" + getLogDate() + logFileExtantion;
            newLogFilePath = writeToLog(newLogFilePath, exception, codeToBuildDiagram, additionalInfo);
            return newLogFilePath;
        }

        private static string writeToLog(string filepath, Exception exception, string codeToBuildDiagram, string additionalInfo)
        {
            try
            {
                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }
                File.Create(filepath).Dispose();
                using (StreamWriter writer = File.AppendText(filepath))
                {
                    if (additionalInfo != "")
                    {
                        writer.WriteLine("Additional Info: " + additionalInfo);
                    }
                    writer.WriteLine("Date: " + DateTime.Now.ToString());
                    writer.WriteLine("Type: " + exception.GetType().ToString());
                    writer.WriteLine("Message: " + exception.Message.ToString());
                    writer.WriteLine("Stacktrace: \n" + exception.StackTrace.ToString());
                    writer.WriteLine("Code:\n" + codeToBuildDiagram);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return "";
            }
            return filepath;
        }

        private static string getLogDate()
        {
            return DateTime.Now.ToString("MM_dd_yyyy-HH_mm_ss");
        }

    }
}
