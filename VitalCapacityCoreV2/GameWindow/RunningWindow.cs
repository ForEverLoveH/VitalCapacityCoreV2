using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.UI.Win32;
using VitalCapacityCoreV2.GameWindowSys;
using VitalCapacityCoreV2.MyControls;
using VitalCapacityV2.Summer.GameSystem;
using VitalCapacityV2.Summer.GameSystem.GameHelper;
using VitalCapacityV2.Summer.GameSystem.GameModel;
using System.Management;
using Sunny.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using Spire.Xls.Core;
using Spire.Pdf.Exporting.XPS.Schema;

namespace VitalCapacityCoreV2.GameWindow
{
    public partial class RunningWindow : UIForm
    {
        public RunningWindow()
        {
            InitializeComponent();
            autoSizeSys = new AutoSizeSys(this.Width, this.Height);
            autoSizeSys.SetTag(this);
        }

        private VitalCapacityV2.Summer.GameSystem.ScanerHook scanerHook = new VitalCapacityV2.Summer.GameSystem.ScanerHook();

        /// <summary>
        ///
        /// </summary>
        private AutoSizeSys autoSizeSys = null;

        private RunningWindowSys RunningWindowSys = new RunningWindowSys();

        /// <summary>
        ///
        /// </summary>
        private NFCHelper NFCHelper = new NFCHelper();

        /// <summary>
        ///
        /// </summary>
        private SportProjectInfos SportProjectInfos { get; set; }

        public string projectName { get; set; }
        private string groupName { get; set; }
        private int currentRound { get; set; }
        private int projectID = 0;

        /// <summary>
        ///
        /// </summary>
        private List<StudentDataModel> studentDataModels = new List<StudentDataModel>();

        /// <summary>
        /// 当前测试考生
        /// </summary>
        private List<StudentDataModel> currentTestingStudents = new List<StudentDataModel>();

        private static List<SelectStudentDataModel> TempstudentDataModels = new List<SelectStudentDataModel>();

        /// <summary>
        ///
        /// </summary>
        private List<UserControl1> userControl1s = new List<UserControl1>();

        /// <summary>
        ///
        /// </summary>
        private SerialReader serialReader = null;

        /// <summary>
        ///
        /// </summary>
        private bool autoMatchFlag = false;

        /// <summary>
        ///
        /// </summary>
        private List<SerialReader> _serialReaders = new List<SerialReader>();

        private List<string> connectionPorts = new List<string>();
        private bool isBeginExam = false;

        /// <summary>
        ///
        /// </summary>
        private Dictionary<string, string> localInfos = new Dictionary<string, string>();

        /// <summary>
        ///
        /// </summary>
        private string portName = "CH340 ";

        private bool isMatchingDevice = false;

        private bool IsReset = false;// 是否存在重置的学生

        /// <summary>
        /// 是否是临时模式
        /// </summary>
        private bool IsTemp = false;

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunningWindow_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.uiTitlePanel1.Text = string.IsNullOrEmpty(projectName) ? "德育龙测试系统" : projectName;
            toolStripStatusLabel1.Text = "程序集版本:" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            LoadingSportsInfos();
            RunningWindowSys.CloseAllSerialPort(userControl1s, _serialReaders, connectionPorts);
            if (!InitControlls())
            {
                this.Close();
            }
            int roundCount = SportProjectInfos.RoundCount;
            if (roundCount > 0)
            {
                for (int i = 0; i < roundCount; i++)
                {
                    uiComboBox2.Items.Add($"第{i + 1}轮");
                }
                if (uiComboBox2.Items.Count > 0)
                    uiComboBox2.SelectedIndex = 0;
            }
            List<LocalInfos> localInfosList = RunningWindowSys.GetLocalInfo();
            foreach (var itsm in localInfosList)
            {
                localInfos.Add(itsm.key, itsm.value);
            }
            NFCHelper.AddUSBEventWatcher(USBEventHandler, USBEventHandler, new TimeSpan(0, 0, 1));
            scanerHook.ScanerEvent += ScanerHook_OnReceiveScanerCodes;
            SetProjectInitData();
        }

        private void SetProjectInitData()
        {
            if (SportProjectInfos != null)
            {
                projectID = SportProjectInfos.Type;
                SetCurrentProjectGroup();
                int round = SportProjectInfos.RoundCount;
                uiComboBox2.Items.Clear();
                for (int i = 0; i < round; i++)
                {
                    uiComboBox2.Items.Add($"第{i + 1}轮");
                }
                if (uiComboBox2.Items.Count > 0) uiComboBox2.SelectedIndex = 0;
            }
            // localInfos = RunningWindowSys.LoadingProjectSettingDatas();
        }

        /// <summary>
        ///
        /// </summary>
        private void SetCurrentProjectGroup()
        {
            List<string> groups = RunningWindowSys.LoadingCurrentProjectGroups(projectID);
            if (groups != null && groups.Count > 0)
            {
                GroupCombox.Items.Clear();
                foreach (string group in groups)
                {
                    GroupCombox.Items.Add(group);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void USBEventHandler(object sender, EventArrivedEventArgs e)
        {
            var watcher = sender as ManagementEventWatcher;
            watcher.Stop();

            if (e.NewEvent.ClassPath.ClassName == "__InstanceCreationEvent")
            {
                Console.WriteLine("设备连接");
                if (isMatchingDevice)
                {
                    ///扫描串口
                    RefreshComPorts();
                }
            }
            else if (e.NewEvent.ClassPath.ClassName == "__InstanceDeletionEvent")
            {
                if (!isMatchingDevice)
                {
                    Console.WriteLine("设备断开");
                    List<string> list = CheckPortISConnected();
                    if (list.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var l in list)
                        {
                            sb.AppendLine($"{l}断开!");
                        }
                        MessageBox.Show(sb.ToString());
                    }
                    //检测断开,断开提示
                    //MessageBox.Show("设备断开请检查");
                }
            }

            watcher.Start();
        }

        private int equipMentCount = 0;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private bool InitControlls()
        {
            if (!RunningWindowSys.ShowRunningMachineWindow(projectName)) return false;
            else
            {
                int mach = RunningWindowSys.GetMachineCount();
                equipMentCount = mach;
                portName = RunningWindowSys.GetPortName();
                EnSureControllsEmpty();
                List<string> list = new List<string>();
                for (int i = 0; i < SportProjectInfos.RoundCount; i++)
                {
                    list.Add($"第{i + 1}轮");
                }
                for (int i = 0; i < mach; i++)
                {
                    UserControl1 user = new UserControl1();
                    user.p_title = $"{i + 1}号设备";
                    user.p_roundCbx_items = list;
                    userControl1s.Add(user);
                    flowLayoutPanel1.Controls.Add(user);
                    SerialReader serialReader = new SerialReader(i);
                    serialReader._AnalyDataCallBack = AnalyData;
                    serialReader._RecieveDataCallBack = ReceieveData;
                    serialReader._SendDataCallBack = SendData;
                    _serialReaders.Add(serialReader);
                }
                return true;
            }
        }

        #region 串口

        /// <summary>
        /// 检查串口是否剖连接
        /// </summary>
        /// <returns></returns>
        private List<string> CheckPortISConnected()
        {
            List<string> ports = new List<string>();
            connectionPorts.Clear();
            int step = 0;
            foreach (var item in _serialReaders)
            {
                step++;
                if (item != null)
                {
                    try
                    {
                        if (item.IsSerialOpen())
                        {
                            connectionPorts.Add(item._serialPort.PortName);
                        }
                        else
                        {
                            ports.Add(item._serialPort.PortName);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
                else
                {
                    ports.Add($"设备{step}号");
                }
            }

            return ports;
        }

        /// <summary>
        ///
        /// </summary>
        private void RefreshComPorts()
        {
            try
            {
                string[] portNames = GetPortDeviceName();
                if (portNames.Length == 0)
                {
                    UIMessageBox.ShowWarning($"未找到{portNames}串口,请检查驱动");
                    MatchBtnSwitch(true);
                    return;
                }
                foreach (var ports in portNames)
                {
                    CheckPortISConnected();
                    //已连接则跳过
                    if (connectionPorts.Contains(ports)) continue;
                    int step = 0;
                    foreach (var sr in _serialReaders)
                    {
                        if (sr != null && !sr.IsSerialOpen())
                        {
                            string strException = string.Empty;
                            int nRet = sr.OpenConnectionSerial(ports, 9600, out strException);
                            if (nRet == 0)
                            {
                                userControl1s[step].p_toolState = $"{ports}已连接";
                                userControl1s[step].p_toolState_color = Color.Green;
                                userControl1s[step].p_title_Color = Color.MediumSpringGreen;
                            }
                            else
                            {
                                UIMessageBox.ShowWarning($"{ports}连接失败\n错误:{strException},请检查");
                            }

                            break;
                        }
                        step++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                CheckPortISConnected();
                int count = connectionPorts.Count;
                if (userControl1s.Count.Equals(count))
                {
                    MatchBtnSwitch(true);
                    string portSavePath = Application.StartupPath + "\\Data\\portSave.log";
                    File.WriteAllLines(portSavePath, connectionPorts);
                    MessageBox.Show("设备串口匹配完成");
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private string[] GetPortDeviceName(string port = "")
        {
            if (string.IsNullOrEmpty(port)) port = portName;
            List<string> strs = new List<string>();
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PnPEntity where Name like '%(COM%'"))
            {
                var hardInfos = searcher.Get();
                foreach (var hardInfo in hardInfos)
                {
                    if (hardInfo.Properties["Name"].Value != null)
                    {
                        string deviceName = hardInfo.Properties["Name"].Value.ToString();
                        if (deviceName.Contains(portName))
                        {
                            int a = deviceName.IndexOf("(COM") + 1;//a会等于1
                            string str = deviceName.Substring(a, deviceName.Length - a);
                            a = str.IndexOf(")");//a会等于1
                            str = str.Substring(0, a);
                            strs.Add(str);
                        }
                    }
                }
            }
            return strs.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        private void MatchBtnSwitch(bool p)
        {
            if (p)
            {
                uiButton6.Text = "匹配设备";
                uiButton6.BackColor = Color.White;
                isMatchingDevice = false;
            }
            else
            {
                uiButton6.Text = "匹配中";
                uiButton6.BackColor = Color.Red;
                isMatchingDevice = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="code"></param>
        private void SendData(MachineMsgCode code)
        {
            Console.WriteLine("发送消息：" + code);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        private void ReceieveData(byte[] data)
        {
            Console.WriteLine("接受消息:" + Encoding.UTF8.GetString(data));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        private void AnalyData(SerialMsg msg)
        {
            if (msg == null) return;
            switch (msg.MsgCode.type)
            {
                case 1:

                    string tem = userControl1s[msg.num].p_toolState;
                    try
                    {
                        string st = connectionPorts[msg.num];
                        userControl1s[msg.num].p_toolState = st + "已连接。登录成功！";
                    }
                    catch (Exception e)
                    {
                        userControl1s[msg.num].p_toolState = tem;
                    }
                    break;

                case 2:
                    if (msg.MsgCode.code == 1)
                    {
                        Console.WriteLine("测试开始！！");
                    }
                    break;

                case 3:
                    GetCurrentStudentScore(msg);
                    break;

                default: break;
            }
        }

        /// <summary>
        ///  是否已经写入
        /// </summary>
        private bool IsWrite = false;

        /// <summary>
        ///
        /// </summary>
        private StringBuilder writeStringBuilder = new StringBuilder();

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        private void GetCurrentStudentScore(SerialMsg msg)
        {
            IsWrite = true;
            double score = 0;
            if (projectName == "握力测试")
                score = msg.MsgCode.wl_result;
            else
                score = msg.MsgCode.fhl_result;
            int index = msg.num;
            if (score > 0)
            {
                string idnum = userControl1s[index].p_IdNumber;
                string stuNum = userControl1s[index].p_Name;
                writeStringBuilder.AppendLine($"考号：{idnum}，姓名：{stuNum}，成绩：{score}");
            }
            userControl1s[index].p_Score = score.ToString();
            if (userControl1s[index].p_stateCbx_selectIndex == 0)
            {
                userControl1s[index].p_stateCbx_selectIndex = 1;
            }
            _serialReaders[index].SendMessage(msg.MsgCode);
        }

        #endregion 串口

        #region hook

        private void StartHookListen()
        {
            if (IsTemp)
            {
                scanerHook.Start();
            }
        }

        private void ScanerHook_OnReceiveScanerCodes(VitalCapacityV2.Summer.GameSystem.ScanerHook.ScanerCodes codes)
        {
            string code = codes.Result;
            if (!string.IsNullOrEmpty(code))
            {
                uiTextBox2.Text = code;
            }
        }

        #endregion hook

        private void EnSureControllsEmpty()
        {
            if (flowLayoutPanel1.Controls.Count > 0)
                flowLayoutPanel1.Controls.Clear();
            if (userControl1s.Count > 0) userControl1s.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        private void LoadingSportsInfos()
        {
            SportProjectInfos = RunningWindowSys.LoadingSportsInfos();
        }

        #region 页面函数

        private void uiButton1_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void uiButton2_Click(object sender, EventArgs e)
        {
            if (!IsTemp)
            {
                AutoMatchStudent();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton3_Click(object sender, EventArgs e)
        {
            if (!IsTemp)
                ChooseMatchStudent();
        }

        /// <summary>
        ///参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void uiButton4_Click(object sender, EventArgs e)
        {
            RunningWindowSys.CloseAllSerialPort(userControl1s, _serialReaders, connectionPorts);
            InitControlls();
        }

        /// <summary>
        /// 导入最近匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton5_Click(object sender, EventArgs e)
        {
            RunningWindowSys.CloseAllSerialPort(userControl1s, _serialReaders, connectionPorts);
            int len = userControl1s.Count;
            for (int i = 0; i < len; i++)
            {
                SerialReader serialReader = new SerialReader(i);
                serialReader._AnalyDataCallBack = AnalyData;
                serialReader._RecieveDataCallBack = ReceieveData;
                serialReader._SendDataCallBack = SendData;
                _serialReaders.Add(serialReader);
            }
            ImportRecentMatchData();
        }

        /// <summary>
        /// 导入最近匹配
        /// </summary>
        private void ImportRecentMatchData()
        {
            string portSavePath = Application.StartupPath + "\\data\\portSave.log";
            if (File.Exists(portSavePath))
            {
                string[] prots = File.ReadAllLines(portSavePath);
                connectionPorts.Clear();
                foreach (var prot in prots)
                {
                    CheckPortISConnected();
                    if (connectionPorts.Contains(prot)) continue;
                    else
                    {
                        int step = 0;
                        foreach (var sr in _serialReaders)
                        {
                            if (sr != null && !sr.IsSerialOpen())
                            {
                                string sy = string.Empty;
                                int ret = sr.OpenConnectionSerial(prot, 9600, out sy);
                                if (ret == 0)
                                {
                                    userControl1s[step].p_toolState = $"{prot}已连接";
                                    userControl1s[step].p_toolState_color = Color.Green;
                                    userControl1s[step].p_title_Color = Color.MediumSpringGreen;
                                    connectionPorts.Add(prot);
                                }
                                else
                                {
                                    UIMessageBox.ShowError($"{prot}连接失败\n错误:{sy},请检查");
                                    return;
                                }
                                break;
                            }
                            step++;
                        }
                    }
                }
            }
            else
            {
                UIMessageBox.ShowError("无法匹配最近的端口数据！！");
                return;
            }
        }

        /// <summary>
        /// 匹配设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void uiButton6_Click(object sender, EventArgs e)
        {
            RunningWindowSys.CloseAllSerialPort(userControl1s, _serialReaders, connectionPorts);
            int len = userControl1s.Count;
            for (int i = 0; i < len; i++)
            {
                //初始化访问读写器实例
                SerialReader reader = new SerialReader(i);
                //回调函数
                reader._AnalyDataCallBack = AnalyData;
                reader._RecieveDataCallBack = ReceieveData;
                reader._SendDataCallBack = SendData;
                _serialReaders.Add(reader);
            }
            MatchBtnSwitch(isMatchingDevice);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiTabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uiTabControl2.SelectedIndex == 1)
            {
                if (UIMessageBox.ShowAsk("当前是否将模式切换到临时分组模式"))
                {
                    IsTemp = true;
                    StartHookListen();
                    RunningWindowSys.LoadingChipInfos(uiComboBox1);
                    timer2.Enabled = true;
                }
            }
            else
            {
                if (UIMessageBox.ShowAsk("当前是否将模式切换到系统分组模式"))
                {
                    IsTemp = false;
                    scanerHook.Stop();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupCombox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string group = GroupCombox.Text.Trim();
            if (!string.IsNullOrEmpty(group))
            {
                groupName = group;
            }
            if (currentRound != -1)
            {
                if (studentDataModels.Count > 0) studentDataModels.Clear();
                studentDataModels = RunningWindowSys.LoadCurreentStudentData(projectID, groupName, currentRound);
                if (studentDataModels != null)
                {
                    ShowCurrentStudentDataInView();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void ShowCurrentStudentDataInView()
        {
            if (uiDataGridView1.Rows.Count > 0) uiDataGridView1.Rows.Clear();
            DataGridViewRow[] rows = new DataGridViewRow[studentDataModels.Count];
            if (studentDataModels.Count > 0)
            {
                int index = 1;
                foreach (var data in studentDataModels)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.Cells.Add(GetNewDataGridViewCell(index, Color.Black, Color.White));
                    row.Cells.Add(GetNewDataGridViewCell(data.GradeName, Color.Black, Color.White));
                    row.Cells.Add(GetNewDataGridViewCell(data.ClassName, Color.Black, Color.White));
                    row.Cells.Add(GetNewDataGridViewCell(data.Name, Color.Black, Color.White));
                    row.Cells.Add(GetNewDataGridViewCell(data.IDNumber, Color.Black, Color.White));
                    row.Cells.Add(GetNewDataGridViewCell(data.Score, data.IsTest == true ? Color.SpringGreen : Color.Red, Color.White));
                    if (data.UpLoadState == 0) row.Cells.Add(GetNewDataGridViewCell("未上传", Color.Red, Color.White));
                    else
                    {
                        row.Cells.Add(GetNewDataGridViewCell("已上传", Color.SpringGreen, Color.White));
                    }
                    row.Cells.Add(GetNewDataGridViewCell(data.Id, Color.Black, Color.White));
                    rows[index - 1] = row;
                    index++;
                }
                uiDataGridView1.Rows.AddRange(rows);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentRound = uiComboBox2.SelectedIndex + 1;
            if (!string.IsNullOrEmpty(groupName))
            {
                UpDataDataGridView1();
            }
        }

        private void UpDataDataGridView1()
        {
            if (studentDataModels.Count > 0) studentDataModels.Clear();
            studentDataModels = RunningWindowSys.LoadCurreentStudentData(projectID, groupName, currentRound);
            if (studentDataModels != null)
            {
                ShowCurrentStudentDataInView();
            }
        }

        /// <summary>
        /// 开始测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton7_Click(object sender, EventArgs e)
        {
            MachineMsgCode mmc = new MachineMsgCode();
            mmc.type = 2;
            StartTesting(mmc);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mmc"></param>
        private void StartTesting(MachineMsgCode mmc)
        {
            List<string> list = CheckPortISConnected();
            if (list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var l in list)
                {
                    sb.AppendLine($"{l}断开!");
                }
                MessageBox.Show(sb.ToString());
                return;
            }
            int step = 0;
            foreach (var sr in _serialReaders)
            {
                if (string.IsNullOrEmpty(userControl1s[step].p_IdNumber) || userControl1s[step].p_IdNumber == "未分配") continue;
                if (sr != null && sr.IsSerialOpen())
                {
                    sr.SendMessage(mmc);
                }
                step++;
            }
        }

        /// <summary>
        /// 保存成绩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton8_Click(object sender, EventArgs e)
        {
            WriteScoreToDataBase();
        }

        private void WriteScoreToDataBase()
        {
            UpDateCurrentDataListScore();
            if (IsTemp == false)
            {
                RunningWindowSys.WriteScoreToDataBase(currentTestingStudents, groupName);
                UpDataDataGridView1();
                if (checkBox1.Checked) AutoMatchStudent();
            }
            else
            {
                RunningWindowSys.WriteScoreToDataBase(currentTestingStudents);
                TempstudentDataModels.Clear();
                currentTestingStudents.Clear();
                uiDataGridView2.Rows.Clear();
                TempstudentDataModels = RunningWindowSys.LoadingStudentChipData(uiComboBox1.Text.Trim());
                List<StudentDataModel> list = new List<StudentDataModel>();
                foreach (var data in TempstudentDataModels)
                {
                    if (data.IsAllTest) continue;
                    else
                    {
                        var round = data.round;

                        list.Add(new StudentDataModel()
                        {
                            IsTest = false,
                            Score = "",
                            Name = data.Name,
                            State = 0,
                            IDNumber = data.idNumber,
                            Round = round,
                            Id = int.Parse(data.Id),
                        });
                    }
                }
                if (list != null)
                {
                    currentTestingStudents = list;
                }
                SetStudentDataInDataView(TempstudentDataModels, uiDataGridView2);
                ClearCurrentUserControll();
            }
            isBeginExam = false;
        }

        /// <summary>
        ///
        /// </summary>
        private void UpDateCurrentDataListScore()
        {
            try
            {
                if (currentTestingStudents.Count > 0)
                {
                    for (int i = 0; i < userControl1s.Count; i++)
                    {
                        string idNumber = userControl1s[i].p_IdNumber;
                        string name = userControl1s[i].p_Name;
                        string score = userControl1s[i].p_Score;
                        var student = currentTestingStudents.Find(a => a.Name.Equals(name) && a.IDNumber.Equals(idNumber));
                        if (student != null) { student.Score = score; student.State = 1; }
                    }
                }
            }
            catch (Exception ex)
            {
                //LoggerHelper.Debug(ex);
                return;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void ClearCurrentUserControll()
        {
            foreach (var user in userControl1s)
            {
                user.p_IdNumber = string.Empty;
                user.p_Name = string.Empty;
                user.p_Score = string.Empty;
                user.p_roundCbx_selectIndex = 0;
                user.p_stateCbx_selectIndex = 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton10_Click(object sender, EventArgs e)
        {
            try
            {
                uiButton10.Text = "上传中";
                uiButton10.ForeColor = Color.Red;
                ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(UpLoadStuGroupScore);
                Thread thread = new Thread(parameterizedThreadStart);
                thread.IsBackground = true;
                thread.Start();
            }
            finally
            {
                uiButton10.Text = "上传本组";
                uiButton10.ForeColor = Color.White;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton11_Click(object sender, EventArgs e)
        {
            string group = GroupCombox.Text;
            new Thread((ThreadStart) =>
            {
                RunningWindowSys.PrintScore(group, SportProjectInfos);
            }).Start();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton12_Click(object sender, EventArgs e)
        {
            ClearMatchControls();
            if (!isBeginExam)
            {
                if (IsMakeGroupSucess) IsMakeGroupSucess = false;
                if (TempstudentDataModels.Count > 0 && TempstudentDataModels.Count > 0)
                {
                    TempstudentDataModels.Clear();
                    SetStudentDataInDataView(TempstudentDataModels, uiDataGridView2);
                }
                string groupName = "DYL__" + DateTime.Now.ToString("yyyy_MMdd HH:ss:MM");
                uiComboBox1.Items.Add(groupName);
                int index = 0;
                for (int i = 0; i < uiComboBox1.Items.Count; i++)
                {
                    if (uiComboBox1.Items[i].ToString() == groupName)
                    {
                        index = i;
                        break;
                    }
                }
                uiComboBox1.SelectedIndex = index;
            }
        }

        private bool IsSucess = false;

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton13_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(uiComboBox1.Text.Trim()))
            {
                SelectStudentData();
            }
            else
            {
                UIMessageBox.ShowWarning("请先确定临时分组信息");
                return;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton14_Click(object sender, EventArgs e)
        {
            if (TempstudentDataModels.Count == 0)
            {
                UIMessageBox.ShowWarning("当前没有考生，无法完成编组,请重试");
                return;
            }
            else
            {
                string name = uiComboBox1.Text;
                if (!string.IsNullOrEmpty(name))
                {
                    IsMakeGroupSucess = true;
                    if (currentTestingStudents.Count > 0) currentTestingStudents.Clear();
                    RunningWindowSys.SaveChipInfosData(TempstudentDataModels, projectID, name);
                    List<StudentDataModel> list = new List<StudentDataModel>();
                    foreach (var data in TempstudentDataModels)
                    {
                        if (data.IsAllTest) continue;
                        else
                        {
                            var round = data.round;

                            list.Add(new StudentDataModel()
                            {
                                IsTest = false,
                                Score = "",
                                Name = data.Name,
                                State = 0,
                                IDNumber = data.idNumber,
                                Round = round,
                                Id = int.Parse(data.Id),
                            });
                        }
                    }
                    if (list != null)
                    {
                        currentTestingStudents = list;
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="foreColor"></param>
        /// <param name="backColor"></param>
        /// <returns></returns>
        private DataGridViewCell GetNewDataGridViewCell(object value, Color foreColor, Color backColor)
        {
            DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
            cell.Value = value.ToString();
            cell.Style.ForeColor = foreColor;
            cell.Style.BackColor = backColor;
            return cell;
        }

        private void UpLoadStuGroupScore(object obj)
        {
            if (RunningWindowSys.UpLoadStuGroupScore(obj))
            {
                UIMessageBox.ShowSuccess("上传成功");
            }
            else
            {
                UIMessageBox.ShowWarning("上传失败");
                return;
            }
        }

        #endregion 页面函数

        /// <summary>
        /// 自动匹配
        /// </summary>
        private void AutoMatchStudent()
        {
            ClearMatchControls();
            if (currentTestingStudents != null) currentTestingStudents.Clear();
            if (studentDataModels.Count > 0)
            {
                var model = studentDataModels.FindAll(A => A.IsTest == false);
                CreateCurrentTestingStudent(model);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        private void CreateCurrentTestingStudent(List<StudentDataModel> model)
        {
            if (model.Count < equipMentCount)
            {
                for (int i = 0; i < equipMentCount; i++)
                {
                    if (i <= model.Count)
                    {
                        currentTestingStudents.Add(new StudentDataModel()
                        {
                            Id = model[i].Id,
                            Name = model[i].Name,
                            IDNumber = model[i].IDNumber,
                            Score = model[i].Score,
                            ClassName = model[i].ClassName,
                            UpLoadState = model[i].UpLoadState,
                            GradeName = model[i].GradeName,
                            Round = model[i].Round,
                            State = 0
                        });
                    }
                    else
                    {
                        currentTestingStudents.Add(new StudentDataModel()
                        {
                            Id = i,
                            Name = "",
                            Score = "",
                            UpLoadState = 0,
                            IDNumber = "",
                            ClassName = "",
                            GradeName = "",
                            Round = 1,
                            State = 0
                        });
                    }
                }
            }
            else
            {
                for (int j = 0; j < equipMentCount; j++)
                {
                    currentTestingStudents.Add(new StudentDataModel()
                    {
                        Id = model[j].Id,
                        Name = model[j].Name,
                        IDNumber = model[j].IDNumber,
                        Score = model[j].Score,
                        UpLoadState = 0,
                        ClassName = model[j].ClassName,
                        GradeName = model[j].GradeName,
                        IsTest = false,
                        Round = model[j].Round,
                    });
                }
            }
            AutoMatchControlls();
        }

        /// <summary>
        ///
        /// </summary>
        private void AutoMatchControlls()
        {
            try
            {
                autoMatchFlag = true;
                for (int i = 0; i < equipMentCount; i++)
                {
                    var sl = currentTestingStudents[i];
                    if (sl.IsTest) continue;
                    userControl1s[i].p_Name = currentTestingStudents[i].Name;
                    userControl1s[i].p_Score = "0";
                    userControl1s[i].p_IdNumber = currentTestingStudents[i].IDNumber;
                    userControl1s[i].p_stateCbx_selectIndex = 0;
                    userControl1s[i].p_roundCbx_selectIndex = sl.Round - 1;
                }
            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void ClearMatchControls()
        {
            currentTestingStudents.Clear();
            for (int i = 0; i < equipMentCount; i++)
            {
                string str = string.Empty;
                userControl1s[i].p_Name = str;
                userControl1s[i].p_Score = str;
                userControl1s[i].p_stateCbx_selectIndex = 0;
                userControl1s[i].p_IdNumber = str;
            }
        }

        /// <summary>
        /// 选择匹配
        /// </summary>
        private void ChooseMatchStudent()
        {
            if (uiDataGridView1.SelectedRows.Count > 0)
            {
                List<StudentDataModel> students = new List<StudentDataModel>();
                for (int j = 0; j < uiDataGridView1.SelectedRows.Count; j++)
                {
                    var name = uiDataGridView1.SelectedRows[j].Cells[3].Value.ToString();
                    var idnum = uiDataGridView1.SelectedRows[j].Cells[4].Value.ToString();
                    if (studentDataModels.Count > 0)
                    {
                        var data = studentDataModels.Find(a => a.Name == name && a.IDNumber.Equals(idnum));
                        if (data != null)
                        {
                            bool isTest = data.IsTest;
                            if (isTest)
                            {
                                UIMessageBox.ShowWarning("当前选中的人中已包含已测试的学生无法开始！！");
                                if (students != null) students.Clear();
                                break;
                            }
                        }
                        else
                        {
                            students.Add(data);
                        }
                    }
                }
                if (students.Count > 0)
                {
                    CreateCurrentTestingStudent(students);
                }
            }
        }

        private void RunningWindow_Resize(object sender, EventArgs e)
        {
            autoSizeSys.ReWinformLayout(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                uiContextMenuStrip1.Show(e.X, e.Y);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 缺考ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetStudentErrorData("缺考");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 中退ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetStudentErrorData("中退");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 犯规ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetStudentErrorData("犯规");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 弃权ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetStudentErrorData("弃权");
        }

        private void SetStudentErrorData(string v)
        {
            if (uiDataGridView2.SelectedRows.Count == 0) return;
            else
            {
                for (int i = 0; i < uiDataGridView2.SelectedRows.Count; i++)
                {
                    string idnumber = uiDataGridView2.SelectedRows[i].Cells[5].Value.ToString();
                    string name = uiDataGridView2.SelectedRows[i].Cells[4].Value.ToString();
                    RunningWindowSys.SetStudentErrorData(projectID.ToString(), idnumber, name, v, currentRound);
                }
                if (studentDataModels.Count > 0) studentDataModels.Clear();
                studentDataModels = RunningWindowSys.LoadCurreentStudentData(projectID, groupName, currentRound);
                if (studentDataModels != null)
                {
                    ShowCurrentStudentDataInView();
                }
            }
        }

        private List<string> ResetStudentExamIDData = new List<string>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 成绩重测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (uiDataGridView2.SelectedRows.Count == 0) return;
                else
                {
                    for (int i = 0; i < uiDataGridView2.SelectedRows.Count; i++)
                    {
                        string idnumber = uiDataGridView2.SelectedRows[i].Cells[5].Value.ToString();
                        ResetStudentExamIDData.Add(idnumber);
                        ChooseRoundWindow fcr = new ChooseRoundWindow();
                        fcr._idNumber = idnumber;
                        fcr.mode = 0;
                        fcr.ShowDialog();
                    }
                    if (studentDataModels.Count > 0) studentDataModels.Clear();
                    studentDataModels = RunningWindowSys.LoadCurreentStudentData(projectID, groupName, currentRound);
                    if (studentDataModels != null)
                    {
                        ShowCurrentStudentDataInView();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修改成绩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* try
             {
                 if (listView1.SelectedItems.Count == 0) return;
                 for (int i = 0; i < listView1.SelectedItems.Count; i++)
                 {
                     string idnumber = listView1.SelectedItems[i].SubItems[3].Text;
                     ChooseRoundWindow fcr = new ChooseRoundWindow();
                     fcr._idNumber = idnumber;
                     fcr.mode = 1;
                     fcr.ShowDialog();
                 }
                 RunningWindowSys.UpDataListView(listView1, GroupCombox, SportProjectInfos);
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);
             }*/
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiDataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                uiContextMenuStrip2.Show(MousePosition);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 清除选中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsMakeGroupSucess)
            {
                if (uiDataGridView1.SelectedRows.Count == 0) return;
                else
                {
                    if (TempstudentDataModels.Count == 0) return;
                    foreach (DataGridViewRow row in uiDataGridView1.SelectedRows)
                    {
                        string idNum = row.Cells[3].Value.ToString();
                        var stu = TempstudentDataModels.Find(a => a.Name.Equals(idNum));
                        if (stu != null)
                        {
                            TempstudentDataModels.Remove(stu);
                        }
                    }
                    // ShowStudentDataInDatGrid();
                    SetStudentDataInDataView(TempstudentDataModels, uiDataGridView2);
                }
            }
            else
            {
                UIMessageBox.ShowWarning("当前数据已保存，无法删除！！");
                return;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 清除本组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsMakeGroupSucess)
            {
                if (TempstudentDataModels.Count == 0) return;
                TempstudentDataModels.Clear();
                SetStudentDataInDataView(TempstudentDataModels, uiDataGridView2);
            }
            else
            {
                UIMessageBox.ShowWarning("当前数据已保存，无法删除！！");
                return;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (writeStringBuilder.Length > 0)
            {
                string path = "成绩日志.txt";
                if (!File.Exists(path)) File.Create(path);
                File.AppendAllText(path, writeStringBuilder.ToString());
                writeStringBuilder.Clear();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uiDataGridView2.Rows.Count > 0) uiDataGridView2.Rows.Clear();
            string sl = uiComboBox1.Text.Trim();
            if (!string.IsNullOrEmpty(sl))
            {
                TempstudentDataModels = RunningWindowSys.LoadingStudentChipData(sl);
                if (TempstudentDataModels != null && TempstudentDataModels.Count > 0)
                {
                    IsMakeGroupSucess = true;
                    uiButton14.Enabled = false;
                    SetStudentDataInDataView(TempstudentDataModels, uiDataGridView2);
                    if (TempstudentDataModels.Count > 0)
                    {
                        List<StudentDataModel> list = new List<StudentDataModel>();
                        foreach (var data in TempstudentDataModels)
                        {
                            if (data.IsAllTest) continue;
                            else
                            {
                                var round = data.round;

                                list.Add(new StudentDataModel()
                                {
                                    IsTest = false,
                                    Score = "",
                                    Name = data.Name,
                                    State = 0,
                                    IDNumber = data.idNumber,
                                    Round = round,
                                    Id = int.Parse(data.Id),
                                });
                            }
                        }
                        if (list != null)
                        {
                            currentTestingStudents = list;
                        }
                    }
                }
                else
                {
                    uiButton14.Enabled = true;
                    IsMakeGroupSucess = false;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="studentDataModels"></param>
        /// <param name="uiDataGridView2"></param>
        public void SetStudentDataInDataView(List<SelectStudentDataModel> studentDataModels, UIDataGridView uiDataGridView1)
        {
            if (studentDataModels != null && studentDataModels.Count > 0)
            {
                try
                {
                    uiDataGridView1.Rows.Clear();
                    int index = 1;
                    List<DataGridViewRow> rows = new List<DataGridViewRow>();
                    foreach (SelectStudentDataModel model in studentDataModels)
                    {
                        DataGridViewRow data = new DataGridViewRow();
                        data.Cells.Add(GetNewDataGridViewCell(index, Color.Black, Color.White));
                        data.Cells.Add(GetNewDataGridViewCell(model.groupName, Color.Black, Color.White));
                        data.Cells.Add(GetNewDataGridViewCell(model.Name, Color.Black, Color.White));
                        data.Cells.Add(GetNewDataGridViewCell(model.Sex, Color.Black, Color.White));
                        data.Cells.Add(GetNewDataGridViewCell(model.idNumber, Color.Black, Color.White));
                        data.Cells.Add(GetNewDataGridViewCell(model.score, model.score == "未测试" ? Color.Red : Color.Black, Color.White));
                        data.Cells.Add(GetNewDataGridViewCell(model.score1, model.score1 == "未测试" ? Color.Red : Color.Black, Color.White));
                        rows.Add(data);
                        index++;
                    }
                    if (rows.Count > 0)
                    {
                        DataGridViewRow[] dats = new DataGridViewRow[rows.Count];
                        for (int i = 0; i < rows.Count; i++)
                        {
                            dats[i] = rows[i];
                        }
                        uiDataGridView1.Rows.Clear();
                        uiDataGridView1.AllowUserToAddRows = false;
                        uiDataGridView1.Rows.AddRange(dats);
                    }
                }
                catch (Exception ex)
                {
                    /// LoggerHelper.Debug(ex);
                    return;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiTextBox2_TextChanged(object sender, EventArgs e)
        {
            var str = uiTextBox2.Text;
            if (!string.IsNullOrEmpty(str))
            {
                if (!string.IsNullOrEmpty(uiComboBox1.Text.Trim()))
                {
                    SelectStudentData();
                }
                else
                {
                    UIMessageBox.ShowWarning("请先确定临时分组信息");
                    return;
                }
            }
        }

        private bool IsMakeGroupSucess = false;

        /// <summary>
        ///
        /// </summary>
        private void SelectStudentData()
        {
            if (uiComboBox1.Text != null)
            {
                string IDNum = uiTextBox2.Text.Trim();
                if (!string.IsNullOrEmpty(IDNum))
                {
                    if (!IsMakeGroupSucess)
                    {
                        SelectStudentDataModel studentDataModel = RunningWindowSys.SelectStudentDataByIDNumber(projectID, IDNum);
                        if (studentDataModel != null && TempstudentDataModels.Count > 0)
                        {
                            var data = TempstudentDataModels.Find(a => a.Name == studentDataModel.Name && a.idNumber.Equals(studentDataModel.idNumber));
                            if (data != null)
                            {
                                UIMessageBox.ShowWarning("已添加");
                                return;
                            }
                        }
                        TempstudentDataModels.Add(studentDataModel);
                        SetStudentDataInDataView(TempstudentDataModels, uiDataGridView2);
                        uiTextBox2.Text = null;
                        var score = studentDataModel.score;
                        var score1 = studentDataModel.score1;
                        if (score != "未测试")
                        {
                            if (score1 != "未测试")
                            {
                                UIMessageBox.ShowWarning("当前考生以全部测试完成！！");
                                studentDataModel.IsAllTest = true;
                                studentDataModel.round = -1;
                            }
                            else
                            {
                                studentDataModel.IsAllTest = false;
                                studentDataModel.round = 2;
                                SpeekHelper.GetInstance().AddDataToQueue($"请{studentDataModel.Name}做好第2轮测试准备");
                            }
                        }
                        else
                        {
                            studentDataModel.IsAllTest = false;
                            studentDataModel.round = 1;
                            SpeekHelper.GetInstance().AddDataToQueue($"请{studentDataModel.Name}做好第1轮测试准备");
                        }
                    }
                    else
                    {
                        UIMessageBox.ShowWarning("未查找到对应的学生数据！！");
                        return;
                    }
                    if (uiButton14.Enabled == false)
                    {
                        uiButton14.Enabled = true;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private void uiButton9_Click(object sender, EventArgs e)
        {
            if (currentTestingStudents.Count > 0)
            {
                if (currentTestingStudents.Count <= equipMentCount)
                {
                    for (int i = currentTestingStudents.Count; i < equipMentCount; i++)
                    {
                        currentTestingStudents.Add(new StudentDataModel()
                        {
                            Id = i,
                            Name = "",
                            IDNumber = "",
                            State = 0,
                            Score = "",
                            IsTest = false,
                            Round = 0,
                        });
                    }
                }

                AutoMatchControlls();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (TempstudentDataModels.Count > 0)
            {
                SetStudentDataInDataView(TempstudentDataModels, uiDataGridView2);
            }
        }
    }
}