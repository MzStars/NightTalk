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
        public void Create(Account model)
        {
            context.Account.Add(model);
            Save();
        }

        /// <summary>
        /// 添加集合 - 账号
        /// </summary>
        /// <param name="listModel"></param>
        public void Create(List<Account> listModel)
        {
            context.Account.AddRange(listModel);
            Save();
        }

        /// <summary>
        /// 删除 - 账号
        /// </summary>
        /// <param name="model"></param>
        public void Delete(Account model)
        {
            context.Account.Remove(model);
            Save();
        }

        /// <summary>
        /// 获取所有 - 账号
        /// </summary>
        public IQueryable<Account> GetAll_Account()
        {
            return context.Account.AsQueryable();
        }
    }
}
