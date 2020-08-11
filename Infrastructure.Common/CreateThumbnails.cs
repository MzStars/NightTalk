using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class CreateThumbnails
    {



        /// <summary>
        /// 创建缩略图
        /// </summary>
        public static void cutAvatar(string imgSrc, int x, int y, int width, int height, long Quality, string SavePath, int t)
        {
            Image original = Image.FromFile(imgSrc);

            Bitmap img = new Bitmap(t, t, PixelFormat.Format24bppRgb);

            img.MakeTransparent(img.GetPixel(0, 0));
            img.SetResolution(72, 72);
            using (Graphics gr = Graphics.FromImage(img))
            {
                if (original.RawFormat.Equals(ImageFormat.Jpeg) || original.RawFormat.Equals(ImageFormat.Png) || original.RawFormat.Equals(ImageFormat.Bmp))
                {
                    gr.Clear(Color.Transparent);
                }
                if (original.RawFormat.Equals(ImageFormat.Gif))
                {
                    gr.Clear(Color.White);
                }


                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                using (var attribute = new System.Drawing.Imaging.ImageAttributes())
                {
                    attribute.SetWrapMode(WrapMode.TileFlipXY);
                    gr.DrawImage(original, new Rectangle(0, 0, t, t), x, y, width, height, GraphicsUnit.Pixel, attribute);
                }
            }
            ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
            if (original.RawFormat.Equals(ImageFormat.Jpeg))
            {
                myImageCodecInfo = GetEncoderInfo("image/jpeg");
            }
            else
                if (original.RawFormat.Equals(ImageFormat.Png))
            {
                myImageCodecInfo = GetEncoderInfo("image/png");
            }
            else
                    if (original.RawFormat.Equals(ImageFormat.Gif))
            {
                myImageCodecInfo = GetEncoderInfo("image/gif");
            }
            else
                        if (original.RawFormat.Equals(ImageFormat.Bmp))
            {
                myImageCodecInfo = GetEncoderInfo("image/bmp");
            }

            Encoder myEncoder = Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, Quality);
            myEncoderParameters.Param[0] = myEncoderParameter;
            img.Save(SavePath, myImageCodecInfo, myEncoderParameters);
        }

        /// <summary>
        /// 根据长宽自适应 按原图比例缩放
        /// </summary>
        /// <param name="original"></param>
        /// <param name="desiredWidth"></param>
        /// <param name="desiredHeight"></param>
        /// <returns></returns>

        private static Size GetThumbnailSize(System.Drawing.Image original, int desiredWidth, int desiredHeight)
        {
            var widthScale = (double)desiredWidth / original.Width;
            var heightScale = (double)desiredHeight / original.Height;
            var scale = widthScale < heightScale ? widthScale : heightScale;
            return new Size
            {
                Width = (int)(scale * original.Width),
                Height = (int)(scale * original.Height)
            };
        }


        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

    }
}
