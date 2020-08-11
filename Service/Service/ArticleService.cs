using Data;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;

namespace Service
{
    public class ArticleService : ServiceBase
    {
        public ArticleService(IDataProvider dataProvider) : base(dataProvider)
        {

        }

        /// <summary>
        /// 查 - 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ArticleDomain GetModelByID(Guid id)
        {
            var model = DataProvider.GetAll_Article().FirstOrDefault(x => x.ID == id);
            return model.ToDomainModel();
        }

        /// <summary>
        /// 查 - 存在数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool ExistData(Guid ID)
        {
            return DataProvider.GetAll_Article().Any(x => x.ID == ID);
        }

        /// <summary>
        /// 获取文章扩展数据
        /// </summary>
        /// <returns></returns>
        public ArticleExtend GetData(string unionID)
        {
            var nowDate = DateTime.Now;
            if (nowDate.TimeOfDay < DateTime.MinValue.AddHours(4).TimeOfDay)
            {
                nowDate = nowDate.AddDays(-1);
            }
            var model = DataProvider.GetAll_Article().OrderByDescending(x => x.CreateTime).FirstOrDefault(x => SqlFunctions.DateDiff("d", x.ArticleDate, nowDate) == 0).ToExtendModel();
            if (model == null)
            {
                throw new Exception("文章数据为空");
            }


            var list = DataProvider.GetAll_ArticleOperating().Where(x => x.ArticleID == model.ID).ToList();
            model.ArticleLike = list.Where(x => x.Like).Count();
            model.ArticleComment = DataProvider.GetAll_Comment().Where(x => x.ArticleID == model.ID).Count();
            //model.ArticleForward = list.Where(x => x.Forward).Count();
            //model.ArticleViews = list.Count;
            if (!string.IsNullOrEmpty(unionID))
            {
                var operating = list.FirstOrDefault(x => x.UnionID == unionID);
                if (operating != null)
                {
                    model.Like = operating.Like;
                    model.Favorite = operating.Favorite;
                    model.Forward = operating.Forward;

                }
                else
                {
                    if (DataProvider.GetAll_WeCharUserInfo().FirstOrDefault(x => x.UnionID == unionID) == null)
                    {
                        throw new Exception("不存在该账号");
                    }
                    ArticleOperating operatingModel = new ArticleOperating();
                    operatingModel.ID = Guid.NewGuid();
                    operatingModel.CraeteTime = DateTime.Now;
                    operatingModel.ArticleID = model.ID;
                    operatingModel.UnionID = unionID;
                    DataProvider.Create(operatingModel);
                }
            }
            return model;
        }

        /// <summary>
        /// 查 - 获取文章数据 - 扩展类型
        /// </summary>
        /// <returns></returns>
        public List<ArticleExtend> GetData(out int listCount, int pageIndex, int pageSize, string articleTitle, string articleAuthor, DateTime? articleDate, ArticleTypeEnumDomain? articleType, int? UID)
        {
            var listOperating = DataProvider.GetAll_ArticleOperating();
            var listComment = DataProvider.GetAll_Comment();
            var listArticle = DataProvider.GetAll_Article();

            if (!string.IsNullOrEmpty(articleTitle))
            {
                listArticle = listArticle.Where(x => x.ArticleTitle.Contains(articleTitle));
            }
            if (!string.IsNullOrEmpty(articleAuthor))
            {
                listArticle = listArticle.Where(x => x.ArticleAuthor.Contains(articleAuthor));
            }

            if (articleDate != null)
            {
                listArticle = listArticle.Where(x => x.ArticleDate == articleDate);
            }
            if (articleType != null)
            {
                var articleTypEnum = (ArticleTypeEnum)articleType;
                listArticle = listArticle.Where(x => x.ArticleType == articleTypEnum);
            }
            if (UID != null)
            {
                listArticle = listArticle.Where(x => x.UID == UID);
            }

            var list = from a in listArticle
                       select new ArticleExtend()
                       {
                           ID = a.ID,
                           UID = a.UID,
                           ArticleAuthor = a.ArticleAuthor,
                           ArticleContent = a.ArticleContent,
                           ArticleDate = a.ArticleDate,
                           ArticleTitle = a.ArticleTitle,
                           ArticleType = a.ArticleType,
                           ArticleStatus = a.ArticleStatus,
                           CreateTime = a.CreateTime,
                           ArticleComment = listComment.Where(x => x.ArticleID == a.ID).Count(),
                           ArticleLike = listOperating.Where(x => x.Like && x.ArticleID == a.ID).Count(),
                           ArticleForward = a.ArticleForward,//listOperating.Where(x => x.Forward && x.ArticleID == a.ID).Count(),
                           ArticleViews = a.ArticleViews//listOperating.Where(x => x.ArticleID == a.ID).Count()
                       };
            listCount = list.Count();

            return list.OrderByDescending(x => x.CreateTime)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();
        }

        /// <summary>
        /// 增 - 直接添加
        /// </summary>
        /// <param name="domain"></param>
        public void NewCreate(ArticleDomain domain)
        {
            DataProvider.Create(domain.ToDbModel());
        }

        /// <summary>
        /// 修改文章状态
        /// </summary>
        /// <param name="ArticleID"></param>
        /// <param name="articleStatus"></param>
        public void UpdateArticleStatus(Guid ArticleID, ArticleStatusEnumDomain articleStatus) 
        {
            var model = DataProvider.GetAll_Article().FirstOrDefault(x => x.ID == ArticleID);
            if (model == null)
            {
                throw new Exception("未找到文章");
            }

            model.ArticleStatus = (ArticleStatusEnum)articleStatus;
            DataProvider.Save();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="domain"></param>
        public void Edit(ArticleDomain domain)
        {
            var model = DataProvider.GetAll_Article().FirstOrDefault(x => x.ID == domain.ID);

            if (model == null)
            {
                throw new Exception("文章数据不存在");
            }

            model.ArticleAuthor = domain.ArticleAuthor;
            model.ArticleContent = domain.ArticleContent;
            model.ArticleDate = domain.ArticleDate;
            model.ArticleTitle = domain.ArticleTitle;
            model.ArticleType = (ArticleTypeEnum)domain.ArticleType;

            DataProvider.Save();
        }

        /// <summary>
        /// 删 - 根据ID删除数据
        /// </summary>
        /// <param name="ID"></param>
        public void DeleteArticle(Guid ID)
        {
            var model = DataProvider.GetAll_Article().FirstOrDefault(x => x.ID == ID);

            if (model == null)
            {
                throw new Exception("文章数据不存在");
            }

            DataProvider.Delete(model);
        }

        /// <summary>
        /// 增 - 浏览次数自增
        /// </summary>
        public void AddViews(Guid articleID) 
        {
            var model = DataProvider.GetAll_Article().FirstOrDefault(x => x.ID == articleID);
            if (model == null)
            {
                throw new Exception("不存在该篇文章");
            }

            model.ArticleViews++;
            DataProvider.Save();
        }
    }
}
