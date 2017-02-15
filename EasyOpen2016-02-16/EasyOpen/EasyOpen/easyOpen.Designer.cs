namespace EasyOpen
{
    partial class easyOpen
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(easyOpen));
            this.lvOneSelf = new CSharpWin.ListViewEx();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblSmall = new System.Windows.Forms.Label();
            this.lblClose = new System.Windows.Forms.Label();
            this.lblAddPlan = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.NotMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._close_item = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.numTxt = new System.Windows.Forms.NumericUpDown();
            this._top_Ch = new System.Windows.Forms.CheckBox();
            this._close_Ch = new System.Windows.Forms.CheckBox();
            this._startOpen_Ch = new System.Windows.Forms.CheckBox();
            this._clear_Ch = new System.Windows.Forms.CheckBox();
            this.tabControl = new CSharpWin.TabControlEx();
            this.tabOne = new System.Windows.Forms.TabPage();
            this._search_ComBox = new System.Windows.Forms.ComboBox();
            this.picBoxAdd = new System.Windows.Forms.PictureBox();
            this.tabTwo = new System.Windows.Forms.TabPage();
            this.lvSystem = new CSharpWin.ListViewEx();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.OneSelfMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.openReMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.removeMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SysMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._openMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.NotMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTxt)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabOne.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAdd)).BeginInit();
            this.tabTwo.SuspendLayout();
            this.OneSelfMenu.SuspendLayout();
            this.SysMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvOneSelf
            // 
            this.lvOneSelf.BackColor = System.Drawing.SystemColors.Control;
            this.lvOneSelf.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvOneSelf.LabelEdit = true;
            this.lvOneSelf.Location = new System.Drawing.Point(5, 38);
            this.lvOneSelf.Name = "lvOneSelf";
            this.lvOneSelf.OwnerDraw = true;
            this.lvOneSelf.Size = new System.Drawing.Size(530, 242);
            this.lvOneSelf.TabIndex = 4;
            this.lvOneSelf.UseCompatibleStateImageBehavior = false;
            this.lvOneSelf.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvOneSelf_AfterLabelEdit);
            this.lvOneSelf.DoubleClick += new System.EventHandler(this.lvEx_DoubleClick);
            this.lvOneSelf.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvOneSelf_MouseDown);
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.Color.SteelBlue;
            this.panelTitle.Controls.Add(this.pictureBox2);
            this.panelTitle.Controls.Add(this.lblLogo);
            this.panelTitle.Controls.Add(this.lblSmall);
            this.panelTitle.Controls.Add(this.lblClose);
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(546, 33);
            this.panelTitle.TabIndex = 5;
            this.panelTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.easyOpen_MouseDown);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(5, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(31, 28);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.easyOpen_MouseDown);
            // 
            // lblLogo
            // 
            this.lblLogo.AutoSize = true;
            this.lblLogo.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblLogo.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLogo.ForeColor = System.Drawing.SystemColors.Control;
            this.lblLogo.Location = new System.Drawing.Point(32, 4);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(52, 27);
            this.lblLogo.TabIndex = 8;
            this.lblLogo.Text = "ｅＯ";
            this.lblLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.easyOpen_MouseDown);
            // 
            // lblSmall
            // 
            this.lblSmall.AutoSize = true;
            this.lblSmall.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblSmall.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSmall.ForeColor = System.Drawing.SystemColors.Control;
            this.lblSmall.Location = new System.Drawing.Point(501, 11);
            this.lblSmall.Name = "lblSmall";
            this.lblSmall.Size = new System.Drawing.Size(20, 21);
            this.lblSmall.TabIndex = 7;
            this.lblSmall.Text = "━";
            this.lblSmall.Click += new System.EventHandler(this.lblSmall_Click);
            // 
            // lblClose
            // 
            this.lblClose.AutoSize = true;
            this.lblClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblClose.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblClose.ForeColor = System.Drawing.SystemColors.Control;
            this.lblClose.Location = new System.Drawing.Point(521, 10);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(19, 19);
            this.lblClose.TabIndex = 6;
            this.lblClose.Text = "X";
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            // 
            // lblAddPlan
            // 
            this.lblAddPlan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAddPlan.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblAddPlan.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblAddPlan.Location = new System.Drawing.Point(32, 0);
            this.lblAddPlan.Name = "lblAddPlan";
            this.lblAddPlan.Size = new System.Drawing.Size(106, 31);
            this.lblAddPlan.TabIndex = 6;
            this.lblAddPlan.Text = "添加自用程序";
            this.lblAddPlan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAddPlan.Click += new System.EventHandler(this.picBoxAdd_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.NotMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "EasyOpen";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // NotMenu
            // 
            this.NotMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._close_item});
            this.NotMenu.Name = "contextMenuStrip";
            this.NotMenu.Size = new System.Drawing.Size(101, 26);
            // 
            // _close_item
            // 
            this._close_item.AutoSize = false;
            this._close_item.Name = "_close_item";
            this._close_item.Size = new System.Drawing.Size(100, 22);
            this._close_item.Text = "关闭";
            this._close_item.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this._close_item.Click += new System.EventHandler(this._close_item_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SteelBlue;
            this.panel1.Controls.Add(this.numTxt);
            this.panel1.Controls.Add(this._top_Ch);
            this.panel1.Controls.Add(this._close_Ch);
            this.panel1.Controls.Add(this._startOpen_Ch);
            this.panel1.Controls.Add(this._clear_Ch);
            this.panel1.Location = new System.Drawing.Point(0, 350);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(546, 19);
            this.panel1.TabIndex = 8;
            // 
            // numTxt
            // 
            this.numTxt.Location = new System.Drawing.Point(375, 0);
            this.numTxt.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTxt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTxt.Name = "numTxt";
            this.numTxt.ReadOnly = true;
            this.numTxt.Size = new System.Drawing.Size(35, 21);
            this.numTxt.TabIndex = 4;
            this.numTxt.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTxt.Visible = false;
            this.numTxt.ValueChanged += new System.EventHandler(this.numTxt_ValueChanged);
            // 
            // _top_Ch
            // 
            this._top_Ch.AutoSize = true;
            this._top_Ch.Location = new System.Drawing.Point(56, 2);
            this._top_Ch.Name = "_top_Ch";
            this._top_Ch.Size = new System.Drawing.Size(72, 16);
            this._top_Ch.TabIndex = 3;
            this._top_Ch.Text = "是否置顶";
            this._top_Ch.UseVisualStyleBackColor = true;
            this._top_Ch.Click += new System.EventHandler(this._startOpen_Ch_Click);
            // 
            // _close_Ch
            // 
            this._close_Ch.AutoSize = true;
            this._close_Ch.Location = new System.Drawing.Point(177, 2);
            this._close_Ch.Name = "_close_Ch";
            this._close_Ch.Size = new System.Drawing.Size(72, 16);
            this._close_Ch.TabIndex = 1;
            this._close_Ch.Text = "直接关闭";
            this._close_Ch.UseVisualStyleBackColor = true;
            this._close_Ch.Click += new System.EventHandler(this._startOpen_Ch_Click);
            // 
            // _startOpen_Ch
            // 
            this._startOpen_Ch.AutoSize = true;
            this._startOpen_Ch.Location = new System.Drawing.Point(419, 2);
            this._startOpen_Ch.Name = "_startOpen_Ch";
            this._startOpen_Ch.Size = new System.Drawing.Size(72, 16);
            this._startOpen_Ch.TabIndex = 0;
            this._startOpen_Ch.Text = "开机启动";
            this._startOpen_Ch.UseVisualStyleBackColor = true;
            this._startOpen_Ch.Click += new System.EventHandler(this._startOpen_Ch_Click);
            // 
            // _clear_Ch
            // 
            this._clear_Ch.AutoSize = true;
            this._clear_Ch.Location = new System.Drawing.Point(300, 2);
            this._clear_Ch.Name = "_clear_Ch";
            this._clear_Ch.Size = new System.Drawing.Size(72, 16);
            this._clear_Ch.TabIndex = 2;
            this._clear_Ch.Text = "智能清理";
            this._clear_Ch.UseVisualStyleBackColor = true;
            this._clear_Ch.Click += new System.EventHandler(this._startOpen_Ch_Click);
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(79)))), ((int)(((byte)(125)))));
            this.tabControl.BackColor = System.Drawing.SystemColors.Control;
            this.tabControl.BaseColor = System.Drawing.Color.LightGray;
            this.tabControl.BorderColor = System.Drawing.SystemColors.AppWorkspace;
            this.tabControl.Controls.Add(this.tabOne);
            this.tabControl.Controls.Add(this.tabTwo);
            this.tabControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabControl.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl.Location = new System.Drawing.Point(0, 33);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(546, 316);
            this.tabControl.TabIndex = 10;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            this.tabControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.easyOpen_KeyDown);
            // 
            // tabOne
            // 
            this.tabOne.BackColor = System.Drawing.SystemColors.Control;
            this.tabOne.Controls.Add(this._search_ComBox);
            this.tabOne.Controls.Add(this.lvOneSelf);
            this.tabOne.Controls.Add(this.picBoxAdd);
            this.tabOne.Controls.Add(this.lblAddPlan);
            this.tabOne.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabOne.Location = new System.Drawing.Point(4, 4);
            this.tabOne.Name = "tabOne";
            this.tabOne.Padding = new System.Windows.Forms.Padding(3);
            this.tabOne.Size = new System.Drawing.Size(538, 286);
            this.tabOne.TabIndex = 0;
            this.tabOne.Text = "自用程序";
            // 
            // _search_ComBox
            // 
            this._search_ComBox.FormattingEnabled = true;
            this._search_ComBox.Location = new System.Drawing.Point(342, 6);
            this._search_ComBox.Name = "_search_ComBox";
            this._search_ComBox.Size = new System.Drawing.Size(121, 25);
            this._search_ComBox.TabIndex = 8;
            this._search_ComBox.Visible = false;
            this._search_ComBox.TextUpdate += new System.EventHandler(this._search_ComBox_TextChanged);
            this._search_ComBox.SelectedValueChanged += new System.EventHandler(this._search_ComBox_TextChanged);
            // 
            // picBoxAdd
            // 
            this.picBoxAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picBoxAdd.Image = ((System.Drawing.Image)(resources.GetObject("picBoxAdd.Image")));
            this.picBoxAdd.Location = new System.Drawing.Point(3, 1);
            this.picBoxAdd.Name = "picBoxAdd";
            this.picBoxAdd.Size = new System.Drawing.Size(33, 31);
            this.picBoxAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBoxAdd.TabIndex = 7;
            this.picBoxAdd.TabStop = false;
            this.picBoxAdd.Click += new System.EventHandler(this.picBoxAdd_Click);
            // 
            // tabTwo
            // 
            this.tabTwo.BackColor = System.Drawing.SystemColors.Control;
            this.tabTwo.Controls.Add(this.lvSystem);
            this.tabTwo.Location = new System.Drawing.Point(4, 4);
            this.tabTwo.Name = "tabTwo";
            this.tabTwo.Padding = new System.Windows.Forms.Padding(3);
            this.tabTwo.Size = new System.Drawing.Size(538, 286);
            this.tabTwo.TabIndex = 1;
            this.tabTwo.Text = "系统功能";
            // 
            // lvSystem
            // 
            this.lvSystem.BackColor = System.Drawing.SystemColors.Control;
            this.lvSystem.BorderColor = System.Drawing.SystemColors.Control;
            this.lvSystem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvSystem.Location = new System.Drawing.Point(3, 38);
            this.lvSystem.Name = "lvSystem";
            this.lvSystem.OwnerDraw = true;
            this.lvSystem.Size = new System.Drawing.Size(530, 242);
            this.lvSystem.TabIndex = 0;
            this.lvSystem.UseCompatibleStateImageBehavior = false;
            this.lvSystem.DoubleClick += new System.EventHandler(this.lvEx_DoubleClick);
            this.lvSystem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvOneSelf_MouseDown);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // OneSelfMenu
            // 
            this.OneSelfMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenu,
            this.openReMenu,
            this.openFolderMenu,
            this.editMenu,
            this.removeMenu});
            this.OneSelfMenu.Name = "cmsOneSelf";
            this.OneSelfMenu.Size = new System.Drawing.Size(161, 114);
            // 
            // openMenu
            // 
            this.openMenu.Name = "openMenu";
            this.openMenu.Size = new System.Drawing.Size(160, 22);
            this.openMenu.Text = "打开";
            this.openMenu.Click += new System.EventHandler(this.lvEx_DoubleClick);
            // 
            // openReMenu
            // 
            this.openReMenu.Name = "openReMenu";
            this.openReMenu.Size = new System.Drawing.Size(160, 22);
            this.openReMenu.Text = "重新打开";
            this.openReMenu.Click += new System.EventHandler(this.openReMenu_Click);
            // 
            // openFolderMenu
            // 
            this.openFolderMenu.Name = "openFolderMenu";
            this.openFolderMenu.Size = new System.Drawing.Size(160, 22);
            this.openFolderMenu.Text = "打开所在文件夹";
            this.openFolderMenu.Click += new System.EventHandler(this.openFolderMenu_Click);
            // 
            // editMenu
            // 
            this.editMenu.Name = "editMenu";
            this.editMenu.Size = new System.Drawing.Size(160, 22);
            this.editMenu.Text = "编辑名称";
            this.editMenu.Click += new System.EventHandler(this.editMenu_Click);
            // 
            // removeMenu
            // 
            this.removeMenu.Name = "removeMenu";
            this.removeMenu.Size = new System.Drawing.Size(160, 22);
            this.removeMenu.Text = "删除";
            this.removeMenu.Click += new System.EventHandler(this.removeMenu_Click);
            // 
            // SysMenu
            // 
            this.SysMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._openMenu});
            this.SysMenu.Name = "contextMenuStrip";
            this.SysMenu.Size = new System.Drawing.Size(101, 26);
            // 
            // _openMenu
            // 
            this._openMenu.AutoSize = false;
            this._openMenu.Name = "_openMenu";
            this._openMenu.Size = new System.Drawing.Size(100, 22);
            this._openMenu.Text = "打开";
            this._openMenu.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this._openMenu.Click += new System.EventHandler(this.openReMenu_Click);
            // 
            // easyOpen
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 369);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "easyOpen";
            this.Text = "EasyOpen";
            this.Load += new System.EventHandler(this.easyOpen_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.easyOpen_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.easyOpen_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.easyOpen_MouseDown);
            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.NotMenu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTxt)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabOne.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAdd)).EndInit();
            this.tabTwo.ResumeLayout(false);
            this.OneSelfMenu.ResumeLayout(false);
            this.SysMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CSharpWin.ListViewEx lvOneSelf;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Label lblSmall;
        private System.Windows.Forms.PictureBox picBoxAdd;
        private System.Windows.Forms.Label lblAddPlan;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Panel panel1;
        private CSharpWin.TabControlEx tabControl;
        private System.Windows.Forms.TabPage tabOne;
        private System.Windows.Forms.TabPage tabTwo;
        private CSharpWin.ListViewEx lvSystem;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ContextMenuStrip NotMenu;
        private System.Windows.Forms.ToolStripMenuItem _close_item;
        private System.Windows.Forms.ContextMenuStrip OneSelfMenu;
        private System.Windows.Forms.ToolStripMenuItem openMenu;
        private System.Windows.Forms.ToolStripMenuItem removeMenu;
        private System.Windows.Forms.ToolStripMenuItem openReMenu;
        private System.Windows.Forms.ContextMenuStrip SysMenu;
        private System.Windows.Forms.ToolStripMenuItem _openMenu;
        private System.Windows.Forms.CheckBox _clear_Ch;
        private System.Windows.Forms.CheckBox _close_Ch;
        private System.Windows.Forms.CheckBox _startOpen_Ch;
        private System.Windows.Forms.CheckBox _top_Ch;
        private System.Windows.Forms.ToolStripMenuItem openFolderMenu;
        private System.Windows.Forms.NumericUpDown numTxt;
        private System.Windows.Forms.ToolStripMenuItem editMenu;
        private System.Windows.Forms.ComboBox _search_ComBox;
    }
}

