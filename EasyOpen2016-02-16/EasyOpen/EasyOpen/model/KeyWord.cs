using System;
using System.Collections.Generic;
using System.Text;

namespace EasyOpen.factory
{
    public enum Key_LogicCore
    {
        LoadOneLvOneSelf,
        LoadLvOneSelf,
        LoadLvSystem,
        OpenApp,
        RomoveApp,
        ClearApp,
        EditName
    }

    public enum Key_XmlCore
    {
        /// <summary>
        /// 初始化系统功能
        /// </summary>
        InitSystemXml,
        /// <summary>
        /// 获取系统功能Xml
        /// </summary>
        GetSystemXml,
        /// <summary>
        /// 开机自启动
        /// </summary>
        AutoRun,
        /// <summary>
        /// 获取自用程序路径
        /// </summary>
        GetFullPath,
        /// <summary>
        /// 获取ICO图标
        /// </summary>
        GetIcon,
        /// <summary>
        /// 获取exe图标
        /// </summary>
        GetEXEIcon,
        /// <summary>
        /// 储存XML
        /// </summary>
        SaveOneSelfXml,
        /// <summary>
        /// 是否存在相同APP
        /// </summary>
        GetSameApp,
        /// <summary>
        /// 删除
        /// </summary>
        RomoveXml
    }
}
