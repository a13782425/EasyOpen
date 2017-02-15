using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyOpen.Model
{
    public class SettingModel
    {
        /// <summary>
        /// 开机自启动
        /// </summary>
        public bool AutoOpen = false;

        /// <summary>
        /// 直接关闭
        /// </summary>
        public bool Close = false;

        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop = false;

        /// <summary>
        /// 主题
        /// </summary>
        public string Theme = EasyOpen.Utils.DataUtils.BaseLight;
    }
}
