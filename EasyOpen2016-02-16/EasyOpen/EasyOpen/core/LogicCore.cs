using CSharpWin;
using EasyOpen.Utils;
using Microsoft.International.Converters.PinYinConverter;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace EasyOpen.core
{
    public class LogicCore
    {
        private XMLHelper mXMLHelper;

        public LogicCore()
        {
            this.mXMLHelper = new XMLHelper();
        }

        #region 绑定ListView

        /// <summary>
        /// 绑定LvOneSelf(单个应用)
        /// </summary>
        /// <param name="imgList"></param>
        /// <param name="appName"></param>
        /// <param name="fullPath"></param>
        /// <param name="iconKey"></param>
        /// <param name="icon"></param>
        /// <param name="listView"></param>
        public bool LoadOneLvOneSelf(params object[] par)
        {
            if (par.Length < 0) return false;

            try
            {
                Size size = new System.Drawing.Size(32, 32);
                ImageList imgList = par[0] as ImageList;
                Icon icon = par[3] as Icon;
                ListViewEx listView = par[4] as ListViewEx;
                string name = par[1].ToString();
                imgList.ImageSize = size;
                imgList.Images.Add(par[2].ToString(), icon);
                listView.LargeImageList = imgList;
                listView.Items.Add(name, par[2].ToString());
                listView.View = View.LargeIcon;
                //string str = name + "|" + Global.GetPingYin(name);
                Global.FileNames.Add(Global.GetPingYin(name));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 绑定LvOneSelf(多个应用)
        /// </summary>
        /// <param name="imgList"></param>
        /// <param name="imgList"></param>
        /// <param name="listIcon"></param>
        public bool LoadLvOneSelf(params object[] par)
        {
            if (par.Length < 0) return false;

            int count = 0;
            Size size = new System.Drawing.Size(32, 32);//42
            try
            {
                ImageList imgList = par[0] as ImageList;
                List<string> listPath = par[1] as List<string>;
                List<Icon> listIcon = par[2] as List<Icon>;
                ListViewEx listView = par[3] as ListViewEx;

                imgList.ImageSize = size;

                foreach (var item in listIcon)
                {
                    imgList.Images.Add(count.ToString(), item);
                    count++;
                    easyOpen.mAddCount++;
                }

                count = 0;
                listView.LargeImageList = imgList;

                foreach (var item in listPath)
                {
                    easyOpen.mListOneSelfPath.Add(item);
                    string Name = mXMLHelper.GetXmlAttrValue(item, "ShowName");
                    if (string.IsNullOrEmpty(Name))
                    {
                        string name = item.Substring(item.LastIndexOf('\\') + 1);
                        //string str = name + "|" + Global.GetPingYin(name);
                        Global.FileNames.Add(Global.GetPingYin(name));
                        listView.Items.Add(name, name, count);
                    }
                    else
                    {
                        //string str = Name + "|" + Global.GetPingYin(Name);
                        Global.FileNames.Add(Global.GetPingYin(Name));
                        listView.Items.Add(Name, Name, count);
                    }
                    count++;
                }
                listView.Tag = easyOpen.mListOneSelfPath;
                easyOpen.mListOneSelfApp = listView.Items.ToList();// as List<ListViewItem>;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 绑定LvSystem(多个应用)
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="fullPath"></param>
        /// <param name="iconKey"></param>
        public bool LoadLvSystem(params object[] par)
        {
            if (par.Length < 0) return false;

            int count = 0;
            Size size = new System.Drawing.Size(58, 58);
            try
            {
                ImageList imgList = par[0] as ImageList;
                List<string> listPath = par[1] as List<string>;
                List<string> listIconName = par[2] as List<string>;
                ListViewEx listView = par[3] as ListViewEx;


                imgList.ImageSize = size;

                Assembly asm = Assembly.GetExecutingAssembly();
                ResourceManager rm = new ResourceManager("EasyOpen.Properties.Resources", asm);

                foreach (var item in listIconName)
                {
                    Image img = rm.GetObject(item) as Image;
                    imgList.Images.Add(count.ToString(), img);
                    count++;
                }

                count = 0;
                listView.LargeImageList = imgList;

                foreach (var item in listPath)
                {
                    easyOpen.mListSystemPath.Add(item.Split(',')[0]);
                    listView.Items.Add(item.Split(',')[1], count.ToString());
                    count++;
                }
                listView.Tag = easyOpen.mListSystemPath;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region 打开应用

        /// <summary>
        /// 打开应用
        /// </summary>
        /// <param name="listView"></param>
        public bool OpenApp(params object[] par)
        {
            if (par.Length < 0) return false;
            try
            {
                ListViewEx listView = par[0] as ListViewEx;

                int index = 0;

                if (listView.Tag != null && (listView.Tag as List<string>).Count != 0)
                {
                    foreach (string item in listView.Tag as List<string>)
                    {
                        if (!index.ToString().Equals(listView.SelectedIndices[0].ToString()))
                        {
                            index++;
                            continue;
                        }
                        else
                        {
                            CheckHWND(item);
                            List<OpenFileInfo> list = Global.FileInfo.FindAll(a => a.Name == item);
                            //MessageBox.Show(list.Count.ToString());
                            if (list.Count > 1)
                            {
                                app ap = new app();
                                ap._fileInfo = list;
                                ap.ShowDialog();
                            }
                            else
                            {
                                OpenSingleApp(item);
                            }

                            break;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 大开文件
        /// </summary>
        /// <param name="item"></param>
        private void OpenSingleApp(string item)
        {

            //判断是从自定义界面打开还是其他界面
            if (Global.IsOneSelf && !Global.IsNew && !Global.IsOpenFolder)
            {
                //如果是自定义则看是可以直接置顶
                int i = FileIsOpen.FileIsOpens(item);
                if (i == 0)
                {
                    Thread th = new Thread(() =>
                    {
                        object obj = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("Policies").OpenSubKey("System").GetValue("EnableLUA");
                        if (obj != null)
                        {
                            if ((int)obj == 0)
                            {
                                OpenProgram(item);
                            }
                            else
                            {  //开启了UAC
                                OpenProgram(item, false);
                            }
                        }
                        else
                        {
                            OpenProgram(item);
                        }
                    });
                    th.IsBackground = true;
                    th.Start();
                }
                string str = mXMLHelper.GetXmlAttrValue(item, "Count");
                str = (int.Parse(str) + 1).ToString();
                mXMLHelper.UpdateNode(item, "Count", str);
                mXMLHelper.UpdateNode(item, "End", DateTime.Now.ToShortDateString());
            }
            else if (Global.IsOneSelf && Global.IsNew && !Global.IsOpenFolder)
            {
                Thread th = new Thread(() =>
                {
                    Process pro = Process.Start(item);
                    pro.WaitForInputIdle();
                    OpenFileInfo op = new OpenFileInfo();
                    op.ID = pro.Id;
                    op.Name = item;
                    op.ProcessName = pro.ProcessName;
                    op.HWND = pro.MainWindowHandle;
                    op.Time = DateTime.Now.ToString();
                    Global.FileInfo.Add(op);
                });
                th.IsBackground = true;
                th.Start();
            }
            else if (Global.IsOneSelf && Global.IsOpenFolder)
            {
                string filename = System.IO.Path.GetDirectoryName(item);
                //MessageBox.Show(filename);
                Process.Start("explorer.exe", filename);
                Global.IsOpenFolder = false;
            }
            else
            {
                string cs = "";
                if (item.ToLower() == "shutdown ")
                {
                    cs = " -s -t 120";
                }
                Process.Start(item, cs);
            }


        }

        /// <summary>
        /// 打开程序
        /// </summary>
        /// <param name="item">程序地址</param>
        private static void OpenProgram(string path, bool isWait = true)
        {
            Process pro = Process.Start(path);
            if (isWait)
            {
                pro.WaitForInputIdle();
            }
            OpenFileInfo op = new OpenFileInfo();
            op.ID = pro.Id;
            op.Name = path;
            op.ProcessName = pro.ProcessName;
            op.HWND = pro.MainWindowHandle;
            op.Time = DateTime.Now.ToString();
            Global.FileInfo.Add(op);
        }

        /// <summary>
        /// 检测句柄
        /// </summary>
        private static void CheckHWND(string item)
        {
            List<OpenFileInfo> list = new List<OpenFileInfo>();
            //Process main = null;
            foreach (OpenFileInfo file in Global.FileInfo)
            {
                Process pro = null;
                try
                {
                    if (file.Name == item)
                    {
                        pro = Process.GetProcessById(file.ID);
                        if ((int)pro.MainWindowHandle == 0)
                        {
                            list.Add(file); continue;
                        }
                        if ((int)file.HWND == 0 || file.HWND != pro.MainWindowHandle)
                        {
                            if ((int)pro.MainWindowHandle != 0)
                            {

                                file.HWND = pro.MainWindowHandle;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    list.Add(file);
                }
            }
            foreach (OpenFileInfo file in list)
            {
                Global.FileInfo.Remove(file);
            }
        }

        #endregion

        #region 修改名字
        public bool EditName(params object[] par)
        {
            if (par.Length < 0) return false;
            try
            {
                ListViewEx listView = par[0] as ListViewEx;
                string name = par[1] as string; int index = 0;

                if (listView.Tag != null && (listView.Tag as List<string>).Count != 0)
                {
                    foreach (var item in listView.Tag as List<string>)
                    {
                        if (!index.ToString().Equals(listView.SelectedIndices[0].ToString()))
                        {
                            index++;
                            continue;
                        }
                        else
                        {
                            mXMLHelper.UpdateNode(item, "ShowName", name);
                            break;
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 删除应用
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public bool RomoveApp(params object[] par)
        {
            if (par.Length < 0) return false;

            int count = 0;
            string path = null;

            ListViewEx listView = par[0] as ListViewEx;
            List<string> listTag = listView.Tag as List<string>;

            if (listView.Tag != null && listTag.Count != 0)
            {
                foreach (var item in listTag)
                {
                    if (!count.ToString().Equals(listView.SelectedIndices[0].ToString()))
                    {
                        count++;
                        continue;
                    }
                    else
                    {
                        path = item;
                        break;
                    }
                }
            }

            if (mXMLHelper.RemoveXML(path, listView.Name))
            {
                listView.Items.RemoveAt(count);
                listTag.RemoveAt(count);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 自动清理
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public bool ClearApp(params object[] par)
        {
            if (par.Length < 0) return false;
            string str = par[0].ToString();
            if (mXMLHelper.RemoveXML(str, "lvOneSelf"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
