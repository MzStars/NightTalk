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
    public static class CommentExtensions
    {
        public static Comment ToDbModel(this CommentDomain model)
        {
            return Mapper.Map<Comment>(model);
        }

        public static CommentDomain ToDomainModel(this Comment model)
        {
            return Mapper.Map<CommentDomain>(model);
        }
    }
}
