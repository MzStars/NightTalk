using Infrastructure.Common;
using NightTalk.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace NightTalk.Controllers
{
    public class ImageController : BaseApi
    {
        private static readonly object WXQrCodeSequenceLock = new object();
        private static string appId = ConfigHelper.AppSettings("WecharAppid");
        private readonly WeCharUserInfoService _weCharUserInfoService;
        public ImageController(WeCharUserInfoService weCharUserInfoService)
        {
            _weCharUserInfoService = weCharUserInfoService;
        }


        /// <summary>
        /// 微信小程序码
        /// </summary>
        [HttpPost]
        [Route("api/image/WxQrcode")]
        public JsonResult<object> WXQrcode(WXQrcodeRequest request)
        {
            if (request == null)
            {
                return JsonError("请求参数错误");
            }
            if (!string.IsNullOrEmpty(request.ShareCode))
            {
                if (!_weCharUserInfoService.ExistShareCode(request.ShareCode))
                {
                    return JsonError("该分享码不存在");
                }
                request.Pages += "?sharecode=" + request.ShareCode;
            }
            var fileName = request.Pages.Replace("/", "_").Replace("?","-") + request.Width + ".jpg";
            var dirpath = System.Web.HttpContext.Current.Server.MapPath("~/Upload/WecharImg/");
            var savePath = dirpath + fileName;
            var servicePath = "/Upload/WecharImg/" + fileName;
            if (File.Exists(savePath))
            {
                return JsonNet(servicePath);
            }

            lock (WXQrCodeSequenceLock)
            {
                using (var ms = new MemoryStream())
                {
                    var result = WXHelper.GetWxaCode(ms, request.Pages, request.Width);
                    if (result.errcode == Senparc.Weixin.ReturnCode.请求成功)
                    {
                        Image image = Image.FromStream(ms);
                        if (!Directory.Exists(dirpath))
                        {
                            Directory.CreateDirectory(dirpath);
                        }
                        image.Save(savePath);
                        image.Dispose();
                    }
                    else
                    {
                        return JsonError(result.errmsg);
                    }

                }
            }

            return JsonNet(servicePath);
        }

        /// <summary>
        /// 重新获取小程序入口图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/image/GetWxImage")]
        public JsonResult<object> GetWxImage(GetWxImageRequest reques)
        {
            //var temp = WXHelper.GetWxaCode();
            string path = WXHelper.GetWxImage(reques.Pages, "1", reques.FileName);
            if (string.IsNullOrEmpty(path))
            {
                return JsonError("生成小程序码失败");
            }
            return JsonNet(path);
        }

        /// <summary>
        /// 获取封面图片
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/image/GetConverImage")]
        public JsonResult<object> GetConverImage()
        {
            //物理路径
            //var fileUrl = HttpContext.Current.Server.MapPath(path + imgName);//图片地址
            //返回路径
            //var returnUrl = System.Web.HttpContext.Current.Request.Url.Authority + path + imgName;
            string path = HttpContext.Current.Server.MapPath("/Content/CoverImage");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var imgs = Directory.GetFiles(path, "*.jpg").ToList();
            imgs.AddRange(Directory.GetFiles(path, "*.png").ToList());
            if (imgs.Count() == 0)
            {
                return JsonError("没有封面图,请联系管理员添加");
            }
            RandomHelper rand = new RandomHelper();
            var number = rand.GetRandomInt(0, imgs.Count());
            var returnPath = imgs[number];
            //根目录
            string rootDirectory = HttpContext.Current.Server.MapPath("/");
            returnPath = returnPath.Replace(rootDirectory, ""); //转换成相对路径
            imgs = imgs.Select(x => "/" + x.Replace(rootDirectory, "").Replace(@"\", @"/")).ToList();
            //斜杠处理
            returnPath = returnPath.Replace(@"\", @"/");
            //链接拼接
            returnPath = "/" + returnPath;

            return JsonNet(imgs);
        }

        /// <summary>
        /// 测试API
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/image/Temp")]
        public JsonResult<object> Temp(TempRequest reques)
        {
            var fileName = "pages/index/index".Replace("/", "_") + 4301 + ".jpg";
            var dirpath = System.Web.HttpContext.Current.Server.MapPath("~/Upload/WecharImg/");
            var savePath = dirpath + fileName;
            var servicePath = "/Upload/WecharImg/" + fileName;

            lock (WXQrCodeSequenceLock)
            {
                using (var ms = new MemoryStream())
                {
                    var result = WXHelper.Temp(ms, "/pages/artcle/index?temp=1", 430, reques.R, reques.G, reques.B);
                    if (result.errcode == Senparc.Weixin.ReturnCode.请求成功)
                    {
                        Image image;
                        try
                        {
                            image = Image.FromStream(ms);
                        }
                        catch (System.ArgumentException)
                        {
                            WXHelper.RemoveAccessToken();
                            throw new Exception("生成图片失败");
                        }
                        if (!Directory.Exists(dirpath))
                        {
                            Directory.CreateDirectory(dirpath);
                        }
                        image.Save(savePath);
                        image.Dispose();
                    }
                    else
                    {
                        return JsonError(result.errmsg);
                    }

                }
            }
            return JsonNet(servicePath);
        }
    }
}