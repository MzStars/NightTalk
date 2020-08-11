using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using Infrastructure.Common;

namespace NightTalk
{
    /// <summary>
    /// API自定义错误过滤器属性
    /// </summary>
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 统一对调用异常信息进行处理，返回自定义的异常信息
        /// </summary>
        /// <param name="context">HTTP上下文对象</param>
        public override void OnException(HttpActionExecutedContext context)
        {
            Exception ex = context.Exception as Exception;

            var actionName = context.Request.RequestUri.AbsolutePath;
            //记录日志
            //Log.Error("出错", ex.Message + "接口地址:" + actionName, 1);

            var code = 0;
            if (ex.Message.Contains("登录信息已过期"))
            {
                code = 1003;
            }
            else if (ex.Message.Contains("请重新登录"))
            {
                code = 1002;
            }
            else
            {
                code = 1001;
            }
            object obj = new
            {
                items = "出错:" + ex.Message,
                status = false,
                code
            };
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(obj.ToJsonFromObject(), Encoding.UTF8, "application/json"),
                ReasonPhrase = "Critical Exception"
            });

        }
    }
}