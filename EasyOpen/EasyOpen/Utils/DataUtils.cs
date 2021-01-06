using EasyOpen.Control;
using EasyOpen.Model;
using EasyOpen.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyOpen.Utils
{
    public class DataUtils
    {

        #region Theme
        public const string Amber = "Amber";
        public const string BaseDark = "BaseDark";
        public const string BaseLight = "BaseLight";
        public const string Blue = "Blue";
        public const string Brown = "Brown";
        public const string Cobalt = "Cobalt";
        public const string Crimson = "Crimson";
        public const string Cyan = "Cyan";
        public const string Emerald = "Emerald";
        public const string Green = "Green";
        public const string Indigo = "Indigo";
        public const string Lime = "Lime";
        public const string Magenta = "Magenta";
        public const string Mauve = "Mauve";
        public const string Olive = "Olive";
        public const string Orange = "Orange";
        public const string Pink = "Pink";
        public const string Purple = "Purple";
        public const string Red = "Red";
        public const string Sienna = "Sienna";
        public const string Steel = "Steel";
        public const string Taupe = "Taupe";
        public const string Teal = "Teal";
        public const string Violet = "Violet";
        public const string Yellow = "Yellow";
        #endregion


        private static EasyOpenViewItem _lastSelectItem = null;
        public static EasyOpenViewItem LastSelectItem { get { return _lastSelectItem; } set { _lastSelectItem = value; } }

        private static List<EasyOpenViewItem> _easyOpenItemList = new List<EasyOpenViewItem>();

        public static List<EasyOpenViewItem> EasyOpenItemList
        {
            get { return _easyOpenItemList; }
        }
        private static List<EasyOpenViewItem> _easyOpenSystemItemList = new List<EasyOpenViewItem>();
        public static List<EasyOpenViewItem> EasyOpenSystemItemList
        {
            get { return _easyOpenSystemItemList; }
        }

        private static List<OpenFileModel> _openFileModelList = new List<OpenFileModel>();

        public static List<OpenFileModel> OpenFileModelList
        {
            get
            {
                return _openFileModelList;
            }
        }

        public static Dictionary<string, string> ThemeDic = new Dictionary<string, string>()
        {
            { Amber, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Amber.xaml" },
            { BaseDark, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseDark.xaml" },
            { BaseLight, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml" },
            { Blue, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml" },
            { Brown, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Brown.xaml" },
            { Cobalt, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Cobalt.xaml" },
            { Crimson, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Crimson.xaml" },
            { Cyan, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Cyan.xaml" },
            { Emerald, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Emerald.xaml" },
            { Green, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Green.xaml" },
            { Indigo, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Indigo.xaml" },
            { Lime, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Lime.xaml" },
            { Magenta, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Magenta.xaml" },
            { Mauve, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Mauve.xaml" },
            { Olive, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Olive.xaml" },
            { Orange, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Orange.xaml" },
            { Pink, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Pink.xaml" },
            { Purple, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Purple.xaml" },
            { Red, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Red.xaml" },
            { Sienna, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Sienna.xaml" },
            { Steel, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Steel.xaml" },
            { Taupe, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Taupe.xaml" },
            { Teal, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Teal.xaml" },
            { Violet, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Violet.xaml" },
            { Yellow, "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Yellow.xaml" },
        };

        public static System.Drawing.Bitmap GetBitmap(string str)
        {
            object obj = Resources.ResourceManager.GetObject(str, Resources.Culture);
            return ((System.Drawing.Bitmap)(obj));
        }

    }
}
