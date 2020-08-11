using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO.Ports;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Common
{
    public static class Json
    {
        /// <summary>  
        /// List转成json   
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="jsonName"></param>  
        /// <param name="list"></param>  
        /// <returns></returns>  
        public static string ToListJson<T>(this IList<T> list, string jsonName, bool IsGetHeader)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = list[0].GetType().Name;
            if (IsGetHeader)
            {
                Json.Append("{\"" + jsonName + "\":[");
            }
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    Json.Append("{");
                    bool isPass = false;
                    for (int j = 0; j < pi.Length; j++)
                    {
                        if (pi[j].GetValue(list[i], null) != null)
                        {
                            Type type = pi[j].GetValue(list[i], null).GetType();
                            if (type.Name.Equals("List`1"))
                            {
                                string abc = ToListJson((List<SerialData>)pi[j].GetValue(list[i], null), pi[j].Name.ToString(), true);
                                Json.Append(abc);
                            }
                            else
                            {
                                Json.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(pi[j].GetValue(list[i], null).ToString(), type));
                            }
                            if (j < pi.Length - 1)
                            {
                                Json.Append(",");
                            }
                        }
                        else
                        {
                            isPass = true;
                        }
                    }
                    if (isPass) Json.Remove(Json.Length - 1, 1);
                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }

            if (IsGetHeader)
            {
                Json.Append("]}");
            }
            return Json.ToString();
        }
        
        /// <summary>  
        /// List转成json   
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="list"></param>  
        /// <returns></returns>  
        public static string ToListJson<T>(this IList<T> list, bool IsGetHeader)
        {
            object obj = list[0];
            return ToListJson<T>(list, obj.GetType().Name, IsGetHeader);
        }
        
        /// <summary>   
        /// 对象转换为Json字符串   
        /// </summary>   
        /// <param name="jsonObject">对象</param>   
        /// <returns>Json字符串</returns>   
        public static string ToObjectJson(this object jsonObject)
        {
            try
            {
                StringBuilder jsonString = new StringBuilder();
                jsonString.Append("{");
                PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();
                for (int i = 0; i < propertyInfo.Length; i++)
                {
                    object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
                    if (objectValue == null)
                    {
                        continue;
                    }
                    StringBuilder value = new StringBuilder();
                    if (objectValue is DateTime || objectValue is Guid || objectValue is TimeSpan)
                    {
                        value.Append("\"" + objectValue.ToString() + "\"");
                    }
                    else if (objectValue is string)
                    {
                        value.Append("\"" + objectValue.ToString() + "\"");
                    }
                    else if (objectValue is IEnumerable)
                    {
                        value.Append(ToObjListJson((IEnumerable)objectValue));
                    }
                    else
                    {
                        value.Append("\"" + objectValue.ToString() + "\"");
                    }

                    jsonString.Append("\"" + propertyInfo[i].Name + "\":" + value + ","); ;
                }
                return jsonString.ToString().TrimEnd(',') + "}";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>   
        /// 对象集合转换Json   
        /// </summary>   
        /// <param name="array">集合对象</param>   
        /// <returns>Json字符串</returns>   
        public static string ToObjListJson(this IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString += ToObjectJson(item) + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "]";
        }
        
        /// <summary>   
        /// 普通集合转换Json   
        /// </summary>   
        /// <param name="array">集合对象</param>   
        /// <returns>Json字符串</returns>   
        public static string ToArrayJson(this IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString = ToObjectJson(item.ToString()) + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "]";
        }
        
        /// <summary>   
        /// Datatable转换为Json   
        /// </summary>   
        /// <param name="table">Datatable对象</param>   
        /// <returns>Json字符串</returns>   
        public static string ToDataTableJson(this DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append(i == drc.Count - 1 ? "}" : "},");
            }
            jsonString.Append("]");
            return jsonString.ToString();
        }
        
        /// <summary>  
        /// DataTable转成Json   
        /// </summary>  
        /// <param name="jsonName"></param>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string ToDataTableJson(this DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = dt.TableName;
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();

        }
        
        /// <summary>
        /// 列转json
        /// </summary>
        /// <param name="dt">表</param>
        /// <param name="r">列</param>
        public static string ToColumnJson(this DataTable dt, int r)
        {
            StringBuilder strSql = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strSql.Append(dt.Rows[i][r]);
                strSql.Append(",");
            }
            return strSql.ToString().Trim(',');
        }
        
        /// <summary>   
        /// DataReader转换为Json   
        /// </summary>   
        /// <param name="dataReader">DataReader对象</param>   
        /// <returns>Json字符串</returns>   
        public static string ToDataReaderJson(this DbDataReader dataReader)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            while (dataReader.Read())
            {
                jsonString.Append("{");
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Type type = dataReader.GetFieldType(i);
                    string strKey = dataReader.GetName(i);
                    string strValue = dataReader[i].ToString();
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (i < dataReader.FieldCount - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            dataReader.Close();
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            if (jsonString.Length == 1)
            {
                return "[]";
            }
            return jsonString.ToString();
        }
        
        /// <summary>   
        /// DataSet转换为Json   
        /// </summary>   
        /// <param name="dataSet">DataSet对象</param>   
        /// <returns>Json字符串</returns>   
        public static string ToDataSetJson(this DataSet dataSet)
        {
            string jsonString = "{";
            foreach (DataTable table in dataSet.Tables)
            {
                jsonString += "\"" + table.TableName + "\":" + ToObjectJson(table) + ",";
            }
            jsonString = jsonString.TrimEnd(',');
            return jsonString + "}";

        }
        
        /// <summary>  
        /// 过滤特殊字符  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        public static string String2Json(this string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("////"); break;
                    //case '\\':  
                    //    sb.Append("///"); break;  
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
        
        /// <summary>  
        /// 格式化字符型、日期型、布尔型  
        /// </summary>  
        /// <param name="str"></param>  
        /// <param name="type"></param>  
        /// <returns></returns>  
        public static string StringFormat(this string str, Type type)
        {
            if (type != typeof(string) && string.IsNullOrEmpty(str))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str.Split(' ')[0] + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            return str;

        }

        /// <summary>
        /// object 转json
        /// </summary>
        /// <param name="ojb"></param>
        /// <returns></returns>
        public static string ToJsonFromObject(this object ojb)
        {
            return JsonConvert.SerializeObject(ojb);
        }

        public static object ToJson(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject(Json);
        }
        public static string ToJson(this object obj)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }
        public static string ToJson(this object obj, string datetimeformats)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }
        public static T ToObject<T>(this string Json)
        {
            return Json == null ? default(T) : JsonConvert.DeserializeObject<T>(Json);
        }
        public static List<T> ToList<T>(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);
        }
        public static DataTable ToTable(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<DataTable>(Json);
        }
        public static JObject ToJObject(this string Json)
        {
            return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
        }

    }
}