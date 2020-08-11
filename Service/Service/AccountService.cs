using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AccountService : ServiceBase
    {
        public AccountService(IDataProvider dataProvider) : base(dataProvider)
        {
        
        }
        public AccountDomain GetModelByID(int UID)
        {
            var model = DataProvider.GetAll_Account().FirstOrDefault(x => x.UID == UID);
            return model.ToDomainModel();
        }

        /// <summary>
        /// 查 - 根据UID与密码获取数据
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public AccountDomain GetModel(string account, string pwd) 
        {
            int UID = 0;
            try
            {
                UID = Convert.ToInt32(account);
            }
            catch (FormatException)
            {
                return DataProvider.GetAll_Account().FirstOrDefault(x => x.NickName == account && x.PassWord == pwd).ToDomainModel();
            }
            catch (OverflowException)
            {
                return DataProvider.GetAll_Account().FirstOrDefault(x => x.NickName == account && x.PassWord == pwd).ToDomainModel();
            }
            return DataProvider.GetAll_Account().FirstOrDefault(x => (x.UID == UID || x.NickName == account) && x.PassWord == pwd).ToDomainModel();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="pwd"></param>
        /// <param name="newPwd"></param>
        public void UpdatePWD(int UID, string pwd, string newPwd) 
        {
            var model = DataProvider.GetAll_Account().FirstOrDefault(x => x.UID == UID && x.PassWord == pwd);

            if (model == null)
            {
                throw new Exception("密码错误");
            }

            model.PassWord = newPwd;
            DataProvider.Save();
        }

        /// <summary>
        /// 增 - 直接添加
        /// </summary>
        /// <param name="domain"></param>
        public void NewCreate(AccountDomain domain) 
        {
            DataProvider.Create(domain.ToDbModel());
        }

        /// <summary>
        /// 查 - 根据昵称查找是否存在
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public bool Exist(string nickName) 
        {
            return DataProvider.GetAll_Account().Any(x => x.NickName == nickName);
        }
    }
}
