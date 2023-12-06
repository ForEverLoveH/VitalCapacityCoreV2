using System.Windows.Forms;
using Sunny.UI;
using VitalCapacityCoreV2.GameWindow;

namespace VitalCapacityCoreV2.GameWindowSys
{
    public class RunningMachineSettingWindowSys
    {
        private RunningMachineSettingWindow RunningMachineSettingWindow = null;
        private static int machine = 0;
        private static string protNames = string.Empty;

        public bool ShowRunningMachineSettingWindow(string projects="")
        {
            RunningMachineSettingWindow = new RunningMachineSettingWindow();
            RunningMachineSettingWindow.projectName = projects;
            var dis = RunningMachineSettingWindow.ShowDialog();
            if (dis == DialogResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SaveData(UIComboBox uiComboBox2, UIComboBox uiComboBox1, ref int machineCount, ref string portName)
        {
            int.TryParse(uiComboBox1.Text, out machineCount);
            if (machineCount == 0)
            {
                machineCount = 5;
            }

            portName = uiComboBox2.Text;
            machine = machineCount;
            protNames = portName;
        }

        public int GetMachineCount()
        {
            return machine;
        }

        public string GetPortName()
        {
            return protNames;
        }
    }
}