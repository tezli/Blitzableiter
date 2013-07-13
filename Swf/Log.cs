using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recurity.Swf
{
    /// <summary>
    /// Provides a log mechanism for thze Swf library
    /// </summary>
    public class Log
    {
        /// <summary>
        /// 
        /// </summary>
        public static event LogEventHandler LogInfoEvent;
        
        /// <summary>
        /// 
        /// </summary>
        public static event LogEventHandler LogsDebugEvent;
        
        /// <summary>
        /// 
        /// </summary>
        public static event LogEventHandler LogsWarningEvent;
        
        /// <summary>
        /// 
        /// </summary>
        public static event LogEventHandler LogsErrorEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="s"></param>
        public static void Info(object o, string s)
        {
            if(null != LogInfoEvent)
                LogInfoEvent(o, new LogEventArgs(LogLevel.Info, DateTime.Now, EscapeNonPrintables(s)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public static void Info(object o, Exception e)
        {
            string s = e.Message + e.InnerException;
            if (null != LogInfoEvent)
                LogInfoEvent(o, new LogEventArgs(LogLevel.Info, DateTime.Now, EscapeNonPrintables(s)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="s"></param>
        public static void Debug(object o, String s)
        {
            if (null != LogsDebugEvent)
                LogsDebugEvent(o, new LogEventArgs(LogLevel.Debug, DateTime.Now, EscapeNonPrintables(s)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public static void Debug(object o, Exception e)
        {
            string s = e.Message + e.InnerException;
            if (null != LogsDebugEvent)
                LogsDebugEvent(o, new LogEventArgs(LogLevel.Debug, DateTime.Now, EscapeNonPrintables(s)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="s"></param>
        public static void Warn(object o, string s)
        {
            if (null != LogsWarningEvent)
                LogsWarningEvent(o, new LogEventArgs(LogLevel.Warn, DateTime.Now, EscapeNonPrintables(s)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public static void Warn(object o, Exception e)
        {
            string s = e.Message + e.InnerException;
            if (null != LogsWarningEvent)
                LogsWarningEvent(o, new LogEventArgs(LogLevel.Warn, DateTime.Now, EscapeNonPrintables(s)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="s"></param>
        public static void Error(object o, string s)
        {
            if (null != LogsErrorEvent)
                LogsErrorEvent(o, new LogEventArgs(LogLevel.Error, DateTime.Now, EscapeNonPrintables(s)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public static void Error(object o, Exception e)
        {
            string s = e.Message;// +e.InnerException;
            if (null != LogsErrorEvent)
                LogsErrorEvent(o, new LogEventArgs(LogLevel.Error, DateTime.Now, EscapeNonPrintables(s)));
        }

        /// <summary>
        /// Replaces non printable characters by dots
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String EscapeNonPrintables(String s)
        {
            byte[] data = ASCIIEncoding.ASCII.GetBytes(s);
            for (int i = 0; i < data.Length; i++)
            {
                // All printables except CR anf LF
                if (!(data[i] == 13 || data[i] == 10 || data[i] == 9) && (data[i] < 32 || data[i] > 126))
                {
                    data[i] = Convert.ToByte('.');
                }
            }
            return ASCIIEncoding.ASCII.GetString(data);
        }
    }
}
