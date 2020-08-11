using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Infrastructure.Common
{

    /// <summary>
    /// VerifiyHelper 的摘要说明
    /// </summary>
    public class VerifiyHelper
    {
        private static Color BackColor = Color.White;
        private static int Width = 62;
        private static int Height = 21;
        private Random _random;
        // private string _code;

        private int _brushNameIndex;

        public byte[] GetVCode(string codeStr)
        {
            _random = new Random();
            using (Bitmap img = new Bitmap(Width, Height))
            {
                // _code = GetRandomCode();
                // System.Web.HttpContext.Current.Session["vcode"] = _code;
                using (Graphics g = Graphics.FromImage(img))
                {
                    g.Clear(Color.White);//绘画背景颜色

                    Paint_Text(g, codeStr);// 绘画文字
                    // g.DrawString(strCode, new Font("微软雅黑", 15), Brushes.Blue, new PointF(5, 2));// 绘画文字
                    Paint_TextStain(img);// 绘画噪音点
                    g.DrawRectangle(Pens.DarkGray, 0, 0, Width - 1, Height - 1);//绘画边框
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        //将图片 保存到内存流中
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                        //将内存流 里的 数据  转成 byte 数组 返回
                        return ms.ToArray();
                    }
                }
            }

        }

        /// <summary>
        /// 绘画文字
        /// </summary>
        /// <param name="g"></param>
        private void Paint_Text(Graphics g, string code)
        {
            g.DrawString(code, GetFont(), GetBrush(), 3, 1);
        }

        /// <summary>
        /// 绘画文字噪音点
        /// </summary>
        /// <param name="g"></param>
        private void Paint_TextStain(Bitmap b)
        {
            string[] BrushName = new string[] {    "OliveDrab",
                                                  "ForestGreen",
                                                  "DarkCyan",
                                                  "LightSlateGray",
                                                  "RoyalBlue",
                                                  "SlateBlue",
                                                  "DarkViolet",
                                                  "MediumVioletRed",
                                                  "IndianRed",
                                                  "Firebrick",
                                                  "Chocolate",
                                                  "Peru",
                                                  "Goldenrod"
                                             };

            for (int n = 0; n < 30; n++)
            {
                int x = _random.Next(Width);
                int y = _random.Next(Height);
                b.SetPixel(x, y, Color.FromName(BrushName[_brushNameIndex]));

            }

        }
        /// <summary>
        /// 随机取一个字体
        /// </summary>
        /// <returns></returns>
        private Font GetFont()
        {
            string[] FontItems = new string[]{   "Arial",
                                                  "Helvetica",
                                                  "Geneva",
                                                  "sans-serif",
                                                  "Verdana"
                                              };

            int fontIndex = _random.Next(0, FontItems.Length);
            FontStyle fontStyle = GetFontStyle(_random.Next(0, 2));
            return new Font(FontItems[fontIndex], 12, fontStyle);
        }
        /**/
        /**/
        /**/
        /// <summary>
        /// 随机取一个笔刷
        /// </summary>
        /// <returns></returns>
        private Brush GetBrush()
        {
            Brush[] BrushItems = new Brush[]{     Brushes.OliveDrab,
                                                  Brushes.ForestGreen,
                                                  Brushes.DarkCyan,
                                                  Brushes.LightSlateGray,
                                                  Brushes.RoyalBlue,
                                                  Brushes.SlateBlue,
                                                  Brushes.DarkViolet,
                                                  Brushes.MediumVioletRed,
                                                  Brushes.IndianRed,
                                                  Brushes.Firebrick,
                                                  Brushes.Chocolate,
                                                  Brushes.Peru,
                                                  Brushes.Goldenrod
                                            };

            int brushIndex = _random.Next(0, BrushItems.Length);
            _brushNameIndex = brushIndex;
            return BrushItems[brushIndex];
        }

        /// <summary>
        /// 绘画背景颜色
        /// </summary>
        /// <param name="g"></param>
        private void Paint_Background(Graphics g)
        {
            g.Clear(BackColor);
        }
        /**/
        /**/
        /**/
        /// <summary>
        /// 取一个字体的样式
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private FontStyle GetFontStyle(int index)
        {
            switch (index)
            {
                case 0:
                    return FontStyle.Bold;
                case 1:
                    return FontStyle.Italic;
                default:
                    return FontStyle.Regular;
            }
        }

        /// <summary>
        /// 取得一个 4 位的随机码
        /// </summary>
        /// <returns></returns>
        public string GetRandomCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 5);
        }
    }

    //使用：

    //前台HTML

    // <img id="checkloing" src="/Login/VCode" class="loginFormCheckCodeImg" onclick="reloadcode('/Login/VCode')" title="Can not see clearly, change one." />

    //JS:

    ////刷新验证码
    //function reloadcode(srcStr) {
    //    document.getElementById("checkloing").src = srcStr + "?rand=" + Math.random();
    //}

    //Controller

    ///// <summary>
    ///// 生成验证码
    ///// </summary>
    ///// <returns></returns>
    //[OutputCache(Duration = 10, VaryByParam = "rand")]//缓存10秒钟
    //[AllowAnonymous]//跳过登陆验证
    //public ActionResult VCode()
    //{
    //    VerificationCodeHelper vcode = new VerificationCodeHelper();
    //    string codeStr = vcode.GetRandomCode();
    //    if (!string.IsNullOrEmpty(codeStr))
    //    {
    //        byte[] arrImg = vcode.GetVCode(codeStr);
    //        Session["code"] = codeStr;
    //        return File(arrImg, "image/gif");
    //    }
    //    else
    //    {
    //        return RedirectToAction("/控制器/VCode?rand=" + Guid.NewGuid().ToString().Substring(1, 10), "image/jpeg");
    //    }
    //}
}