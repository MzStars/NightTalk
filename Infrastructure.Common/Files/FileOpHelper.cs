using System;
using System.IO;
using System.Web;

namespace Infrastructure.Common
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public class FileOpHelper
    {
        #region 文件操作
        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        public static bool DeleteFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return false;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除上传的文件(及缩略图)
        /// </summary>
        /// <param name="_filepath"></param>
        public static void DeleteUpFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return;
            }
            string thumbnailpath = _filepath.Substring(0, _filepath.LastIndexOf("/")) + "mall_" + _filepath.Substring(_filepath.LastIndexOf("/") + 1);
            string fullTPATH = GetMapPath(_filepath); //宿略图
            string fullpath = GetMapPath(_filepath); //原图
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
            if (File.Exists(fullTPATH))
            {
                File.Delete(fullTPATH);
            }
        }

        /// <summary>
        /// 返回文件大小KB
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>int</returns>
        public static int GetFileSize(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return 0;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                FileInfo fileInfo = new FileInfo(fullpath);
                return ((int)fileInfo.Length) / 1024;
            }
            return 0;
        }

        /// <summary>
        /// 返回文件扩展名，不含“.”
        /// </summary>
        /// <param name="_filepath">文件全名称</param>
        /// <returns>string</returns>
        public static string GetFileExt(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return "";
            }
            if (_filepath.LastIndexOf(".") > 0)
            {
                return _filepath.Substring(_filepath.LastIndexOf(".") + 1); //文件扩展名，不含“.”
            }
            return "";
        }

        /// <summary>
        /// 返回文件名，不含路径
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>string</returns>
        public static string GetFileName(string _filepath)
        {
            return _filepath.Substring(_filepath.LastIndexOf(@"/") + 1);
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>bool</returns>
        public static bool FileExists(string _filepath)
        {
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获得远程字符串
        /// </summary>
        public static string GetDomainStr(string key, string uriPath)
        {
            string result = string.Empty;// CacheHelper.Get(key) as string;
            if (result == null)
            {
                System.Net.WebClient client = new System.Net.WebClient();
                try
                {
                    client.Encoding = System.Text.Encoding.UTF8;
                    result = client.DownloadString(uriPath);
                }
                catch
                {
                    result = "暂时无法连接!";
                }
                //CacheHelper.Insert(key, result, 60);
            }

            return result;
        }

        /// <summary>
        /// 读取指定文件中的内容,文件名为空或找不到文件返回空串
        /// </summary>
        /// <param name="FileName">文件全路径</param>
        /// <param name="isLineWay">是否按行读取返回字符串 true为是</param>
        public static string GetFileContent(string FileName, bool isLineWay)
        {
            string result = string.Empty;
            using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    StreamReader sr = new StreamReader(fs);
                    if (isLineWay)
                    {
                        while (!sr.EndOfStream)
                        {
                            result += sr.ReadLine() + "\n";
                        }
                    }
                    else
                    {
                        result = sr.ReadToEnd();
                    }
                    sr.Close();
                    fs.Close();
                }
                catch (Exception ee)
                {
                    throw ee;
                }
            }
            return result;
        }

        #endregion

        #region 获得当前绝对路径
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http://"))
            {
                return strPath;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion

    }
}
