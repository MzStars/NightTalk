using Infrastructure.Common;
using NightTalkBack.Models;
using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace NightTalkBack.Controllers
{
    /// <summary>
    /// 图片
    /// </summary>
    public class UploadController : BaseApi
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly FileInfoService _fileInfoService;
        /// <summary>
        /// 构造
        /// </summary>
        public UploadController(FileInfoService fileInfoService)
        {
            _fileInfoService = fileInfoService;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="goodsid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/upload/UploadImage")]
        public JsonResult<object> UploadImage([FromBody]string imgBase64)
        {
            var baseInfo = GetBaseInfo();
            string fileName = Guid.NewGuid().ToString().Replace("-", "");
            var filePath = ImageHelp.CreateImage(imgBase64, fileName, "upload");

            return JsonNet(filePath);

        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="goodsid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/upload/UploadFile")]
        public JsonResult<object> UploadFile()
        {
            var baseInfo = GetBaseInfo();
            List<string> listPath = new List<string>();
            HttpFileCollection filelist = HttpContext.Current.Request.Files;
            var Authority = System.Web.HttpContext.Current.Request.Url.Authority;
            string localhost = System.AppDomain.CurrentDomain.BaseDirectory;

            if (filelist != null && filelist.Count > 0)
            {
                for (int i = 0; i < filelist.Count; i++)
                {
                    HttpPostedFile file = filelist[i];
                    String Tpath = DateTime.Now.ToString("yyyy-MM-dd");
                    string filename = file.FileName;
                    string fileSuffix = file.FileName.Split('.').Last().ToLower();
                    string fileName = Guid.NewGuid().ToString().Replace("-", "");
                    string FilePath = "\\Content\\File\\" + fileSuffix + "\\" + Tpath + "\\";
                    string savePath = localhost + FilePath;
                    string servicePath = "/" + (FilePath + fileName + "." + fileSuffix).Replace("\\", "/");
                    //目录是否存在
                    Directory.CreateDirectory(savePath);
                    try
                    {

                        file.SaveAs(savePath + fileName + "." + fileSuffix);
                        listPath.Add(servicePath);


                        FileInfoDomain domain = new FileInfoDomain();
                        domain.FileName = fileName;
                        domain.FilePath = servicePath;
                        domain.FileSuffix = fileSuffix;
                        if (fileSuffix == "jpg" || fileSuffix == "webp" || fileSuffix == "jpeg" || fileSuffix == "bmp")
                            domain.FileType = FileTypeEnumDomain.图片;
                        else if (fileSuffix == "mp4" || fileSuffix == "avi" || fileSuffix == "wmv")
                            domain.FileType = FileTypeEnumDomain.视频;
                        else
                            domain.FileType = FileTypeEnumDomain.其他;
                        domain.CreateTime = DateTime.Now;
                        domain.ID = Guid.NewGuid();
                        domain.UID = baseInfo.UID;
                        _fileInfoService.NewCreate(domain);

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("上传文件写入失败：" + ex.Message);
                    }
                }
            }
            else
            {
                throw new Exception("上传的文件信息不存在! ");
            }

            return JsonNet(listPath);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/fileInfo/GetListData")]
        public JsonResult<object> GetListData(GetListDataRequest request) 
        {
            if (request == null)
            {
                return JsonError("请求参数错误");
            }
            int listCount = 0;
            var list = _fileInfoService.GetListData(out listCount,request.PageIndex,request.PageSize,request.FileType,request.UID, request.FileName);

            return JsonNet(list,listCount);
        }
    }
}