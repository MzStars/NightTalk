using System;
using System.Configuration;
using System.Xml;

namespace Infrastructure.Common
{
    /// <summary>
    /// web.config操作类
    /// </summary>
    public class ConfigHelper
    {
        static string appPath = System.Web.HttpContext.Current.Request.ApplicationPath;

        #region AppSettings操作
        /// <summary>
        /// 根据Key取Value值
        /// </summary>
        /// <param name="key"></param>
        public static string GetAppConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
        }

        /// <summary>
        /// 根据Key修改Value
        /// </summary>
        /// <param name="key">要修改的Key</param>
        /// <param name="value">要修改为的值</param>
        public static void SetValue(string key, string value)
        {
            ConfigurationManager.AppSettings.Set(key, value);
        }

        /// <summary>
        /// 添加新的Key ，Value键值对
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public static void Add(string key, string value)
        {
            ConfigurationManager.AppSettings.Add(key, value);
        }

        /// <summary>
        /// 根据Key删除项
        /// </summary>
        /// <param name="key">Key</param>
        public static void Remove(string key)
        {
            ConfigurationManager.AppSettings.Remove(key);
        }
        #endregion
        
        #region 获取xml文档对象

        /// <summary>
        /// 获取xml文档对象
        /// </summary>
        /// <returns></returns>
        public static XmlDocument CreateXmlDoc()
        {
            string appPath = System.Web.HttpContext.Current.Request.ApplicationPath;
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(System.Web.HttpContext.Current.Server.MapPath(appPath + "/WebSite.Config"));
            }
            catch
            {

            }
            return xmlDoc;
        }

        #endregion

        #region 保存xml文档

        /// <summary>
        /// 保存xml文档
        /// </summary>
        /// <param name="xmlDoc"></param>
        public static void SaveXmlDoc(XmlDocument xmlDoc)
        {
            xmlDoc.Save(System.Web.HttpContext.Current.Server.MapPath(appPath + "/WebSite.Config"));
        }

        #endregion

        #region 获取一个节点

        /// <summary>
        /// 获取一个节点
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public static XmlNode GetNode(string xPath)
        {
            XmlDocument xmlDoc = CreateXmlDoc();
            XmlNode xn = xmlDoc.SelectSingleNode(xPath);
            return xn;
        }

        #endregion

        #region 获取一个节点

        /// <summary>
        /// 获取一个节点
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public static string GetNodeValue(string KeyName)
        {
            XmlDocument xmlDoc = CreateXmlDoc();
            XmlNode xn = xmlDoc.SelectSingleNode("config/" + KeyName + "/value");
            return xn.InnerText;
        }

        #endregion

        #region 获取所有节点

        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public static XmlNodeList GetNodes(string xPath)
        {
            XmlDocument xmlDoc = CreateXmlDoc();
            XmlNodeList xnl = xmlDoc.SelectNodes(xPath);
            return xnl;
        }

        public static XmlNodeList GetChildNodes(string xPath)
        {
            XmlDocument xmlDoc = CreateXmlDoc();
            XmlNodeList xnl = xmlDoc.SelectSingleNode(xPath).ChildNodes;
            return xnl;
        }

        #endregion

        #region 向一个节点写入值

        /// <summary>
        /// 向一个节点写入值
        /// </summary>
        public static void WriteInnerText(string xPath, string innerText)
        {
            XmlDocument xmlDoc = CreateXmlDoc();
            ((XmlElement)xmlDoc.SelectSingleNode(xPath)).InnerText = innerText;
            SaveXmlDoc(xmlDoc);
        }

        #endregion

        /// <summary>
        /// 根据Key取Value值
        /// </summary>
        /// <param name="key"></param>
        public static string AppSettings(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString().Trim();
        }
    }
}