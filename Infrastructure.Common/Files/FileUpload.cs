using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.Common
{
    public class FileUpload
    {

        #region 私有成员

        /// <summary>
        /// 最大单个上传文件 (默认)
        /// </summary>
        private int _MaxSize = 1024 * 1024 * 1024;

        /// <summary>
        /// 所支持的上传类型用";"隔开 
        /// </summary>
        private string _FileType = "jpg;jpge;png;gif;doc;docx;xls;xlsx;txt;rar;zip;pdf;mp3";

        /// <summary>
        /// 上传控件
        /// </summary>
        private HttpPostedFile _FormFile;

        /// <summary>
        /// 上传控件
        /// </summary>
        private HttpPostedFileBase _FormFileBase;

        /// <summary>
        /// 生成文件名设置，为空代表自动生成文件名
        /// </summary>
        private string _InFileName = "";

        #endregion

        #region 公有属性

        /// <summary>
        /// 返回上传状态
        /// </summary>
        public bool Success { get; private set; } = false;

        /// <summary>
        /// 返回上传消息
        /// </summary>
        public string Message { get; private set; } = "";

        /// <summary>
        /// 最大单个上传文件
        /// </summary>
        public int MaxSize
        {
            set { _MaxSize = value; }
        }

        /// <summary>
        /// 所支持的上传类型用";"隔开 
        /// </summary>
        public string FileType
        {
            set { _FileType = value; }
        }

        /// <summary>
        /// 保存文件的实际路径 
        /// </summary>
        public string SavePath { set; get; } = ConfigHelper.GetAppConfigValue("uploads_files");

        /// <summary>
        /// 返回保存后的文件路径
        /// </summary>
        public string ToSavePath { get; private set; } = "";

        /// <summary>
        /// 上传控件
        /// </summary>
        public HttpPostedFile FormFile
        {
            set { _FormFile = value; }
        }

        /// <summary>
        /// 上传控件
        /// </summary>
        public HttpPostedFileBase FormFileBase
        {
            set { _FormFileBase = value; }
        }

        /// <summary>
        /// 默认 HttpPostedFile，false：HttpPostedFileBase
        /// </summary>
        public bool FormFileType { get; private set; } = true;

        /// <summary>
        /// 是否更改文件名，false：否
        /// </summary>
        public bool IsChangeFileName { get; private set; } = false;

        /// <summary>
        /// 非自动生成文件名设置。
        /// </summary>
        public string InFileName
        {
            set { _InFileName = value; }
        }

        /// <summary>
        /// 输出文件名
        /// </summary>
        public string OutFileName { get; private set; } = "";

        /// <summary>
        /// 输出文件的后缀
        /// </summary>
        public string OutFileExt { get; private set; } = "";

        /// <summary>
        /// 获取已经上传文件的大小
        /// </summary>
        public long FileSize { get; private set; } = 0;

        /// <summary>
        /// 获取已经上传文件的大小 已换算
        /// </summary>
        public string ToFileSize { get; private set; } = "0 B";

        #endregion

        #region 私有方法

        /// <summary>
        /// 是否为图片文件
        /// </summary>
        /// <param name="Ext">文件扩展名，不含“.”</param>
        /// <returns></returns>
        private bool IsImage(string Ext)
        {
            ArrayList al = new ArrayList();
            al.Add("bmp");
            al.Add("jpeg");
            al.Add("jpg");
            al.Add("gif");
            al.Add("png");
            if (al.Contains(Ext.ToLower()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取文件的后缀名 
        /// </summary>
        private string GetExt(string path)
        {
            return Path.GetExtension(path).Replace(".", "");
        }

        /// <summary>
        /// 获取输出文件的文件名
        /// </summary>
        private string GetFileName(string Ext)
        {
            if (_InFileName.Trim() == "")
                return DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + Ext;
            else
                return _InFileName + "." + Ext;
        }


        /// <summary>
        /// 获取输出文件的文件名
        /// </summary>
        private string GetFileName(string FileName, string Ext)
        {
            if (FileName.Trim() == "")
                return DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + Ext;
            else
                return FileName;
        }


        /// <summary>
        /// 创建一个目录
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        private void CreateDirectory(string directoryPath)
        {
            //如果目录不存在则创建该目录
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        
        /// <summary>
        /// 计算文件大小函数(保留两位小数)，Size为字节大小，获取文件大小并以B，KB，GB，TB
        /// </summary>
        /// <param name="size">初始文件大小</param>
        /// <returns></returns>
        private string GetToFileSize(long size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " B";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " KB";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " MB";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " GB";
            return m_strSize;
        }
        
        /// <summary>
        /// 检查上传的文件的类型，是否允许上传。
        /// </summary>
        private bool IsUpload(string Ext)
        {
            bool b = false;
            string[] arrFileType = _FileType.Split(';');
            foreach (string str in arrFileType)
            {
                if (str.ToLower() == Ext.ToLower())
                {
                    b = true;
                    break;
                }
            }
            return b;
        }


        #endregion

        #region 文件上传

        /// <summary>
        /// HttpPostedFile
        /// </summary>
        public void Upload()
        {
            //文件后缀
            string Ext = GetExt(_FormFile.FileName);
            //文件名
            string FileName = GetFileName(Ext);
            //获得文件大小，以字节为单位
            long FileLength = _FormFile.ContentLength;


            if (_FormFile == null || _FormFile.FileName.Trim() == "")
            {
                Message = "请选择要上传文件！";
                return;
            }
            if (!IsUpload(Ext))
            {
                Message = "不允许上传" + Ext + "类型的文件！";
                return;
            }
            if (FileLength > _MaxSize)
            {
                Message = "文件超过限制的大小！";
                return;
            }
            try
            {
                //检查上传的物理路径是否存在，不存在则创建
                string SavePath = HttpContext.Current.Server.MapPath(this.SavePath);
                CreateDirectory(SavePath);
                
                //保存文件
                _FormFile.SaveAs(SavePath + FileName);
                
                //返回
                OutFileName = FileName;
                OutFileExt = Ext;
                ToSavePath = this.SavePath + FileName;
                FileSize = FileLength;
                ToFileSize = GetToFileSize(FileSize);
                
                Success = true;
                Message = "上传成功";
                return;
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
            {
                Message = "上传失败！";
                return;
            }
        }

        /// <summary>
        /// HttpPostedFileBase
        /// </summary>
        public void Uploads()
        {
            //文件后缀
            string Ext = GetExt(_FormFileBase.FileName);
            //文件名
            string FileName = GetFileName(Ext);
            //获得文件大小，以字节为单位
            long FileLength = _FormFileBase.ContentLength;


            if (_FormFileBase == null || _FormFileBase.FileName.Trim() == "")
            {
                Message = "请选择要上传文件！";
                return;
            }
            if (!IsUpload(Ext))
            {
                Message = "不允许上传" + Ext + "类型的文件！";
                return;
            }
            if (FileLength > _MaxSize)
            {
                Message = "文件超过限制的大小！";
                return;
            }
            try
            {
                //检查上传的物理路径是否存在，不存在则创建
                string SavePath = HttpContext.Current.Server.MapPath(this.SavePath);
                CreateDirectory(SavePath);

                //保存文件
                _FormFileBase.SaveAs(SavePath + FileName);

                //返回
                OutFileName = FileName;
                OutFileExt = Ext;
                ToSavePath = this.SavePath + FileName;
                FileSize = FileLength;
                ToFileSize = GetToFileSize(FileSize);

                Success = true;
                Message = "上传成功";
                return;
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
            {
                Message = "上传失败！";
                return;
            }
        }

        #endregion

        #region 上传图片

        /// <summary>
        /// 图片上传 HttpPostedFile
        /// </summary>
        public void ImgUpload()
        {
            //文件后缀
            string Ext = GetExt(_FormFile.FileName);
            //文件名
            string FileName = GetFileName(Ext);
            //获得文件大小，以字节为单位
            long FileLength = _FormFile.ContentLength;

            if (_FormFile == null || _FormFile.FileName.Trim() == "")
            {
                Message = "请选择要上传图片！";
                return;
            }
            if (!IsImage(Ext))
            {
                Message = "不允许上传【" + Ext + "】类型的图片！";
                return;
            }
            if (FileLength > _MaxSize)
            {
                Message = "图片超过限制的大小（" + (_MaxSize / 1024 / 1024) + "M）！";
                return;
            }
            try
            {
                //检查上传的物理路径是否存在，不存在则创建
                //保存路径
                string SavePath = HttpContext.Current.Server.MapPath(this.SavePath);
                CreateDirectory(SavePath);
                
                //保存文件
                _FormFile.SaveAs(SavePath + FileName);

                //返回
                OutFileName = FileName;
                OutFileExt = Ext;
                ToSavePath = this.SavePath + FileName;
                FileSize = FileLength;
                ToFileSize = GetToFileSize(FileSize);

                Success = true;
                Message = "上传成功！";
                return;
            }
            catch (Exception ex)
            {
                Message = "上传失败！" + ex.Message;
                return;
            }
        }

        /// <summary>
        /// HttpPostedFileBase
        /// </summary>
        public void ImgUploads()
        {
            //文件后缀
            string Ext = GetExt(_FormFileBase.FileName);
            //文件名
            string FileName = GetFileName(Ext);
            //获得文件大小，以字节为单位
            long FileLength = _FormFileBase.ContentLength;


            if (_FormFileBase == null || _FormFileBase.FileName.Trim() == "")
            {
                Message = "请选择要上传图片！";
                return;
            }
            if (!IsUpload(Ext))
            {
                Message = "不允许上传" + Ext + "类型的图片！";
                return;
            }
            if (FileLength > _MaxSize)
            {
                Message = "图片超过限制的大小（" + (_MaxSize / 1024 / 1024) + "M）！";
                return;
            }
            try
            {
                //检查上传的物理路径是否存在，不存在则创建
                string SavePath = HttpContext.Current.Server.MapPath(this.SavePath);
                CreateDirectory(SavePath);

                //保存文件
                _FormFileBase.SaveAs(SavePath + FileName);

                //返回
                OutFileName = FileName;
                OutFileExt = Ext;
                ToSavePath = this.SavePath + FileName;
                FileSize = FileLength;
                ToFileSize = GetToFileSize(FileSize);

                Success = true;
                Message = "上传成功";
                return;
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
            {
                Message = "上传失败！";
                return;
            }
        }

        #endregion

        #region 阿里云
        private IStorage imageStorage = new AliyunStorage();


        #region 文件上传

        /// <summary>
        /// HttpPostedFile
        /// </summary>
        public void AliyunUploadFile()
        {
            //文件后缀
            string Ext = GetExt(_FormFile.FileName);
            
            //获得文件大小，以字节为单位
            long FileLength = _FormFile.ContentLength;


            if (_FormFile == null || _FormFile.FileName.Trim() == "")
            {
                Message = "请选择要上传文件！";
                return;
            }
            if (!IsUpload(Ext))
            {
                Message = "不允许上传" + Ext + "类型的文件！";
                return;
            }
            if (FileLength > _MaxSize)
            {
                Message = "文件超过限制的大小！";
                return;
            }
            try
            {
                //文件名
                string FileName = _FormFile.FileName;
                if (IsChangeFileName)
                {
                    FileName = GetFileName(Ext);
                }

                var fileStream = _FormFile.InputStream;
                var savedImagePath = imageStorage.Save("", SavePath.ToLower() + "/" + FileName, fileStream);
                
                //返回
                OutFileName = GetFileName(_FormFile.FileName, Ext);
                OutFileExt = Ext;
                ToSavePath = savedImagePath.ToString();
                FileSize = FileLength;
                ToFileSize = GetToFileSize(FileSize);

                Success = true;
                Message = "上传成功";
                return;
            }
            catch (Exception ex)
            {
                Message = "上传失败！";
                return;
            }
        }

        /// <summary>
        /// HttpPostedFileBase
        /// </summary>
        public void AliyunUploadFileBase()
        {
            //文件后缀
            string Ext = GetExt(_FormFileBase.FileName);
            //获得文件大小，以字节为单位
            long FileLength = _FormFileBase.ContentLength;


            if (_FormFileBase == null || _FormFileBase.FileName.Trim() == "")
            {
                Message = "请选择要上传文件！";
                return;
            }
            if (!IsUpload(Ext))
            {
                Message = "不允许上传" + Ext + "类型的文件！";
                return;
            }
            if (FileLength > _MaxSize)
            {
                Message = "文件超过限制的大小！";
                return;
            }
            try
            {
                //文件名
                string FileName = _FormFile.FileName;
                if (IsChangeFileName)
                {
                    FileName = GetFileName(Ext);
                }

                var fileStream = _FormFileBase.InputStream;
                var savedImagePath = imageStorage.Save("", SavePath.ToLower() + "/" + FileName, fileStream);
                
                //返回
                OutFileName = GetFileName(_FormFileBase.FileName, Ext);
                OutFileExt = Ext;
                ToSavePath = savedImagePath.ToString();
                FileSize = FileLength;
                ToFileSize = GetToFileSize(FileSize);

                Success = true;
                Message = "上传成功";
                return;
            }
            catch (Exception ex)
            {
                Message = "上传失败！";
                return;
            }
        }

        #endregion

        #region 上传图片

        /// <summary>
        /// 图片上传 HttpPostedFile
        /// </summary>
        public void AliyunImgUploadFile()
        {
            //文件后缀
            string Ext = GetExt(_FormFile.FileName);
            //获得文件大小，以字节为单位
            long FileLength = _FormFile.ContentLength;

            if (_FormFile == null || _FormFile.FileName.Trim() == "")
            {
                Message = "请选择要上传图片！";
                return;
            }
            if (!IsImage(Ext))
            {
                Message = "不允许上传【" + Ext + "】类型的图片！";
                return;
            }
            if (FileLength > _MaxSize)
            {
                Message = "图片超过限制的大小（" + (_MaxSize / 1024 / 1024) + "M）！";
                return;
            }
            try
            {

                //文件名
                string FileName = _FormFile.FileName;
                if (IsChangeFileName)
                {
                    FileName = GetFileName(Ext);
                }
                
                var fileStream = _FormFile.InputStream;
                var savedImagePath = imageStorage.Save("", SavePath.ToLower() + "/" + FileName, fileStream);
                
                //返回
                OutFileName = GetFileName(_FormFile.FileName, Ext);
                OutFileExt = Ext;
                ToSavePath = savedImagePath.ToString();
                FileSize = FileLength;
                ToFileSize = GetToFileSize(FileSize);

                Success = true;
                Message = "上传成功！";
                return;
            }
            catch (Exception ex)
            {
                Message = "上传失败！" + ex.Message;
                return;
            }
        }

        /// <summary>
        /// HttpPostedFileBase
        /// </summary>
        public void AliyunImgUploadFileBase()
        {
            //文件后缀
            string Ext = GetExt(_FormFileBase.FileName);
            //获得文件大小，以字节为单位
            long FileLength = _FormFileBase.ContentLength;


            if (_FormFileBase == null || _FormFileBase.FileName.Trim() == "")
            {
                Message = "请选择要上传图片！";
                return;
            }
            if (!IsUpload(Ext))
            {
                Message = "不允许上传" + Ext + "类型的图片！";
                return;
            }
            if (FileLength > _MaxSize)
            {
                Message = "图片超过限制的大小（" + (_MaxSize / 1024 / 1024) + "M）！";
                return;
            }
            try
            {
                //文件名
                string FileName = _FormFile.FileName;
                if (IsChangeFileName)
                {
                    FileName = GetFileName(Ext);
                }

                var fileStream = _FormFileBase.InputStream;
                var savedImagePath = imageStorage.Save("", SavePath.ToLower() + "/" + FileName, fileStream);

                //返回
                OutFileName = GetFileName(_FormFileBase.FileName, Ext);
                OutFileExt = Ext;
                ToSavePath = savedImagePath.ToString();
                FileSize = FileLength;
                ToFileSize = GetToFileSize(FileSize);

                Success = true;
                Message = "上传成功";
                return;
            }
            catch (Exception ex)
            {
                Message = "上传失败！";
                return;
            }
        }

        #endregion
        
        #endregion

        /// <summary>
        /// 获取上传文件信息
        /// </summary>
        public void GetUploadFileINfo()
        {
            //文件后缀
            string Ext = GetExt(_FormFile.FileName);
            //文件名
            string FileName = _FormFile.FileName;
            if (IsChangeFileName)
            {
                FileName = GetFileName(Ext);
            }

            //获得文件大小，以字节为单位
            long FileLength = _FormFile.ContentLength;
            OutFileName = FileName;
            OutFileExt = Ext;
            FileSize = FileLength;
            ToFileSize = GetToFileSize(FileSize);
            Success = true;
        }

    }
}
