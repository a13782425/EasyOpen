using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EasyOpen.Model
{
    public class OpenFileModel
    {
        public int ID
        {
            get
            {
                if (CurrentProcess == null)
                {
                    return -1;
                }
                else
                {
                    return CurrentProcess.Id;
                }
            }
        }
        public string Name { get; set; }
        public string ProcessName { get; set; }
        public string Path { get; set; }
        public Process CurrentProcess { get; set; }
        public IntPtr HWND
        {
            get
            {
                if (CurrentProcess == null)
                {
                    return IntPtr.Zero;
                }
                else
                {
                    return CurrentProcess.Handle;
                }
            }
        }
        public IntPtr MainHandle
        {
            get
            {
                if (CurrentProcess == null)
                {
                    return IntPtr.Zero;
                }
                else
                {
                    return CurrentProcess.MainWindowHandle;
                }
            }
        }
    }
}
