using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public static class OrderNumber
    {
        /// <summary>
        /// 获取16位随机数
        /// </summary>
        /// <returns></returns>
        public static string GetOrderNumber()
        {
            var datetime = DateTime.Now;
            var day = datetime.Day.ToString();
            day = day.Substring(day.Length - 1, 1);
            var hours = datetime.Hour.ToString().ToArray();
            var hour = hours.Sum(item => int.Parse(item.ToString())).ToString();
            hour = hour.Substring(hour.Length - 1, 1);
            Random r = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            var rand = r.Next(1000,9999);
            var num = day + hour + datetime.ToString("mmssffff") + rand + r.Next(10, 99).ToString();
            return num;
        }

        /// <summary>
        /// 获取11位随机数
        /// </summary>
        /// <returns></returns>
        public static string GetNumber()
        {
            var datetime = DateTime.Now;
            var day = datetime.Day.ToString();
            day = day.Substring(day.Length - 1, 1);
            var hours = datetime.Hour.ToString().ToArray();
            var hour = hours.Sum(item => int.Parse(item.ToString())).ToString();
            hour = hour.Substring(hour.Length - 1, 1);
            Random r = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            var rand = r.Next(1000, 9999);
            var num = day + hour + datetime.ToString("MMddhhmmssffff");
            return num;
        }

        /// <summary>
        /// 获取8位随机数
        /// </summary>
        /// <returns></returns>
        public static string GetPapersNo()
        {
            var datetime = DateTime.Now;
            var day = datetime.Day.ToString();
            day = day.Substring(day.Length - 1, 1);
            Random r = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            var rand = r.Next(1000, 9999);
            var num = day  + datetime.ToString("MMddhhmmssff");
            return num;
        }

        public static string GetOrderNumberNotEqual(string num)
        {
            var number = GetOrderNumber();
            while (number == num)
            {
                number = GetOrderNumber();
            }

            return number;
        }
    }
}
