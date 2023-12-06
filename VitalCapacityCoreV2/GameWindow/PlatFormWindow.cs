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

namespace VitalCapacityCoreV2.GameWindow
{
    public partial class PlatFormWindow : UIForm
    {
        public PlatFormWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///
        /// </summary>
        private string MachineCode = String.Empty;

        /// <summary>
        ///
        /// </summary>
        private string ExamId = String.Empty;

        /// <summary>
        ///
        /// </summary>
        private string Platform = String.Empty;

        /// <summary>
        ///
        /// </summary>
        private string Platforms = String.Empty;

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, string> localValues = null;

        /// <summary>
        ///
        /// </summary>
        private PlatFormWindowSys PlatFormWindowSys = new PlatFormWindowSys();

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlatFormWindow_Load(object sender, EventArgs e)
        {
            PlatFormWindowSys.LoadingInitData(ref MachineCode, ref ExamId, ref Platforms, ref Platform, uiComboBox1, uiComboBox2, uiComboBox3, ref localValues);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            uiComboBox3.Items.Clear();
            string url = uiComboBox2.Text;
            if (url == String.Empty)
            {
                UIMessageBox.ShowError("网址为空！！");
                return;
            }
            PlatFormWindowSys.GetExamNum(uiComboBox3, url, localValues);
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            uiComboBox1.Items.Clear();
            string examID = uiComboBox3.Text;
            if (string.IsNullOrEmpty(examID))
            {
                UIMessageBox.ShowWarning("考试id为空!");
                return;
            }
            PlatFormWindowSys.GetCode(examID, uiComboBox2, uiComboBox1, localValues);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton3_Click(object sender, EventArgs e)
        {
            if (PlatFormWindowSys.SaveData(uiComboBox1, uiComboBox2, uiComboBox3))
            {
                DialogResult dialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                DialogResult dialog = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}