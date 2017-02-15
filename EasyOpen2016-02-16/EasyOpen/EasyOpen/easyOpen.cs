using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using CSharpWin;
using System.Reflection;
using System.Resources;
using EasyOpen.Utils;
using System.Threading;
using EasyOpen.model;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Drawing.Drawing2D;
using EasyOpen.factory;
using EasyOpen.core;
using System.Windows.Interop;

namespace EasyOpen
{
    public partial class easyOpen : Form
    {

        #region 字段

        /* 自用程序ImgList对象 */
        public static ImageList mImgListOneSelf = new ImageList();

        /* 系统功能ImgList对象 */
        public static ImageList mImgListSystem = new ImageList();

        /* 自用程序路径 */
        public static List<string> mListOneSelfPath = new List<string>();
        /*自用程序的列表*/
        public static List<ListViewItem> mListOneSelfApp;

        /* 系统功能路径 */
        public static List<string> mListSystemPath = new List<string>();

        /* 添加应用程序次数 */
        public static int mAddCount = 0;

        /* 窗体是否停靠 */
        private static bool mIsStop = false;

        /* 最小化、关闭窗体次数 */
        private static int mMinCount = 0;
        /* 是否靠边缩进 */
        private bool IsIndent = true;

        private KeyBoardHook k_hook = null;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public easyOpen()
        {
            //Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            k_hook = new KeyBoardHook();
            k_hook.KeyDownEvent += new KeyEventHandler(hook_KeyDown);//钩住键按下
            k_hook.Start();//安装键盘钩子
            InitData();
            //this.ShowInTaskbar = true; //不显示在系统任务栏
        }



        #endregion

        #region 窗体事件

        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == WM_SYSCOMMAND)
        //    {
        //        if (m.WParam.ToInt64() == SC_MAXIMIZE)
        //        {
        //            //MessageBox.Show("MAXIMIZE ");  
        //            return;
        //        }
        //        if (m.WParam.ToInt64() == SC_MINIMIZE)
        //        {
        //            //MessageBox.Show("MINIMIZE ");  
        //            return;
        //        }
        //        if (m.WParam.ToInt64() == SC_CLOSE)
        //        {
        //            //MessageBox.Show("CLOSE ");  
        //            return;
        //        }
        //    }
        //    base.WndProc(ref m);
        //}

        #region 窗体拖动

        private void easyOpen_MouseDown(object sender, MouseEventArgs e)
        {
            FormEffect.drag(Handle);
        }

        #endregion

        #region 窗体停靠

        AnchorStyles anchors;
        const int OFFSET = 2;

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_TOPMOST = 8;
                base.CreateParams.Parent = MemoryAddress.GetDesktopWindow();
                base.CreateParams.ExStyle |= WS_EX_TOPMOST;
                return base.CreateParams;
            }
        }

        protected override void WndProc(ref Message m)
        {

            const int WM_MOVING = 534;
            switch (m.Msg)
            {
                case WM_MOVING: // 窗体移动的消息，控制窗体不会移出屏幕外
                    int left = Marshal.ReadInt32(m.LParam, 0);
                    int top = Marshal.ReadInt32(m.LParam, 4);
                    int right = Marshal.ReadInt32(m.LParam, 8);
                    int bottom = Marshal.ReadInt32(m.LParam, 12);
                    left = Math.Min(Math.Max(0, left),
                        Screen.PrimaryScreen.Bounds.Width - Width);
                    top = Math.Min(Math.Max(0, top),
                        Screen.PrimaryScreen.Bounds.Height - Height);
                    right = Math.Min(Math.Max(Width, right),
                        Screen.PrimaryScreen.Bounds.Width);
                    bottom = Math.Min(Math.Max(Height, bottom),
                        Screen.PrimaryScreen.Bounds.Height);
                    Marshal.WriteInt32(m.LParam, 0, left);
                    Marshal.WriteInt32(m.LParam, 4, top);
                    Marshal.WriteInt32(m.LParam, 8, right);
                    Marshal.WriteInt32(m.LParam, 12, bottom);
                    anchors = AnchorStyles.None;
                    if (left <= OFFSET) anchors |= AnchorStyles.Left;
                    if (top <= OFFSET) anchors |= AnchorStyles.Top;
                    if (bottom >= Screen.PrimaryScreen.Bounds.Height - OFFSET)
                        anchors |= AnchorStyles.Bottom;
                    if (right >= Screen.PrimaryScreen.Bounds.Width - OFFSET)
                        anchors |= AnchorStyles.Right;
                    timer.Enabled = anchors != AnchorStyles.None;
                    break;
            }

            base.WndProc(ref m);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (IsIndent)
            {
                IntPtr vHandle = MemoryAddress.WindowFromPoint(Control.MousePosition);
                while (vHandle != IntPtr.Zero && vHandle != Handle)
                    vHandle = MemoryAddress.GetParent(vHandle);
                if (vHandle == Handle) // 如果鼠标停留的窗体是本窗体，还原位置
                {
                    if ((anchors & AnchorStyles.Left) == AnchorStyles.Left) Left = 0;
                    if ((anchors & AnchorStyles.Top) == AnchorStyles.Top) Top = 0;
                    if ((anchors & AnchorStyles.Right) == AnchorStyles.Right)
                        Left = Screen.PrimaryScreen.Bounds.Width - Width;
                    if ((anchors & AnchorStyles.Bottom) == AnchorStyles.Bottom)
                        Top = Screen.PrimaryScreen.Bounds.Height - Height;
                }
                else if (mIsStop)
                {
                    if ((anchors & AnchorStyles.Left) == AnchorStyles.Left) Left = 0;
                    if ((anchors & AnchorStyles.Top) == AnchorStyles.Top) Top = 0;
                    if ((anchors & AnchorStyles.Right) == AnchorStyles.Right)
                        Left = Screen.PrimaryScreen.Bounds.Width - Width;
                    if ((anchors & AnchorStyles.Bottom) == AnchorStyles.Bottom)
                        Top = Screen.PrimaryScreen.Bounds.Height - Height;
                    mIsStop = false;
                    timer.Enabled = false;
                }
                else // 隐藏起来
                {
                    if ((anchors & AnchorStyles.Left) == AnchorStyles.Left)
                    {
                        Left = -Width + OFFSET;
                    }
                    else if ((anchors & AnchorStyles.Top) == AnchorStyles.Top)
                    {
                        Top = -Height + OFFSET;
                    }
                    else if ((anchors & AnchorStyles.Right) == AnchorStyles.Right)
                    {
                        Left = Screen.PrimaryScreen.Bounds.Width - OFFSET;
                    }
                    else if ((anchors & AnchorStyles.Bottom) == AnchorStyles.Bottom)
                    {
                        Top = Screen.PrimaryScreen.Bounds.Height - OFFSET;
                    }
                }
            }
        }

        #endregion

        #region 加载

        private void easyOpen_Load(object sender, EventArgs e)
        {
            if (CheckWindows())
            {
                this.Close();
                Application.Exit();
                return;
            }
            _clear_Ch.Checked = XMLHelper.GetRootValue("Clear") == "1" ? true : false;
            _close_Ch.Checked = XMLHelper.GetRootValue("Close") == "1" ? true : false;
            _top_Ch.Checked = XMLHelper.GetRootValue("IsTop") == "1" ? true : false;
            _startOpen_Ch.Checked = XMLHelper.GetRootValue("AutoRun") == "1" ? true : false;
            //是否置顶
            if (_top_Ch.Checked)
            {
                //置顶
                FileIsOpen.SetWindowPos(this.Handle, -1, 0, 0, 0, 0, 1 | 2);
            }
            else
            {
                FileIsOpen.SetWindowPos(this.Handle, -2, 0, 0, 0, 0, 1 | 2);// 1 | 2);
            }
            if (_clear_Ch.Checked)
            {
                numTxt.Visible = true;
                numTxt.Value = decimal.Parse(XMLHelper.GetRootValue("DayCount"));
                ClearApp();
            }

            ExecuteCommon execute = new ExecuteCommon();
            /* 获取路径 */
            if (!(bool)execute.doMethod(Key_XmlCore.GetFullPath.ToString(), null)) return;

            /* 获取图标 */
            if (!(bool)execute.doMethod(Key_XmlCore.GetIcon.ToString())) return;

            /* 绑定数据 */
            if (!(bool)execute.doMethod(Key_LogicCore.LoadLvOneSelf.ToString(), new object[] { mImgListOneSelf, XmlCore.listOneSelfPath, XmlCore.listIcon, lvOneSelf })) return;

            //设置图标
            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"img\Title.png"))
            {
                Bitmap ico = new Bitmap(System.AppDomain.CurrentDomain.BaseDirectory + @"img\Title.png");
                pictureBox2.Image = ico;
            }
            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"img\Superbar.ico"))
            {
                Icon ico = new Icon(System.AppDomain.CurrentDomain.BaseDirectory + @"img\Superbar.ico");
                notifyIcon.Icon = ico;
            }


            Global.IsOneSelf = true;
            mAddCount++;
        }


        #endregion

        #region 添加应用

        /// <summary>
        /// 添加应用事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picBoxAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "所有文件(*.*)|*.*";
            fileDialog.DereferenceLinks = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                Icon icon;
                string fullPath = fileDialog.FileName;
                string extension = Path.GetExtension(fullPath);
                string appName = Path.GetFileName(fullPath);

                ExecuteCommon execute = new ExecuteCommon();
                /* 读取xml */
                if (!(bool)execute.doMethod(Key_XmlCore.GetSameApp.ToString(), new object[] { appName }))
                {
                    MessageBox.Show("已存在" + appName + "的应用程序！");
                    return;
                }

                /* 获取图标 */
                if (!(bool)execute.doMethod(Key_XmlCore.GetIcon.ToString(), new object[] { fullPath }))
                {
                    MessageBox.Show("添加应用程序失败！");
                    return;
                }
                else
                {
                    icon = XmlCore.listIcon[0];
                }

                /* 绑定listview 写入XML */
                if (!(bool)execute.doMethod(Key_LogicCore.LoadOneLvOneSelf.ToString(), new object[] { mImgListOneSelf, appName, mAddCount.ToString(), icon, lvOneSelf }) || !(bool)execute.doMethod(Key_XmlCore.SaveOneSelfXml.ToString(), new object[] { new Dictionary<string, string>() { { "fullpath", fullPath }, { "InnerText", appName } } }))
                {
                    MessageBox.Show("添加应用程序失败！");
                    return;
                }

                mListOneSelfPath.Add(fullPath);
                lvOneSelf.Tag = mListOneSelfPath;
                mListOneSelfApp = lvOneSelf.Items.ToList();
                mAddCount++;
            }
        }

        #endregion

        #region 最小化

        /// <summary>
        /// 窗体最小化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblSmall_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.notifyIcon.Visible = true;
            this.ShowInTaskbar = false; //不显示在系统任务栏

            if (mMinCount == 0) //第一次最小化窗体 显示提示
            {
                notifyIcon.BalloonTipText = "最小化到托盘,右键关闭";
                notifyIcon.BalloonTipTitle = "提醒";
                notifyIcon.ShowBalloonTip(2000);
                mMinCount++;
                FormEffect.animate(this.Handle, 100, MemoryAddress.AW_SLIDE + MemoryAddress.AW_HOR_POSITIVE + MemoryAddress.AW_HIDE);
            }
        }
        #endregion

        #region 打开应用

        /// <summary>
        /// ListView双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvEx_DoubleClick(object sender, EventArgs e)
        {
            Global.IsNew = false;
            if (this._search_ComBox.Visible)
            {
                this._search_ComBox.Visible = false;
                lvOneSelf.Items.Clear();
                lvOneSelf.Items.AddRange(mListOneSelfApp.ToArray());
                lvOneSelf.Tag = mListOneSelfPath;
                IsIndent = true;
            }
            OpenFile(sender);
        }



        #endregion

        #region 删除应用

        /// <summary>
        /// 删除应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeMenu_Click(object sender, EventArgs e)
        {
            ListViewEx list = sender as ListViewEx;
            list = SelectListView(list);

            ExecuteCommon execute = new ExecuteCommon();
            if (!(bool)execute.doMethod(Key_LogicCore.RomoveApp.ToString(), new object[] { list }))
            {
                MessageBox.Show("删除应用失败！");
            }
        }

        #endregion

        #region 右键菜单

        /// <summary>
        /// 打开关闭 右键菜单功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvOneSelf_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lvOneSelf.SelectedItems.Count > 0 && lvOneSelf.HitTest(e.X, e.Y).Item != null)
                {
                    lvOneSelf.ContextMenuStrip = OneSelfMenu;
                }
                else if (lvOneSelf.HitTest(e.X, e.Y).Item != null)
                {
                    lvOneSelf.ContextMenuStrip = OneSelfMenu;
                }
                else if (lvSystem.SelectedItems.Count > 0 && lvSystem.HitTest(e.X, e.Y).Item != null)
                {
                    lvSystem.ContextMenuStrip = SysMenu;
                }
                else if (lvSystem.HitTest(e.X, e.Y).Item != null)
                {
                    lvSystem.ContextMenuStrip = SysMenu;
                }
                else
                {
                    lvOneSelf.ContextMenuStrip = null;
                    lvSystem.ContextMenuStrip = null;
                }
            }
        }

        #endregion

        #region 切换Tab

        /// <summary>
        /// Tab选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tabTwo)
            {
                ExecuteCommon execute = new ExecuteCommon();
                /* 绑定系统功能 */
                if (!(bool)execute.doMethod(Key_XmlCore.GetSystemXml.ToString(), new object[] { lvSystem.Items.Count }) || !(bool)execute.doMethod(Key_LogicCore.LoadLvSystem.ToString(), new object[] { mImgListSystem, XmlCore.listSystemPath, XmlCore.listIconName, lvSystem }))
                {
                    MessageBox.Show("启动系统功能失败！");
                    return;
                }
                Global.IsOneSelf = false;
            }
            else if (e.TabPage == tabOne)
            {
                Global.IsOneSelf = true;
            }
            else
            {
                Global.IsOneSelf = false;
            }
        }

        #endregion

        #region 双击托盘图标

        /// <summary>
        /// 托盘图标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.Show();
                this.ShowInTaskbar = true;
                if (_top_Ch.Checked)
                {
                    //置顶
                    FileIsOpen.SetWindowPos(this.Handle, -1, 0, 0, 0, 0, 1 | 2);
                }
                else
                {
                    FileIsOpen.SetWindowPos(this.Handle, -2, 0, 0, 0, 0, 1 | 2);
                }

                this.notifyIcon.Visible = true;
            }
            mIsStop = true;
        }

        #endregion

        #region 窗体圆角修饰

        /// <summary>
        /// 窗体圆角修饰
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void easyOpen_Paint(object sender, PaintEventArgs e)
        {
            PaintForm.Paint(sender, e);
        }

        #endregion

        #region 右键关闭

        /// <summary>
        /// 右键关闭按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _close_item_Click(object sender, EventArgs e)
        {
            k_hook.Stop();

            this.Close();
            Application.Exit();
        }

        #endregion

        #region 窗体关闭

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblClose_Click(object sender, EventArgs e)
        {
            if (_close_Ch.Checked)
            {
                k_hook.Stop();
                this.Close();
                Application.Exit();
            }
            else
            {

                this.WindowState = FormWindowState.Minimized;
                this.notifyIcon.Visible = true;
                this.ShowInTaskbar = true; //不显示在系统任务栏
                //this.Hide();
                if (mMinCount == 0) //第一次最小化窗体 显示提示
                {
                    notifyIcon.BalloonTipText = "最小化到托盘,右键关闭";
                    notifyIcon.BalloonTipTitle = "提醒";
                    notifyIcon.ShowBalloonTip(2000);
                    //动画，如果第一次不执行则不能还原
                    FormEffect.animate(this.Handle, 200, MemoryAddress.AW_SLIDE + MemoryAddress.AW_HOR_POSITIVE + MemoryAddress.AW_HIDE);
                    mMinCount++;
                }
            }
        }

        #endregion

        #region 重新打开
        private void openReMenu_Click(object sender, EventArgs e)
        {
            Global.IsNew = true;
            OpenFile(sender);
        }
        #endregion

        #region Check点击事件
        private void _startOpen_Ch_Click(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            string name = cb.Name;
            string value = "";
            switch (name)
            {
                case "_clear_Ch":
                    value = _clear_Ch.Checked ? "1" : "0";
                    XMLHelper.SetRootValue("Clear", value);
                    if (_clear_Ch.Checked)
                    {
                        numTxt.Value = decimal.Parse(XMLHelper.GetRootValue("DayCount"));
                        numTxt.Visible = true;
                        //ClearApp();
                    }
                    else
                    {
                        XMLHelper.SetRootValue("DayCount", numTxt.Value.ToString());
                        numTxt.Visible = false;
                    }
                    break;
                case "_startOpen_Ch":
                    value = _startOpen_Ch.Checked ? "1" : "0";
                    XMLHelper.SetRootValue("AutoRun", value);
                    SetOpenStart(value);
                    break;
                case "_close_Ch":
                    value = _close_Ch.Checked ? "1" : "0";
                    XMLHelper.SetRootValue("Close", value);
                    break;
                case "_top_Ch":
                    value = _top_Ch.Checked ? "1" : "0";
                    XMLHelper.SetRootValue("IsTop", value);
                    SetWindows(value);
                    break;
                default:
                    MessageBox.Show("未知错误！");
                    break;
            }

        }


        #endregion

        #region 打开程序所在文件夹
        private void openFolderMenu_Click(object sender, EventArgs e)
        {
            Global.IsOpenFolder = true;
            OpenFile(sender);
        }
        #endregion

        #region 自动清理次数发生变化时
        /// <summary>
        /// Num发生改变的时候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numTxt_ValueChanged(object sender, EventArgs e)
        {
            XMLHelper.SetRootValue("DayCount", numTxt.Value.ToString());
        }
        #endregion

        /// <summary>
        /// 开启搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void easyOpen_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control) && e.KeyCode == Keys.F)
            {

                _search_ComBox.Visible = !_search_ComBox.Visible;
                IsIndent = !_search_ComBox.Visible;
                _search_ComBox.Items.Clear();
                if (_search_ComBox.Visible)
                {
                    _search_ComBox.Focus();

                    _search_ComBox.Items.AddRange(Global.FileNames.ToArray());
                    _search_ComBox.DisplayMember = "Name";
                    _search_ComBox.ValueMember = "Value";

                    //_search_ComBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    //_search_ComBox.AutoCompleteSource = AutoCompleteSource.ListItems;
                    _search_ComBox.DroppedDown = true;
                    _search_ComBox.Text = "";
                    _search_ComBox.SelectedText = "";
                }
                else
                {
                    lvOneSelf.Items.Clear();
                    lvOneSelf.Items.AddRange(mListOneSelfApp.ToArray());
                    lvOneSelf.Tag = mListOneSelfPath;
                }

            }
        }

        private void _search_ComBox_TextChanged(object sender, EventArgs e)
        {
            var input = _search_ComBox.Text.ToUpper();
            _search_ComBox.Items.Clear();
            if (string.IsNullOrEmpty(input))
            {
                _search_ComBox.Items.AddRange(Global.FileNames.ToArray());
                lvOneSelf.Items.Clear();
                lvOneSelf.Items.AddRange(mListOneSelfApp.ToArray());
                lvOneSelf.Tag = mListOneSelfPath;
                //_search_ComBox.DropDownHeight = _search_ComBox.ItemHeight * (Global.FileNames.Count + 1);
            }
            else
            {
                var newList = Global.FileNames.FindAll(x => x.Value.IndexOf(input, StringComparison.CurrentCultureIgnoreCase) != -1).ToArray();
                _search_ComBox.Items.AddRange(newList);
                //_search_ComBox.DropDownHeight = _search_ComBox.ItemHeight * (newList.Length + 1);
                List<ListViewItem> mList = new List<ListViewItem>();
                mList.Clear();
                for (int i = 0; i < newList.Length; i++)
                {
                    ListViewItem view = mListOneSelfApp.FindOrDefault(a => a.Text == newList[i].Name);
                    if (view != null)
                    {
                        mList.Add(view);
                    }

                }

                lvOneSelf.Items.Clear();
                lvOneSelf.Items.AddRange(mList.ToArray());

            }
            this._search_ComBox.SelectionStart = this._search_ComBox.Text.Length;
            ////_search_ComBox.Select(_search_ComBox.Text.Length, 0);
            Cursor = Cursors.Default;
            _search_ComBox.DroppedDown = true;
            //////保持鼠标指针形状  
            //Cursor = Cursors.Default;
            ////string str = _search_ComBox.Text;

            ////Console.WriteLine(_search_ComBox.Text);
        }
        /// <summary>
        /// 选择某一项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _search_ComBox_SelectedValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(_search_ComBox.Text);
            //MessageBox.Show(_search_ComBox.SelectedItem.ToString());
            if (_search_ComBox.SelectedItem != null)
            {
                SelectItem item = _search_ComBox.SelectedItem as SelectItem;
                ListView.ListViewItemCollection mList = new ListView.ListViewItemCollection(lvOneSelf);
                mList = lvOneSelf.Items;//.Find(item.Name, true);
                List<string> tagList = mListOneSelfPath;// lvOneSelf.Tag as List<string>;
                List<ListViewItem> listView = new List<ListViewItem>();
                int i = 0;
                foreach (ListViewItem view in mListOneSelfApp)
                {
                    if (view.Text == item.Name)
                    {
                        listView.Add(view);
                        break;
                    }
                    i++;
                }
                lvOneSelf.Items.Clear();
                lvOneSelf.Items.AddRange(listView.ToArray());
                List<string> strlist = new List<string>() { mListOneSelfPath[i] };
                //ExecuteCommon execute = new ExecuteCommon();
                ///* 绑定数据 */
                //if (!(bool)execute.doMethod(Key_LogicCore.LoadLvOneSelf.ToString(), new object[] { mImgListOneSelf, XmlCore.listOneSelfPath, XmlCore.listIcon, lvOneSelf })) return;

                lvOneSelf.Tag = strlist;
                //MessageBox.Show(item.Value);
            }

        }
        //private void _search_ComBox_SelectedValueChanged1(object sender, EventArgs e)
        //{
        //    //MessageBox.Show(_search_ComBox.Text);
        //    //MessageBox.Show(_search_ComBox.SelectedItem.ToString());
        //    if (_search_ComBox.SelectedItem != null)
        //    {
        //        SelectItem item = _search_ComBox.SelectedItem as SelectItem;
        //        ListView.ListViewItemCollection mList = new ListView.ListViewItemCollection(lvOneSelf);
        //        mList = lvOneSelf.Items;//.Find(item.Name, true);
        //        List<string> tagList = mListOneSelfPath;// lvOneSelf.Tag as List<string>;
        //        List<ListViewItem> listView = new List<ListViewItem>();
        //        int i = 0;
        //        foreach (ListViewItem view in mListOneSelfApp)
        //        {
        //            if (view.Text == item.Name)
        //            {
        //                listView.Add(view);
        //                break;
        //            }
        //            i++;
        //        }
        //        lvOneSelf.Items.Clear();
        //        lvOneSelf.Items.AddRange(listView.ToArray());
        //        List<string> strlist = new List<string>() { mListOneSelfPath[i] };
        //        //ExecuteCommon execute = new ExecuteCommon();
        //        ///* 绑定数据 */
        //        //if (!(bool)execute.doMethod(Key_LogicCore.LoadLvOneSelf.ToString(), new object[] { mImgListOneSelf, XmlCore.listOneSelfPath, XmlCore.listIcon, lvOneSelf })) return;

        //        lvOneSelf.Tag = strlist;
        //        //MessageBox.Show(item.Value);
        //    }

        //}

        #region 修改名字
        /// <summary>
        /// 修改名字开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editMenu_Click(object sender, EventArgs e)
        {
            //13429889546
            lvOneSelf.LabelEdit = true;
            lvOneSelf.SelectedItems[0].BeginEdit();
        }
        /// <summary>
        /// 修改名字结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvOneSelf_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            ExecuteCommon execute = new ExecuteCommon();
            string name = lvOneSelf.SelectedItems[0].Text;
            if ((bool)execute.doMethod(Key_LogicCore.EditName.ToString(), lvOneSelf, e.Label))
            {
                lvOneSelf.LabelEdit = false;
            }
            else
            {
                lvOneSelf.SelectedItems[0].Text = name;
            }
        }
        #endregion

        #endregion

        #region 私有方法
        /// <summary>
        /// 全局钩子事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            //判断按下的键（Alt + A）
            if (e.KeyValue == (int)Keys.Q && (int)Control.ModifierKeys == (int)Keys.Control)
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.Show();
                    this.ShowInTaskbar = true;
                    if (_top_Ch.Checked)
                    {
                        //置顶
                        FileIsOpen.SetWindowPos(this.Handle, -1, 0, 0, 0, 0, 1 | 2);
                    }
                    else
                    {
                        FileIsOpen.SetWindowPos(this.Handle, -2, 0, 0, 0, 0, 1 | 2);
                    }

                    this.notifyIcon.Visible = true;
                    mIsStop = true;
                }
                else
                {
                    if ((anchors & AnchorStyles.Left) == AnchorStyles.Left)
                    {
                        Left = OFFSET;
                    }
                    else if ((anchors & AnchorStyles.Top) == AnchorStyles.Top)
                    {
                        Top = OFFSET;
                    }
                    else if ((anchors & AnchorStyles.Right) == AnchorStyles.Right)
                    {
                        Left = Screen.PrimaryScreen.Bounds.Width - Width - OFFSET;
                    }
                    else if ((anchors & AnchorStyles.Bottom) == AnchorStyles.Bottom)
                    {
                        Top = Screen.PrimaryScreen.Bounds.Height - Height - OFFSET;
                    }
                    timer.Enabled = false;
                    anchors = AnchorStyles.None;
                }

               

                //System.Windows.Forms.MessageBox.Show("按下了指定快捷键组合");
            }
        }

        /// <summary>
        /// 检测此窗口是否启动
        /// </summary>
        private bool CheckWindows()
        {
            Process[] pros = Process.GetProcessesByName("EasyOpen");
            if (pros.Length > 1)
            {
                //foreach (Process item in pros)
                //{
                //    MessageBox.Show(item.ProcessName);
                //}
                return true;
                //this.Close();
            }
            return false;
        }

        #region 准备数据

        /// <summary>
        /// 准备数据
        /// </summary>
        private void InitData()
        {
            ExecuteCommon execute = new ExecuteCommon();
            if (!(bool)execute.doMethod(Key_XmlCore.InitSystemXml.ToString(), null))
            {
                MessageBox.Show("初始化数据失败！");
                return;
            }
            SetToolTip();
            FormEffect.animate(this.Handle, 1000, MemoryAddress.AW_BLEND);
        }

        #endregion

        private void SetToolTip()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;
            toolTip.IsBalloon = true;
            toolTip.SetToolTip(this.picBoxAdd, "程序安装路径下exe文件");

            timer.Enabled = false;
            timer.Interval = 200;
            TopMost = true;
        }

        #region 判断ListView

        /// <summary>
        /// 判断ListView
        /// </summary>
        /// <param name="list">ListView</param>
        /// <returns></returns>
        private ListViewEx SelectListView(ListViewEx list)
        {
            string index = null;

            if (list == null)
            {
                try
                {//&& (lvOneSelf.ContextMenu.Name == "lvOneSelf")
                    index = !string.IsNullOrEmpty(lvOneSelf.Items[lvOneSelf.SelectedIndices[0]].ToString()) ? lvOneSelf.Items[lvOneSelf.SelectedIndices[0]].ToString() : null;
                    if (!string.IsNullOrEmpty(index))
                    {
                        list = lvOneSelf;
                        return list;
                    }
                    else
                    {// && (removeMenu.Name == "lvSystem")
                        index = !string.IsNullOrEmpty(lvSystem.Items[lvSystem.SelectedIndices[0]].ToString()) ? lvSystem.Items[lvSystem.SelectedIndices[0]].ToString() : null;
                        if (!string.IsNullOrEmpty(index))
                        {
                            list = lvSystem;
                            return list;
                        }
                    }
                }
                catch
                {//&& (removeMenu.Name == "lvSystem") 
                    index = !string.IsNullOrEmpty(lvSystem.Items[lvSystem.SelectedIndices[0]].ToString()) ? lvSystem.Items[lvSystem.SelectedIndices[0]].ToString() : null;
                    if (!string.IsNullOrEmpty(index))
                    {
                        list = lvSystem;
                        return list;
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        private void OpenFile(object sender)
        {
            ListViewEx list = sender as ListViewEx;
            list = SelectListView(list);
            ExecuteCommon execute = new ExecuteCommon();
            if (!(bool)execute.doMethod(Key_LogicCore.OpenApp.ToString(), new object[] { list }))
            {
                MessageBox.Show("打开应用程序失败！");
            }
        }

        #endregion

        /// <summary>
        /// 设置是否置顶
        /// </summary>
        /// <param name="value"></param>
        private void SetWindows(string value)
        {
            if (value == "1")
            {
                FileIsOpen.SetWindowPos(this.Handle, -1, 0, 0, 0, 0, 1 | 2);
            }
            else
            {
                FileIsOpen.SetWindowPos(this.Handle, -2, 0, 0, 0, 0, 3);
            }
        }

        /// <summary>
        /// 设置开机启动
        /// </summary>
        /// <param name="value"></param>
        private void SetOpenStart(string value)
        {
            RegistryKey reg = null;
            try
            {
                bool isAutoRun = value == "1" ? true : false;
                string fileName = Application.ExecutablePath;
                //RegistryKey rk = Registry.LocalMachine;
                //RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                //if (isAutoRun)
                //{
                //    rk2.SetValue("EasyOpen", fileName);
                //}
                //else
                //{
                //    rk2.DeleteValue("EasyOpen", false);
                //}
                //rk2.Close();
                //rk.Close();
                // string fileName=AppDomain.CurrentDomain.BaseDirectory + "EasyOpen.exe";
                //bool isAutoRun = value == "1" ? true : false;
                //string fileName = AppDomain.CurrentDomain.BaseDirectory + "EasyOpen.exe";
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

        /// <summary>
        /// 清理程序
        /// </summary>
        private void ClearApp()
        {
            string time = XMLHelper.GetRootValue("NearClear");
            if (string.IsNullOrEmpty(time))
            {
                Clear();
            }
            else
            {
                DateTime dt;
                if (DateTime.TryParse(time, out dt))
                {
                    TimeSpan ts = DateTime.Now.Subtract(dt);
                    if (ts.Days >= 7)
                    {
                        Clear();
                    }
                }
                else
                {
                    MessageBox.Show("请不要修改配置文件，否则会引发致命错误！");
                    XMLHelper.SetRootValue("NearClear", DateTime.Now.ToShortDateString());
                }
            }
        }
        /// <summary>
        /// 清理
        /// </summary>
        private void Clear()
        {
            double count = (double)numTxt.Value;
            Dictionary<string, string> dic = XMLHelper.GetApps();
            ExecuteCommon execute = new ExecuteCommon();
            foreach (KeyValuePair<string, string> item in dic)
            {
                string[] strs = item.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                int num = int.Parse(strs[0]);
                DateTime dt = DateTime.Parse(strs[1]);
                TimeSpan ts = DateTime.Now.Subtract(dt);
                int day = ts.Days + 1;
                double n = (num * 1.0) / (day * 1.0);
                if (n < count)
                {
                    if (!(bool)execute.doMethod(Key_LogicCore.ClearApp.ToString(), new object[] { item.Key }))
                    {
                        MessageBox.Show("清理应用失败！");
                    }
                }
            }
            XMLHelper.SetRootValue("NearClear", DateTime.Now.ToShortDateString());
        }







        #endregion

        ///// <summary>
        ///// 当激活搜索框时
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void _search_ComBox_Enter(object sender, EventArgs e)
        //{
        //    //_search_ComBox.Items.AddRange(Global.FileNames.ToArray());
        //    _search_ComBox.DroppedDown = true;

        //}
        ///// <summary>
        ///// 当离开搜索框时
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void _search_ComBox_Leave(object sender, EventArgs e)
        //{
        //    _search_ComBox.DroppedDown = false;
        //    _search_ComBox.Visible = false;
        //}
    }
}
