using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Common
{
    /// <summary>
    /// 正则验证工具类
    /// </summary>
    public class RegexHelper
    {

        #region 验证IP地址是否合法


        /// <summary>
        /// 验证IP地址是否合法，合法返回true
        /// </summary>
        /// <param name="ip">要验证的IP地址</param> 
        /// <returns>合法返回true</returns>       
        public static bool IsIP(string ip)
        {
            //如果为空，认为验证合格
            if (StringHelper.IsNullOrEmpty(ip))
            {
                return false;
            }

            //清除要验证字符串中的空格
            ip = ip.Trim();

            //模式字符串
            string pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";

            //验证
            return IsMatch(ip, pattern);
        }

        #endregion

        #region  验证EMail是否合法
        /// <summary>
        /// 验证EMail是否合法，合法返回true
        /// </summary>
        /// <param name="email">要验证的Email</param>
        /// <returns>正确返回true</returns>
        public static bool IsEmail(string email)
        {
            //如果为空，认为验证不合格
            if (StringHelper.IsNullOrEmpty(email))
            {
                return false;
            }

            //清除要验证字符串中的空格
            email = email.Trim();

            //模式字符串
            string pattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";

            //验证
            return IsMatch(email, pattern);
        }
        #endregion

        #region 验证是否为整数
        /// <summary>
        /// 验证是否为整数，是整数返回true
        /// </summary>
        /// <param name="number">要验证的整数</param>        
        public static bool IsInt(string number)
        {
            //如果为空，认为验证不合格
            if (StringHelper.IsNullOrEmpty(number))
            {
                return false;
            }

            //清除要验证字符串中的空格
            number = number.Trim();

            //模式字符串
            string pattern = @"^[0-9]+[0-9]*$";

            //验证
            return IsMatch(number, pattern);
        }
        #endregion

        #region 验证是否为数字
        /// <summary>
        /// 验证是否为数字，是数字返回true
        /// </summary>
        /// <param name="number">要验证的数字</param>        
        public static bool IsNumber(string number)
        {
            //如果为空，认为验证不合格
            if (StringHelper.IsNullOrEmpty(number))
            {
                return false;
            }

            //清除要验证字符串中的空格
            number = number.Trim();

            //模式字符串
            string pattern = @"^[0-9]+[0-9]*[.]?[0-9]*$";

            //验证
            return IsMatch(number, pattern);
        }
        #endregion

        #region 验证日期是否合法
        /// <summary>
        /// 验证日期是否合法,对不规则的作了简单处理
        /// </summary>
        /// <param name="date">日期</param>
        public static bool IsDate(ref string date)
        {
            //如果为空，认为不验证合格
            if (StringHelper.IsNullOrEmpty(date))
            {
                return false;
            }

            //清除要验证字符串中的空格
            date = date.Trim();

            //替换\
            date = date.Replace(@"\", "-");
            //替换/
            date = date.Replace(@"/", "-");

            //如果查找到汉字"今",则认为是当前日期
            if (date.IndexOf("今") != -1)
            {
                date = DateTime.Now.ToString();
            }

            try
            {
                //用转换测试是否为规则的日期字符
                date = Convert.ToDateTime(date).ToString("d");
                return true;
            }
            catch
            {
                //如果日期字符串中存在非数字，则返回false
                if (!IsInt(date))
                {
                    return false;
                }

                #region 对纯数字进行解析
                //对8位纯数字进行解析
                if (date.Length == 8)
                {
                    //获取年月日
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);
                    string day = date.Substring(6, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12 || Convert.ToInt32(day) > 31)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month + "-" + day).ToString("d");
                    return true;
                }

                //对6位纯数字进行解析
                if (date.Length == 6)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month).ToString("d");
                    return true;
                }

                //对5位纯数字进行解析
                if (date.Length == 5)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 1);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = year + "-" + month;
                    return true;
                }

                //对4位纯数字进行解析
                if (date.Length == 4)
                {
                    //获取年
                    string year = date.Substring(0, 4);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year).ToString("d");
                    return true;
                }
                #endregion

                return false;
            }
        }
        #endregion

        #region 验证身份证是否合法
        /// <summary>
        /// 验证身份证是否合法，合法返回true
        /// </summary>
        /// <param name="idCard">要验证的身份证</param>        
        public static bool IsIdCard(string idCard)
        {
            //如果为空，认为不验证合格
            if (StringHelper.IsNullOrEmpty(idCard))
            {
                return false;
            }

            //清除要验证字符串中的空格
            idCard = idCard.Trim();

            //模式字符串
            StringBuilder pattern = new StringBuilder();
            pattern.Append(@"^(11|12|13|14|15|21|22|23|31|32|33|34|35|36|37|41|42|43|44|45|46|");
            pattern.Append(@"50|51|52|53|54|61|62|63|64|65|71|81|82|91)");
            pattern.Append(@"(\d{13}|\d{15}[\dx])$");

            //验证
            return IsMatch(idCard, pattern.ToString());
        }
        #endregion
        
        #region 正则表达式

        /// <summary>
        /// 判断用户名称格式，
        /// 只能以字母开头，长度的6-20
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CheckUserName(string name)
        {
            return Regex.IsMatch(name, @"^[a-zA-Z]{1}[a-zA-Z0-9]{5,19}$");
        }

        /// <summary>
        /// 必须为数字
        /// </summary>
        /// <param name="strNum"></param>
        /// <returns></returns>
        public static bool CheckIsNumber(string strNum)
        {
            return Regex.IsMatch(strNum, @"^\+?[1-9][0-9]*$");
        }

        /// <summary>
        /// 邮箱检测 
        /// </summary>
        /// <param name="strEMail"></param>
        /// <returns></returns>
        public static bool CheckIsEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }

        /// <summary>
        /// 判断密码格式，
        /// 只能以字母开头，由6-20位字母、数字、下划线、点组成!
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool CheckPassWord(string password)
        {
            return Regex.IsMatch(password, @"^[a-zA-Z]{1}([a-zA-Z0-9]|[._]){5,19}$");
        }

        /// <summary>
        /// 替换单行文本框输入的非法字符
        /// </summary>
        /// <returns></returns>
        public static string CheckReplaceBadChar(string strSource)
        {
            strSource = strSource.Replace("<", "&lt;");
            strSource = strSource.Replace(">", "&gt;").ToString().Replace(">", "&gt;");
            strSource = strSource.Replace("'", "&#39;");
            strSource = strSource.Replace("\"", "&quot;");
            strSource = strSource.Trim();
            return strSource;
        }
        
        /// <summary>
        /// 判断手机号格式是否正确
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public static bool CheckTelephone(string Phone)
        {
            return Regex.IsMatch(Phone, @"^1[34578]\d{9}$");
        }

        /// <summary>
        /// 匹配国内电话号码（匹配形式如 0511-4405222 或 021-87888822）
        /// </summary>
        /// <param name="FixTel"></param>
        /// <returns></returns>
        public static bool CheckFixTel(string FixTel)
        {
            return Regex.IsMatch(FixTel, @"^\d{3}-\d{8}|\d{4}-\d{7}$");
        }





        #endregion

        #region 用“*”号替换


        /// <summary>
        /// 手机号用“*”号隐藏中间四位数字
        /// </summary>
        /// <param name="Phone">手机号</param>
        /// <returns></returns>
        public static string ToRepPhone(string Phone)
        {
            if (!string.IsNullOrWhiteSpace(Phone))
            {
                return Regex.Replace(Phone, "(\\d{3})\\d{4}(\\d{4})", "$1****$2");
            }
            return Phone;
        }

        /// <summary>
        /// 邮箱用“*”号隐藏前面的字母
        /// </summary>
        /// <param name="Email">邮箱</param>
        /// <returns></returns>
        public static string ToRepEmail(string Email)
        {
            if (!string.IsNullOrWhiteSpace(Email))
            {
                string emails = Regex.Replace(Email, "(\\w?)(\\w+)(\\w)(@\\w+\\.[a-z]+(\\.[a-z]+)?)", "$1****$3$4");
                return emails;
            }
            return Email;
        }

        /// <summary>
        /// 证件号用“*”号隐藏中间部分
        /// </summary>
        /// <param name="IDCardNo">证件号</param>
        /// <returns>替换后的证件号</returns>
        public static string ToRepIDCardNo(string IDCardNo)
        {
            string IdCardNo = "";
            if (!string.IsNullOrWhiteSpace(IDCardNo))
            {
                IdCardNo = IDCardNo;
                switch (IDCardNo.Length)
                {
                    case 15:
                        IdCardNo = Regex.Replace(IDCardNo, "(\\d{4})\\d{8}(\\w{3})", "$1********$2");
                        break;
                    case 18:
                        IdCardNo = Regex.Replace(IDCardNo, "(\\d{4})\\d{10}(\\w{4})", "$1********$2");
                        break;
                    default:
                        IdCardNo = ToRepStr(IDCardNo, "********");
                        break;
                }
            }
            return IdCardNo;
        }

        /// <summary>
        /// 去掉各种括号中的内容，包含括号
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string ToRepBracket(string Str)
        {
            if (!string.IsNullOrWhiteSpace(Str))
            {
               return Regex.Replace(Str, "\\(.*?\\)|\\{.*?}|\\[.*?]|【.*?】|（.*?）", "").Trim();
            }
            return Str;
        }

        /// <summary>
        /// 任意字符串隐藏中间部分
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string ToRepStr(string Str, string rep = "**")
        {
            string newStr = "";
            if (!string.IsNullOrWhiteSpace(Str))
            {
                int length = Str.Length;
                if (length >= 2 && length <= 10)
                {
                    newStr = Str.Substring(0, 1) + rep + Str.Substring(length - 1, 1);
                }
                else if (length > 10)
                {
                    newStr = Str.Substring(0, 4) + rep + Str.Substring(length - 4, 4);
                }
                else
                {
                    newStr = Str;
                }
            }
            return newStr;
        }

        #endregion

        #region 去除所有空格

        /// <summary>
        /// 去除所有空格
        /// </summary>
        /// <param name="myString"></param>
        /// <returns></returns>
        public static string ToRepTrim(string myString)
        {
            if (!string.IsNullOrWhiteSpace(myString))
            {
                return Regex.Replace(myString, @"\s", "");
            }
            return myString;
        }

        #endregion




        #region 验证输入字符串是否与模式字符串匹配
        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>        
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="options">筛选条件</param>
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }
        #endregion

    }
}
