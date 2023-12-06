using Sunny.UI;
using System;
using System.Windows.Forms;
using VitalCapacityCoreV2.GameWindowSys;

namespace VitalCapacityCoreV2.GameWindow
{
    public partial class RunningMachineSettingWindow : UIForm
    {
        public RunningMachineSettingWindow()
        {
            InitializeComponent();
        }

        public string projectName = "    ";
        public int machineCount = 0;
        public string portName = string.Empty;
        private RunningMachineSettingWindowSys RunningMachineSettingWindowSys = new RunningMachineSettingWindowSys();
        private void RunningMachineSettingWindow_Load(object sender, System.EventArgs e)
        {
            this.Text =this.uiTitlePanel1.Text = projectName == null ? "德育龙测试系统" : projectName;
            
            
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            RunningMachineSettingWindowSys.SaveData(uiComboBox2,uiComboBox1 ,ref machineCount,ref portName);
            DialogResult= DialogResult.OK;
            this.Close();
        }
    }
}