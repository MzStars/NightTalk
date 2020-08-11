using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class Log
    {
        //在网站根目录下创建日志目录
        public static string path = "~/Log/WxLog";
        public static string pathError = "~/Log/ErrorLog/";
        public static string ApplicationStatus = "~/Log/ApplicationLog/";
        /**
         * 向日志文件写入调试信息
         * @param className 类名
         * @param content 写入内容
         */
        public static void Debug(string className, string content, int? state = 0)
        {

            WriteLog("DEBUG", className, content, state);

        }

        /**
        * 向日志文件写入运行时信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Info(string className, string content, int? state = 0)
        {

            WriteLog("INFO", className, content, state);

        }

        /**
        * 向日志文件写入出错信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Error(string className, string content, int? state = 0)
        {

            WriteLog("ERROR", className, content, state);

        }

        /**
        * 实际的写日志操作
        * @param type 日志记录类型
        * @param className 类名
        * @param content 写入内容
        */
        protected static bool WriteLog(string type, string className, string content, int? state)
        {

            try
            {


                var paths = System.Web.Hosting.HostingEnvironment.MapPath(path);//微信出错
                if (state == 1)
                {
                    //普通出错
                    paths = System.Web.Hosting.HostingEnvironment.MapPath(pathError);
                }
                if (state == 2)
                {
                    //重启 或者 项目回收、崩溃
                    paths = System.Web.Hosting.HostingEnvironment.MapPath(ApplicationStatus);
                }
                if (!Directory.Exists(paths))//如果日志目录不存在就创建
                {
                    Directory.CreateDirectory(paths);
                }

                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
                string filename = paths + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名

                //创建或打开日志文件，向日志文件末尾追加记录
                StreamWriter mySw = File.AppendText(filename);

                //向日志文件写入内容
                string write_content = type + ": " + time + " " + className + ": " + content;
                mySw.WriteLine(write_content);

                //关闭日志文件
                mySw.Close();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
