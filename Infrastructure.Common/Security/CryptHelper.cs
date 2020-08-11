using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace Infrastructure.Common
{
    /// <summary>
    /// 字符串加密组件
    /// </summary>
    public class CryptHelper
    {
        /// <summary>
        /// 秘钥
        /// </summary>
        public static readonly string DESKey = "crm_desencrypt_2019";

        #region "定义加密字串变量"
        private SymmetricAlgorithm mCSP;  //声明对称算法变量
        private const string CIV = "Mi9l/+7Zujhy12se6Yjy111A";  //初始化向量
        private const string CKEY = "jkHuIy9D/9i="; //密钥（常量）
        #endregion

        /// <summary>
        /// 实例化
        /// </summary>
        public CryptHelper()
        {
            //定义访问数据加密标准 (DES) 算法的加密服务提供程序 (CSP) 版本的包装对象,
            //此类是SymmetricAlgorithm的派生类
            mCSP = new DESCryptoServiceProvider(); 
        }

        #region 加密

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="Value">需加密的字符串</param>
        /// <returns></returns>
        public string EncryptString(string Value)
        {
            //定义基本的加密转换运算
            ICryptoTransform ct;

            //定义内存流
            MemoryStream ms;

            //定义将内存流链接到加密转换的流
            CryptoStream cs;
            byte[] byt;

            //CreateEncryptor创建(对称数据)加密对象
            //用指定的密钥和初始化向量创建对称数据加密标准
            ct = mCSP.CreateEncryptor(Convert.FromBase64String(CKEY), Convert.FromBase64String(CIV));

            //将Value字符转换为UTF-8编码的字节序列
            byt = Encoding.UTF8.GetBytes(Value);

            //创建内存流
            ms = new MemoryStream();

            //将内存流链接到加密转换的流
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);

            //写入内存流
            cs.Write(byt, 0, byt.Length);

            //将缓冲区中的数据写入内存流，并清除缓冲区
            cs.FlushFinalBlock();

            //释放内存流
            cs.Close();

            //将内存流转写入字节数组并转换为string字符
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text">需加密的字符串</param> 
        /// <param name="sKey">秘钥</param> 
        /// <returns></returns> 
        public static string EncryptString(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
#pragma warning disable CS0618 // '“FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
            des.Key = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
#pragma warning restore CS0618 // '“FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
#pragma warning disable CS0618 // '“FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
            des.IV = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
#pragma warning restore CS0618 // '“FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }
        
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString)
        {
            #region
            try
            {
                //向量
                byte[] Keys = Encoding.UTF8.GetBytes("SuperFun");
                //密钥
                byte[] rgbKey = Encoding.UTF8.GetBytes("YouAndMe");
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
            #endregion
        }


        #endregion


        #region 解密

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="Value">要解密的字符串</param>
        /// <returns>string</returns>
        public string DecryptString(string Value)
        {
            //定义基本的加密转换运算
            ICryptoTransform ct;

            //定义内存流
            MemoryStream ms;

            //定义将数据流链接到加密转换的流
            CryptoStream cs;
            byte[] byt;

            //用指定的密钥和初始化向量创建对称数据解密标准
            ct = mCSP.CreateDecryptor(Convert.FromBase64String(CKEY), Convert.FromBase64String(CIV));

            //将Value(Base 64)字符转换成字节数组
            byt = Convert.FromBase64String(Value);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();

            //将字节数组中的所有字符解码为一个字符串
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text">要解密的字符串</param> 
        /// <param name="sKey">秘钥</param> 
        /// <returns></returns> 
        public static string DecryptString(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
#pragma warning disable CS0618 // '“FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
            des.Key = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
#pragma warning restore CS0618 // '“FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
#pragma warning disable CS0618 // '“FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
            des.IV = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
#pragma warning restore CS0618 // '“FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }


        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString)
        {
            #region
            try
            {
                //向量
                byte[] Keys = Encoding.UTF8.GetBytes("SuperFun");
                //密钥
                byte[] rgbKey = Encoding.UTF8.GetBytes("YouAndMe");
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
            #endregion
        }


        #endregion

    }
}
