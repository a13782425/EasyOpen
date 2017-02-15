using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.International.Converters.PinYinConverter;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;

namespace EasyOpen.Utils
{
    public class PinYinUtils
    {
        private static PinYinUtils _instance = null;
        private PinYinUtils()
        {

        }

        public static PinYinUtils Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PinYinUtils();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 获取汉子首字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetPingYin(string str)
        {
            string linshi = ChineseConverter.Convert(str, ChineseConversionDirection.TraditionalToSimplified);//繁转简
            string result = string.Empty;
            foreach (char c in str)
            {
                if (ChineseChar.IsValidChar(c))
                {
                    ChineseChar china = new ChineseChar(c);
                    result = result + china.Pinyins[0][0].ToString().ToUpper();
                }
                else
                {
                    result = result + c.ToString().ToUpper();
                }
            }

            return str + "|" + result;
        }

    }
}
