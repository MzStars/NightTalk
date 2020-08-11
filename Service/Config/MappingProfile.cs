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
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            //后台账号
            CreateMap<Account, AccountDomain>();
            CreateMap<AccountDomain, Account>();

            //文章
            CreateMap<Article, ArticleDomain>();
            CreateMap<Article, ArticleExtend>();
            CreateMap<ArticleDomain, Article>();

            //文章操作
            CreateMap<ArticleOperating, ArticleOperatingDomain>();
            CreateMap<ArticleOperatingDomain, ArticleOperating>();

            //评论
            CreateMap<Comment, CommentDomain>();
            CreateMap<CommentDomain, Comment>();

            //评论扩展
            CreateMap<CommentExtend, CommentExtendDomain>();
            CreateMap<CommentExtendDomain, CommentExtend>();

            //文件
            CreateMap<FileInfo, FileInfoDomain>();
            CreateMap<FileInfoDomain, FileInfo>();

            //微信用户
            CreateMap<WeCharUserInfo, WeCharUserInfoDomain>();
            CreateMap<WeCharUserInfoDomain, WeCharUserInfo>();
        }
    }
}
