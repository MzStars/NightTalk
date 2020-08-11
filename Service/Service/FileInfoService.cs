using Data.Model;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class FileInfoService : ServiceBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataProvider"></param>
        public FileInfoService(IDataProvider dataProvider) : base(dataProvider) { }

        /// <summary>
        /// 增 - 直接添加
        /// </summary>
        /// <param name="domain"></param>
        public void NewCreate(FileInfoDomain domain)
        {
            DataProvider.Create(domain.ToDbModel());
        }

        /// <summary>
        /// 查 - 获取集合
        /// </summary>
        /// <returns></returns>
        public List<FileInfoDomain> GetListData(out int listCount, int pageIndex, int pageSize, int? fileType, int? UID, string fileName) 
        {
            var list = DataProvider.GetAll_FileInfo();
            if (UID != null)
            {
                list = list.Where(x => x.UID == UID);
            }
            if (fileType != null)
            {
                var fileTypeEnum = (FileTypeEnum)fileType;
                list = list.Where(x => x.FileType == fileTypeEnum);
            }
            if (!string.IsNullOrEmpty(fileName))
            {
                list = list.Where(x => x.FileName.Contains(fileName));
            }

            listCount = list.Count();

            return list
                .OrderByDescending(x => x.CreateTime)
                .Skip(pageIndex * pageSize).Take(pageSize)
                .ToList()
                .Select(x => x.ToDomainModel())
                .ToList();

        }
    }
}
