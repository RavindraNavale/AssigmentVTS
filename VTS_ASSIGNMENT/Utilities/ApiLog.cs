using System;
using System.Configuration;
using System.IO;

namespace VTS_ASSIGNMENT
{
    public class ApiLog
    {
        public static void WriteLog(string message, string methodName)
        {
            try
            {
                string LogLocation = ConfigurationManager.AppSettings["LogLocation"].ToString();
                string logFile = LogLocation + "\\VTS_ASSIGNMENTLog" + DateTime.Now.ToString("ddMMMyyyy").Trim() + ".txt";

                if (!Directory.Exists(LogLocation))
                {
                    Directory.CreateDirectory(LogLocation);
                }
                if (!File.Exists(logFile))
                {
                    File.Create(logFile).Close();
                }
                using (StreamWriter streamWriter = new StreamWriter(logFile, true))
                {
                    streamWriter.WriteLineAsync("Datetime : " + DateTime.Now + ", Method Name : " + methodName + ", Exception : " + message);
                    streamWriter.Flush();
                    streamWriter.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}