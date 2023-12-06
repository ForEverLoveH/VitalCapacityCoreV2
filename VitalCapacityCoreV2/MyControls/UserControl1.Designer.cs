namespace VitalCapacityCoreV2.MyControls
{
    partial class UserControl1
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.mtitle = new Sunny.UI.UILabel();
            this.label1 = new System.Windows.Forms.Label();
            this.mIdNumber = new Sunny.UI.UILabel();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolState = new System.Windows.Forms.ToolStripStatusLabel();
            this.mName = new Sunny.UI.UILabel();
            this.label3 = new System.Windows.Forms.Label();
            this.mScore = new Sunny.UI.UILabel();
            this.label4 = new System.Windows.Forms.Label();
            this.roundCbx = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.stateCbx = new System.Windows.Forms.ComboBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mtitle
            // 
            this.mtitle.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.mtitle.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mtitle.Location = new System.Drawing.Point(3, 0);
            this.mtitle.Name = "mtitle";
            this.mtitle.Size = new System.Drawing.Size(170, 23);
            this.mtitle.TabIndex = 0;
            this.mtitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "考号：";
            // 
            // mIdNumber
            // 
            this.mIdNumber.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mIdNumber.Location = new System.Drawing.Point(42, 23);
            this.mIdNumber.Name = "mIdNumber";
            this.mIdNumber.Size = new System.Drawing.Size(131, 24);
            this.mIdNumber.TabIndex = 7;
            this.mIdNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "姓名：";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 200);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(176, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolState
            // 
            this.toolState.ForeColor = System.Drawing.Color.Red;
            this.toolState.Name = "toolState";
            this.toolState.Size = new System.Drawing.Size(68, 17);
            this.toolState.Text = "设备未连接";
            // 
            // mName
            // 
            this.mName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mName.Location = new System.Drawing.Point(46, 57);
            this.mName.Name = "mName";
            this.mName.Size = new System.Drawing.Size(127, 23);
            this.mName.TabIndex = 13;
            this.mName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "成绩：";
            // 
            // mScore
            // 
            this.mScore.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mScore.Location = new System.Drawing.Point(52, 101);
            this.mScore.Name = "mScore";
            this.mScore.Size = new System.Drawing.Size(111, 23);
            this.mScore.TabIndex = 15;
            this.mScore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "轮次：";
            // 
            // roundCbx
            // 
            this.roundCbx.FormattingEnabled = true;
            this.roundCbx.Items.AddRange(new object[] {
            "第一轮",
            "第二轮"});
            this.roundCbx.Location = new System.Drawing.Point(56, 137);
            this.roundCbx.Name = "roundCbx";
            this.roundCbx.Size = new System.Drawing.Size(107, 20);
            this.roundCbx.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 172);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "状态：";
            // 
            // stateCbx
            // 
            this.stateCbx.FormattingEnabled = true;
            this.stateCbx.Items.AddRange(new object[] {
            "未测试",
            "已测试",
            "中退",
            "缺考",
            "犯规",
            "弃权"});
            this.stateCbx.Location = new System.Drawing.Point(56, 164);
            this.stateCbx.Name = "stateCbx";
            this.stateCbx.Size = new System.Drawing.Size(107, 20);
            this.stateCbx.TabIndex = 19;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.stateCbx);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.roundCbx);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.mScore);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mName);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mIdNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mtitle);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(176, 222);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sunny.UI.UILabel mtitle;
        private System.Windows.Forms.Label label1;
        private Sunny.UI.UILabel mIdNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolState;
        private Sunny.UI.UILabel mName;
        private System.Windows.Forms.Label label3;
        private Sunny.UI.UILabel mScore;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox roundCbx;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox stateCbx;
    }
}
