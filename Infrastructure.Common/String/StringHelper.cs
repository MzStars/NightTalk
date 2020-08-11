using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Infrastructure.Common
{
    /// <summary>
    /// 字符串操作类
    /// 1、GetStrArray(string str, char speater, bool toLower)  把字符串按照分隔符转换成 List
    /// 2、GetStrArray(string str) 把字符串转 按照, 分割 换为数据
    /// 3、GetArrayStr(List list, string speater) 把 List 按照分隔符组装成 string
    /// 4、GetArrayStr(List list)  得到数组列表以逗号分隔的字符串
    /// 5、GetArrayValueStr(Dictionary<int, int> list)得到数组列表以逗号分隔的字符串
    /// 6、DelLastComma(string str)删除最后结尾的一个逗号
    /// 7、DelLastChar(string str, string strchar)删除最后结尾的指定字符后的字符
    /// 10、GetSubStringList(string o_str, char sepeater)把字符串按照指定分隔符装成 List 去除重复
    /// 11、GetCleanStyle(string StrList, string SplitString)将字符串样式转换为纯字符串
    /// 12、GetNewStyle(string StrList, string NewStyle, string SplitString, out string Error)将字符串转换为新样式
    /// 13、SplitMulti(string str, string splitstr)分割字符串
    /// 14、SqlSafeString(string String, bool IsDel)
    /// </summary>
    public class StringHelper
    {
        #region 常规字符串操作

        /// <summary>
        /// 检查字符串是否为空
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回值，true为空</returns>
        public static bool IsEmpty(string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;
            return false;
        }

        /// <summary>
        /// 检查字符串中是否包含非法字符
        /// </summary>
        /// <param name="s">单字符</param>
        /// <returns>返回值，true不包含</returns>
        public static bool CheckValidity(string s)
        {
            string str = s;
            if (str.IndexOf("'") > 0 || str.IndexOf("&") > 0 || str.IndexOf("%") > 0 || str.IndexOf("+") > 0 ||
                str.IndexOf("\"") > 0 || str.IndexOf("=") > 0 || str.IndexOf("!") > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 把价格精确至小数点两位
        /// </summary>
        /// <param name="dPrice">价格</param>
        /// <returns>返回值</returns>
        public static string TransformPrice(double dPrice)
        {
            double d = dPrice;
            var myNfi = new NumberFormatInfo { NumberNegativePattern = 2 };
            string s = d.ToString("N", myNfi);
            return s;
        }

        /// <summary> 
        /// 检测含有中文字符串的实际长度 
        /// </summary> 
        /// <param name="str">字符串</param> 
        /// <returns>返回值</returns>
        public static int GetLength(string str)
        {
            var n = new ASCIIEncoding();
            byte[] b = n.GetBytes(str);
            int l = 0; // l 为字符串之实际长度 
            for (int i = 0; i <= b.Length - 1; i++)
            {
                if (b[i] == 63) //判断是否为汉字或全脚符号 
                {
                    l++;
                }
                l++;
            }
            return l;
        }

        /// <summary>
        /// 截取长度,num是英文字母的总数，一个中文算两个英文
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="iNum">英文字母的总数</param>
        /// <param name="bAddDot">是否计算标点</param>
        /// <returns>返回值</returns>
        public static string GetLetter(string str, int iNum, bool bAddDot)
        {
            if (str == null || iNum <= 0) return "";

            if (str.Length < iNum && str.Length * 2 < iNum)
            {
                return str;
            }

            string sContent = str;
            int iTmp = iNum;

            char[] arrC = str.ToCharArray(0, sContent.Length >= iTmp ? iTmp : sContent.Length);

            int i = 0;
            int iLength = 0;
            foreach (char ch in arrC)
            {
                iLength++;

                int k = ch;
                if (k > 127 || k < 0)
                {
                    i += 2;
                }
                else
                {
                    i++;
                }

                if (i > iTmp)
                {
                    iLength--;
                    break;
                }
                if (i == iTmp)
                {
                    break;
                }
            }

            if (iLength < str.Length && bAddDot)
                sContent = sContent.Substring(0, iLength - 3) + "...";
            else
                sContent = sContent.Substring(0, iLength);

            return sContent;
        }

        /// <summary>
        /// 获取日期字符串
        /// </summary>
        /// <param name="dt">日期时间</param>
        /// <returns>返回值</returns>
        public static string GetDateString(DateTime dt)
        {
            return dt.Year.ToString() + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0');
        }

        /// <summary>
        /// 根据指定字符，截取相应字符串
        /// </summary>
        /// <param name="sOrg">源字符串</param>
        /// <param name="sLast">指定字符串</param>
        /// <returns>返回值</returns>
        public static string GetStrByLast(string sOrg, string sLast)
        {
            int iLast = sOrg.LastIndexOf(sLast);
            if (iLast > 0)
                return sOrg.Substring(iLast + 1);
            return sOrg;
        }

        /// <summary>
        /// 根据指定字符，截取相应字符串
        /// </summary>
        /// <param name="sOrg">源字符串</param>
        /// <param name="sLast">指定字符串</param>
        /// <returns>返回值</returns>
        public static string GetPreStrByLast(string sOrg, string sLast)
        {
            int iLast = sOrg.LastIndexOf(sLast);
            if (iLast > 0)
                return sOrg.Substring(0, iLast);
            return sOrg;
        }

        /// <summary>
        /// 根据指定字符，截取相应字符串
        /// </summary>
        /// <param name="sOrg">源字符串</param>
        /// <param name="sEnd">终止字符串</param>
        /// <returns>返回值</returns>
        public static string RemoveEndWith(string sOrg, string sEnd)
        {
            if (sOrg.EndsWith(sEnd))
                sOrg = sOrg.Remove(sOrg.IndexOf(sEnd), sEnd.Length);
            return sOrg;
        }

        /// <summary>
        /// 过滤掉不合格的字符
        /// </summary>
        /// <param name="strSource">要过滤的字符串</param>
        /// <returns></returns>
        public static string CheckFiltrate(string strSource)
        {
            strSource = strSource.Replace("'", "");
            strSource = strSource.Replace("\"", "");
            strSource = strSource.Replace("<", "");
            strSource = strSource.Replace(">", "");
            strSource = strSource.Replace("=", "");
            strSource = strSource.Replace("or", "");
            strSource = strSource.Replace("select", "");
            strSource = strSource.Trim();
            return strSource;

        }

        #endregion  常规字符串操作

        #region HTML相关操作

        /// <summary>
        /// 过滤JS脚本
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string WipeScript(string html)
        {
            Regex[] regex = new Regex[12];
            RegexOptions options;
            options = RegexOptions.Singleline | RegexOptions.IgnoreCase;
            regex[0] = new Regex(@"<marquee[\s\S]+</marquee *>", options);
            regex[1] = new Regex(@"<script[\s\S]+</script *>", options);
            regex[2] = new Regex(@"href *= *[\s\S]*script *:", options);
            regex[3] = new Regex(@"<iframe[\s\S]+</iframe *>", options);
            regex[4] = new Regex(@"<frameset[\s\S]+</frameset *>", options);
            regex[5] = new Regex(@"<input[\s\S]+</input *>", options);
            regex[6] = new Regex(@"<button[\s\S]+</button *>", options);
            regex[7] = new Regex(@"<select[\s\S]+</select *>", options);
            regex[8] = new Regex(@"<textarea[\s\S]+</textarea *>", options);
            regex[9] = new Regex(@"<form[\s\S]+</form *>", options);
            regex[10] = new Regex(@"<embed[\s\S]+</embed *>", options);
            regex[11] = new Regex(@"on[/s/S]*=", options);
            for (int i = 0; i < regex.Length - 1; i++)
            {
                foreach (Match match in regex[i].Matches(html))
                {
                    html = html.Replace(match.Groups[0].ToString(), "");
                }
            }
            return html;
        }

        /// <summary>
        /// 清除HTML标记
        /// </summary>
        /// <param name="sHtml">HTML标记</param>
        /// <returns>返回值</returns>
        public static string ClearTag(string sHtml)
        {
            if (sHtml == "")
                return "";
            var re = new Regex(@"(<[^>\s]*\b(\w)+\b[^>]*>)|(<>)|(&nbsp;)|(&gt;)|(&lt;)|(&amp;)|\r|\n|\t",
                               RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            return re.Replace(sHtml, "");
        }

        /// <summary>
        /// 根据正则清除HTML标记
        /// </summary>
        /// <param name="sHtml">HTML标记</param>
        /// <param name="sRegex">正则</param>
        /// <returns>返回值</returns>
        public static string ClearTag(string sHtml, string sRegex)
        {
            var re = new Regex(sRegex,
                               RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            return re.Replace(sHtml, "");
        }

        /// <summary>
        /// 转化成JS
        /// </summary>
        /// <param name="sHtml">HTML标记</param>
        /// <returns>返回值</returns>
        public static string ConvertToJS(string sHtml)
        {
            var sText = new StringBuilder();
            var re = new Regex(@"\r\n", RegexOptions.IgnoreCase);
            string[] strArray = re.Split(sHtml);
            foreach (string strLine in strArray)
            {
                sText.Append("document.writeln(\"" + strLine.Replace("\"", "\\\"") + "\");\r\n");
            }
            return sText.ToString();
        }

        /// <summary>
        /// 替换空格
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回值</returns>
        public static string ReplaceNbsp(string str)
        {
            string sContent = str;
            if (sContent.Length > 0)
            {
                sContent = sContent.Replace(" ", "");
                sContent = sContent.Replace("&nbsp;", "");
                sContent = "&nbsp;&nbsp;&nbsp;&nbsp;" + sContent;
            }
            return sContent;
        }

        /// <summary>
        /// 字符串转HTML
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回值</returns>
        public static string StringToHtml(string str)
        {
            string sContent = str;
            if (sContent.Length > 0)
            {
                const char csCr = (char)13;
                sContent = sContent.Replace(csCr.ToString(), "<br>");
                sContent = sContent.Replace(" ", "&nbsp;");
                sContent = sContent.Replace("　", "&nbsp;&nbsp;");
            }
            return sContent;
        }

        /// <summary>
        /// 截取长度并转换为HTML
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="num">长度</param>
        /// <returns>返回值</returns>
        public static string AcquireAssignString(string str, int num)
        {
            string sContent = str;
            sContent = GetLetter(sContent, num, false);
            sContent = StringToHtml(sContent);
            return sContent;
        }

        /// <summary>
        /// 此方法与AcquireAssignString的功能已经一样，为了不报错，故保留此方法
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="num">长度</param>
        /// <returns>返回值</returns>
        public static string TranslateToHtmlString(string str, int num)
        {
            string sContent = str;
            sContent = GetLetter(sContent, num, false);
            sContent = StringToHtml(sContent);
            return sContent;
        }

        /// <summary>
        /// 删除所有的html标记 
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回值</returns>
        public static string DelHtmlString(string str)
        {
            string[] regexs =
                {
                    @"<script[^>]*?>.*?</script>",
                    @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                    @"([\r\n])[\s]+",
                    @"&(quot|#34);",
                    @"&(amp|#38);",
                    @"&(lt|#60);",
                    @"&(gt|#62);",
                    @"&(nbsp|#160);",
                    @"&(iexcl|#161);",
                    @"&(cent|#162);",
                    @"&(pound|#163);",
                    @"&(copy|#169);",
                    @"&#(\d+);",
                    @"-->",
                    @"<!--.*\n"
                };

            string[] replaces =
                {
                    "",
                    "",
                    "",
                    "\"",
                    "&",
                    "<",
                    ">",
                    " ",
                    "\xa1", //chr(161),
                    "\xa2", //chr(162),
                    "\xa3", //chr(163),
                    "\xa9", //chr(169),
                    "",
                    "\r\n",
                    ""
                };

            string s = str;
            for (int i = 0; i < regexs.Length; i++)
            {
                s = new Regex(regexs[i], RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(s, replaces[i]);
            }
            return s.Replace("<", "").Replace(">", "").Replace("\r\n", "");
        }

        /// <summary>
        /// 删除字符串中的特定标记 
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="tag">标签</param>
        /// <param name="isContent">是否清除内容 </param>
        /// <returns>返回值</returns>
        public static string DelTag(string str, string tag, bool isContent)
        {
            if (tag == null || tag == " ")
            {
                return str;
            }

            if (isContent) //要求清除内容 
            {
                return Regex.Replace(str, string.Format("<({0})[^>]*>([\\s\\S]*?)<\\/\\1>", tag), "",
                                     RegexOptions.IgnoreCase);
            }

            return Regex.Replace(str, string.Format(@"(<{0}[^>]*(>)?)|(</{0}[^>] *>)|", tag), "",
                                 RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 删除字符串中的一组标记 
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="tagA">标签</param>
        /// <param name="isContent">是否清除内容 </param>
        /// <returns>返回值</returns>
        public static string DelTagArray(string str, string tagA, bool isContent)
        {
            string[] tagAa = tagA.Split(',');

            return tagAa.Aggregate(str, (current, sr1) => DelTag(current, sr1, isContent));
        }

        /// <summary>
        /// 清除HTML标记
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string DropHTML(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring)) return "";
            //删除脚本  
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML  
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }
        #endregion HTML相关操作

        #region 其他字符串操作

        /// <summary>
        /// 格式化为版本号字符串
        /// </summary>
        /// <param name="sVersion">版本号</param>
        /// <returns>返回值</returns>
        public static string SetVersionFormat(string sVersion)
        {
            if (string.IsNullOrEmpty(sVersion)) return "";
            int n = 0, k = 0;

            while (n < 4 && k > -1)
            {
                k = sVersion.IndexOf(".", k + 1);
                n++;
            }
            string stmVersion = k > 0 ? sVersion.Substring(0, k) : sVersion;

            return stmVersion;
        }

        /// <summary>
        /// 在前面补0
        /// </summary>
        /// <param name="sheep">数字</param>
        /// <param name="length">补0长度</param>
        /// <returns>返回值</returns>
        public static string AddZero(int sheep, int length)
        {
            return AddZero(sheep.ToString(), length);
        }

        /// <summary>
        /// 在前面补0
        /// </summary>
        /// <param name="sheep">数字</param>
        /// <param name="length">补0长度</param>
        /// <returns>返回值</returns>
        public static string AddZero(string sheep, int length)
        {
            var goat = new StringBuilder(sheep);
            for (int i = goat.Length; i < length; i++)
            {
                goat.Insert(0, "0");
            }

            return goat.ToString();
        }

        /// <summary>
        /// 简介：获得唯一的字符串
        /// </summary>
        /// <returns>返回值</returns>
        public static string GetUniqueString()
        {
            var rand = new Random();
            return ((int)(rand.NextDouble() * 10000)).ToString() + DateTime.Now.Ticks.ToString();
        }

        /// <summary>
        /// 获得干净,无非法字符的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回值</returns>
        public static string GetCleanJsString(string str)
        {
            str = str.Replace("\"", "“");
            str = str.Replace("'", "”");
            str = str.Replace("\\", "\\\\");
            var re = new Regex(@"\r|\n|\t",
                               RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            str = re.Replace(str, " ");

            return str;
        }

        /// <summary>
        /// 获得干净,无非法字符的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回值</returns>
        public static string GetCleanJsString2(string str)
        {
            str = str.Replace("\"", "\\\"");
            //str = str.Replace("'", "\\\'");
            //str = str.Replace("\\", "\\\\");
            var re = new Regex(@"\r|\n|\t",
                               RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            str = re.Replace(str, " ");

            return str;
        }

        /// <summary>
        /// 将原始字串转换为unicode,格式为\u.\u.
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回值</returns>
        public static string StringToUnicode(string str)
        {
            //中文转为UNICODE字符
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                outStr = str.Aggregate(outStr, (current, t) => current + ("\\u" + ((int)t).ToString("x")));
            }
            return outStr;
        }

        /// <summary>
        /// 将Unicode字串\u.\u.格式字串转换为原始字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回值</returns>
        public static string UnicodeToString(string str)
        {
            string outStr = "";

            str = Regex.Replace(str, "[\r\n]", "", 0);

            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("\\u", "㊣").Split('㊣');
                try
                {
                    outStr += strlist[0];
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        string strTemp = strlist[i];
                        if (!string.IsNullOrEmpty(strTemp) && strTemp.Length >= 4)
                        {
                            strTemp = strlist[i].Substring(0, 4);
                            //将unicode字符转为10进制整数，然后转为char中文字符
                            outStr += (char)int.Parse(strTemp, NumberStyles.HexNumber);
                            outStr += strlist[i].Substring(4);
                        }
                    }
                }
                catch (FormatException)
                {
                    outStr += "Erorr"; //ex.Message;
                }
            }
            return outStr;
        }

        /// <summary>
        /// GB2312转换成unicode编码 
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回值</returns>
        public static string GB2Unicode(string str)
        {
            string hexs = "";
            Encoding gb = Encoding.GetEncoding("GB2312");
            byte[] gbBytes = gb.GetBytes(str);
            for (int i = 0; i < gbBytes.Length; i++)
            {
                string hh = "%" + gbBytes[i].ToString("X");
                hexs += hh;
            }
            return hexs;
        }

        /// <summary>
        /// 得到单个字符的值
        /// </summary>
        /// <param name="ch"></param>
        /// <returns>返回值</returns>
        private static ushort GetByte(char ch)
        {
            ushort rtnNum;
            switch (ch)
            {
                case 'a':
                case 'A':
                    rtnNum = 10;
                    break;
                case 'b':
                case 'B':
                    rtnNum = 11;
                    break;
                case 'c':
                case 'C':
                    rtnNum = 12;
                    break;
                case 'd':
                case 'D':
                    rtnNum = 13;
                    break;
                case 'e':
                case 'E':
                    rtnNum = 14;
                    break;
                case 'f':
                case 'F':
                    rtnNum = 15;
                    break;
                default:
                    rtnNum = ushort.Parse(ch.ToString());
                    break;
            }
            return rtnNum;
        }

        /// <summary>
        /// 转换一个字符，输入如"Π"中的"03a0"
        /// </summary>
        /// <param name="unicodeSingle">unicode字符</param>
        /// <returns>返回值</returns>
        public static string ConvertSingle(string unicodeSingle)
        {
            if (unicodeSingle.Length != 4)
                return null;
            Encoding unicode = Encoding.Unicode;
            var unicodeBytes = new byte[] { 0, 0 };
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        unicodeBytes[1] += (byte)(GetByte(unicodeSingle[i]) * 16);
                        break;
                    case 1:
                        unicodeBytes[1] += (byte)GetByte(unicodeSingle[i]);
                        break;
                    case 2:
                        unicodeBytes[0] += (byte)(GetByte(unicodeSingle[i]) * 16);
                        break;
                    case 3:
                        unicodeBytes[0] += (byte)GetByte(unicodeSingle[i]);
                        break;
                }
            }

            var asciiChars = new char[unicode.GetCharCount(unicodeBytes, 0, unicodeBytes.Length)];
            unicode.GetChars(unicodeBytes, 0, unicodeBytes.Length, asciiChars, 0);
            var asciiString = new string(asciiChars);

            return asciiString;
        }

        /// <summary>
        /// unicode编码转换成GB2312汉字 
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回值</returns>
        public static string UtoGB(string str)
        {
            string[] ss = str.Replace("\\", "").Split('u');
            var bs = new Byte[ss.Length - 1];
            for (int i = 1; i < ss.Length; i++)
            {
                bs[i - 1] = Convert.ToByte(Convert2Hex(ss[i])); //ss[0]为空串   
            }
            char[] chrs = Encoding.GetEncoding("GB2312").GetChars(bs);
            string s = "";
            for (int i = 0; i < chrs.Length; i++)
            {
                s += chrs[i].ToString();
            }
            return s;
        }

        /// <summary>
        /// 字符串转成Hex
        /// </summary>
        /// <param name="pstr">字符串</param>
        /// <returns>返回值</returns>
        private static string Convert2Hex(string pstr)
        {
            if (pstr.Length == 2)
            {
                pstr = pstr.ToUpper();
                const string hexstr = "0123456789ABCDEF";
                int cint = hexstr.IndexOf(pstr.Substring(0, 1)) * 16 + hexstr.IndexOf(pstr.Substring(1, 1));
                return cint.ToString();
            }
            return "";
        }

        #endregion 其他字符串操作

        #region 判断对象是否为空
        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="data">要验证的对象</param>        
        public static bool IsNullOrEmpty<T>(T data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //如果为""
            if (data.GetType() == typeof(string))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                {
                    return true;
                }
            }

            //如果为DBNull
            if (data.GetType() == typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }

        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <param name="data">要验证的对象</param>
        public static bool IsNullOrEmpty(object data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //如果为""
            if (data.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                {
                    return true;
                }
            }

            //如果为DBNull
            if (data.GetType() == typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }
        #endregion

        
        
        #region 分割字符串


        /// <summary>
        /// 把字符串按照分隔符转换成 List
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符</param>
        /// <param name="toLower">是否转换为小写</param>
        /// <returns></returns>
        public static List<string> GetStrArray(string str, char speater, bool toLower)
        {
            List<string> list = new List<string>();
            string[] ss = str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    string strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }
            }
            return list;
        }

        /// <summary>
        /// 把字符串转 按照, 分割 换为数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new Char[] { ',' });
        }

        /// <summary>
        /// 把 List<string> 按照分隔符组装成 string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="speater"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 得到数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<int> list)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i].ToString());
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 得到数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayValueStr(Dictionary<int, int> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<int, int> kvp in list)
            {
                sb.Append(kvp.Value + ",");
            }
            if (list.Count > 0)
            {
                return DelLastComma(sb.ToString());
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splitstr"></param>
        /// <returns></returns>
        public static string[] SplitMulti(string str, string splitstr)
        {
            string[] strArray = null;
            if ((str != null) && (str != ""))
            {
                strArray = new Regex(splitstr).Split(str);
            }
            return strArray;
        }


        /// <summary>
        /// 把字符串按照指定分隔符装成 List 去除重复
        /// </summary>
        /// <param name="o_str"></param>
        /// <param name="sepeater"></param>
        /// <returns></returns>
        public static List<string> GetSubStringList(string o_str, char sepeater)
        {
            List<string> list = new List<string>();
            string[] ss = o_str.Split(sepeater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != sepeater.ToString())
                {
                    list.Add(s);
                }
            }
            return list;
        }
        
        #endregion
        
        #region 删除最后一个字符之后的字符

        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }

        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }

        #endregion
        
        #region 将字符串样式转换为纯字符串
        /// <summary>
        ///  将字符串样式转换为纯字符串
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="SplitString"></param>
        /// <returns></returns>
        public static string GetCleanStyle(string StrList, string SplitString)
        {
            string RetrunValue = "";
            //如果为空，返回空值
            if (StrList == null)
            {
                RetrunValue = "";
            }
            else
            {
                //返回去掉分隔符
                string NewString = "";
                NewString = StrList.Replace(SplitString, "");
                RetrunValue = NewString;
            }
            return RetrunValue;
        }
        #endregion

        #region 将字符串转换为新样式
        /// <summary>
        /// 将字符串转换为新样式
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="NewStyle"></param>
        /// <param name="SplitString"></param>
        /// <param name="Error"></param>
        /// <returns></returns>
        public static string GetNewStyle(string StrList, string NewStyle, string SplitString, out string Error)
        {
            string ReturnValue = "";
            //如果输入空值，返回空，并给出错误提示
            if (StrList == null)
            {
                ReturnValue = "";
                Error = "请输入需要划分格式的字符串";
            }
            else
            {
                //检查传入的字符串长度和样式是否匹配,如果不匹配，则说明使用错误。给出错误信息并返回空值
                int strListLength = StrList.Length;
                int NewStyleLength = GetCleanStyle(NewStyle, SplitString).Length;
                if (strListLength != NewStyleLength)
                {
                    ReturnValue = "";
                    Error = "样式格式的长度与输入的字符长度不符，请重新输入";
                }
                else
                {
                    //检查新样式中分隔符的位置
                    string Lengstr = "";
                    for (int i = 0; i < NewStyle.Length; i++)
                    {
                        if (NewStyle.Substring(i, 1) == SplitString)
                        {
                            Lengstr = Lengstr + "," + i;
                        }
                    }
                    if (Lengstr != "")
                    {
                        Lengstr = Lengstr.Substring(1);
                    }
                    //将分隔符放在新样式中的位置
                    string[] str = Lengstr.Split(',');
                    foreach (string bb in str)
                    {
                        StrList = StrList.Insert(int.Parse(bb), SplitString);
                    }
                    //给出最后的结果
                    ReturnValue = StrList;
                    //因为是正常的输出，没有错误
                    Error = "";
                }
            }
            return ReturnValue;
        }
        #endregion
        
        #region 获取正确的Id，如果不是正整数，返回0
        /// <summary>
        /// 获取正确的Id，如果不是正整数，返回0
        /// </summary>
        /// <param name="_value"></param>
        /// <returns>返回正确的整数ID，失败返回0</returns>
        public static int StrToId(string _value)
        {
            if (IsNumberId(_value))
                return int.Parse(_value);
            else
                return 0;
        }
        #endregion

        #region 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。
        /// <summary>
        /// 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。(0除外)
        /// </summary>
        /// <param name="_value">需验证的字符串。。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsNumberId(string _value)
        {
            return QuickValidate("^[1-9]*[0-9]*$", _value);
        }
        #endregion

        #region 快速验证一个字符串是否符合指定的正则表达式。
        /// <summary>
        /// 快速验证一个字符串是否符合指定的正则表达式。
        /// </summary>
        /// <param name="_express">正则表达式的内容。</param>
        /// <param name="_value">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool QuickValidate(string _express, string _value)
        {
            if (_value == null) return false;
            Regex myRegex = new Regex(_express);
            if (_value.Length == 0)
            {
                return false;
            }
            return myRegex.IsMatch(_value);
        }
        #endregion
        
        #region 截取字符串

        /// <summary>
        /// 截取字符长度
        /// </summary>
        /// <param name="inputString">字符</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string CutString(string inputString, int len)
        {
            if (string.IsNullOrEmpty(inputString))
                return "";
            inputString = DropHTML(inputString);
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号 
            byte[] mybyte = Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString += "…";
            return tempString;
        }

        /// <summary>
        /// 截取指定长度的字符串
        /// </summary>
        /// <param name="StrText">要截取的字符串</param>
        /// <param name="Index">开始截取的位置</param>
        /// <param name="Last">结束位置</param>
        /// <returns></returns>
        public static string CutString(string strText, int Index, int Last)
        {
            //判断要截取的字符串是否为空
            if (!string.IsNullOrEmpty(strText))
            {
                //要截取的字符串的长度
                int Lenght = strText.Length;
                if (Lenght > Last)
                    return strText.Substring(Index, Last) + "…";
                else
                    return strText;

            }

            return "--";
        }

        /// <summary>
        /// 截取指定位置以后的字符串
        /// </summary>
        /// <param name="obj">要截取的字符串</param>
        /// <param name="len">开始截取位置</param>
        /// <returns></returns>
        public static string CutString(object obj, int len)
        {
            if (obj != null)
            {
                string str = obj.ToString();
                if (str.Length > len)
                    return str = str.Substring(len) + "…";
            }
            return obj + "";
        }

        #region   字符串长度区分中英文截取
        /// <summary>   
        /// 截取文本，区分中英文字符，中文算两个长度，英文算一个长度
        /// </summary>
        /// <param name="str">待截取的字符串</param>
        /// <param name="length">需计算长度的字符串</param>
        /// <returns>string</returns>
        public static string GetSubString(string str, int length)
        {
            string temp = str;
            int j = 0;
            int k = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (Regex.IsMatch(temp.Substring(i, 1), @"[\u4e00-\u9fa5]+"))
                {
                    j += 2;
                }
                else
                {
                    j += 1;
                }
                if (j <= length)
                {
                    k += 1;
                }
                if (j > length)
                {
                    return temp.Substring(0, k) + "……";
                }
            }
            return temp;
        }
        #endregion

        #endregion

        #region 获取唯一标识

        /// <summary>
        /// 获取唯一标识（true：不带符号 “-”）
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string GetGuidString(bool flag = true)
        {
            if (flag)
            {
                //返回不带符号 “-”的唯一标识
                return Guid.NewGuid().ToString("N");
            }
            //返回带符号 “-”的唯一标识
            return Guid.NewGuid().ToString();
        }

        #endregion

        #region 检测是否有Sql危险字符
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 检查危险字符
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Filter(string sInput)
        {
            if (sInput == null || sInput == "")
                return null;
            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("字符串中含有非法字符!");
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output;
        }

        /// <summary> 
        /// 检查过滤设定的危险字符
        /// </summary> 
        /// <param name="InText">要过滤的字符串 </param> 
        /// <returns>如果参数存在不安全字符，则返回true </returns> 
        public static bool SqlFilter(string word, string InText)
        {
            if (InText == null)
                return false;
            foreach (string i in word.Split('|'))
            {
                if ((InText.ToLower().IndexOf(i + " ") > -1) || (InText.ToLower().IndexOf(" " + i) > -1))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        ///  非法SQL字符检测
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckSql(string str)
        {
            string pattern = @"select|insert|delete|from|count\(|drop table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec   master|netlocalgroup administrators|net user|or|and";
            if (Regex.IsMatch(str, pattern, RegexOptions.IgnoreCase))
                return true;
            return false;
        }

        public static string CheckFilterSql(string str)
        {
            string[] pattern = { "select", "insert", "delete", "from", "count\\(", "drop table", "update", "truncate", "asc\\(", "mid\\(", "char\\(", "xp_cmdshell", "exec   master", "netlocalgroup administrators", "net user", "and" };// 去掉 "or" 关键字
            for (int i = 0; i < pattern.Length; i++)
            {
                str = str.Replace(pattern[i].ToString(), "");
            }
            return str;
        }


        #endregion

        #region 检查是否为IP地址
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion
    }
}
