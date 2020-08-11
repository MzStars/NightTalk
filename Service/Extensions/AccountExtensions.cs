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
    public static class AccountExtensions
    {
        public static Account ToDbModel(this AccountDomain model)
        {
            return Mapper.Map<Account>(model);
        }

        public static AccountDomain ToDomainModel(this Account model)
        {
            return Mapper.Map<AccountDomain>(model);
        }
    }
}
