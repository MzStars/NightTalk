using AutoMapper;
using Data;
using NightTalkBack.Models;
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
        public static ArticleDomain ToDomainModel(this ArticleRequest model)
        {
            return Mapper.Map<ArticleDomain>(model);
        }
    }
}
