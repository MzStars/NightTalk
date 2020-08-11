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
    public static class AccountExtensions
    {
        public static AccountDomain ToDomainModel(this AccountRequest model)
        {
            return Mapper.Map<AccountDomain>(model);
        }
    }
}
