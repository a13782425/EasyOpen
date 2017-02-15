namespace EasyOpen
{
    partial class app
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._app_ListView = new CSharpWin.ListViewEx();
            this.label1 = new System.Windows.Forms.Label();
            this._title_Panel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this._app_RightMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._close_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._active_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._title_Panel.SuspendLayout();
            this._app_RightMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // _app_ListView
            // 
            this._app_ListView.BackColor = System.Drawing.SystemColors.Control;
            this._app_ListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._app_ListView.Location = new System.Drawing.Point(12, 33);
            this._app_ListView.Name = "_app_ListView";
            this._app_ListView.OwnerDraw = true;
            this._app_ListView.ShowItemToolTips = true;
            this._app_ListView.Size = new System.Drawing.Size(221, 216);
            this._app_ListView.TabIndex = 5;
            this._app_ListView.UseCompatibleStateImageBehavior = false;
            this._app_ListView.DoubleClick += new System.EventHandler(this._app_ListView_DoubleClick);
            this._app_ListView.MouseDown += new System.Windows.Forms.MouseEventHandler(this._app_ListView_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(4, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "请选择你要激活的窗口：";
            // 
            // _title_Panel
            // 
            this._title_Panel.BackColor = System.Drawing.Color.SteelBlue;
            this._title_Panel.Controls.Add(this.label2);
            this._title_Panel.Controls.Add(this.label1);
            this._title_Panel.Location = new System.Drawing.Point(0, 0);
            this._title_Panel.Name = "_title_Panel";
            this._title_Panel.Size = new System.Drawing.Size(245, 25);
            this._title_Panel.TabIndex = 7;
            this._title_Panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this._title_Panel_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(223, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 19);
            this.label2.TabIndex = 7;
            this.label2.Text = "X";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // _app_RightMenu
            // 
            this._app_RightMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._active_MenuItem,
            this._close_MenuItem});
            this._app_RightMenu.Name = "_app_RightMenu";
            this._app_RightMenu.Size = new System.Drawing.Size(153, 70);
            // 
            // _close_MenuItem
            // 
            this._close_MenuItem.Name = "_close_MenuItem";
            this._close_MenuItem.Size = new System.Drawing.Size(152, 22);
            this._close_MenuItem.Text = "关闭此程序";
            this._close_MenuItem.Click += new System.EventHandler(this._close_MenuItem_Click);
            // 
            // _active_MenuItem
            // 
            this._active_MenuItem.Name = "_active_MenuItem";
            this._active_MenuItem.Size = new System.Drawing.Size(152, 22);
            this._active_MenuItem.Text = "激活";
            this._active_MenuItem.Click += new System.EventHandler(this._active_MenuItem_Click);
            // 
            // app
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 260);
            this.Controls.Add(this._title_Panel);
            this.Controls.Add(this._app_ListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "app";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "app";
            this.Load += new System.EventHandler(this.app_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.app_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.app_MouseDown);
            this._title_Panel.ResumeLayout(false);
            this._title_Panel.PerformLayout();
            this._app_RightMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private CSharpWin.ListViewEx _app_ListView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel _title_Panel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip _app_RightMenu;
        private System.Windows.Forms.ToolStripMenuItem _close_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem _active_MenuItem;
    }
}