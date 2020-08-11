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
    public static class CommentExtendExtensions
    {
        public static CommentExtend ToDbModel(this CommentExtendDomain model)
        {
            return Mapper.Map<CommentExtend>(model);
        }

        public static CommentExtendDomain ToDomainModel(this CommentExtend model)
        {
            var domainModel = Mapper.Map<CommentExtendDomain>(model);
            domainModel.CreateTime = model.CreateTime.ToString("MM-dd hh:mm");
            return domainModel;
        }
    }
}
