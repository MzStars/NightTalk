using System;
using System.Web;
using System.Text.RegularExpressions;

namespace Infrastructure.Common
{
    /// <summary>
    /// Request地址栏参数获取操作类
    /// </summary>
    public class DNTHelper
    {
        /// <summary>
        /// 获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询字符串(如果存在))
        /// </summary>
        /// <returns>原始 URL</returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        /// <summary>
        /// 获取 域名
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            string host = HttpContext.Current.Request.Url.Host;
            return host;
        }

        /// <summary>
        /// 获得当前完整Url地址
        /// </summary>
        /// <returns>当前完整Url地址</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }


        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.QueryString[strName];
        }

        /// <summary>
        ///  获得指定Url参数编码过的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数解码过的值</returns>
        public static string GetQueryStringDecode(string strName)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            return HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString[strName]);
        }

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.Form[strName];
        }

        /// <summary>
        ///  获得指定表单参数编码过的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数解码过的值</returns>
        public static string GetFormStringDecode(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return "";
            }
            return HttpUtility.UrlDecode(HttpContext.Current.Request.Form[strName]);
        }

        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的int类型值，空返回0</returns>
        public static int GetQueryInt(string strName)
        {
            return GetQueryInt(strName, 0);
        }


        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值，空返回缺省值</returns>
        public static int GetQueryInt(string strName, int defValue)
        {
            return StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
        }


        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值，空返回0</returns>
        public static int GetFormInt(string strName)
        {
            return GetFormInt(strName, 0);
        }

        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值，空返回缺省值</returns>
        public static int GetFormInt(string strName, int defValue)
        {
            return StrToInt(HttpContext.Current.Request.Form[strName], defValue);
        }

        /// <summary>
        /// 获得指定Url参数的float类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static float GetQueryFloat(string strName, float defValue)
        {
            return StrToFloat(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        /// <summary>
        /// 获得指定表单参数的float类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static float GetFormFloat(string strName, float defValue)
        {
            return StrToFloat(HttpContext.Current.Request.Form[strName], defValue);
        }

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result) || !Regex.IsMatch(result, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
            {
                return "";
            }
            return result;
        }

        /// <summary>
        /// string型转换为int型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        private static int StrToInt(object strValue, int defValue)
        {
            if ((strValue == null) || (strValue.ToString() == string.Empty) || (strValue.ToString().Length > 10))
            {
                return defValue;
            }
            string val = strValue.ToString();
            string firstletter = val[0].ToString();
            bool isfirstletter = new Regex(@"^([0-9])[0-9]*(\.\w*)?$").IsMatch(firstletter);
            if (val.Length == 10 && isfirstletter && int.Parse(firstletter) > 1)
            {
                return defValue;
            }
            else if (val.Length == 10 && !isfirstletter)
            {
                return defValue;
            }
            int intValue = defValue;
            if (strValue != null)
            {
                bool IsInt = new Regex(@"^([-]|[0-9])[0-9]*$").IsMatch(strValue.ToString());
                if (IsInt)
                {
                    intValue = Convert.ToInt32(strValue);
                }
            }
            return intValue;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        private static float StrToFloat(object strValue, float defValue)
        {
            if ((strValue == null) || (strValue.ToString().Length > 10))
            {
                return defValue;
            }
            float intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = new Regex(@"^([-]|[0-9])[0-9]*(\.\w*)?$").IsMatch(strValue.ToString());
                if (IsFloat)
                {
                    intValue = Convert.ToSingle(strValue);
                }
            }
            return intValue;
        }

    }
}
