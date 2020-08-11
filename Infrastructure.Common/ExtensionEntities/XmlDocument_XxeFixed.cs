using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Infrastructure.Common.ExtensionEntities
{
    /// <summary>
    /// 解决 XXE 漏洞，自动将 XmlResolver 设为 null
    /// </summary>
    public class XmlDocument_XxeFixed : XmlDocument
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public XmlDocument_XxeFixed(XmlResolver xmlResolver = null)
        {
            XmlResolver = null;
        }
    }
}
