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
        /// 增 - 添加单条数据
        /// </summary>
        /// <param name="model"></param>
        public void Create(Article model)
        {
            context.Article.Add(model);
            Save();
        }

        /// <summary>
        /// 增 - 添加集合
        /// </summary>
        /// <param name="listModel"></param>
        public void Create(List<Article> listModel)
        {
            context.Article.AddRange(listModel);
            Save();
        }

        /// <summary>
        /// 删 - 删除单条数据
        /// </summary>
        /// <param name="model"></param>
        public void Delete(Article model)
        {
            context.Article.Remove(model);
            Save();
        }

        /// <summary>
        /// 查 - 获取所有数据
        /// </summary>
        public IQueryable<Article> GetAll_Article()
        {
            return context.Article.AsQueryable();
        }
    }
}
