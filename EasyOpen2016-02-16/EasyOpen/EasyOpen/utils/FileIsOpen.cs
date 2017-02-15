using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace EasyOpen.Utils
{
    public delegate bool CallBack(int hwnd, int lParam);
    public class FileIsOpen
    {
        /// <summary>
        /// 文件名
        /// </summary>
        private static string FileName = "";
        /// <summary>
        /// 要查找窗体的句柄
        /// </summary>
        private static int FileIntPtr = 0;
        #region win32
        /// <summary>
        /// 激活窗体
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll ")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 激活窗体
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll ")]
        private static extern bool OpenIcon(IntPtr hWnd);

        /// <summary>
        /// 判断窗体是否最小化
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll ")]
        private static extern bool IsIconic(IntPtr hWnd);

        /// <summary>
        /// 发消息控制窗体
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="wMsg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("user32")]
        private static extern int EnumWindows(CallBack x, int y);
        [DllImport("user32")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32")]
        public static extern IntPtr GetForegroundWindow();
        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="hWndInsertAffter"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="wFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int SetWindowPos(IntPtr hwnd, int hWndInsertAffter, int x, int y, int cx, int cy, int wFlags);


        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(
        int hwnd,
        StringBuilder lpString,
        int cch
        );

        #endregion
        /// <summary>
        /// 是否有打开的文件
        /// </summary>
        /// <param name="fileFullName">文件全路径</param>
        /// <returns>如果打开则置顶返回1，如果没有打开返回0</returns>
        public static int FileIsOpens(string fileFullName)
        {
            foreach (OpenFileInfo item in Global.FileInfo)
            {
                if (item.Name == fileFullName)
                {
                    if (ShowWindow(item.HWND, 9))
                    {
                        SetWindowPos(item.HWND, 0, 0, 0, 0, 0, 1 | 2);
                    }


                    //if (IsIconic(item.HWND))
                    //{
                    //    if (OpenIcon(item.HWND))
                    //    {
                    //        SetWindowPos(item.HWND, 0, 0, 0, 0, 0, 1 | 2);
                    //    }
                    //}
                    //else
                    //{
                    //    if (ShowWindowAsync(item.HWND, 9))
                    //    {
                    //        SetWindowPos(item.HWND, 0, 0, 0, 0, 0, 1 | 2);
                    //    }
                    //}




                    //SetForegroundWindow(item.HWND);
                    return 1;
                }
            }
            return 0;
            //if (!File.Exists(fileFullName))
            //{
            //    return -1;
            //}
            //string endname = Path.GetExtension(fileFullName);

            //FileName = Path.GetFileNameWithoutExtension(fileFullName);

            //CallBack myCallBack = new CallBack(GetIntPtr);

            //EnumWindows(myCallBack, 0);
            //if (FileIntPtr != 0)
            //{
            //    SetForegroundWindow((IntPtr)FileIntPtr);
            //    FileIntPtr = 0;
            //    return 1;
            //}
            //return 0;
        }
        private static bool GetIntPtr(int hwnd, int lParam)
        {
            StringBuilder s = new StringBuilder(512);
            int i = GetWindowText(hwnd, s, s.Capacity);
            if (s.ToString().Contains(FileName))
            {
                FileIntPtr = hwnd;
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
