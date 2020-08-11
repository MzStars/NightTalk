using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Aliyun.OSS;

namespace Infrastructure.Common
{
    public class AliyunStorage: IStorage
    {
        private readonly static string accessKeyId = ConfigurationManager.AppSettings["AliYunAccessKeyId"];

        private readonly static string accessKeySecret = ConfigurationManager.AppSettings["AliYunAccessKeySecret"];

        private readonly static string endpoint = ConfigurationManager.AppSettings["AliYunEndpoint"];

        private readonly static string bucketName = ConfigurationManager.AppSettings["AliYunBucketName"];

        private readonly static string cdnUrl = ConfigurationManager.AppSettings["CDNUrl"];

        private static OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);

        public Uri Save(string container, string fileName, System.IO.Stream stream, bool lowercaseUri = true)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("path is required");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (stream.CanSeek)
            {
                stream.Seek(0, System.IO.SeekOrigin.Begin);
            }

            Uri uri;

            try
            {
                fileName = fileName.ToLower();

                var putObjectRequest = new PutObjectRequest(bucketName, fileName, stream);
                putObjectRequest.StreamTransferProgress += streamProgressCallback;

                client.PutObject(putObjectRequest);

                string path = string.Format("https://{0}.{1}/{2}", bucketName, endpoint, fileName);
                uri = new Uri(path, UriKind.Absolute);
            }
            catch (Exception)
            {
                throw;
            }

            return uri;
        }
        public Uri ReadNotCreate(string container, string fileName, System.IO.Stream stream, bool lowercaseUri = true)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("path is required");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (stream.CanSeek)
            {
                stream.Seek(0, System.IO.SeekOrigin.Begin);
            }

            Uri uri;

            try
            {
                fileName = fileName.ToLower();
                var exist = client.DoesObjectExist(bucketName, fileName);
                if (!exist)
                {
                    var putObjectRequest = new PutObjectRequest(bucketName, fileName, stream);
                    putObjectRequest.StreamTransferProgress += streamProgressCallback;

                    client.PutObject(putObjectRequest);
                }

                string path = string.Empty;
                if (string.IsNullOrEmpty(cdnUrl))
                {
                    path = string.Format("https://{0}.{1}/{2}", bucketName, endpoint, fileName);
                }
                else
                {
                    path = string.Format("https://{0}/{1}", cdnUrl, fileName);
                }

                uri = new Uri(path, UriKind.Absolute);
            }
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
            catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
            {
                throw;
            }

            return uri;
        }
        private static void streamProgressCallback(object sender, StreamTransferProgressArgs args)
        {
            //var progressItems = JsonHelper.SerializeObject(new
            //{
            //    TotalBytes = args.TotalBytes.ToString(),
            //    TransferredBytes = args.TransferredBytes.ToString(),
            //    TransferredAccounte = ((double)args.TransferredBytes / args.TotalBytes).ToString("f2")
            //});

            //CookieHelper.SetCookie("ProgressItems", progressItems);
            //CacheHelper.Set("ProgressItems", progressItems, 60 * 1);
        }

        public void Remove(string container, string fileName, bool lowercaseUri = true)
        {
            if (string.IsNullOrWhiteSpace(container) || string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("Both container and path are required");
            }

            try
            {
                container = container.ToLower();
                fileName = fileName.ToLower();
                client.DeleteObject(bucketName, fileName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
