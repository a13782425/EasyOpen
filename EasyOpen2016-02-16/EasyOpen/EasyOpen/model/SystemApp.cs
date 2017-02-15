using System;
using System.Collections.Generic;
using System.Text;

namespace EasyOpen.model
{
    public static class SystemApp
    {
        public static Dictionary<string, string> GetSystemDic()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("iexplore", "ie,IE");
            dic.Add("notepad", "jsb,记事本");
            dic.Add("mspaint.exe", "hb,画板");
            dic.Add("calc", "jsq,计算器");
            dic.Add("services.msc", "fw,服务");
            dic.Add("mstsc", "yczm,远程桌面");
            dic.Add("regedit.exe", "zcb,注册表");
            //dic.Add("Shutdown.exe -s -t 120", "gj,关机");
            return dic;
        }
    }
}
