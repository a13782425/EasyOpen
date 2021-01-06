using EasyOpen.Control;
using EasyOpen.User32;
using EasyOpen.Utils;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Draws = System.Drawing;
using Forms = System.Windows.Forms;
using IO = System.IO;

namespace EasyOpen
{
    public delegate void MainThreadDelegate();

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region 成员变量

        private static MainWindow _instance = null;

        public static MainWindow Instance
        {
            get
            {
                return _instance;
            }
        }

        private const int OFFSET = 2;

        private Forms.NotifyIcon _notifyIcon = null;

        private ContextMenu _iconMenu = null;
        private ContextMenu _panelMenu = null;

        /// <summary>
        /// 钩子
        /// </summary>
        private KeyBoardHook _keyHook = null;

        /// <summary>
        /// 当前所在容器边缘
        /// </summary>
        private Forms.AnchorStyles _anchors;

        /// <summary>
        /// 计时器
        /// </summary>
        private System.Timers.Timer _timer = new System.Timers.Timer();

        /* 窗体是否停靠 */
        private bool IsStop = false;

        private bool IsCanClose = false;

        /// <summary>
        /// 搜索窗口
        /// </summary>
        private SearchWindow _searchWindow = null;

        private ContextMenu IconMenu
        {
            get
            {
                if (_iconMenu == null)
                {
                    _iconMenu = new ContextMenu();
                    MenuItem @openMenu = new MenuItem();
                    @openMenu.Header = "打开";
                    @openMenu.Name = "OpenFile";
                    @openMenu.Click += OnEasyOpenViewItem_OpenClick;
                    _iconMenu.Items.Add(@openMenu);
                    MenuItem @openAgainMenu = new MenuItem();
                    @openAgainMenu.Header = "再次打开";
                    @openAgainMenu.Name = "OpenFileAgain";
                    @openAgainMenu.Click += OnEasyOpenViewItem_AgainClick;
                    _iconMenu.Items.Add(@openAgainMenu);
                    MenuItem @openFolderMenu = new MenuItem();
                    @openFolderMenu.Header = "打开所在文件夹";
                    @openFolderMenu.Name = "OpenFolder";
                    @openFolderMenu.Click += OnEasyOpenViewItem_FolderClick;
                    _iconMenu.Items.Add(@openFolderMenu);
                    MenuItem @editNameMenu = new MenuItem();
                    @editNameMenu.Header = "重命名";
                    @editNameMenu.Name = "EditName";
                    @editNameMenu.Click += OnEasyOpenViewItem_EditNameClick;
                    _iconMenu.Items.Add(@editNameMenu);
                    MenuItem @deleteMenu = new MenuItem();
                    @deleteMenu.Header = "删除";
                    @deleteMenu.Name = "DeleteMenu";
                    @deleteMenu.Click += OnEasyOpenViewItem_DeleteClick;
                    _iconMenu.Items.Add(@deleteMenu);
                    _iconMenu.Width = 150;
                }
                return _iconMenu;
            }
        }

        private ContextMenu PanelMenu
        {
            get
            {
                if (_panelMenu == null)
                {
                    _panelMenu = new ContextMenu();
                    MenuItem @addMenu = new MenuItem();
                    @addMenu.Header = "添加文件";
                    @addMenu.Name = "AddFile";
                    @addMenu.Click += OnEasyOpenViewItem_AddClick;
                    _panelMenu.Items.Add(@addMenu);
                    MenuItem @addFolderMenu = new MenuItem();
                    @addFolderMenu.Header = "添加文件夹";
                    @addFolderMenu.Name = "AddFolder";
                    _panelMenu.Items.Add(@addFolderMenu);
                    _panelMenu.Width = 150;
                }
                return _panelMenu;
            }
        }

        public IntPtr Handle = IntPtr.Zero;
        //{
        //    get
        //    {
        //        lock ("aaa")
        //        {
        //            return new WindowInteropHelper(this).Handle;
        //        }
        //    }
        //}

        #endregion 成员变量

        #region 窗体方法

        public MainWindow()
        {
            InitializeComponent();
            //_searchWindow = new SearchWindow();
            _keyHook = new KeyBoardHook();
            _keyHook.KeyDownEvent += new Forms.KeyEventHandler(OnHook_KeyDown);//钩住键按下
            _keyHook.Start();//安装键盘钩子
            InitData();
            InitNotify();
            _instance = this;
        }

        private void EasyOpenWindows_Loaded(object sender, RoutedEventArgs e)
        {
            Handle = new WindowInteropHelper(this).Handle;
            _timer.Interval = 100;
            _timer.Elapsed += OnTimerEvent;

            HwndSource hwndSource = HwndSource.FromHwnd(this.Handle);
            hwndSource.AddHook(new HwndSourceHook(WndProc));
            //for (int i = 0; i < 100; i++)
            //{
            //    EasyOpenViewItem b = new EasyOpenViewItem();
            //    b.StaticData = ModelUtils.Instance.CurrentFileModelList[0];
            //    DataUtils.EasyOpenItemList.Add(b);
            //    this.EasyOpenItemPanel.Children.Add(b);
            //}
        }

        /// <summary>
        /// 钩子按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHook_KeyDown(object sender, Forms.KeyEventArgs e)
        {
            return;
            //if (e.KeyValue == (int)Forms.Keys.F && (int)Forms.Control.ModifierKeys == (int)Forms.Keys.Alt)
            //{
            //    if (_searchWindow.Visibility != System.Windows.Visibility.Visible)
            //    {
            //        _searchWindow.Show();
            //    }
            //}
            //else if (e.KeyValue == (int)Forms.Keys.Escape)
            //{
            //    if (_searchWindow.Visibility == System.Windows.Visibility.Visible)
            //    {
            //        _searchWindow.Hide();
            //    }
            //}
        }

        #region 拖拽

        private void TitlePanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataUtils.LastSelectItem != null)
            {
                DataUtils.LastSelectItem.Background = Brushes.Transparent;
                DataUtils.LastSelectItem = null;
            }
            if (this.SettingFlyout.IsOpen)
            {
                this.SettingFlyout.IsOpen = false;
            }

            this.DragMove();
        }

        #endregion 拖拽

        #region 最小化和关闭窗口，托盘

        private void EasyOpenWindows_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 取消关闭窗体
            if (!ModelUtils.Instance.CurrentSettingModel.Close && !this.IsCanClose)
            {
                e.Cancel = true;
                this.ShowInTaskbar = false;
                this._notifyIcon.BalloonTipText = "EasyOpen 最小化到托盘";
                this._notifyIcon.ShowBalloonTip(500);
                this.Hide();
            }
            else
            {
                Forms.Application.ExitThread();
            }
        }

        private void EasyOpenWindows_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this._notifyIcon.BalloonTipText = "EasyOpen 最小化到托盘";
                this._notifyIcon.ShowBalloonTip(500);
                this.Hide();
            }
        }

        private void Btn_Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //todo 判断是否自动关闭
            ContentControl control = sender as ContentControl;
            if (control.Name == "Btn_Close")
            {
                if (ModelUtils.Instance.CurrentSettingModel.Close)
                {
                    this.Close();
                    return;
                }
            }
            this.ShowInTaskbar = false;
            this._notifyIcon.BalloonTipText = "EasyOpen 最小化到托盘";
            this._notifyIcon.ShowBalloonTip(500);
            //Win32.AnimateWindow(this.Handle, 100, Win32.AW_SLIDE + Win32.AW_HOR_POSITIVE + Win32.AW_HIDE);
            //DoubleAnimation a = new DoubleAnimation();
            //a.From = 0;
            //a.To = 200;
            //a.Duration = new Duration(TimeSpan.Parse("0:0:3"));
            //this.BeginAnimation(LeftProperty, a);
            this.Hide();
            //this.WindowState = System.Windows.WindowState.Minimized;
            //this.Close();
        }

        /// <summary>
        /// 托盘双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNotifyIconDoubleClick(object sender, Forms.MouseEventArgs e)
        {
            this.IsCanClose = false;
            //todo 判断是否自动关闭
            this.Show();
            this.Activate();
            this.ShowInTaskbar = true;
            this.WindowState = System.Windows.WindowState.Normal;
        }

        private void OnCloseWindows(object sender, EventArgs e)
        {
            this.IsCanClose = true;
            this.Close();
            Forms.Application.Exit();
        }

        #endregion 最小化和关闭窗口，托盘

        #region 右键列表

        /// <summary>
        /// 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool isSelect = false;
            for (int i = 0; i < DataUtils.EasyOpenItemList.Count; i++)
            {
                isSelect = DataUtils.EasyOpenItemList[i].IsSelect;
                if (isSelect)
                {
                    DataUtils.LastSelectItem = DataUtils.EasyOpenItemList[i];
                    DataUtils.LastSelectItem.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE979FF")); ;//FFE979
                    break;
                }
            }
            if (isSelect)
            {
                this.UserPanel.ContextMenu = this.IconMenu;
            }
            else
            {
                this.UserPanel.ContextMenu = this.PanelMenu;
                if (DataUtils.LastSelectItem != null)
                {
                    DataUtils.LastSelectItem.Background = Brushes.Transparent;
                    DataUtils.LastSelectItem = null;
                }
            }
            if (this.SettingFlyout.IsOpen)
            {
                this.SettingFlyout.IsOpen = false;
            }
        }

        #endregion 右键列表

        #region 列表单击高亮

        /// <summary>
        /// 物品单击高亮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEasyOpenViewItem_Click(object sender, MouseButtonEventArgs e)
        {
            EasyOpenViewItem easyOpenViewItem = null;
            for (int i = 0; i < DataUtils.EasyOpenItemList.Count; i++)
            {
                if (DataUtils.EasyOpenItemList[i].IsSelect)
                {
                    easyOpenViewItem = DataUtils.EasyOpenItemList[i];
                    break;
                }
            }
            if (DataUtils.LastSelectItem != null)
            {
                DataUtils.LastSelectItem.Background = Brushes.Transparent;
                DataUtils.LastSelectItem = null;
            }
            if (easyOpenViewItem != null)
            {
                easyOpenViewItem.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE979FF")); ;//FFE979
                DataUtils.LastSelectItem = easyOpenViewItem;
            }
            if (this.SettingFlyout.IsOpen)
            {
                this.SettingFlyout.IsOpen = false;
            }
        }

        /// <summary>
        /// 清除物品单击高亮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EasyOpenItemPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool isSelect = false;
            for (int i = 0; i < DataUtils.EasyOpenItemList.Count; i++)
            {
                isSelect = DataUtils.EasyOpenItemList[i].IsSelect;
                if (isSelect)
                {
                    break;
                }
            }
            if (!isSelect)
            {
                if (DataUtils.LastSelectItem != null)
                {
                    DataUtils.LastSelectItem.Background = Brushes.Transparent;
                    DataUtils.LastSelectItem = null;
                }
            }
            if (this.SettingFlyout.IsOpen)
            {
                this.SettingFlyout.IsOpen = false;
            }
        }

        /// <summary>
        /// 取消Tab按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                return;
            }
        }

        #endregion 列表单击高亮

        #region 重命名

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEasyOpenViewItem_EditNameClick(object sender, RoutedEventArgs e)
        {
            if (DataUtils.LastSelectItem != null)
            {
                string newName = DialogManager.ShowModalInputExternal(this, "修改名字", "输入名字", new MetroDialogSettings()
                {
                    DefaultText = DataUtils.LastSelectItem.StaticData.ShowName,
                    AffirmativeButtonText = "确定",
                    NegativeButtonText = "取消"
                });
                if (string.IsNullOrWhiteSpace(newName))
                {
                    return;
                }
                DataUtils.LastSelectItem.StaticData.ShowName = newName;
                DataUtils.LastSelectItem.StaticData = DataUtils.LastSelectItem.StaticData;
                ModelUtils.Instance.Save();
                DataUtils.LastSelectItem.Background = Brushes.Transparent;
                DataUtils.LastSelectItem = null;
            }
        }

        #endregion 重命名

        #region 打开应用

        /// <summary>
        /// 打开应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEasyOpenViewItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataUtils.LastSelectItem != null)
            {
                this.IsStop = true;
                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    if (DataUtils.LastSelectItem.IsSystem)
                    {
                        ProcessUtils.Instance.OpenSystemInstruct(DataUtils.LastSelectItem.SystemStaticData.Instruct, DataUtils.LastSelectItem.SystemStaticData.OtherInstruct);
                    }
                    else
                    {
                        ProcessUtils.Instance.OpenFile(DataUtils.LastSelectItem.StaticData.FullPath);
                    }

                    DataUtils.LastSelectItem.Dispatcher.Invoke(new MainThreadDelegate(() =>
            {
                DataUtils.LastSelectItem.Background = Brushes.Transparent;
                DataUtils.LastSelectItem = null;
                this.IsStop = false;
            }));
                });
            }
        }

        /// <summary>
        /// 打开应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEasyOpenViewItem_OpenClick(object sender, RoutedEventArgs e)
        {
            if (DataUtils.LastSelectItem != null)
            {
                this.IsStop = true;
                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    ProcessUtils.Instance.OpenFile(DataUtils.LastSelectItem.StaticData.FullPath);
                    DataUtils.LastSelectItem.Dispatcher.Invoke(new MainThreadDelegate(() =>
            {
                DataUtils.LastSelectItem.Background = Brushes.Transparent;
                DataUtils.LastSelectItem = null;
                this.IsStop = false;
            }));
                });
            }
        }

        /// <summary>
        /// 打开应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEasyOpenViewItem_DeleteClick(object sender, RoutedEventArgs e)
        {
            if (DataUtils.LastSelectItem != null)
            {
                if (ModelUtils.Instance.Remove(DataUtils.LastSelectItem.StaticData.FullPath))
                {
                    ModelUtils.Instance.Save();
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("删除失败");
                }
            }
        }

        /// <summary>
        /// 打开应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEasyOpenViewItem_AgainClick(object sender, RoutedEventArgs e)
        {
            if (DataUtils.LastSelectItem != null)
            {
                this.IsStop = true;
                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    ProcessUtils.Instance.OpenFile(DataUtils.LastSelectItem.StaticData.FullPath, true);
                    DataUtils.LastSelectItem.Dispatcher.Invoke(new MainThreadDelegate(() =>
            {
                DataUtils.LastSelectItem.Background = Brushes.Transparent;
                DataUtils.LastSelectItem = null;
                this.IsStop = false;
            }));
                });
            }
        }

        /// <summary>
        /// 打开应用文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEasyOpenViewItem_FolderClick(object sender, RoutedEventArgs e)
        {
            if (DataUtils.LastSelectItem != null)
            {
                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    ProcessUtils.Instance.OpenFileFolder(DataUtils.LastSelectItem.StaticData.FullPath);
                    DataUtils.LastSelectItem.Dispatcher.Invoke(new MainThreadDelegate(() =>
            {
                DataUtils.LastSelectItem.Background = Brushes.Transparent;
                DataUtils.LastSelectItem = null;
            }));
                });
            }
        }

        #endregion 打开应用

        #region 添加应用

        /// <summary>
        /// 添加应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_AddApp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddAppFile();
        }

        /// <summary>
        /// 添加应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEasyOpenViewItem_AddClick(object sender, RoutedEventArgs e)
        {
            AddAppFile();
        }

        /// <summary>
        /// 拖拽文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EasyOpenItemPanel_Drag(object sender, DragEventArgs e)
        {
            string path = string.Empty;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            }
            else
            {
                MessageBox.Show("拖拽物体无法识别");
            }
            string extension = IO.Path.GetExtension(path);
            if (extension.ToLower() == ".lnk")
            {
                path = FileUtils.Instance.GetLnkFile(path);
            }
            if (ModelUtils.Instance.CurrentFileModelList.FirstOrDefault(a => a.FullPath == path) != null)
            {
                return;
            }
            if (FileUtils.Instance.AddFileModel(path))
            {
                ModelUtils.Instance.Save();
                this.RefreshData();
            }
            else
            {
                MessageBox.Show("如果您要添加文件夹请右键！");
            }
        }

        #endregion 添加应用

        /// <summary>
        /// 打开设置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSettingMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataUtils.LastSelectItem != null)
            {
                DataUtils.LastSelectItem.Background = Brushes.Transparent;
                DataUtils.LastSelectItem = null;
            }
            if (!this.SettingFlyout.IsOpen)
            {
                this.SettingFlyout.IsOpen = true;
            }
        }

        #region 系统设置

        /// <summary>
        /// 系统设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnToggleSwitch_Click(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggle = sender as ToggleSwitch;
            if (toggle != null)
            {
                switch (toggle.Name)
                {
                    case "TopToggle":
                        ModelUtils.Instance.CurrentSettingModel.IsTop = toggle.IsChecked == null ? false : toggle.IsChecked.Value;
                        this.Topmost = ModelUtils.Instance.CurrentSettingModel.IsTop;
                        break;

                    case "CloseToggle":
                        ModelUtils.Instance.CurrentSettingModel.Close = toggle.IsChecked == null ? false : toggle.IsChecked.Value;
                        this.Topmost = ModelUtils.Instance.CurrentSettingModel.IsTop;
                        break;

                    case "AutoToggle":
                        ModelUtils.Instance.CurrentSettingModel.AutoOpen = toggle.IsChecked == null ? false : toggle.IsChecked.Value;
                        SetOpenStart();
                        break;

                    default:
                        break;
                }

                ModelUtils.Instance.Save();
            }
        }

        #endregion 系统设置

        #endregion 窗体方法

        #region 私有方法

        #region 初始化数据

        /// <summary>
        /// 初始化托盘
        /// </summary>
        private void InitNotify()
        {
            this._notifyIcon = new Forms.NotifyIcon();

            this._notifyIcon.Text = "TimeSlip!";
            Draws.Bitmap bitmap = EasyOpen.Properties.Resources.DefaultIcon;
            System.IntPtr iconHandle = bitmap.GetHicon();
            this._notifyIcon.Icon = Draws.Icon.FromHandle(iconHandle);
            //if (IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"img\DefaultIcon.png"))
            //{
            //    Draws.Bitmap bitmap = new Draws.Bitmap(System.AppDomain.CurrentDomain.BaseDirectory + @"img\DefaultIcon.png");

            //    System.IntPtr iconHandle = bitmap.GetHicon();
            //    this._notifyIcon.Icon = Draws.Icon.FromHandle(iconHandle);
            //}

            //Draws.Icon ico = new Draws.Icon("/Resources/DefaulyIcon.png");
            this._notifyIcon.Visible = true;
            _notifyIcon.MouseDoubleClick += OnNotifyIconDoubleClick;
            Forms.MenuItem menuItem = new Forms.MenuItem();
            menuItem.Text = "关闭";
            menuItem.Click += OnCloseWindows;
            this._notifyIcon.ContextMenu = new Forms.ContextMenu(new Forms.MenuItem[] { menuItem });
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            this.TopToggle.IsChecked = ModelUtils.Instance.CurrentSettingModel.IsTop;
            this.AutoToggle.IsChecked = ModelUtils.Instance.CurrentSettingModel.AutoOpen;
            this.CloseToggle.IsChecked = ModelUtils.Instance.CurrentSettingModel.Close;
            this.Topmost = ModelUtils.Instance.CurrentSettingModel.IsTop;
            this.CheckTheme();
            this.InitSystemData();
            //this.SettingFlyout.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
            //if (DataUtils.ThemeDic.ContainsKey(ModelUtils.Instance.CurrentSettingModel.Theme))
            //{
            //    ChangeTheme(DataUtils.ThemeDic[ModelUtils.Instance.CurrentSettingModel.Theme]);
            //    this.SettingFlyout.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000")); ;//FFE979
            //    //for (int i = 0; i < this.ThemePanel.Children.Count; i++)
            //    //{
            //    //    RadioButton radio = this.ThemePanel.Children[i] as RadioButton;
            //    //    if (radio != null)
            //    //    {
            //    //        if (radio.Name == ModelUtils.Instance.CurrentSettingModel.Theme)
            //    //        {
            //    //            this.SettingFlyout.Background = radio.Background;
            //    //            break;
            //    //        }
            //    //    }
            //    //}

            //}
            this.RefreshData();
        }

        /// <summary>
        /// 选中主题
        /// </summary>
        private void CheckTheme()
        {
            switch (ModelUtils.Instance.CurrentSettingModel.Theme)
            {
                case DataUtils.Amber:
                    this.Amber.IsChecked = true;
                    break;

                case DataUtils.BaseDark:
                    this.BaseDark.IsChecked = true;
                    break;

                case DataUtils.Blue:
                    this.Blue.IsChecked = true;
                    break;

                case DataUtils.Brown:
                    this.Brown.IsChecked = true;
                    break;

                case DataUtils.Cobalt:
                    this.Cobalt.IsChecked = true;
                    break;

                case DataUtils.Crimson:
                    this.Crimson.IsChecked = true;
                    break;

                case DataUtils.Cyan:
                    this.Cyan.IsChecked = true;
                    break;

                case DataUtils.Emerald:
                    this.Emerald.IsChecked = true;
                    break;

                case DataUtils.Green:
                    this.Green.IsChecked = true;
                    break;

                case DataUtils.Indigo:
                    this.Indigo.IsChecked = true;
                    break;

                case DataUtils.Lime:
                    this.Lime.IsChecked = true;
                    break;

                case DataUtils.Magenta:
                    this.Magenta.IsChecked = true;
                    break;

                case DataUtils.Mauve:
                    this.Mauve.IsChecked = true;
                    break;

                case DataUtils.Olive:
                    this.Olive.IsChecked = true;
                    break;

                case DataUtils.Orange:
                    this.Orange.IsChecked = true;
                    break;

                case DataUtils.Purple:
                    this.Purple.IsChecked = true;
                    break;

                case DataUtils.Pink:
                    this.Pink.IsChecked = true;
                    break;

                case DataUtils.Red:
                    this.Red.IsChecked = true;
                    break;

                case DataUtils.Sienna:
                    this.Sienna.IsChecked = true;
                    break;

                case DataUtils.Steel:
                    this.Steel.IsChecked = true;
                    break;

                case DataUtils.Taupe:
                    this.Taupe.IsChecked = true;
                    break;

                case DataUtils.Violet:
                    this.Violet.IsChecked = true;
                    break;

                case DataUtils.Yellow:
                    this.Yellow.IsChecked = true;
                    break;

                case DataUtils.BaseLight:
                default:
                    this.BaseLight.IsChecked = true;
                    break;
            }
        }

        #endregion 初始化数据

        #region 刷新列表

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            this.EasyOpenItemPanel.Children.Clear();
            DataUtils.EasyOpenItemList.Clear();
            for (int i = 0; i < ModelUtils.Instance.CurrentFileModelList.Count; i++)
            {
                EasyOpenViewItem b = new EasyOpenViewItem();
                b.StaticData = ModelUtils.Instance.CurrentFileModelList[i];
                b.MouseLeftButtonDown += OnEasyOpenViewItem_Click;
                b.MouseDoubleClick += OnEasyOpenViewItem_DoubleClick;
                DataUtils.EasyOpenItemList.Add(b);
                this.EasyOpenItemPanel.Children.Add(b);
            }
        }

        private void InitSystemData()
        {
            this.EasyOpenSystemItemPanel.Children.Clear();
            DataUtils.EasyOpenSystemItemList.Clear();
            for (int i = 0; i < ModelUtils.Instance.CurrentSystemModelList.Count; i++)
            {
                EasyOpenViewItem b = new EasyOpenViewItem();
                b.SystemStaticData = ModelUtils.Instance.CurrentSystemModelList[i];
                b.MouseLeftButtonDown += OnEasyOpenViewItem_Click;
                b.MouseDoubleClick += OnEasyOpenViewItem_DoubleClick;
                DataUtils.EasyOpenSystemItemList.Add(b);
                this.EasyOpenSystemItemPanel.Children.Add(b);
            }
        }

        #endregion 刷新列表

        #region 边缘停靠

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32.WM_MOVING: // 窗体移动的消息，控制窗体不会移出屏幕外
                    if (DataUtils.LastSelectItem != null)
                    {
                        DataUtils.LastSelectItem.Background = Brushes.Transparent;
                        DataUtils.LastSelectItem = null;
                    }
                    if (this.SettingFlyout.IsOpen)
                    {
                        this.SettingFlyout.IsOpen = false;
                    }
                    int left = Marshal.ReadInt32(lParam, 0);
                    int top = Marshal.ReadInt32(lParam, 4);
                    int right = Marshal.ReadInt32(lParam, 8);
                    int bottom = Marshal.ReadInt32(lParam, 12);
                    left = Math.Min(Math.Max(0, left),
                        Forms.Screen.PrimaryScreen.Bounds.Width - (int)this.Width);
                    top = Math.Min(Math.Max(0, top),
                        Forms.Screen.PrimaryScreen.Bounds.Height - (int)this.Height);
                    right = Math.Min(Math.Max((int)this.Width, right),
                        Forms.Screen.PrimaryScreen.Bounds.Width);
                    bottom = Math.Min(Math.Max((int)this.Height, bottom),
                        Forms.Screen.PrimaryScreen.Bounds.Height);
                    Marshal.WriteInt32(lParam, 0, left);
                    Marshal.WriteInt32(lParam, 4, top);
                    Marshal.WriteInt32(lParam, 8, right);
                    Marshal.WriteInt32(lParam, 12, bottom);
                    _anchors = Forms.AnchorStyles.None;
                    if (left <= OFFSET) _anchors |= Forms.AnchorStyles.Left;
                    if (top <= OFFSET) _anchors |= Forms.AnchorStyles.Top;
                    if (bottom >= Forms.Screen.PrimaryScreen.Bounds.Height - OFFSET)
                        _anchors |= Forms.AnchorStyles.Bottom;
                    if (right >= Forms.Screen.PrimaryScreen.Bounds.Width - OFFSET)
                        _anchors |= Forms.AnchorStyles.Right;
                    _timer.Enabled = _anchors != Forms.AnchorStyles.None;
                    break;
            }
            // Handle the message.
            return IntPtr.Zero;
        }

        // <summary>
        /// Timer的Elapsed事件处理程序
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimerEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            this.EasyOpenWindows.Dispatcher.Invoke(new MainThreadDelegate(() =>
            {
                IntPtr vHandle = Win32.WindowFromPoint(Forms.Control.MousePosition);
                while (vHandle != IntPtr.Zero && vHandle != Handle)
                    vHandle = Win32.GetParent(vHandle);
                if (vHandle == this.Handle) // 如果鼠标停留的窗体是本窗体，还原位置
                {
                    if ((_anchors & Forms.AnchorStyles.Left) == Forms.AnchorStyles.Left) Left = 0;
                    if ((_anchors & Forms.AnchorStyles.Top) == Forms.AnchorStyles.Top) Top = 0;
                    if ((_anchors & Forms.AnchorStyles.Right) == Forms.AnchorStyles.Right)
                        Left = Forms.Screen.PrimaryScreen.Bounds.Width - Width;
                    if ((_anchors & Forms.AnchorStyles.Bottom) == Forms.AnchorStyles.Bottom)
                        Top = Forms.Screen.PrimaryScreen.Bounds.Height - Height;
                }
                else if (IsStop)
                {
                    if ((_anchors & Forms.AnchorStyles.Left) == Forms.AnchorStyles.Left) Left = 0;
                    if ((_anchors & Forms.AnchorStyles.Top) == Forms.AnchorStyles.Top) Top = 0;
                    if ((_anchors & Forms.AnchorStyles.Right) == Forms.AnchorStyles.Right)
                        Left = Forms.Screen.PrimaryScreen.Bounds.Width - Width;
                    if ((_anchors & Forms.AnchorStyles.Bottom) == Forms.AnchorStyles.Bottom)
                        Top = Forms.Screen.PrimaryScreen.Bounds.Height - Height;
                    IsStop = false;
                    _timer.Enabled = false;
                }
                else // 隐藏起来
                {
                    if ((_anchors & Forms.AnchorStyles.Left) == Forms.AnchorStyles.Left)
                    {
                        Left = -Width + OFFSET;
                    }
                    else if ((_anchors & Forms.AnchorStyles.Top) == Forms.AnchorStyles.Top)
                    {
                        Top = -Height + OFFSET;
                    }
                    else if ((_anchors & Forms.AnchorStyles.Right) == Forms.AnchorStyles.Right)
                    {
                        Left = Forms.Screen.PrimaryScreen.Bounds.Width - OFFSET;
                    }
                    else if ((_anchors & Forms.AnchorStyles.Bottom) == Forms.AnchorStyles.Bottom)
                    {
                        Top = Forms.Screen.PrimaryScreen.Bounds.Height - OFFSET;
                    }
                }
            }));
        }

        #endregion 边缘停靠

        private void AddAppFile()
        {
            Forms.OpenFileDialog fileDialog = new Forms.OpenFileDialog();
            fileDialog.Filter = "所有文件(*.*)|*.*";
            fileDialog.DereferenceLinks = true;
            if (fileDialog.ShowDialog() == Forms.DialogResult.OK)
            {
                string extension = IO.Path.GetExtension(fileDialog.FileName);
                if (extension.ToLower() == ".lnk")
                {
                    fileDialog.FileName = FileUtils.Instance.GetLnkFile(fileDialog.FileName);
                }
                if (ModelUtils.Instance.CurrentFileModelList.FirstOrDefault(a => a.FullPath == fileDialog.FileName) != null)
                {
                    return;
                }
                if (FileUtils.Instance.AddFileModel(fileDialog.FileName))
                {
                    ModelUtils.Instance.Save();
                    this.RefreshData();
                }
                else
                {
                    MessageBox.Show("如果您要添加文件夹请右键！");
                }
                //  MessageBox.Show(fileDialog.FileName);
                //Icon icon;
                //string fullPath = fileDialog.FileName;
                //string extension = Path.GetExtension(fullPath);
                //string appName = Path.GetFileName(fullPath);

                //ExecuteCommon execute = new ExecuteCommon();
                ///* 读取xml */
                //if (!(bool)execute.doMethod(Key_XmlCore.GetSameApp.ToString(), new object[] { appName }))
                //{
                //    MessageBox.Show("已存在" + appName + "的应用程序！");
                //    return;
                //}

                ///* 获取图标 */
                //if (!(bool)execute.doMethod(Key_XmlCore.GetIcon.ToString(), new object[] { fullPath }))
                //{
                //    MessageBox.Show("添加应用程序失败！");
                //    return;
                //}
                //else
                //{
                //    icon = XmlCore.listIcon[0];
                //}

                ///* 绑定listview 写入XML */
                //if (!(bool)execute.doMethod(Key_LogicCore.LoadOneLvOneSelf.ToString(), new object[] { mImgListOneSelf, appName, mAddCount.ToString(), icon, lvOneSelf }) || !(bool)execute.doMethod(Key_XmlCore.SaveOneSelfXml.ToString(), new object[] { new Dictionary<string, string>() { { "fullpath", fullPath }, { "InnerText", appName } } }))
                //{
                //    MessageBox.Show("添加应用程序失败！");
                //    return;
                //}

                //mListOneSelfPath.Add(fullPath);
                //lvOneSelf.Tag = mListOneSelfPath;
                //mListOneSelfApp = lvOneSelf.Items.ToList();
                //mAddCount++;
            }
        }

        /// <summary>
        /// 设置开机启动
        /// </summary>
        /// <param name="value"></param>
        private void SetOpenStart()
        {
            RegistryKey reg = null;
            try
            {
                bool isAutoRun = ModelUtils.Instance.CurrentSettingModel.AutoOpen;
                string fileName = Forms.Application.ExecutablePath;
                String name = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (reg == null)
                    reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (isAutoRun)
                    reg.SetValue(name, fileName);
                else
                    reg.SetValue(name, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("请以管理员身份运行！");
                //throw new Exception(ex.ToString());
            }
            finally
            {
                if (reg != null)
                    reg.Close();
            }
        }

        private void ChangeTheme(string themePath)
        {
            List<ResourceDictionary> dictionaryList = new List<ResourceDictionary>();
            foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries)
            {
                dictionaryList.Add(dictionary);
            }
            string requestedCulture = themePath;
            ResourceDictionary resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString.StartsWith("pack://application:,,,/MahApps.Metro;component/Styles/Accents/"));
            Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(themePath) });
        }

        #endregion 私有方法

        private void Theme_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            if (DataUtils.ThemeDic.ContainsKey(radio.Name))
            {
                ModelUtils.Instance.CurrentSettingModel.Theme = radio.Name;
                ModelUtils.Instance.Save();
                ChangeTheme(DataUtils.ThemeDic[radio.Name]);

                this.SettingFlyout.Background = radio.Background;
                //if (radio.Background == Brushes.Black)
                if (radio.Name == DataUtils.BaseDark)
                {
                    this.SettingFlyout.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
                }
                else
                {
                    this.SettingFlyout.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
                }
            }
            else
            {
                DialogManager.ShowModalMessageExternal(this, "选择主题", "主题选择错误！");
            }
        }
    }
}