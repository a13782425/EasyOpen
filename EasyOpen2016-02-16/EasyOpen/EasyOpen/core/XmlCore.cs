using EasyOpen.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using CSharpWin;
using EasyOpen.model;
using System.Threading;
using EasyOpen.factory;

namespace EasyOpen
{
    public class XmlCore
    {
        /* 自用程序 */
        public static List<string> listOneSelfPath = new List<string>();
        /* 系统功能 */
        public static List<string> listSystemPath = new List<string>();

        public static List<string> listIconName = new List<string>();

        public static List<Icon> listIcon = new List<Icon>();

        private XMLHelper mXMLHelper;

        public XmlCore()
        {
            this.mXMLHelper = new XMLHelper();
        }

        /// <summary>
        /// 初始化系统功能
        /// </summary>
        /// <returns></returns>
        public bool InitSystemXml()
        {
            return initSystemAbility() ? true : false;
        }

        /// <summary>
        /// 获取系统功能Xml
        /// </summary>
        /// <returns></returns>
        public bool GetSystemXml(params object[] par)
        {
            if (par.Length < 0) return false;

            listSystemPath.Clear();
            listIconName.Clear();

            XmlNodeList personNodes = mXMLHelper.GetXmlNodeListByXpath(Global.mXMLFilePath, "//app/system");

            if (personNodes.Count <= 0) return false;

            if (personNodes.Count == int.Parse(par[0].ToString())) return true;

            foreach (var item in personNodes)
            {
                listSystemPath.Add(((XmlElement)item).GetAttribute("dos") + "," + ((XmlElement)item).InnerText);
                listIconName.Add(((XmlElement)item).GetAttribute("iconname"));
            }
            return (listSystemPath.Count > 0 && listSystemPath != null) && (listIconName.Count > 0 && listIconName != null) ? true : false;
        }

        /// <summary>
        /// 开机自启动
        /// </summary>
        /// <returns></returns>
        public bool AutoRun()
        {
            bool autoRun = false;
            XmlElement root = mXMLHelper.GetRootElement(Global.mXMLFilePath, true);
            return autoRun = root.GetAttribute("autorun").ToString().Equals("true") ? true : false;
        }

        /// <summary>
        /// 获取自用程序路径
        /// </summary>
        /// <returns></returns>
        public bool GetFullPath()
        {
            XmlNodeList personNodes = mXMLHelper.GetXmlNodeListByXpath(Global.mXMLFilePath, "//app/apps");

            foreach (var node in personNodes)
            {
                listOneSelfPath.Add(((XmlElement)node).GetAttribute("fullpath"));
            }
            return (listOneSelfPath.Count > 0 && listOneSelfPath != null) ? true : false;
        }

        /// <summary>
        /// 获取EXEICO图标
        /// </summary>
        /// <returns></returns>
        public bool GetEXEIcon(params object[] par)
        {
            if (par.Length > 0)
            {
                listIcon.Clear();
                Icon icon = GetIconCM.GetIconByFileName(par[0] as string, true);

                if (icon != null)
                {
                    listIcon.Add(icon);
                }
            }
            else
            {
                XmlNodeList personNodes = mXMLHelper.GetXmlNodeListByXpath(Global.mXMLFilePath, "//app/apps");

                foreach (var node in personNodes)
                {
                    Icon icon = GetIconCM.GetIconByFileName(((XmlElement)node).GetAttribute("fullpath"), true);
                    if (icon != null)
                    {
                        listIcon.Add(icon);
                    }
                }
            }

            return (listIcon.Count > 0 && listIcon != null) ? true : false;
        }
        /// <summary>
        /// 获取ICO图标
        /// </summary>
        /// <returns></returns>
        public bool GetIcon(params object[] par)
        {
            listIcon.Clear();
            if (par.Length > 0)
            {
                
                Icon icon = GetOtherIcon.GetFileIcon(par[0] as string, GetOtherIcon.IconSize.Large, false);

                if (icon != null)
                {
                    listIcon.Add(icon);
                }
            }
            else
            {
                XmlNodeList personNodes = mXMLHelper.GetXmlNodeListByXpath(Global.mXMLFilePath, "//app/apps");

                foreach (var node in personNodes)
                {
                    Icon icon = GetOtherIcon.GetFileIcon(((XmlElement)node).GetAttribute("fullpath"), GetOtherIcon.IconSize.Large, false);
                    if (icon != null)
                    {
                        listIcon.Add(icon);
                    }
                }
            }

            return (listIcon.Count > 0 && listIcon != null) ? true : false;
        }

        ///// <summary>
        ///// 获取某个节点的值
        ///// </summary>
        ///// <param name="par"></param>
        ///// <returns></returns>
        //public string GetAppValue(params object[] par)
        //{
        //    return mXMLHelper.GetXmlAttrValue((string)par[0], (string)par[1]);
        //}
        ///// <summary>
        ///// 获取某个节点的值
        ///// </summary>
        ///// <param name="par"></param>
        ///// <returns></returns>
        //public string GetRootValue(params object[] par)
        //{
        //    return mXMLHelper.GetXmlAttrValue((string)par[0], (string)par[1]);
        //}
        //public bool SetValue(params object[] par)
        //{
        //    return mXMLHelper.UpdateNode((string)par[0], (string)par[1], (string)par[2], (string)par[3]);
        //}

        /// <summary>
        /// 储存XML
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public bool SaveOneSelfXml(params object[] par)
        {
            Dictionary<string, string> dicOneSelf = par[0] as Dictionary<string, string>;

            if (dicOneSelf == null && dicOneSelf.Count == 0) return false;
            string name = "";
            dicOneSelf.TryGetValue("InnerText", out name);
            dicOneSelf.Add("ShowName", name);
            dicOneSelf.Add("Count", "0");
            dicOneSelf.Add("Start", DateTime.Now.ToShortDateString());
            dicOneSelf.Add("End", DateTime.Now.ToShortDateString());
            return mXMLHelper.InsertNode("app", "apps", dicOneSelf);
        }

        /// <summary>
        /// 是否存在相同APP
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public bool GetSameApp(params object[] par)
        {
            bool result = true;
            XmlNodeList personNodes = mXMLHelper.GetXmlNodeListByXpath(Global.mXMLFilePath, "//app/apps");

            foreach (XmlNode node in personNodes)
            {
                if (node.InnerText.Equals(par[0] as string))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        public bool RemoveXml(string md5)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Global.mXMLFilePath);
            XmlElement rootElem = doc.DocumentElement;
            XmlNodeList personNodes = rootElem.SelectNodes("apps");
            bool isok = false;
            foreach (XmlNode item in personNodes)
            {
                if (((XmlElement)item).GetAttribute("MD5").Equals(md5))
                {
                    rootElem.RemoveChild(item);
                    doc.Save(Global.mXMLFilePath);
                    isok = true;
                    break;
                }
            }
            return isok;
        }


        #region 私有方法

        /// <summary>
        /// 初始化系统功能
        /// </summary>
        /// <param name="dosKey"></param>
        /// <param name="icoNameKey"></param>
        /// <param name="dicSystem"></param>
        private bool initSystemAbility()
        {
            if (!File.Exists(Global.mXMLFilePath))
            {
                if (!mXMLHelper.CreateXmlDocument("app", "UTF-8")) return false;
                if (!mXMLHelper.InsertNode("app", "system", new string[] { "dos", "iconname" }, SystemApp.GetSystemDic())) return false;
                DialogResult res = MessageBox.Show("是否扫描除C盘外所有已安装应用？", "扫描程序", MessageBoxButtons.OKCancel);
                if (res == DialogResult.OK)
                {
                    List<SelectItem> dics = Global.GetApp();
                    foreach (SelectItem item in dics)
                    {
                        Icon icon;
                        string fullPath = item.Value;
                        string appName = item.Name;

                        ExecuteCommon execute = new ExecuteCommon();
                        /* 读取xml */
                        if (!GetSameApp(appName))
                        {
                            MessageBox.Show("已存在" + appName + "的应用程序！");
                            continue;
                        }

                        /* 绑定listview 写入XML */
                        if (!SaveOneSelfXml(new Dictionary<string, string>() { { "fullpath", fullPath }, { "InnerText", appName } }))
                        {
                            MessageBox.Show("添加应用程序失败！");
                            continue;
                        }
                    }
                }

            }
            return true;
        }

        #endregion

    }
}
