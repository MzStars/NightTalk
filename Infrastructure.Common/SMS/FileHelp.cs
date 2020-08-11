using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    /// <summary>
    /// File 的摘要说明。
    /// </summary>
    public sealed class FileHelp
    {
        /// <summary>
        /// 读取指定流中的文本内容。
        /// </summary>
        /// <param name="stream">流对象。</param>
        /// <param name="encoding">文本编码。</param>
        /// <returns>文件内容。</returns>
        public static string ReadContent(Stream stream, Encoding encoding)
        {
            string str = string.Empty;
            if (stream != null)
            {
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    str = reader.ReadToEnd();
                    reader.Close();
                }
            }
            return str;
        }

        public static bool Exists(string path, bool checkDirectory)
        {
            if (!checkDirectory)
            {
                return System.IO.File.Exists(path);
            }
            if (!System.IO.File.Exists(path))
            {
                return Directory.Exists(path);
            }
            return true;
        }

        public static bool Delete(string path)
        {
            try
            {
                if (Exists(path, false))
                {
                    File.Delete(path);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static string ReadFile(string filepath)
        {
            if (Exists(filepath, false))
            {
                using (StreamReader reader = new StreamReader(filepath, Encoding.GetEncoding("utf-8")))
                {
                    string str = reader.ReadToEnd();
                    reader.Close();
                    return str;
                }
            }

            return "Error: Not Exists " + filepath;
        }

    }
}
