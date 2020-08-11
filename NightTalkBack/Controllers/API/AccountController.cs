using Infrastructure.Common;
using NightTalkBack.Models;
using Service;
using Service.Model;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace NightTalkBack.Controllers
{
    public class AccountController : BaseApi
    {
        private string RedisPassword = ConfigurationManager.AppSettings["RedisPassword"];
        private string RedisHost = ConfigurationManager.AppSettings["RedisHost"];
        private string RedisPort = ConfigurationManager.AppSettings["RedisPort"];
        private readonly AccountService _accountService;
        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/account/Login")]
        [AllowAnonymous]
        public JsonResult<object> Login(LoginRequest request)
        {
            if (request == null)
            {
                return JsonError("请求数据不能为空");
            }
            string randStr = Guid.NewGuid().ToString().Replace("-", "");

            var model = _accountService.GetModel(request.Account, request.PassWord);
            if (model == null)
            {
                return JsonError("用户名或密码错误");
            }
            using (var redis = new RedisClient(RedisHost, int.Parse(RedisPort), RedisPassword))
            {
                var ts = TimeSpan.FromMinutes(30);//30min 后过期
                redis.Set("NightTalkBack:BaseInfo:" + randStr, JsonHelper.SerializeObject(model), ts);
            }

            var returnModel = new
            {
                model.NickName,
                model.UID,
                model.CreateTime,
                ApiKey = randStr
            };

            return JsonNet(returnModel);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/account/UpdatePWD")]
        public JsonResult<object> UpdatePWD(UpdatePWDRequest request)
        {
            if (request == null)
            {
                return JsonError("请求数据不能为空");
            }

            //获取基本信息
            var baseInfo = GetBaseInfo();
            //修改密码
            _accountService.UpdatePWD(baseInfo.UID, request.PassWord, request.NewPassWord);

            return JsonNet("修改成功,请重新登录");
        }

        /// <summary>
        /// 添加账号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/account/AddAccount")]
        public JsonResult<object> AddAccount(AccountRequest request)
        {
            var baseInfo = GetBaseInfo();
            if (request == null)
            {
                return JsonError("请求数据错误");
            }
            if (_accountService.Exist(request.NickName))
            {
                return JsonError("昵称已经存在");
            }
            AccountDomain domain = request.ToDomainModel();
            domain.CreateTime = DateTime.Now;
            _accountService.NewCreate(domain);
            return JsonNet("添加成功");
        }

        /// <summary>
        /// 删除账号
        /// </summary>
        /// <returns></returns>
        public JsonResult<object> DeleteAccount() 
        {
            return JsonNet("删除成功");
        }


    }
}