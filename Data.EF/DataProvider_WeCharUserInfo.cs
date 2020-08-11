using Data;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF
{
    public partial class DataProvider : IDataProvider
    {
        /// <summary>
        /// 添加 - 账号
        /// </summary>
        /// <param name="model"></param>
        public void Create(WeCharUserInfo model)
        {
            context.WeCharUserInfo.Add(model);
            Save();
        }

        /// <summary>
        /// 添加集合 - 账号
        /// </summary>
        /// <param name="listModel"></param>
        public void Create(List<WeCharUserInfo> listModel)
        {
            context.WeCharUserInfo.AddRange(listModel);
            Save();
        }

        /// <summary>
        /// 删除 - 账号
        /// </summary>
        /// <param name="model"></param>
        public void Delete(WeCharUserInfo model)
        {
            context.WeCharUserInfo.Remove(model);
            Save();
        }

        /// <summary>
        /// 获取所有 - 账号
        /// </summary>
        public IQueryable<WeCharUserInfo> GetAll_WeCharUserInfo()
        {
            return context.WeCharUserInfo.AsQueryable();
        }
    }
}
