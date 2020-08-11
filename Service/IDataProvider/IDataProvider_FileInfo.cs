using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public partial interface IDataProvider
    {
        /// <summary>
        /// 添加 - 账号
        /// </summary>
        /// <param name="model"></param>
        void Create(FileInfo model);

        /// <summary>
        /// 添加集合 - 账号
        /// </summary>
        /// <param name="listModel"></param>
        void Create(List<FileInfo> listModel);

        /// <summary>
        /// 删除 - 账号
        /// </summary>
        /// <param name="model"></param>
        void Delete(FileInfo model);

        /// <summary>
        /// 获取所有 - 账号
        /// </summary>
        IQueryable<FileInfo> GetAll_FileInfo();
    }
}
