using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EasyOpen.model
{
    public class AboutIcon
    {
        public AboutIcon(string appName, string fullPath, Icon icon)
        {
            this.appName = appName;
            this.fullPath = fullPath;
            this.Icon = icon;
        }

        private string appname; //应用程序名称 
        private string fullpath; //图标路径
        private Icon icon; //图标对象

        public string appName
        {
            get { return appname; }
            set { appname = value; }
        }

        public string fullPath
        {
            get { return fullpath; }
            set { fullpath = value; }
        }

        public Icon Icon
        {
            get { return icon; }
            set { icon = value; }
        }
    }
}
