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
    public partial class SystemSettingWindow : Form
    {
        public SystemSettingWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///
        /// </summary>
        private SportProjectInfos sportProjectInfos = null;

        private SystemSettingWindowSys SystemSettingWindowSys = new SystemSettingWindowSys();

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemSettingWindow_Load(object sender, EventArgs e)
        {
            sportProjectInfos = SystemSettingWindowSys.LoadingInitData();
            if (sportProjectInfos != null)
            {
                string name = sportProjectInfos.Name;
                int round = sportProjectInfos.RoundCount;
                int BestScoreMode = sportProjectInfos.BestScoreMode;
                int TestMethod = sportProjectInfos.TestMethod;
                int FloatType = sportProjectInfos.FloatType;
                int index = 0;
                if (uiComboBox5.Items.Count > 0)
                {
                    for (int i = 0; i < uiComboBox5.Items.Count; i++)
                    {
                        if (uiComboBox5.Items[i].ToString() == name)
                        {
                            index = i;
                        }
                    }
                    uiComboBox5.SelectedIndex = index;
                    uiComboBox1.SelectedIndex = round;
                    uiComboBox2.SelectedIndex = BestScoreMode;
                    uiComboBox3.SelectedIndex = TestMethod;
                    uiComboBox4.SelectedIndex = FloatType;
                }
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (SystemSettingWindowSys.SaveSportProjectsSetting(ProjectName, RoundCount, BestMethod, TestMethod, FloatType, sportProjectInfos))
            {
                UIMessageBox.ShowSuccess("保存成功！！");
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private string ProjectName = "";

        /// <summary>
        ///
        /// </summary>
        private int RoundCount = 0;

        /// <summary>
        ///
        /// </summary>
        private int BestMethod = 0;

        /// <summary>
        ///
        /// </summary>
        private int TestMethod = 0;

        /// <summary>
        ///
        /// </summary>
        private int FloatType = 0;

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiComboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProjectName = uiComboBox5.Text.Trim();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RoundCount = int.Parse(uiComboBox1.Text.Trim());
        }

        private void uiComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            BestMethod = uiComboBox2.SelectedIndex;
        }

        private void uiComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            TestMethod = uiComboBox3.SelectedIndex;
        }

        private void uiComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            FloatType = uiComboBox4.SelectedIndex;
        }
    }
}