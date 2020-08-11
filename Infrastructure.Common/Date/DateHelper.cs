using System;
using System.Globalization;

namespace Infrastructure.Common
{
    /// <summary>
    /// 时间操作类
    /// </summary>
    public class DateHelper
    {

        #region 类实例方法

        private DateTime dt = DateTime.Now;

        public DateHelper(DateTime dateTime)
        {
            dt = dateTime;
        }
        public DateHelper(string dateTime)
        {
            dt = DateTime.Parse(dateTime);
        }
        /// <summary>
        /// 哪天
        /// </summary>
        /// <param name="days">7天前:-7 7天后:+7</param>
        /// <returns></returns>
        public string GetTheDay(int? days)
        {
            int day = days ?? 0;
            return dt.AddDays(day).ToShortDateString();
        }

        /// <summary>
        /// 周日
        /// </summary>
        /// <param name="weeks">上周-1 下周+1 本周0</param>
        /// <returns></returns>
        public string GetSunday(int? weeks)
        {
            int week = weeks ?? 0;
            return dt.AddDays(Convert.ToDouble((0 - Convert.ToInt16(dt.DayOfWeek))) + 7 * week).ToShortDateString();
        }
        /// <summary>
        /// 周六
        /// </summary>
        /// <param name="weeks">上周-1 下周+1 本周0</param>
        /// <returns></returns>
        public string GetSaturday(int? weeks)
        {
            int week = weeks ?? 0;
            return dt.AddDays(Convert.ToDouble((6 - Convert.ToInt16(dt.DayOfWeek))) + 7 * week).ToShortDateString();
        }
        /// <summary>
        /// 月第一天
        /// </summary>
        /// <param name="months">上月-1 本月0 下月1</param>
        /// <returns></returns>
        public string GetFirstDayOfMonth(int? months)
        {
            int month = months ?? 0;
            return DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(month).ToShortDateString();
        }
        /// <summary>
        /// 月最后一天
        /// </summary>
        /// <param name="months">上月0 本月1 下月2</param>
        /// <returns></returns>
        public string GetLastDayOfMonth(int? months)
        {
            int month = months ?? 0;
            return DateTime.Parse(dt.ToString("yyyy-MM-01")).AddMonths(month).AddDays(-1).ToShortDateString();
        }
        /// <summary>
        /// 年度第一天
        /// </summary>
        /// <param name="years">上年度-1 下年度+1</param>
        /// <returns></returns>
        public string GetFirstDayOfYear(int? years)
        {
            int year = years ?? 0;
            return DateTime.Parse(dt.ToString("yyyy-01-01")).AddYears(year).ToShortDateString();
        }
        /// <summary>
        /// 年度最后一天
        /// </summary>
        /// <param name="years">上年度0 本年度1 下年度2</param>
        /// <returns></returns>
        public string GetLastDayOfYear(int? years)
        {
            int year = years ?? 0;
            return DateTime.Parse(dt.ToString("yyyy-01-01")).AddYears(year).AddDays(-1).ToShortDateString();
        }
        /// <summary>
        /// 季度第一天
        /// </summary>
        /// <param name="quarters">上季度-1 下季度+1</param>
        /// <returns></returns>
        public string GetFirstDayOfQuarter(int? quarters)
        {
            int quarter = quarters ?? 0;
            return dt.AddMonths(quarter * 3 - ((dt.Month - 1) % 3)).ToString("yyyy-MM-01");
        }
        /// <summary>
        /// 季度最后一天
        /// </summary>
        /// <param name="quarters">上季度0 本季度1 下季度2</param>
        /// <returns></returns>
        public string GetLastDayOfQuarter(int? quarters)
        {
            int quarter = quarters ?? 0;
            return DateTime.Parse(dt.AddMonths(quarter * 3 - ((dt.Month - 1) % 3)).ToString("yyyy-MM-01")).AddDays(-1).ToShortDateString();
        }
        /// <summary>
        /// 中文星期
        /// </summary>
        /// <returns></returns>
        public string GetDayOfWeekCN()
        {
            string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return Day[Convert.ToInt16(dt.DayOfWeek)];
        }
        /// <summary>
        /// 获取星期数字形式,周一开始
        /// </summary>
        /// <returns></returns>
        public int GetDayOfWeekNum()
        {
            int day = (Convert.ToInt16(dt.DayOfWeek) == 0) ? 7 : Convert.ToInt16(dt.DayOfWeek);
            return day;
        }
        #endregion
        
        /// <summary>
        /// 获取某一年有多少周
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns>该年周数</returns>
        public static int GetWeekAmount(int year)
        {
            var end = new DateTime(year, 12, 31); //该年最后一天
            var gc = new GregorianCalendar();
            return gc.GetWeekOfYear(end, CalendarWeekRule.FirstDay, DayOfWeek.Monday); //该年星期数
        }

        /// <summary>
        /// 返回年度第几个星期   默认星期日是第一天
        /// </summary>
        /// <param name="date">时间</param>
        /// <returns></returns>
        public static int WeekOfYear(DateTime date)
        {
            GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }

        /// <summary>
        /// 返回年度第几个星期
        /// </summary>
        /// <param name="date">时间</param>
        /// <param name="week">一周的开始日期</param>
        /// <returns></returns>
        public static int WeekOfYear(DateTime date, DayOfWeek week)
        {
            GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(date, CalendarWeekRule.FirstDay, week);
        }


        /// <summary>
        /// 得到一年中的某周的起始日和截止日
        /// 年 nYear
        /// 周数 nNumWeek
        /// 周始 out dtWeekStart
        /// 周终 out dtWeekeEnd
        /// </summary>
        /// <param name="nYear">年份</param>
        /// <param name="nNumWeek">第几周</param>
        /// <param name="dtWeekStart">开始日期</param>
        /// <param name="dtWeekeEnd">结束日期</param>
        public static void GetWeekTime(int nYear, int nNumWeek, out DateTime dtWeekStart, out DateTime dtWeekeEnd)
        {
            DateTime dt = new DateTime(nYear, 1, 1);
            dt = dt + new TimeSpan((nNumWeek - 1) * 7, 0, 0, 0);
            dtWeekStart = dt.AddDays(-(int)dt.DayOfWeek + (int)DayOfWeek.Monday);
            dtWeekeEnd = dt.AddDays((int)DayOfWeek.Saturday - (int)dt.DayOfWeek + 1);
        }

        /// <summary>
        /// 得到一年中的某周的起始日和截止日    周一到周五  工作日
        /// </summary>
        /// <param name="nYear">年份</param>
        /// <param name="nNumWeek">第几周</param>
        /// <param name="dtWeekStart">开始日期</param>
        /// <param name="dtWeekeEnd">结束日期</param>
        public static void GetWeekWorkTime(int nYear, int nNumWeek, out DateTime dtWeekStart, out DateTime dtWeekeEnd)
        {
            DateTime dt = new DateTime(nYear, 1, 1);
            dt = dt + new TimeSpan((nNumWeek - 1) * 7, 0, 0, 0);
            dtWeekStart = dt.AddDays(-(int)dt.DayOfWeek + (int)DayOfWeek.Monday);
            dtWeekeEnd = dt.AddDays((int)DayOfWeek.Saturday - (int)dt.DayOfWeek + 1).AddDays(-2);
        }
        
        #region 格式化日期时间
        /// <summary>
        /// 格式化日期时间
        /// </summary>
        /// <param name="dateTime1">日期时间</param>
        /// <param name="dateMode">显示模式</param>
        /// <returns>0-9种模式的日期</returns>
        public static string FormatDate(DateTime dateTime1, int dateMode)
        {
            string strTime = "";

            switch (dateMode)
            {
                case 0:
                    strTime = dateTime1.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
                    break;
                case 1:
                    strTime = dateTime1.ToString("yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
                    break;
                case 2:
                    strTime = dateTime1.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
                    break;
                case 3:
                    strTime = dateTime1.ToString("yyyy/MM/dd HH:mm", DateTimeFormatInfo.InvariantInfo);
                    break;
                case 4:
                    strTime = dateTime1.ToString("yyyy年MM月dd日", DateTimeFormatInfo.InvariantInfo);
                    break;
                case 5:
                    strTime = dateTime1.ToString("MM-dd", DateTimeFormatInfo.InvariantInfo);
                    break;
                case 6:
                    strTime = dateTime1.ToString("MM/dd", DateTimeFormatInfo.InvariantInfo);
                    break;
                case 7:
                    strTime = dateTime1.ToString("MM月dd日", DateTimeFormatInfo.InvariantInfo);
                    break;
                case 8:
                    strTime = dateTime1.ToString("yyyy-MM", DateTimeFormatInfo.InvariantInfo);
                    break;
                case 9:
                    strTime = dateTime1.ToString("yyyy/MM", DateTimeFormatInfo.InvariantInfo);
                    break;
                case 10:
                    strTime = dateTime1.ToString("yyyy年MM月", DateTimeFormatInfo.InvariantInfo);
                    break;
                default:
                    strTime = dateTime1.ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
                    break;
            }
            string returnTime = dateTime1.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
            if (returnTime == "1900-01-01")
            {
                return "";
            }
            return strTime;
        }
        
        /// <summary>
        /// 格式化日期时间
        /// </summary>
        /// <param name="dateTime1">日期时间</param>
        /// <param name="dateMode">显示模式，如：yyyy-MM-dd</param>
        /// <returns></returns>
        public static string FormatDate(DateTime dateTime1, string dateMode)
        {
            if (string.IsNullOrEmpty(dateMode))
            {
                dateMode = "yyyy-MM-dd";
            }
            string returnTime = dateTime1.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
            if (returnTime == "1900-01-01")
            {
                return "--";
            }
            return dateTime1.ToString(dateMode, DateTimeFormatInfo.InvariantInfo);
        }

        #endregion

        #region 得到随机日期
        /// <summary>
        /// 得到随机日期
        /// </summary>
        /// <param name="time1">起始日期</param>
        /// <param name="time2">结束日期</param>
        /// <returns>间隔日期之间的 随机日期</returns>
        public static DateTime GetRandomTime(DateTime time1, DateTime time2)
        {
            Random random = new Random();
            DateTime minTime = new DateTime();
            DateTime maxTime = new DateTime();

            TimeSpan ts = new TimeSpan(time1.Ticks - time2.Ticks);

            // 获取两个时间相隔的秒数
            double dTotalSecontds = ts.TotalSeconds;
            int iTotalSecontds = 0;

            if (dTotalSecontds > int.MaxValue)
            {
                iTotalSecontds = int.MaxValue;
            }
            else if (dTotalSecontds < int.MinValue)
            {
                iTotalSecontds = int.MinValue;
            }
            else
            {
                iTotalSecontds = (int)dTotalSecontds;
            }


            if (iTotalSecontds > 0)
            {
                minTime = time2;
                maxTime = time1;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = time1;
                maxTime = time2;
            }
            else
            {
                return time1;
            }

            int maxValue = iTotalSecontds;

            if (iTotalSecontds <= int.MinValue)
                maxValue = int.MinValue + 1;

            int i = random.Next(Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }
        #endregion

        #region 获取时间

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDateTime()
        {
            return Convert.ToDateTime(FormatDate(DateTime.Now, "yyyy-MM-dd HH:mm:ss.fff"));
        }

        /// <summary>
        /// 获取日期 1900-01-01
        /// </summary>
        /// <returns></returns>
        public static DateTime mDataTime()
        {
            return new DateTime(1900, 1, 1);
        }

        #endregion




        #region 得到两个日期相差的天数
        /// <summary>
        /// 得到两个日期相差的天数
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public static int GetDays(DateTime startDate, DateTime endDate)
        {
            //获取两个时间之间的间隔  
            TimeSpan ts = endDate.Subtract(startDate);

            return ts.Days;
        }

        #endregion

        #region 得到两个日期相差的小时数
        /// <summary>
        /// 得到两个日期相差的小时数
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>string</returns>
        public static string GetStrHours(DateTime startDate, DateTime endDate)
        {
            //获取两个时间之间的间隔  
            TimeSpan ts = endDate.Subtract(startDate);

            return (ts.Hours < 10) ? ("0" + ts.Hours.ToString()) : ts.Hours.ToString();
        }

        /// <summary>
        /// 得到两个日期相差的小时数
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>int</returns>
        public static int GetHours(DateTime startDate, DateTime endDate)
        {
            //获取两个时间之间的间隔  
            TimeSpan ts = endDate.Subtract(startDate);

            return ts.Hours;
        }

        #endregion

        #region 得到两个日期相差分钟数
        /// <summary>
        /// 得到两个日期相差分钟数
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>string</returns>
        public static string GetStrMinutes(DateTime startDate, DateTime endDate)
        {
            //获取两个时间之间的间隔  
            TimeSpan ts = endDate.Subtract(startDate);
            return (ts.Minutes < 10) ? ("0" + ts.Minutes.ToString()) : ts.Minutes.ToString();
        }

        /// <summary>
        /// 得到两个日期相差分钟数
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>int</returns>
        public static int GetMinutes(DateTime startDate, DateTime endDate)
        {
            //得到两个日期相差分钟数  
            TimeSpan ts = endDate.Subtract(startDate);
            return ts.Minutes;
        }
        #endregion

        #region 得到两个日期相差秒数
        /// <summary>
        /// 得到两个日期相差秒数
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>string</returns>
        public static string GetStrSeconds(DateTime startDate, DateTime endDate)
        {
            //获取两个时间之间的间隔  
            TimeSpan ts = endDate.Subtract(startDate);
            return (ts.Seconds < 10) ? ("0" + ts.Seconds.ToString()) : ts.Seconds.ToString();
        }

        /// <summary>
        /// 得到两个日期相差秒数
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>int</returns>
        public static int GetSeconds(DateTime startDate, DateTime endDate)
        {
            //得到两个日期相差分钟数  
            TimeSpan ts = endDate.Subtract(startDate);
            return ts.Seconds;
        }
        #endregion

        #region 返回每月的第一天和最后一天
        /// <summary>
        /// 返回每月的第一天和最后一天
        /// </summary>
        /// <param name="month"></param>
        /// <param name="firstDay"></param>
        /// <param name="lastDay"></param>
        public static void ReturnDateFormat(int month, out string firstDay, out string lastDay)
        {
            int year = DateTime.Now.Year + month / 12;
            if (month != 12)
            {
                month = month % 12;
            }
            switch (month)
            {
                case 1:
                    firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                    lastDay = DateTime.Now.ToString(year + "-0" + month + "-31");
                    break;
                case 2:
                    firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                    if (DateTime.IsLeapYear(DateTime.Now.Year))
                        lastDay = DateTime.Now.ToString(year + "-0" + month + "-29");
                    else
                        lastDay = DateTime.Now.ToString(year + "-0" + month + "-28");
                    break;
                case 3:
                    firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                    lastDay = DateTime.Now.ToString("yyyy-0" + month + "-31");
                    break;
                case 4:
                    firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                    lastDay = DateTime.Now.ToString(year + "-0" + month + "-30");
                    break;
                case 5:
                    firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                    lastDay = DateTime.Now.ToString(year + "-0" + month + "-31");
                    break;
                case 6:
                    firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                    lastDay = DateTime.Now.ToString(year + "-0" + month + "-30");
                    break;
                case 7:
                    firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                    lastDay = DateTime.Now.ToString(year + "-0" + month + "-31");
                    break;
                case 8:
                    firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                    lastDay = DateTime.Now.ToString(year + "-0" + month + "-31");
                    break;
                case 9:
                    firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                    lastDay = DateTime.Now.ToString(year + "-0" + month + "-30");
                    break;
                case 10:
                    firstDay = DateTime.Now.ToString(year + "-" + month + "-01");
                    lastDay = DateTime.Now.ToString(year + "-" + month + "-31");
                    break;
                case 11:
                    firstDay = DateTime.Now.ToString(year + "-" + month + "-01");
                    lastDay = DateTime.Now.ToString(year + "-" + month + "-30");
                    break;
                default:
                    firstDay = DateTime.Now.ToString(year + "-" + month + "-01");
                    lastDay = DateTime.Now.ToString(year + "-" + month + "-31");
                    break;
            }
        }

        #endregion

        #region 获取日期格式 如：2分钟前、3月15日 (周五) 16点19分

        /// <summary>
        /// 中文星期
        /// </summary>
        /// <returns></returns>
        public static string GetDayOfWeekCN(DateTime dt)
        {
            string[] Day = new string[] { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            return Day[Convert.ToInt16(dt.DayOfWeek)];
        }

        /// <summary>
        /// 获取日期格式 如：2分钟前、3月15日 (周五) 16点19分
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public static string GetDateOfWeek(DateTime startDate, DateTime endDate)
        {
            int startDays = Convert.ToInt32(startDate.ToString("yyyyMMdd"));
            int endDays = Convert.ToInt32(endDate.ToString("yyyyMMdd"));

            if (startDays == endDays)
            {
                int Hours = GetHours(startDate, endDate);
                if (Hours > 0 && Hours <= 3)
                {
                    return Hours + "小时前";
                }
                else if (Hours > 3 && Hours < 24)
                {
                    return FormatDate(startDate, "HH点mm分");
                }
                int Minutes = GetMinutes(startDate, endDate);
                if (Minutes > 0 && Minutes < 60)
                {
                    return Minutes + "分钟前";
                }
                int Seconds = GetSeconds(startDate, endDate);
                if (Seconds > 0 && Seconds < 60)
                {
                    return Seconds + "秒前";
                }
            }
            else if (startDays == endDays - 1)
            {
                return "昨天" + FormatDate(startDate, "HH点mm分");
            }
            string returnDateStr = string.Format("{0}({1}){2}", FormatDate(startDate, 4), GetDayOfWeekCN(startDate), FormatDate(startDate, "HH点mm分"));
            return returnDateStr;
        }

        #endregion

        #region MyRegion

        //dt.ToString();//2005-11-5 13:21:25
        //dt.ToFileTime().ToString();//127756416859912816
        //dt.ToFileTimeUtc().ToString();//127756704859912816
        //dt.ToLocalTime().ToString();//2005-11-5 21:21:25
        //dt.ToLongDateString().ToString();//2005年11月5日
        //dt.ToLongTimeString().ToString();//13:21:25
        //dt.ToOADate().ToString();//38661.5565508218
        //dt.ToShortDateString().ToString();//2005-11-5
        //dt.ToShortTimeString().ToString();//13:21
        //dt.ToUniversalTime().ToString();//2005-11-5 5:21:25
        //dt.Year.ToString();//2005
        //dt.Date.ToString();//2005-11-5 0:00:00
        //dt.DayOfWeek.ToString();//Saturday
        //dt.DayOfYear.ToString();//309
        //dt.Hour.ToString();//13
        //dt.Millisecond.ToString();//441
        //dt.Minute.ToString();//30
        //dt.Month.ToString();//11
        //dt.Second.ToString();//28
        //dt.Ticks.ToString();//632667942284412864
        //dt.TimeOfDay.ToString();//13:30:28.4412864
        //dt.ToString();//2005-11-5 13:47:04
        //dt.AddYears(1).ToString();//2006-11-5 13:47:04
        //dt.AddDays(1.1).ToString();//2005-11-6 16:11:04
        //dt.AddHours(1.1).ToString();//2005-11-5 14:53:04
        //dt.AddMilliseconds(1.1).ToString();//2005-11-5 13:47:04
        //dt.AddMonths(1).ToString();//2005-12-5 13:47:04
        //dt.AddSeconds(1.1).ToString();//2005-11-5 13:47:05
        //dt.AddMinutes(1.1).ToString();//2005-11-5 13:48:10
        //dt.AddTicks(1000).ToString();//2005-11-5 13:47:04
        //dt.CompareTo(dt).ToString();//0
        //dt.Add(?).ToString();//问号为一个时间段
        //dt.Equals("2005-11-6 16:11:04").ToString();//False
        //dt.Equals(dt).ToString();//True
        //dt.GetHashCode().ToString();//1474088234
        //dt.GetType().ToString();//System.DateTime
        //dt.GetTypeCode().ToString();//DateTime

        //dt.GetDateTimeFormats('s')[0].ToString();//2005-11-05T14:06:25
        //dt.GetDateTimeFormats('t')[0].ToString();//14:06
        //dt.GetDateTimeFormats('y')[0].ToString();//2005年11月
        //dt.GetDateTimeFormats('D')[0].ToString();//2005年11月5日
        //dt.GetDateTimeFormats('D')[1].ToString();//2005 11 05
        //dt.GetDateTimeFormats('D')[2].ToString();//星期六 2005 11 05
        //dt.GetDateTimeFormats('D')[3].ToString();//星期六 2005年11月5日
        //dt.GetDateTimeFormats('M')[0].ToString();//11月5日
        //dt.GetDateTimeFormats('f')[0].ToString();//2005年11月5日 14:06
        //dt.GetDateTimeFormats('g')[0].ToString();//2005-11-5 14:06
        //dt.GetDateTimeFormats('r')[0].ToString();//Sat, 05 Nov 2005 14:06:25 GMT

        //string.Format("{0:d}",dt);//2005-11-5
        //string.Format("{0:D}",dt);//2005年11月5日
        //string.Format("{0:f}",dt);//2005年11月5日 14:23
        //string.Format("{0:F}",dt);//2005年11月5日 14:23:23
        //string.Format("{0:g}",dt);//2005-11-5 14:23
        //string.Format("{0:G}",dt);//2005-11-5 14:23:23
        //string.Format("{0:M}",dt);//11月5日
        //string.Format("{0:R}",dt);//Sat, 05 Nov 2005 14:23:23 GMT
        //string.Format("{0:s}",dt);//2005-11-05T14:23:23
        //string.Format("{0:t}",dt);//14:23
        //string.Format("{0:T}",dt);//14:23:23
        //string.Format("{0:u}",dt);//2005-11-05 14:23:23Z
        //string.Format("{0:U}",dt);//2005年11月5日 6:23:23
        //string.Format("{0:Y}",dt);//2005年11月
        //string.Format("{0}",dt);//2005-11-5 14:23:23
        //string.Format("{0:yyyyMMddHHmmssffff}",dt); 

        #endregion


    }
}
