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
    public static class ArticleOperatingExtensions
    {
        public static ArticleOperating ToDbModel(this ArticleOperatingDomain model)
        {
            return Mapper.Map<ArticleOperating>(model);
        }

        public static ArticleOperatingDomain ToDomainModel(this ArticleOperating model)
        {
            return Mapper.Map<ArticleOperatingDomain>(model);
        }
    }
}
