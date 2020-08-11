using Infrastructure.Common;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class WeCharUserInfoService : ServiceBase
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="dataProvider"></param>
        public WeCharUserInfoService(IDataProvider dataProvider) : base(dataProvider)
        {

        }

        /// <summary>
        /// 增 - 创建单条数据
        /// </summary>
        /// <param name="domain"></param>
        public void Create(WeCharUserInfoDomain domain) 
        {
            if (string.IsNullOrEmpty(domain.UnionID))
            {
                throw new Exception("UnionID为空");
            }
            if (domain.CreateTime == DateTime.MinValue)
            {
                domain.CreateTime = DateTime.Now;
            }
            if (string.IsNullOrEmpty(domain.ShareCode))
            {
                domain.ShareCode = RandomHelper.RandCode(7, true);
                while (DataProvider.GetAll_WeCharUserInfo().Any(x => x.ShareCode == domain.ShareCode))
                {
                    domain.ShareCode = RandomHelper.RandCode(7, true);
                }
                
            }
            DataProvider.Create(domain.ToDbModel());
        }

        public string CreateGetCode(WeCharUserInfoDomain domain)
        {
            if (string.IsNullOrEmpty(domain.UnionID))
            {
                throw new Exception("UnionID为空");
            }
            if (domain.CreateTime == DateTime.MinValue)
            {
                domain.CreateTime = DateTime.Now;
            }
            if (string.IsNullOrEmpty(domain.ShareCode))
            {
                domain.ShareCode = RandomHelper.RandCode(7, true);
                while (DataProvider.GetAll_WeCharUserInfo().Any(x => x.ShareCode == domain.ShareCode))
                {
                    domain.ShareCode = RandomHelper.RandCode(7, true);
                }

            }
            DataProvider.Create(domain.ToDbModel());

            return domain.ShareCode;
        }

        /// <summary>
        /// 查 - 获取微信用户
        /// </summary>
        /// <param name="unionID"></param>
        /// <returns></returns>
        public WeCharUserInfoDomain GetData(string unionID) 
        {
            return DataProvider.GetAll_WeCharUserInfo().FirstOrDefault(x => x.UnionID == unionID).ToDomainModel();
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="unionID"></param>
        /// <returns></returns>
        public bool Exist(string unionID) 
        {
            return DataProvider.GetAll_WeCharUserInfo().Any(x => x.UnionID == unionID);
        }

        /// <summary>
        /// 是否存在分享码
        /// </summary>
        /// <param name="unionID"></param>
        /// <returns></returns>
        public bool ExistShareCode(string shareCode)
        {
            return DataProvider.GetAll_WeCharUserInfo().Any(x => x.ShareCode == shareCode);
        }

        /// <summary>
        /// 修改手机号
        /// </summary>
        public void EditPhone(string unionID, string phone) 
        {
            var model = DataProvider.GetAll_WeCharUserInfo().FirstOrDefault(x => x.UnionID == unionID);

            if (model == null)
            {
                throw new Exception("账号获取错误,请重新登录");
            }

            model.Phone = phone;
            DataProvider.Save();
        }
    }
}
