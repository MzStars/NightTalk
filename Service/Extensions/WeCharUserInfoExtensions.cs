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
    public static class WeCharUserInfoExtensions
    {
        public static WeCharUserInfo ToDbModel(this WeCharUserInfoDomain model)
        {
            return Mapper.Map<WeCharUserInfo>(model);
        }

        public static WeCharUserInfoDomain ToDomainModel(this WeCharUserInfo model)
        {
            return Mapper.Map<WeCharUserInfoDomain>(model);
        }
    }
}
