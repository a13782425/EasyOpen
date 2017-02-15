using EasyOpen.model;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace EasyOpen.Utils
{
    public static class FormEffect
    {

        /// <summary>
        /// 拖动
        /// </summary>
        /// <param name="handle"></param>
        public static void drag(IntPtr handle)
        {
            MemoryAddress.ReleaseCapture();
            MemoryAddress.SendMessage(handle, MemoryAddress.LL_NCLBUTTONDOWN, MemoryAddress.HTCAPTION, 0);
        }

        /// <summary>
        /// 动画
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="dwTime"></param>
        /// <param name="dwFlags"></param>
        public static void animate(IntPtr handle, int dwTime, int dwFlags)
        {
            MemoryAddress.AnimateWindow(handle, dwTime, dwFlags);
        }
    }
}
