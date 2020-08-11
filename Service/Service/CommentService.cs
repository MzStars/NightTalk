using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    /// <summary>
    /// 评论
    /// </summary>
    public class CommentService : ServiceBase
    {
        public CommentService(IDataProvider dataProvider) : base(dataProvider)
        {

        }

        /// <summary>
        /// 查 - 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CommentDomain GetModelByID(Guid id)
        {
            var model = DataProvider.GetAll_Comment().FirstOrDefault(x => x.ID == id);
            return model.ToDomainModel();
        }

        /// <summary>
        /// 查 - 获取数据
        /// </summary>
        /// <returns></returns>
        public List<CommentExtendDomain> GetData(out int listCount, Guid articleID, int pageIndex, int pageSize)
        {

            var list = from a in DataProvider.GetAll_Comment().Where(x => x.ArticleID == articleID)
                       join b in DataProvider.GetAll_WeCharUserInfo() on a.UnionID equals b.UnionID
                       select new CommentExtend()
                       {
                           UnionID = a.UnionID,
                           Avater = b.Avater,
                           CommentContent = a.CommentContent,
                           CreateTime = a.CreateTime,
                           Gender = b.Gender,
                           ID = a.ID,
                           NickName = b.NickName
                       };

            listCount = list.Count();
            if (listCount == 0)
            {
                return null;
            }
            return list
                .OrderByDescending(x => x.CreateTime)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(x => x.ToDomainModel())
                .ToList();
        }

        /// <summary>
        /// 增 - 直接添加
        /// </summary>
        /// <param name="domain"></param>
        public void NewCreate(CommentDomain domain) 
        {
            DataProvider.Create(domain.ToDbModel());
        }

        /// <summary>
        /// //删除评论
        /// </summary>
        /// <param name="ID"></param>
        public void DeleteComment(Guid ID) 
        {
            var model = DataProvider.GetAll_Comment().FirstOrDefault(x => x.ID == ID);
            if (model == null)
            {
                throw new Exception("评论数据不存在");
            }
            DataProvider.Delete(model);
        }
    }
}
