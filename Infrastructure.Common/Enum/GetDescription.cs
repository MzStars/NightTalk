using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Enum
{
    public static class GetEnumDescription
    {
        ///<summary>
        //// 获取描述信息
        ///</summary>
        ///<param name="en"></param>
        ///<returns></returns>
        public static string GetDescription(this System.Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        public static List<EnumItem<int>> GetIntItems(this Type enumType)
        {
            if (!enumType.IsEnum)
                throw new InvalidOperationException();

            List<EnumItem<int>> list = new List<EnumItem<int>>();

            // 获取Description特性 
            Type typeDescription = typeof(DescriptionAttribute);
            // 获取枚举字段
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (!field.FieldType.IsEnum)
                    continue;

                // 获取枚举值
                int value = (int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);

                // 不包括空项
                if (value > 0)
                {
                    string text = string.Empty;
                    object[] array = field.GetCustomAttributes(typeDescription, false);

                    if (array.Length > 0) text = ((DescriptionAttribute)array[0]).Description;
                    else text = field.Name; //没有描述，直接取值

                    //添加到列表
                    list.Add(new EnumItem<int> {Text = text,Value = value });
                }
            }
            return list;
        }

        public static List<EnumItem<string>> GetStringItems(this Type enumType)
        {
            if (!enumType.IsEnum)
                return null;

            List<EnumItem<string>> list = new List<EnumItem<string>>();

            // 获取Description特性 
            Type typeDescription = typeof(DescriptionAttribute);
            // 获取枚举字段
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (!field.FieldType.IsEnum)
                    continue;

                // 获取枚举值
                string value = field.Name;

                // 不包括空项
                if (!string.IsNullOrEmpty(value))
                {
                    string text = string.Empty;
                    object[] array = field.GetCustomAttributes(typeDescription, false);

                    if (array.Length > 0) text = ((DescriptionAttribute)array[0]).Description;
                    else text = field.Name; //没有描述，直接取值

                    //添加到列表
                    list.Add(new EnumItem<string> { Text = text, Value = value });
                }
            }
            return list;
        }

        public static Dictionary<string, int> GetEnums<T>()
        {
            Dictionary<string, int> enums = new Dictionary<string, int>();
            FieldInfo[] userType = typeof(T).GetFields();
            if (userType.Length > 0)
            {
                foreach (FieldInfo fi in userType)
                {
                    if (fi.CustomAttributes.Any())
                    {
                        string enumName = fi.CustomAttributes.First().ConstructorArguments.First().Value.ToString();
                        int enumNum = (int)System.Enum.Parse(typeof(T), fi.Name);
                        enums.Add(enumName, enumNum);
                    }
                }
            }
            return enums;
        }

        public static List<EnumItem<string>> GetItemsString<T>()
        {
            List<EnumItem<string>> enums = new List<EnumItem<string>>();
            FieldInfo[] userType = typeof(T).GetFields();
            if (userType.Length > 0)
            {
                foreach (FieldInfo fi in userType)
                {
                    if (fi.CustomAttributes.Any())
                    {
                        string enumName = fi.CustomAttributes.First().ConstructorArguments.First().Value.ToString();
                        
                        enums.Add(new EnumItem<string>() { Text = enumName, Value = fi.Name});
                    }
                }
            }
            return enums;
        }
    }
}
