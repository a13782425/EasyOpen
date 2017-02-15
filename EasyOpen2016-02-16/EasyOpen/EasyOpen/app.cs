using CSharpWin;
using EasyOpen.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace EasyOpen
{
    public partial class app : Form
    {
        public app()
        {
            InitializeComponent();
        }
        public List<OpenFileInfo> _fileInfo;
        public static ImageList mImgListOneSelf = new ImageList();
        Size size = new System.Drawing.Size(32, 32);
        private void app_Load(object sender, EventArgs e)
        {
            FileIsOpen.SetWindowPos(this.Handle, -1, 0, 0, 0, 0, 1 | 2);
            //_app_ListView.LargeImageList.Images.Clear();/* mImgListOneSelf.Images.Clear();*/
            mImgListOneSelf.ImageSize = size;
            foreach (OpenFileInfo item in _fileInfo)
            {
                Icon icon = GetOtherIcon.GetFileIcon(item.Name, GetOtherIcon.IconSize.Large, false);
                mImgListOneSelf.Images.Add(item.HWND.ToString(), icon);
            }
            _app_ListView.LargeImageList = mImgListOneSelf;
            List<IntPtr> ptrList = new List<IntPtr>();
            foreach (OpenFileInfo item in _fileInfo)
            {
                _app_ListView.Items.Add(Path.GetFileNameWithoutExtension(item.Name), item.HWND.ToString());
                ptrList.Add(item.HWND);
            }
            _app_ListView.Tag = ptrList;
            _app_ListView.View = View.LargeIcon;
        }

        private void app_MouseDown(object sender, MouseEventArgs e)
        {
            FormEffect.drag(Handle);
        }

        private void _app_ListView_DoubleClick(object sender, EventArgs e)
        {
            ListViewEx list = sender as ListViewEx;
            OpenFile(list);
        }

        private void OpenFile(ListViewEx list)
        {

            int index = list.SelectedIndices[0];
            List<IntPtr> ptrs = list.Tag as List<IntPtr>;
            if (FileIsOpen.ShowWindow(ptrs[index], 9))
            {
                FileIsOpen.SetWindowPos(ptrs[index], 0, 0, 0, 0, 0, 1 | 2);
            }
            this.Close();
        }

        //private void _app_ListView_MouseDown(object sender, MouseEventArgs e)
        //{
        //    FormEffect.drag(Handle);
        //}

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void app_Paint(object sender, PaintEventArgs e)
        {
            PaintForm.Paint(sender, e);
        }

        private void _title_Panel_MouseDown(object sender, MouseEventArgs e)
        {
            FormEffect.drag(Handle);
        }

        private void _app_ListView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (_app_ListView.SelectedItems.Count > 0 && _app_ListView.HitTest(e.X, e.Y).Item != null)
                {
                    _app_ListView.ContextMenuStrip = _app_RightMenu;
                }
                else if (_app_ListView.HitTest(e.X, e.Y).Item != null)
                {
                    _app_ListView.ContextMenuStrip = _app_RightMenu;
                }
                else
                {
                    _app_ListView.ContextMenuStrip = null;
                }
            }
        }

        private void _close_MenuItem_Click(object sender, EventArgs e)
        {
            int index = _app_ListView.SelectedIndices[0];
            List<IntPtr> ptrs = _app_ListView.Tag as List<IntPtr>;
            FileIsOpen.SendMessage(ptrs[index], 0x10, 0, 0);
            _app_ListView.Items.RemoveAt(index);
            if (_app_ListView.Items.Count < 1)
            {
                this.Close();
            }
            //MessageBox.Show(_app_ListView.SelectedIndices[0].ToString());
        }

        private void _active_MenuItem_Click(object sender, EventArgs e)
        {
            int index = _app_ListView.SelectedIndices[0];
            List<IntPtr> ptrs = _app_ListView.Tag as List<IntPtr>;
            if (FileIsOpen.ShowWindow(ptrs[index], 9))
            {
                FileIsOpen.SetWindowPos(ptrs[index], 0, 0, 0, 0, 0, 1 | 2);
            }
        }
    }
}
