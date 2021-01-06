using EasyOpen.Utils;
using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model = EasyOpen.Model;

namespace EasyOpen.Control
{
    /// <summary>
    /// ListViewItem.xaml 的交互逻辑
    /// </summary>
    public partial class EasyOpenViewItem : UserControl
    {
        public EasyOpenViewItem()
        {
            InitializeComponent();
            //this.Background = Brushes.Red;
        }

        public string ShowName
        {
            get
            {
                if (StaticData != null)
                {
                    return StaticData.ShowName;
                }
                else
                {
                    return SystemStaticData.ShowName;
                }

            }
        }

        public bool IsSystem
        {
            get
            {
                return StaticData == null;
            }
        }


        private Model.FileModel _staticData = null;
        public Model.FileModel StaticData
        {
            get { return _staticData; }
            set
            {
                _staticData = value;
                this.SetAppName(_staticData.ShowName);
                this.SetImage(User32.WinImage.GetIcon(_staticData.FullPath));
            }
        }

        private Model.SystemModel _systemStaticData = null;
        public Model.SystemModel SystemStaticData
        {
            get { return _systemStaticData; }
            set
            {
                _systemStaticData = value;
                this.SetAppName(_systemStaticData.ShowName);
                MemoryStream ms = new MemoryStream();
                DataUtils.GetBitmap(_systemStaticData.IconName).Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                this.SetImage(ms);
                //ms.Close();
                //ms.Dispose();
            }
        }

        private bool _isSelect = false;
        public bool IsSelect { get { return _isSelect; } }
        private void SetImage(ImageSource image)
        {
            //this.HeadImage.Height = 32;
            //this.HeadImage.Width = 32;
            this.HeadImage.ImageSource = image;
        }
        private void SetImage(string uri)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(uri);
            bitmapImage.EndInit();
            SetImage(bitmapImage);
        }
        private void SetImage(Stream stream)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            SetImage(bitmapImage);
        }

        private void SetAppName(string name)
        {

            string appName = string.Empty;
            int count = 0;
            int num = 0;
            while (count < 6)
            {
                if (num >= name.Length)
                {
                    break;
                }
                if (ChineseChar.IsValidChar(name[num]))
                {
                    count += 2;
                }
                else
                {
                    count += 1;
                }
                num++;
            }
            if (count > 6)
            {
                appName = name.Substring(0, num) + "...";
            }
            else
            {
                appName = name;
            }
            this.TileContainer.Title = appName;
            this.GridToolTip.Text = System.IO.Path.GetFileName(this.ShowName);
        }

        private void MyListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            _isSelect = true;
        }

        private void MyListViewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            _isSelect = false;
        }

        public void SetTileDoubleClick(MouseButtonEventHandler doubleClick)
        {
            this.TileContainer.MouseDoubleClick += doubleClick;
        }
    }
}
