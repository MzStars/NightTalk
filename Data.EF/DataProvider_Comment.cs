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
        public void Create(Comment model)
        {
            context.Comment.Add(model);
            Save();
        }

        /// <summary>
        /// 增 - 添加集合
        /// </summary>
        /// <param name="listModel"></param>
        public void Create(List<Comment> listModel)
        {
            context.Comment.AddRange(listModel);
            Save();
        }

        /// <summary>
        /// 删 - 删除单条数据
        /// </summary>
        /// <param name="model"></param>
        public void Delete(Comment model)
        {
            context.Comment.Remove(model);
            Save();
        }

        /// <summary>
        /// 查 - 获取所有数据
        /// </summary>
        public IQueryable<Comment> GetAll_Comment()
        {
            return context.Comment.AsQueryable();
        }
    }
}
