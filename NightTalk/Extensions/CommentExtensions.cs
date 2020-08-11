using AutoMapper;
using NightTalk.Models;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalk
{
    public static class CommentExtensions
    {
        public static CommentDomain ToDomainModel(this AddCommentRequest request)
        {
            return Mapper.Map<CommentDomain>(request);
        }
    }
}
