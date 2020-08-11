using AutoMapper;
using NightTalk.Models;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalk
{
    public static class WeCharUserInfoExtensions
    {
        public static WeCharUserInfoDomain ToDomainModel(this WeCharUserInfoVM viewModel)
        {
            return Mapper.Map<WeCharUserInfoDomain>(viewModel);
        }

        public static WeCharUserInfoDomain ToDomainModel(this WXDecodeInfo decodeInfo)
        {
            return Mapper.Map<WeCharUserInfoDomain>(decodeInfo);
        }

        public static WeCharUserInfoVM ToViewModel(this WeCharUserInfoDomain domainModel)
        {
            return Mapper.Map<WeCharUserInfoVM>(domainModel);
        }
    }
}
