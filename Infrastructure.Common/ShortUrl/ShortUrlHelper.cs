using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.Common
{
    public static class ShortUrlHelper
    {
        public class sina_short_url
        {
            public string url_short { get; set; }

            public string url_long { get; set; }
            public int type { get; set; }
        }
        /// <summary>
        /// 新浪转换短链接
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Convert_SINA_Short_Url(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return "";
            }

            HttpHelper http = new HttpHelper();
            //api地址
            var address = "http://api.t.sina.com.cn/short_url/shorten.json?source=211160679";
            address += "&url_long=" + HttpUtility.UrlEncode(url);
            //http请求
            var item = new HttpItem()
            {
                URL = address,
                Method = "get",
            };

            var result = http.GetHtml(item);
            //json转换
            var urls = JsonHelper.DeserializeJsonToObject<List<sina_short_url>>(result.Html);
            if (urls != null && urls.Count > 0)
            {
                return urls[0].url_short;
            }
            return "";
        }
    }
}
