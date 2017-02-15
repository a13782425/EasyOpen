using EasyOpen.Model;
using EasyOpen.User32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace EasyOpen.Utils
{
    public class ProcessUtils
    {
        public static object LOCK_OBJ = new object();

        private static ProcessUtils _instance = null;


        private ProcessUtils()
        {

        }

        public static ProcessUtils Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProcessUtils();
                }
                return _instance;
            }
        }

        public void OpenSystemInstruct(string instruct, bool otherInstruct)
        {

        }


        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="isOpenNew">是否打开新的</param>
        public void OpenFile(string path, bool isOpenNew = false)
        {
            lock (LOCK_OBJ)
            {

                if (!isOpenNew)
                {
                    CheckHWND(path);
                    int prt = FileIsOpens(path);
                    if (prt == 0)
                    {
                        goto End;
                    }
                    return;
                }

            End: OpenFileModel openFileModel = new OpenFileModel();
                openFileModel.Path = path;
                try
                {
                    openFileModel.CurrentProcess = Process.Start(path);
                    if (openFileModel.CurrentProcess == null)
                    {
                        return;
                    }
                    openFileModel.CurrentProcess.WaitForInputIdle();
                    DataUtils.OpenFileModelList.Add(openFileModel);
                }
                catch (Exception ex)
                {
              
                    MainWindow.Instance.Dispatcher.Invoke(new MainThreadDelegate(() =>
                    {
                        MahApps.Metro.Controls.Dialogs.DialogManager.ShowModalMessageExternal(MainWindow.Instance, "开启应用失败", "没有应用程序与此操作的指定文件有关联");
                    }));
                    return;
                }
            }
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="isOpenNew">是否打开新的</param>
        public void OpenFileFolder(string path)
        {
            lock (LOCK_OBJ)
            {
                string filename = System.IO.Path.GetDirectoryName(path);
                //MessageBox.Show(filename);
                Process.Start("explorer.exe", filename);
            }
        }


        /// <summary>
        /// 是否有打开的文件
        /// </summary>
        /// <param name="fileFullName">文件全路径</param>
        /// <returns>如果打开则置顶返回1，如果没有打开返回0</returns>
        public int FileIsOpens(string path)
        {
            foreach (OpenFileModel item in DataUtils.OpenFileModelList)
            {
                if (item.Path == path)
                {
                    if (Win32.ShowWindow(item.MainHandle, Win32.SW_SHOWNORMAL))
                    {
                        Win32.SetForegroundWindow(item.MainHandle);
                    }
                    else
                    {
                        return 0;
                    }
                    return 1;
                }
            }
            return 0;

        }

        /// <summary>
        /// 检测句柄
        /// </summary>
        private void CheckHWND(string path)
        {
            List<OpenFileModel> list = new List<OpenFileModel>();
            //Process main = null;
            foreach (OpenFileModel file in DataUtils.OpenFileModelList)
            {
                Process pro = null;
                try
                {
                    if (file.Path == path)
                    {
                        pro = Process.GetProcessById(file.ID);
                        if ((int)pro.MainWindowHandle == 0)
                        {
                            list.Add(file); continue;
                        }
                        //if ((int)file.HWND == 0 || file.HWND != pro.MainWindowHandle)
                        //{
                        //    if ((int)pro.MainWindowHandle != 0)
                        //    {
                        //        //file.CurrentProcess = pro;
                        //    }
                        //}
                    }

                }
                catch (Exception ex)
                {
                    list.Add(file);
                }
            }
            foreach (OpenFileModel file in list)
            {
                DataUtils.OpenFileModelList.Remove(file);
            }
        }
    }
}
