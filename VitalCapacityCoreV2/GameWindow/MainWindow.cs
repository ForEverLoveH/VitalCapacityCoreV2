using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VitalCapacityCoreV2.GameWindowSys;
using VitalCapacityV2.Summer.GameSystem;
using VitalCapacityV2.Summer.GameSystem.GameModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace VitalCapacityCoreV2.GameWindow
{
    public partial class MainWindow : UIForm
    {
        public MainWindow()
        {
            InitializeComponent();
            autoSizeSys = new AutoSizeSys(this.Width, this.Height);
            autoSizeSys.SetTag(this);
        }

        /// <summary>
        ///
        /// </summary>
        private AutoSizeSys autoSizeSys = null;

        /// <summary>
        ///
        /// </summary>
        private MainWindowSys MainWindowSys = new MainWindowSys();

        private SportProjectInfos sportProjectInfos = null;

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            string code = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = "德育龙体育测试系统" + code;
            sportProjectInfos = MainWindowSys.LoadingSportProjectInfos();
            if (sportProjectInfos != null)
            {
                uiTitlePanel1.Text = sportProjectInfos.Name;
                projectName = sportProjectInfos.Name;
            }
            MainWindowSys.LoadingProject(uiTreeView1, projectName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Resize(object sender, EventArgs e)
        {
             autoSizeSys?.ReWinformLayout(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiTreeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                TreeNode node = uiTreeView1.GetNodeAt(e.X, e.Y);
                if (node != null)
                {
                    uiTreeView1.SelectedNode = node;
                }
            }
        }

        private string createTime = string.Empty;
        private string schoolName = string.Empty;
        private string groupName = string.Empty;
        private string projectName = string.Empty;
        private int proMax = 0;
        private int proVal = 0;

        private void uiTreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            string txt = e.Node.Text;
            string fullpath = e.Node.FullPath;
            string[] paths = fullpath.Split('\\');
            if (e.Node.Level == 1)
            {
                createTime = paths[1];
            }
            else if (e.Node.Level == 2)
            {
                createTime = paths[1];
                schoolName = paths[2];
            }
            else if (e.Node.Level == 3)
            {
                createTime = paths[1];
                schoolName = paths[2];
                groupName = paths[3];
            }
            MainWindowSys.UpdataGroupDataView(createTime, schoolName, groupName, listView1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 导入名单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                if (MainWindowSys.ShowPersonWindow(projectName))
                {
                    MainWindowSys.LoadingProject(uiTreeView1, projectName);
                }
            }
            finally
            {
                this.Show();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 系统参数设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWindowSys.ShowSystemSetting();
            sportProjectInfos = MainWindowSys.LoadingSportProjectInfos();
            if (sportProjectInfos != null)
            {
                uiTitlePanel1.Text = sportProjectInfos.Name;
                projectName = sportProjectInfos.Name;
                MainWindowSys.LoadingProject(uiTreeView1, projectName);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 数据库初始化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 平台设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWindowSys.ShowPlatFormSetting();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
            MainWindowSys.SetExporWindowData(groupName, projectName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton2_Click(object sender, EventArgs e)
        {
            Thread newThread = new Thread(new ParameterizedThreadStart((o) =>
            {
                UpLoadingGradeToServer();
            }));
            newThread.IsBackground = true;
            newThread.Start();
        }

        /// <summary>
        /// 上传成绩
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void UpLoadingGradeToServer()
        {
            if (uiProcessBar1.Enabled)
            {
                if (uiTreeView1.SelectedNode != null)
                {
                    String path = uiTreeView1.SelectedNode.FullPath;
                    string[] fsp = path.Split('\\');
                    string projectName = string.Empty;
                    if (fsp.Length > 0)
                    {
                        projectName = fsp[0];
                    }
                    if (string.IsNullOrEmpty(projectName))
                    {
                        UIMessageBox.ShowError("请先选择上传的成绩项目！！");
                        return;
                    }
                    string outMes = MainWindowSys.UpLoadingGradeToServer(fsp, ref proMax, ref proVal, uiProcessBar1, timer1);
                    var str = outMes.Trim();
                    if (string.IsNullOrEmpty(outMes))
                    {
                        MessageBox.Show("上传成功");
                    }
                    else
                    {
                        MessageBox.Show(outMes);
                    }
                    if (!string.IsNullOrEmpty(projectName))

                        MainWindowSys.UpdataGroupDataView(createTime, schoolName, groupName, listView1);
                }
                else
                {
                    UIMessageBox.ShowError("请先选择项目数据！！");
                    return;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (proVal == 0 || proMax == proVal || proMax == 0)
            {
                return;
            }
            int upV = (int)(((double)proVal / (double)proMax) * 100);
            uiProcessBar1.Value = upV;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(createTime) && !String.IsNullOrEmpty(schoolName))
            {
                this.Hide();
                MainWindowSys.ShowRunningWindow(projectName);
            }
            else
            {
                UIMessageBox.ShowWarning("请选择项目信息");
                return;
            }
        }

        private void 条形码导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
    }
}