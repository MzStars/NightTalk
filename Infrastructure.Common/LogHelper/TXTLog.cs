using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.Common
{
    public class TXTLog
    {
        //在网站根目录下创建日志目录
        public static string path = System.AppDomain.CurrentDomain.BaseDirectory + "log";

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        private static readonly int logLEVENL = int.Parse(ConfigurationManager.AppSettings["LOG_LEVENL"] ?? "3");

        private static readonly object sequenceLock = new object();

        /**
         * 向日志文件写入调试信息
         * @param className 类名
         * @param content 写入内容
         */
        public static void Debug(string className, string content)
        {
            if (logLEVENL >= 3)
            {
                WriteLog("DEBUG", className, content);
            }
        }

        /**
        * 向日志文件写入运行时信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Info(string className, string content)
        {
            if (logLEVENL >= 2)
            {
                WriteLog("INFO", className, content);
            }
        }

        /**
        * 向日志文件写入出错信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Error(string className, string content)
        {
            if (logLEVENL >= 1)
            {
                WriteLog("ERROR", className, content);
            }
        }

        /**
        * 实际的写日志操作
        * @param type 日志记录类型
        * @param className 类名
        * @param content 写入内容
        */
        protected static void WriteLog(string type, string className, string content)
        {
            lock (sequenceLock)
            {
                //如果日志目录不存在就创建
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
                string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名
                string write_content = time + " " + type + " " + className + ": " + content;//向日志文件写入内容
                System.IO.StreamWriter sr = null;
                try
                {
                    if (!System.IO.File.Exists(filename))
                    {
                        sr = System.IO.File.CreateText(filename);
                    }
                    else
                    {
                        sr = System.IO.File.AppendText(filename);
                    }

                    sr.WriteLine(write_content);
                }
                catch
                {
                }
                finally
                {
                    if (sr != null)
                    {
                        sr.Close();
                    }
                }
            }
        }
    }
}
