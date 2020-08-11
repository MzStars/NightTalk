using AutoMapper;
using Data;
using NightTalk.Models;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalk
{
    public class ModelMappingConfig
    {
        public static void RegisterModelMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<WeCharUserInfoDomain, WeCharUserInfoVM>();
                cfg.CreateMap<WeCharUserInfoVM, WeCharUserInfoDomain>();
                cfg.CreateMap<WXDecodeInfo, WeCharUserInfoDomain>()
                .ForMember(d => d.Avater, opt => opt.MapFrom(x => x.avatarUrl))
                .ForMember(d => d.NickName, opt => opt.MapFrom(x => x.nickName));
                cfg.CreateMap<AddCommentRequest, CommentDomain>();


                cfg.CreateMap<WeCharUserInfoDomain, WeCharUserInfo>();
                cfg.CreateMap<WeCharUserInfo, WeCharUserInfoDomain>();
                cfg.CreateMap<ArticleDomain, Article>();
                cfg.CreateMap<Article, ArticleExtend>();
                cfg.CreateMap<Article, ArticleDomain>();
                cfg.CreateMap<ArticleOperatingDomain, ArticleOperating>();
                cfg.CreateMap<ArticleOperating, ArticleOperatingDomain>();
                cfg.CreateMap<CommentDomain, Comment>();
                cfg.CreateMap<Comment, CommentDomain>();
                cfg.CreateMap<CommentExtendDomain, CommentExtend>();
                cfg.CreateMap<CommentExtend, CommentExtendDomain>();

            });
        }
    }
}