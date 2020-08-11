using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Infrastructure.Common
{
    public class Storage:IStorage
    {
        private readonly string assetFolder = "Content";

        public Uri Save(string container, string fileName, System.IO.Stream stream, bool lowercaseUri = true)
        {
            if (string.IsNullOrWhiteSpace(container) || string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("Both container and path are required");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (stream.CanSeek)
            {
                stream.Seek(0, System.IO.SeekOrigin.Begin);
            }

            container = container.ToLower();
            fileName = fileName.ToLower();
            var virtualPath = string.Format("~/{0}/{1}/{2}", assetFolder, container, fileName);
            string physicalPath = HostingEnvironment.MapPath(virtualPath);
            var directory = Path.GetDirectoryName(physicalPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var fs = File.Open(physicalPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }

                stream.CopyTo(fs);
            }

            var path = virtualPath.Replace("~/", HostingEnvironment.ApplicationVirtualPath);
            if (lowercaseUri)
            {
                path = path.ToLower();
            }

            return new Uri(path, UriKind.Relative);
        }

        public void Remove(string container, string fileName, bool lowercaseUri = true)
        {
            if (string.IsNullOrWhiteSpace(container) || string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("Both container and path are required");
            }

            container = container.ToLower();
            fileName = fileName.ToLower();
            var appPath = HostingEnvironment.ApplicationVirtualPath.ToLower();
            var physicalPath = HostingEnvironment.MapPath(string.Format("~/{0}/{1}/{2}", assetFolder, container, fileName));
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }
        }
    }
}
