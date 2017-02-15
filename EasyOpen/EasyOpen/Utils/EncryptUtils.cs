using System;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
//
//                  _ooOoo_
//                 o8888888o
//                 88' . '88
//                 (| -_- |)
//                 O\  =  /O
//              ____/`---'\____
//            .'  \\|     |//  `.
//          /  \\|||  :  |||//  \
//          /  _||||| -:- |||||-  \
//          |   | \\\  -  /// |   |
//          | \_|  ''\---/''  |   |
//          \  .-\__  `-`  ___/-. /
//        ___`. .'  /--.--\  `. . __
//     .'' '<  `.___\_<|>_/___.'  >'''.
//    | | :  `- \`.;`\ _ /`;.`/ - ` : | |
//    \  \ `-.   \_ __\ /__ _/   .-` /  /
//=====`-.____`-.___\_____/___.-`____.-'======
//                  `=---='
//
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
//          佛祖保佑       永无Bug
//          快加工资       不改需求
//

namespace EasyOpen.Utils
{
    public class EncryptUtils
    {
        private static EncryptUtils _instance = null;
        private EncryptUtils()
        {

        }

        public static EncryptUtils Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EncryptUtils();
                }
                return _instance;
            }
        }

        #region 256加密
        //private Random ran = new Random();
        char[] dicChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };


        //加密和解密采用相同的key,可以任意数字，但是必须为32位
        //private string strkeyValue = '12345678901234567890198915689039';

        private static string strkeyValue = "abcdefghijklmnopqrstuvwxyzzyxwvu";

        private static string _encryptKey = null;
        private static string EncryptKey
        {
            get
            {
                if (string.IsNullOrEmpty(_encryptKey))
                {
                    _encryptKey = GetMoAddress();

                    while (_encryptKey.Length < strkeyValue.Length)
                    {
                        _encryptKey += _encryptKey;
                    }
                    _encryptKey = _encryptKey.Substring(0, strkeyValue.Length);
                }
                return _encryptKey;
            }

        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="inputText">需要加密的字段</param>
        /// <returns></returns>
        public string Encryption(string inputText)
        {
            return encryptionContent(inputText, EncryptKey);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="inputText">需要解密的字段</param>
        /// <returns></returns>
        public string Decipher(string inputText)
        {
            return decipheringContent(inputText, EncryptKey);
        }

        /// <summary>
        /// 内容加密
        /// </summary>
        /// <param name='ContentInfo'>要加密内容</param>
        /// <param name='strkey'>key值</param>
        /// <returns></returns>
        private string encryptionContent(string ContentInfo, string strkey)
        {

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(strkey);
            byte[] resultArray;
            using (RijndaelManaged encryption = new RijndaelManaged())
            {
                encryption.Key = keyArray;

                //count = encryption.KeySize;

                encryption.Mode = CipherMode.ECB;

                encryption.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = encryption.CreateEncryptor();

                byte[] _EncryptArray = UTF8Encoding.UTF8.GetBytes(ContentInfo);

                resultArray = cTransform.TransformFinalBlock(_EncryptArray, 0, _EncryptArray.Length);
            }

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);

        }

        /// <summary>
        /// 内容解密
        /// </summary>
        /// <param name='encryptionContent'>被加密内容</param>
        /// <param name='strkey'>key值</param>
        /// <returns></returns>
        private string decipheringContent(string encryptionContent, string strkey)
        {

            try
            {
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(strkey);
                byte[] resultArray;
                using (RijndaelManaged decipher = new RijndaelManaged())
                {
                    decipher.Key = keyArray;

                    //count = decipher.KeySize;

                    decipher.Mode = CipherMode.ECB;

                    decipher.Padding = PaddingMode.PKCS7;

                    ICryptoTransform cTransform = decipher.CreateDecryptor();

                    byte[] _EncryptArray = Convert.FromBase64String(encryptionContent);//UTF8Encoding.UTF8.GetBytes(encryptionContent);// 

                    resultArray = cTransform.TransformFinalBlock(_EncryptArray, 0, _EncryptArray.Length);
                }

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region MD5加密
        /// <summary>
        /// MD5加密,和动网上的16/32位MD5加密结果相同
        /// </summary>
        /// <param name="strSource">待加密字串</param>
        /// <param name="encryptType">16或32值之一,其它则采用.net默认MD5加密算法</param>
        /// <returns>加密后的字串</returns>
        public string MD5Encrypt(string strSource, MD5EncryptEnum encryptEnum = MD5EncryptEnum.Default)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(strSource);
            byte[] hashValue =
                ((System.Security.Cryptography.HashAlgorithm)
                    System.Security.Cryptography.CryptoConfig.CreateFromName("MD5")).ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            switch (encryptEnum)
            {
                case MD5EncryptEnum.Short:
                    for (int i = 4; i < 12; i++)
                        sb.Append(hashValue[i].ToString("x2"));
                    break;
                case MD5EncryptEnum.Long:
                    for (int i = 0; i < 16; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
                default:
                    for (int i = 0; i < hashValue.Length; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
            }
            return sb.ToString();
        }
        #endregion

        ///   <summary>    
        ///   获取网卡硬件地址    
        ///   </summary>    
        ///   <returns> string </returns>  
        //using System.Management;  
        private static string GetMoAddress()
        {
            string MoAddress = " ";
            using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                ManagementObjectCollection moc2 = mc.GetInstances();
                foreach (ManagementObject mo in moc2)
                {
                    if ((bool)mo["IPEnabled"] == true)
                        MoAddress = mo["MacAddress"].ToString();
                    mo.Dispose();
                }
            }
            return MoAddress.ToString();
        }

        ///// <summary>        
        ///// 根据网卡类型来获取mac地址        
        ///// </summary>        
        ///// <param name="networkType">网卡类型</param>        
        ///// <param name="macAddressFormatHanlder">格式化获取到的mac地址</param>        
        ///// <returns>获取到的mac地址</returns>        
        //public static string GetMacAddress(NetworkInterfaceType networkType, Func<string, string> macAddressFormatHanlder)
        //{
        //    string _mac = string.Empty;
        //    NetworkInterface[] _networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        //    foreach (NetworkInterface adapter in _networkInterfaces)
        //    {
        //        if (adapter.NetworkInterfaceType == networkType) { _mac = adapter.GetPhysicalAddress().ToString(); if (!String.IsNullOrEmpty(_mac))                        break; }
        //    } if (macAddressFormatHanlder != null) _mac = macAddressFormatHanlder(_mac); return _mac;
        //}


        ///// <summary>       
        ///// 根据网卡类型以及网卡状态获取mac地址        
        ///// </summary>        
        ///// <param name="networkType">网卡类型</param>        
        ///// <param name="status">网卡状态</param>        
        /////Up 网络接口已运行，可以传输数据包。         
        /////Down 网络接口无法传输数据包。         
        /////Testing 网络接口正在运行测试。         
        /////Unknown 网络接口的状态未知。         
        /////Dormant 网络接口不处于传输数据包的状态；它正等待外部事件。         
        /////NotPresent 由于缺少组件（通常为硬件组件），网络接口无法传输数据包。         
        /////LowerLayerDown 网络接口无法传输数据包，因为它运行在一个或多个其他接口之上，而这些“低层”接口中至少有一个已关闭。         
        ///// <param name="macAddressFormatHanlder">格式化获取到的mac地址</param>        
        ///// <returns>获取到的mac地址</returns>        
        //public static string GetMacAddress(NetworkInterfaceType networkType, OperationalStatus status, Func<string, string> macAddressFormatHanlder)
        //{
        //    string _mac = string.Empty; NetworkInterface[] _networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        //    foreach (NetworkInterface adapter in _networkInterfaces)
        //    {
        //        if (adapter.NetworkInterfaceType == networkType)
        //        {
        //            if (adapter.OperationalStatus != status)
        //                continue;
        //            _mac = adapter.GetPhysicalAddress().ToString();
        //            if (!String.IsNullOrEmpty(_mac))
        //                break;
        //        }
        //    }
        //    if (macAddressFormatHanlder != null)
        //        _mac = macAddressFormatHanlder(_mac);
        //    return _mac;
        //}
        ///// <summary>        
        ///// 获取读到的第一个mac地址        
        ///// </summary>        
        ///// <returns>获取到的mac地址</returns>        
        //public static string GetMacAddress(Func<string, string> macAddressFormatHanlder)
        //{
        //    string _mac = string.Empty;
        //    NetworkInterface[] _networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        //    foreach (NetworkInterface adapter in _networkInterfaces)
        //    {
        //        _mac = adapter.GetPhysicalAddress().ToString();
        //        if (!string.IsNullOrEmpty(_mac))
        //            break;
        //    }
        //    if (macAddressFormatHanlder != null)
        //        _mac = macAddressFormatHanlder(_mac);
        //    return _mac;
        //}



    }

    /// <summary>
    /// MD5加密模式
    /// </summary>
    public enum MD5EncryptEnum
    {
        /// <summary>
        /// 16位
        /// </summary>
        Short,
        /// <summary>
        /// 32位
        /// </summary>
        Long,
        /// <summary>
        /// c#默认
        /// </summary>
        Default
    }

}

