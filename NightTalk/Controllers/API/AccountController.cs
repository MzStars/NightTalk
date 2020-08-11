using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace NightTalk.Controllers
{
    public class AccountController : BaseApi
    {
        public static AccountService _accountService;

        public AccountController(AccountService accountService) 
        {
            _accountService = accountService;
        }

        [Route("api/account/test")]
        [HttpGet]
        public JsonResult<object> test() 
        {
            var model = _accountService.GetModelByID(123);
            if (model == null)
            {
                return JsonNet("2333333333");
            }
            return JsonNet("2333");
        }
    }
}