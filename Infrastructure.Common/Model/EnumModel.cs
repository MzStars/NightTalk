using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    /// <summary>
    /// 枚举Item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumItem<T>
    {
        public string Text { get; set; }

        public T Value { get; set; }
    }
}
