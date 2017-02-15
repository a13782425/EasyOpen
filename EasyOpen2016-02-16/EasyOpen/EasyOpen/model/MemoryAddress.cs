using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace EasyOpen.model
{
    public static class MemoryAddress
    {
        /// <summary>
        /// 自左向右显示窗口
        /// </summary>
        public const Int32 AW_HOR_POSITIVE = 0x00000001;

        /// <summary>
        /// 自右向左显示窗口
        /// </summary>
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;

        /// <summary>
        /// 自顶向下显示窗口
        /// </summary>
        public const Int32 AW_VER_POSITIVE = 0x00000004;

        /// <summary>
        /// 自下向上显示窗口
        /// </summary>
        public const Int32 AW_VER_NEGATIVE = 0x00000008;

        /// <summary>
        /// 与 AW_HIDE 效果配合使用则效果为窗口几内重叠,  单独使用窗口向外扩展.
        /// </summary>
        public const Int32 AW_CENTER = 0x00000010;

        /// <summary>
        /// 隐藏窗口
        /// </summary>
        public const Int32 AW_HIDE = 0x00010000;

        /// <summary>
        /// 激活窗口, 在使用了 AW_HIDE 效果时不可使用此效果
        /// </summary>
        public const Int32 AW_ACTIVATE = 0x00020000;

        /// <summary>
        /// 使用滑动类型, 默认为该类型. 当使用 AW_CENTER 效果时, 此效果被忽略
        /// </summary>
        public const Int32 AW_SLIDE = 0x00040000;

        /// <summary>
        /// 使用淡入效果
        /// </summary>
        public const Int32 AW_BLEND = 0x00080000;

        /// <summary>
        /// 左击非客户区时响应
        /// </summary>
        public const int LL_NCLBUTTONDOWN = 0xA1;

        /// <summary>
        /// 标题栏
        /// </summary>
        public const int HTCAPTION = 0x2;

        /// <summary>
        /// 指定的消息发送到一个或多个窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        /// <summary>
        /// 恢复鼠标输入处理
        /// </summary>
        /// <returns></returns>
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        /// <summary>
        /// 获取桌面窗口的句柄
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// 获取包含指定点的窗口的句柄
        /// </summary>
        /// <param name="Point"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point Point);

        /// <summary>
        /// 获取一个指定子窗口的父窗口句柄
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);


        /// <summary>
        /// 显示与隐藏窗口时能产生特殊的效果
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="dwTime"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImportAttribute("user32.dll")]
        public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
    }
}
