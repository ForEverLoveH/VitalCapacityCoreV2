namespace VitalCapacityCoreV2.GameWindow
{
    partial class MainWindow
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
            this.uiTitlePanel1 = new Sunny.UI.UITitlePanel();
            this.uiProcessBar1 = new Sunny.UI.UIProcessBar();
            this.uiButton3 = new Sunny.UI.UIButton();
            this.uiButton2 = new Sunny.UI.UIButton();
            this.uiButton1 = new Sunny.UI.UIButton();
            this.uiTitlePanel3 = new Sunny.UI.UITitlePanel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.uiTitlePanel2 = new Sunny.UI.UITitlePanel();
            this.uiTreeView1 = new Sunny.UI.UITreeView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.导入名单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.系统参数设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.平台设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.条形码导出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.uiStyleManager1 = new Sunny.UI.UIStyleManager(this.components);
            this.uiTitlePanel1.SuspendLayout();
            this.uiTitlePanel3.SuspendLayout();
            this.uiTitlePanel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiTitlePanel1
            // 
            this.uiTitlePanel1.Controls.Add(this.uiProcessBar1);
            this.uiTitlePanel1.Controls.Add(this.uiButton3);
            this.uiTitlePanel1.Controls.Add(this.uiButton2);
            this.uiTitlePanel1.Controls.Add(this.uiButton1);
            this.uiTitlePanel1.Controls.Add(this.uiTitlePanel3);
            this.uiTitlePanel1.Controls.Add(this.uiTitlePanel2);
            this.uiTitlePanel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTitlePanel1.Location = new System.Drawing.Point(3, 59);
            this.uiTitlePanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTitlePanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel1.Name = "uiTitlePanel1";
            this.uiTitlePanel1.ShowText = false;
            this.uiTitlePanel1.Size = new System.Drawing.Size(1369, 756);
            this.uiTitlePanel1.TabIndex = 0;
            this.uiTitlePanel1.Text = "德育龙测试系统";
            this.uiTitlePanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiProcessBar1
            // 
            this.uiProcessBar1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiProcessBar1.Location = new System.Drawing.Point(295, 753);
            this.uiProcessBar1.MinimumSize = new System.Drawing.Size(70, 3);
            this.uiProcessBar1.Name = "uiProcessBar1";
            this.uiProcessBar1.Size = new System.Drawing.Size(1071, 22);
            this.uiProcessBar1.TabIndex = 5;
            this.uiProcessBar1.Visible = false;
            // 
            // uiButton3
            // 
            this.uiButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton3.Location = new System.Drawing.Point(721, 49);
            this.uiButton3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton3.Name = "uiButton3";
            this.uiButton3.Size = new System.Drawing.Size(100, 35);
            this.uiButton3.TabIndex = 4;
            this.uiButton3.Text = "导出成绩";
            this.uiButton3.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton3.Click += new System.EventHandler(this.uiButton3_Click);
            // 
            // uiButton2
            // 
            this.uiButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton2.Location = new System.Drawing.Point(592, 49);
            this.uiButton2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(100, 35);
            this.uiButton2.TabIndex = 3;
            this.uiButton2.Text = "上传成绩";
            this.uiButton2.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton2.Click += new System.EventHandler(this.uiButton2_Click);
            // 
            // uiButton1
            // 
            this.uiButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Location = new System.Drawing.Point(447, 49);
            this.uiButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(100, 35);
            this.uiButton1.TabIndex = 2;
            this.uiButton1.Text = "启动测试";
            this.uiButton1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // uiTitlePanel3
            // 
            this.uiTitlePanel3.Controls.Add(this.listView1);
            this.uiTitlePanel3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTitlePanel3.Location = new System.Drawing.Point(295, 92);
            this.uiTitlePanel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTitlePanel3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel3.Name = "uiTitlePanel3";
            this.uiTitlePanel3.ShowText = false;
            this.uiTitlePanel3.Size = new System.Drawing.Size(1065, 653);
            this.uiTitlePanel3.TabIndex = 1;
            this.uiTitlePanel3.Text = "学生信息";
            this.uiTitlePanel3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listView1
            // 
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(4, 40);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1058, 610);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // uiTitlePanel2
            // 
            this.uiTitlePanel2.Controls.Add(this.uiTreeView1);
            this.uiTitlePanel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTitlePanel2.Location = new System.Drawing.Point(4, 40);
            this.uiTitlePanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTitlePanel2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel2.Name = "uiTitlePanel2";
            this.uiTitlePanel2.ShowText = false;
            this.uiTitlePanel2.Size = new System.Drawing.Size(283, 735);
            this.uiTitlePanel2.TabIndex = 0;
            this.uiTitlePanel2.Text = "项目信息";
            this.uiTitlePanel2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiTreeView1
            // 
            this.uiTreeView1.FillColor = System.Drawing.Color.White;
            this.uiTreeView1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTreeView1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiTreeView1.Location = new System.Drawing.Point(4, 39);
            this.uiTreeView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTreeView1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTreeView1.Name = "uiTreeView1";
            this.uiTreeView1.ShowText = false;
            this.uiTreeView1.Size = new System.Drawing.Size(275, 696);
            this.uiTreeView1.TabIndex = 0;
            this.uiTreeView1.Text = "uiTreeView1";
            this.uiTreeView1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiTreeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uiTreeView1_MouseDown);
            this.uiTreeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.uiTreeView1_NodeMouseClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入名单ToolStripMenuItem,
            this.系统参数设置ToolStripMenuItem,
            this.平台设置ToolStripMenuItem,
            this.条形码导出ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 35);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1376, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 导入名单ToolStripMenuItem
            // 
            this.导入名单ToolStripMenuItem.Name = "导入名单ToolStripMenuItem";
            this.导入名单ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.导入名单ToolStripMenuItem.Text = "导入名单";
            this.导入名单ToolStripMenuItem.Click += new System.EventHandler(this.导入名单ToolStripMenuItem_Click);
            // 
            // 系统参数设置ToolStripMenuItem
            // 
            this.系统参数设置ToolStripMenuItem.Name = "系统参数设置ToolStripMenuItem";
            this.系统参数设置ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.系统参数设置ToolStripMenuItem.Text = "系统参数设置";
            this.系统参数设置ToolStripMenuItem.Click += new System.EventHandler(this.系统参数设置ToolStripMenuItem_Click);
            // 
            // 平台设置ToolStripMenuItem
            // 
            this.平台设置ToolStripMenuItem.Name = "平台设置ToolStripMenuItem";
            this.平台设置ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.平台设置ToolStripMenuItem.Text = "平台设置";
            this.平台设置ToolStripMenuItem.Click += new System.EventHandler(this.平台设置ToolStripMenuItem_Click);
            // 
            // 条形码导出ToolStripMenuItem
            // 
            this.条形码导出ToolStripMenuItem.Name = "条形码导出ToolStripMenuItem";
            this.条形码导出ToolStripMenuItem.Size = new System.Drawing.Size(80, 21);
            this.条形码导出ToolStripMenuItem.Text = "条形码导出";
            this.条形码导出ToolStripMenuItem.Click += new System.EventHandler(this.条形码导出ToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // uiStyleManager1
            // 
            this.uiStyleManager1.DPIScale = true;
            // 
            // MainWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1376, 824);
            this.Controls.Add(this.uiTitlePanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "主页 ";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 1376, 824);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.uiTitlePanel1.ResumeLayout(false);
            this.uiTitlePanel3.ResumeLayout(false);
            this.uiTitlePanel2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sunny.UI.UITitlePanel uiTitlePanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 导入名单ToolStripMenuItem;
        private Sunny.UI.UIButton uiButton1;
        private Sunny.UI.UITitlePanel uiTitlePanel3;
        private Sunny.UI.UITitlePanel uiTitlePanel2;
        private System.Windows.Forms.ToolStripMenuItem 系统参数设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 平台设置ToolStripMenuItem;
        private Sunny.UI.UIProcessBar uiProcessBar1;
        private Sunny.UI.UIButton uiButton3;
        private Sunny.UI.UIButton uiButton2;
        private System.Windows.Forms.ListView listView1;
        private Sunny.UI.UITreeView uiTreeView1;
        private System.Windows.Forms.ToolStripMenuItem 条形码导出ToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private Sunny.UI.UIStyleManager uiStyleManager1;
    }
}