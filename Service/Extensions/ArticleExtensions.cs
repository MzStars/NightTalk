using AutoMapper;
using Data;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class ArticleExtensions
    {
        public static Article ToDbModel(this ArticleDomain model)
        {
            return Mapper.Map<Article>(model);
        }

        public static ArticleDomain ToDomainModel(this Article model)
        {
            return Mapper.Map<ArticleDomain>(model);
        }

        public static ArticleExtend ToExtendModel(this Article model)
        {
            return Mapper.Map<ArticleExtend>(model);
        }
    }
}
