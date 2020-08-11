using AutoMapper;
using Data;
using NightTalkBack.Models;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalkBack
{
    public class ModelMappingConfig
    {
        public static void RegisterModelMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ArticleRequest, ArticleDomain>();
                cfg.CreateMap<AccountRequest, AccountDomain>();




                cfg.CreateMap<FileInfoDomain, FileInfo>();
                cfg.CreateMap<FileInfo, FileInfoDomain>();
                cfg.CreateMap<WeCharUserInfoDomain, WeCharUserInfo>();
                cfg.CreateMap<WeCharUserInfo, WeCharUserInfoDomain>();
                cfg.CreateMap<ArticleDomain, Article>();
                cfg.CreateMap<Article, ArticleExtend>();
                cfg.CreateMap<Article, ArticleDomain>();
                cfg.CreateMap<ArticleOperatingDomain, ArticleOperating>();
                cfg.CreateMap<ArticleOperating, ArticleOperatingDomain>();
                cfg.CreateMap<CommentDomain, Comment>();
                cfg.CreateMap<Comment, CommentDomain>();
                cfg.CreateMap<AccountDomain, Account>();
                cfg.CreateMap<Account, AccountDomain>();

            });
        }
    }
}