using DiagramConstructor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramConsructorV2.src.Config
{
    class ExceptionLogger
    {

        public static string logException(Exception exception)
        {
            string exceptionType = exception.GetType().ToString();
            string trace = exception.StackTrace.ToString();
            string message = exception.Message.ToString();
            string newLogFolder = AppDomain.CurrentDomain.BaseDirectory + "logs\\";
            string newLogFilePath = newLogFolder + "excaption_" + DateTime.Now.ToString("MM_dd_yyyy-HH_mm_ss") + ".txt";
            try
            {
                if (!Directory.Exists(newLogFolder))
                {
                    Directory.CreateDirectory(newLogFolder);
                }
                File.Create(newLogFilePath).Dispose();
                using (StreamWriter writer = File.AppendText(newLogFilePath))
                {
                    writer.WriteLine("Excaption date: " + DateTime.Now.ToString());
                    writer.WriteLine("exceptionType: " + exceptionType);
                    writer.WriteLine("exceptionMessage: " + message);
                    writer.WriteLine("Stacktrace: \n" + trace);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return "";
            }
            return newLogFilePath;
        }

    }
}
