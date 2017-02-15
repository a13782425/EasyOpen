using Microsoft.International.Converters.PinYinConverter;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EasyOpen.Utils
{
    public class Global
    {
        /* xml文件路径 */
        public static readonly string mXMLFilePath = System.AppDomain.CurrentDomain.BaseDirectory + @"apps.xml";
        /*是否是在自定义页面*/
        public static bool IsOneSelf = false;
        /*是否打开新的*/
        public static bool IsNew = false;
        public static bool IsOpenFolder = false;
        public static List<OpenFileInfo> FileInfo = new List<OpenFileInfo>();

        public static List<SelectItem> FileNames = new List<SelectItem>();
        //public static List<string> FileValue = new List<string>();
        /// <summary>
        /// 获取汉子首字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static SelectItem GetPingYin(string str)
        {
            string linshi = ChineseConverter.Convert(str, ChineseConversionDirection.TraditionalToSimplified);//繁转简
            string result = string.Empty;
            foreach (char c in str)
            {
                if (ChineseChar.IsValidChar(c))
                {
                    ChineseChar china = new ChineseChar(c);
                    result = result + china.Pinyins[0][0].ToString().ToUpper();
                }
                else
                {
                    result = result + c.ToString().ToUpper();
                }
            }

            return new SelectItem { Name = str, Value = str + "|" + result };
        }

        public static List<SelectItem> GetApp()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<SelectItem> list = new List<SelectItem>();
            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall 此键的子健为本机所有注册过的软件的卸载程序,通过此思路进行遍历安装的软件
            RegistryKey key32 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            RegistryKey key64 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            RegistryKey key = null;
            List<string> keys = new List<string>();
            if (key64 != null)
            {
                key = key64;
                keys = key64.GetSubKeyNames().ToList();//返回此键所有的子键名称;
            }
            else
            {
                key = key32;
                keys = key32.GetSubKeyNames().ToList();//返回此键所有的子键名称;
            }


            //List<string> key2 = key1.ToList<string>();//因为有的项木有"DisplayName"或"DisplayName"的键值的时候要把键值所在数组中的的元素进行删除
            RegistryKey subkey = null;
            for (int i = 0; i < keys.Count; i++)
            {
                subkey = key.OpenSubKey(keys[i]);//通过list泛型数组进行遍历,某款软件项下的子键
                if (subkey.GetValue("DisplayName") != null)
                {
                    if (subkey.GetValue("DisplayIcon") != null)
                    {
                        string path = subkey.GetValue("DisplayIcon").ToString();
                        path = path.TrimStart('"');
                        path = path.TrimEnd('"');
                        string SubPath = path.Substring(path.Length - 1, 1);//截取子键值的最后一位进行判断
                        if (SubPath == "o" || path.IndexOf("exe") == -1)//如果为o 就是ico 或 找不到exe的 表示为图标文件或只有个标识而没有地址的
                        {
                            continue;
                        }
                        if (SubPath == "0" || SubPath == "1")//因为根据观察 取的是DisplayIcon的值 表示为图片所在路径 如果为0或1,则是为可执行文件的图标 
                        {
                            path = path.Substring(0, path.LastIndexOf("e") + 1);
                        }
                        try
                        {
                            if (Path.GetFileNameWithoutExtension(path).ToLower() == "uninstall")
                            {
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                            //throw;
                        }
                        if (!Path.IsPathRooted(path) || Path.GetPathRoot(path).ToLower() == "c:\\")
                        {
                            continue;
                        }
                        list.Add(new SelectItem() { Name = subkey.GetValue("DisplayName").ToString(), Value = path });
                        continue;

                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return list;
        }
    }
    public class OpenFileInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ProcessName { get; set; }
        public string Time { get; set; }
        public IntPtr HWND { get; set; }
    }

    public class SelectItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
