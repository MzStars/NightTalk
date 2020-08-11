using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    /// <summary>
    /// 图片帮助类
    /// </summary>
    public class ImageHelp
    {
        /// <summary>
        /// 生产图片
        /// </summary>
        /// <param name="validateNum"></param>
        /// <returns></returns>
        public static MemoryStream CreateImage(string validateNum)
        {
            if (validateNum == null || validateNum.Trim() == string.Empty)
                return null;
            //生成BitMap图像
            Bitmap image = new Bitmap(validateNum.Length * 12 + 12, 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景
                g.Clear(Color.White);
                //画图片的背景噪音线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, x2, y1, y2);
                }
                Font font = new Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateNum, font, brush, 2, 2);
                //画图片的前景噪音点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                MemoryStream ms = new MemoryStream();
                //将图像保存到指定流
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);

                return ms;

                //Response.ClearContent();
                //Response.ContentType = "image/Gif";
                //Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 图片转为base64编码的文本
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static string ImgToBase64String(MemoryStream ms)
        {
            try
            {
                //Bitmap bmp = new Bitmap(Imagefilename);
                ////this.pictureBox1.Image = bmp;
                //FileStream fs = new FileStream(Imagefilename + ".txt", FileMode.Create);
                //StreamWriter sw = new StreamWriter(fs);

                //MemoryStream ms = new MemoryStream();
                //bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                String strbaser64 = Convert.ToBase64String(arr);

                return strbaser64;
            }
            catch (Exception ex)
            {
                return $"ImgToBase64String 转换失败\nException: {ex.Message}";
            }
        }

        /// <summary>
        /// base64 转 Image
        /// </summary>
        /// <param name="base64"></param>
        public static MemoryStream Base64ToImage(string base64)
        {
            base64 = base64.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");//将base64头部信息替换
            byte[] bytes = Convert.FromBase64String(base64);
            return new MemoryStream(bytes);
        }

        /// <summary>
        /// 根据Base64 路径 名称 创建图片并保存
        /// </summary>
        /// <param name="base64"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public static string CreateImage(string base64, string fileName, string path = "Public")
        {

            var imageType = System.Drawing.Imaging.ImageFormat.Jpeg;
            if (base64.Contains("data:image/png;base64,"))
            {
                base64 = base64.Replace("data:image/png;base64,", "");
                imageType = System.Drawing.Imaging.ImageFormat.Png;
                fileName += ".png";
            }
            else if (base64.Contains("data:image/jpg;base64,"))
            {
                base64 = base64.Replace("data:image/jpg;base64,", "");
                fileName += ".jpg";
            }
            else if (base64.Contains("data:image/jgp;base64,"))
            {
                base64 = base64.Replace("data:image/jgp;base64,", "");
                fileName += ".jgp";
            }
            else if (base64.Contains("data:image/jpeg;base64,"))
            {
                base64 = base64.Replace("data:image/jpeg;base64,", "");
                fileName += ".jpeg";
            }
            else
            {
                throw new Exception("数据错误,不是图片的Base64");
            }

            string localhost = System.AppDomain.CurrentDomain.BaseDirectory;
            path = "Content\\File\\Image\\" + path + "\\";
            string savePath = localhost + path;

            //目录是否存在
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            //保存路径 = 目录 + 文件名称
            //转成二进制
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                //写入流
                var ms = new MemoryStream(bytes);
                //转成image格式
                Image image = Image.FromStream(ms);
                //保存
                image.Save(savePath + fileName, imageType);
                //释放资源
                image.Dispose();
            }
            catch (System.FormatException)
            {
                throw new Exception("base64数据缺损");
            }

            return System.Web.HttpContext.Current.Request.Url.Authority + "/" + path + fileName;


        }

        /// <summary>
        /// 网络图片转base64编码的文本
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static string ImgUrlToBase64(string strUrl)
        {
            WebRequest webreq = WebRequest.Create(strUrl);
            WebResponse webres = webreq.GetResponse();
            Stream stream = webres.GetResponseStream();
            MemoryStream outstream = new MemoryStream();
            const int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int count = 0;
            while ((count = stream.Read(buffer, 0, bufferLen)) > 0)
            {
                outstream.Write(buffer, 0, count);
            }
            return ImgToBase64String(outstream);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileUrl">路径</param>
        public static void DeleteImgFile(string fileUrl)
        {
            //Content\Image\Activity\2019110623434508651.jpeg

            string deletePath = System.AppDomain.CurrentDomain.BaseDirectory + fileUrl;
            if (System.IO.File.Exists(deletePath))
            {
                System.IO.File.Delete(deletePath);
            }

        }
    }
}
