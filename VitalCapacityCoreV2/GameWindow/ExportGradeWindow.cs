using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VitalCapacityCoreV2.GameWindowSys;
using VitalCapacityV2.Summer.GameSystem.GameModel;

namespace VitalCapacityCoreV2.GameWindow
{
    public partial class ExportGradeWindow : Form
    {
        public ExportGradeWindow()
        {
            InitializeComponent();
        }

        public string projectName = "";
        public string groupName = "";
        private bool isAllTest = false;
        private bool isOnlyGroup = false;

        private SportProjectInfos SportProjectInfos = null;
        private ExportGradeWindowSys ExportGradeWindowSys = new ExportGradeWindowSys();

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportGradeWindow_Load(object sender, EventArgs e)
        {
            uiCheckBox1.Checked = true;
            if (string.IsNullOrEmpty(groupName))
            {
                uiCheckBox1.Checked = false;
                uiCheckBox2.Visible = false;
                uiTextBox1.Text = "未选择组别";
            }
            else
            {
                uiCheckBox2.Checked = true;
                uiCheckBox2.Visible = true;
                uiTextBox1.Text = groupName;
            }
            SportProjectInfos = ExportGradeWindowSys.LoadingInitData();
        }

        private void uiCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void uiCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (uiCheckBox2.Checked)
            {
                uiButton1.Text = "导出当前组";
            }
            else
            {
                uiButton1.Text = "导出全部成绩";
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            isAllTest = uiCheckBox1.Checked;
            isOnlyGroup = uiCheckBox2.Checked;
            uiButton1.Enabled = false;
            bool result = ExportGradeWindowSys.OutPutScore(SportProjectInfos, isOnlyGroup, groupName, isAllTest);
            if (result)
            {
                UIMessageBox.Show("导出成功");
                uiButton1.Enabled = true;
            }
        }
    }
}