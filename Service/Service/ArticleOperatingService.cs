using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ArticleOperatingService : ServiceBase
    {
        public ArticleOperatingService(IDataProvider dataProvider) : base(dataProvider)
        {
        
        }

        /// <summary>
        /// 查 - 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ArticleOperatingDomain GetModelByID(Guid id)
        {
            var model = DataProvider.GetAll_ArticleOperating().FirstOrDefault(x => x.ID == id);
            return model.ToDomainModel();
        }

        /// <summary>
        /// 改 - 点赞/去掉点赞
        /// </summary>
        public void LikeArticle(string unionID, Guid articleID)
        {
            var model = DataProvider.GetAll_ArticleOperating().FirstOrDefault(x => x.UnionID == unionID && articleID == x.ArticleID) ?? new Data.ArticleOperating();

            if (model == null)
            {
                model.ID = Guid.NewGuid();
                model.UnionID = unionID;
                model.ArticleID = articleID;
                model.CraeteTime = DateTime.Now;
                model.Like = true;

                DataProvider.Create(model);
            }
            else
            {
                model.Like = !model.Like;
                DataProvider.Save();

            }
        }


        /// <summary>
        /// 改 - 转发
        /// </summary>
        public void ForwardArticle(string unionID, Guid articleID)
        {
            var model = DataProvider.GetAll_ArticleOperating().FirstOrDefault(x => x.UnionID == unionID && articleID == x.ArticleID) ?? new Data.ArticleOperating();

            if (model == null)
            {
                model.ID = Guid.NewGuid();
                model.UnionID = unionID;
                model.ArticleID = articleID;
                model.CraeteTime = DateTime.Now;
                model.Forward = true;

                DataProvider.Create(model);
            }
            else
            {
                model.Forward = true;
                DataProvider.Save();

            }
        }

        /// <summary>
        /// 转发自增
        /// </summary>
        /// <param name="articleID"></param>
        public void AddForward(Guid articleID) 
        {
            var model = DataProvider.GetAll_Article().FirstOrDefault(x => x.ID == articleID);

            if (model == null)
            {
                throw new Exception("不存在该篇文章");
            }

            model.ArticleForward++;
            DataProvider.Save();

        }


        /// <summary>
        /// 改 - 收藏/去掉收藏
        /// </summary>
        public void FavoriteArticle(string unionID, Guid articleID)
        {
            var model = DataProvider.GetAll_ArticleOperating().FirstOrDefault(x => x.UnionID == unionID && articleID == x.ArticleID) ?? new Data.ArticleOperating();

            if (model == null)
            {
                model.ID = Guid.NewGuid();
                model.UnionID = unionID;
                model.ArticleID = articleID;
                model.CraeteTime = DateTime.Now;
                model.Favorite = true;

                DataProvider.Create(model);
            }
            else
            {
                model.Favorite = !model.Favorite;
                DataProvider.Save();

            }
        }
    }
}
