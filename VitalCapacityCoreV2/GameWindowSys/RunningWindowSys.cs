using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MiniExcelLibs;
using Newtonsoft.Json;
using Spire.Xls;
using Sunny.UI;
using Sunny.UI.Win32;
using VitalCapacityCoreV2.GameWindow;
using VitalCapacityCoreV2.MyControls;
using VitalCapacityV2.Summer.GameSystem;
using VitalCapacityV2.Summer.GameSystem.FreeSqlHelper;
using VitalCapacityV2.Summer.GameSystem.GameHelper;
using VitalCapacityV2.Summer.GameSystem.GameModel;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ListView = System.Windows.Forms.ListView;

namespace VitalCapacityCoreV2.GameWindowSys
{
    public class RunningWindowSys
    {
        private RunningWindow runningWindow;
        private readonly IFreeSql freeSql = FreeSqlHelper.Sqlite;
        private RunningMachineSettingWindowSys RunningMachineSettingWindowSys = new RunningMachineSettingWindowSys();

        /// <summary>
        ///
        /// </summary>
        /// <param name="project"></param>
        /// <param name="createTime"></param>
        /// <param name="schoolName"></param>
        public void ShowRunningWindow(string project)
        {
            runningWindow = new RunningWindow();
            runningWindow.projectName = project;

            runningWindow.Show();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public SportProjectInfos LoadingSportsInfos()
        {
            return freeSql.Select<SportProjectInfos>().ToOne();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userControl1S"></param>
        /// <param name="userControl2S"></param>
        /// <param name="serialReaders"></param>
        /// <param name="connectionPorts"></param>
        public void CloseAllSerialPort(List<UserControl1> userControl1S, List<SerialReader> serialReaders, List<string> connectionPorts)
        {
            if (serialReaders.Count > 0)
            {
                foreach (var item in serialReaders)
                {
                    if (item != null)
                    {
                        item.CloseConnectionSerial();
                    }
                }
                serialReaders.Clear();
                connectionPorts.Clear();
                if (userControl1S.Count > 0)
                {
                    foreach (var item in userControl1S)
                    {
                        item.p_toolState = "设备未连接   ";
                        item.p_toolState_color = Color.Red;
                        item.p_title_Color = System.Drawing.SystemColors.ControlLight;
                    }
                }

                GC.Collect();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool ShowRunningMachineWindow(string projects = "")
        {
            return RunningMachineSettingWindowSys.ShowRunningMachineSettingWindow(projects);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public int GetMachineCount()
        {
            return RunningMachineSettingWindowSys.GetMachineCount();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string GetPortName()
        {
            return RunningMachineSettingWindowSys.GetPortName();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<LocalInfos> GetLocalInfo()
        {
            return freeSql.Select<LocalInfos>().ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listView1"></param>
        /// <param name="roundCount"></param>
        public void InitListViewHeader(ListView listView1, int roundCount)
        {
            listView1.View = View.Details;
            ColumnHeader[] Header = new ColumnHeader[100];
            int sp = 0;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "序号";
            Header[sp].Width = 50;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "学校";
            Header[sp].Width = 200;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "组号";
            Header[sp].Width = 100;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "准考证号";
            Header[sp].Width = 160;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "姓名";
            Header[sp].Width = 130;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "最好成绩";
            Header[sp].Width = 120;
            sp++;
            for (int i = 1; i <= roundCount; i++)
            {
                Header[sp] = new ColumnHeader();
                Header[sp].Text = $"第{i}轮";
                Header[sp].Width = 120;
                sp++;

                Header[sp] = new ColumnHeader();
                Header[sp].Text = "上传状态";
                Header[sp].Width = 120;
                sp++;
            }

            ColumnHeader[] Header1 = new ColumnHeader[sp];
            listView1.Columns.Clear();
            for (int i = 0; i < Header1.Length; i++)
            {
                Header1[i] = Header[i];
            }
            listView1.Columns.AddRange(Header1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="creatime"></param>
        /// <param name="school"></param>
        /// <param name="uiComboBox"></param>
        public void UpDataGroupData(string creatime, string school, UIComboBox uiComboBox)
        {
            try
            {
                var ls = freeSql.Select<DbPersonInfos>().Where(a => a.CreateTime == creatime && a.SchoolName == school).ToList();
                if (ls.Count > 0)
                {
                    uiComboBox.Items.Clear();
                    foreach (var po in ls)
                    {
                        if (uiComboBox.Items.Contains(po.GroupName)) continue;
                        else
                        {
                            uiComboBox.Items.Add(po.GroupName);
                        }
                    }
                    if (uiComboBox.Items.Count > 0) { uiComboBox.SelectedIndex = 0; }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                uiComboBox.Items.Clear();
            }
        }

        private string AutoMatchLog = Application.StartupPath + "\\Data\\AutoMatchLog.log";
        private string AutoUploadLog = Application.StartupPath + "\\Data\\AutoUploadLog.log";
        private string AutoPrintLog = Application.StartupPath + "\\Data\\AutoPrintLog.log";

        /// <summary>
        ///
        /// </summary>
        /// <param name="checkBox1"></param>
        /// <param name="checkBox2"></param>
        /// <param name="checkBox3"></param>
        public void SetRunningWindowCheck(CheckBox checkBox1, CheckBox checkBox2, CheckBox checkBox3)
        {
            try
            {
                string[] strg = File.ReadAllLines(AutoMatchLog);
                if (strg.Length > 0)
                {
                    if (strg[0] == "1")
                    {
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                    }
                }
                strg = File.ReadAllLines(AutoUploadLog);
                if (strg.Length > 0)
                {
                    if (strg[0] == "1")
                    {
                        checkBox2.Checked = true;
                    }
                    else
                    {
                        checkBox2.Checked = false;
                    }
                }

                strg = File.ReadAllLines(AutoPrintLog);
                if (strg.Length > 0)
                {
                    if (strg[0] == "1")
                    {
                        checkBox3.Checked = true;
                    }
                    else
                    {
                        checkBox3.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listView1"></param>
        /// <param name="groupsCbx"></param>
        /// <param name="sportProjectInfos"></param>
        /// <returns></returns>
        public List<DbPersonInfos> UpDataListView(ListView listView1, UIComboBox groupsCbx, SportProjectInfos sportProjectInfos)
        {
            List<DbPersonInfos> dbPersonInfos = new List<DbPersonInfos>();
            try
            {
                listView1.Items.Clear();
                int index = groupsCbx.SelectedIndex;
                string groupName = groupsCbx.Text;
                dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == groupName).ToList();
                if (dbPersonInfos.Count == 0) return dbPersonInfos;
                int step = 1;
                listView1.BeginUpdate();
                Font f = new Font(Control.DefaultFont, FontStyle.Bold);
                bool isBestScore = sportProjectInfos.BestScoreMode == 0 ? true : false;
                foreach (var dbPersonInfo in dbPersonInfos)
                {
                    ListViewItem li = new ListViewItem();
                    li.UseItemStyleForSubItems = false;
                    li.Text = step.ToString();
                    li.SubItems.Add(dbPersonInfo.SchoolName);
                    li.SubItems.Add(dbPersonInfo.GroupName);
                    li.SubItems.Add(dbPersonInfo.IdNumber);
                    li.SubItems.Add(dbPersonInfo.Name);
                    li.SubItems.Add("未测试");
                    List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>()
                        .Where(a => a.PersonId == dbPersonInfo.Id.ToString() && a.IsRemoved == 0)
                        .OrderBy(a => a.Id)
                        .ToList();
                    int resultRound = 0;
                    double MaxScore = 99999;
                    if (isBestScore) MaxScore = 0;
                    bool getScore = false;
                    foreach (var resultInfo in resultInfos)
                    {
                        if (resultInfo.State != 1)
                        {
                            string s_rstate = ResultStateType.Match(resultInfo.State);
                            li.SubItems.Add(s_rstate);
                            li.SubItems[li.SubItems.Count - 1].ForeColor = Color.Red;
                        }
                        else
                        {
                            getScore = true;
                            li.SubItems.Add(resultInfo.Result.ToString());
                            li.SubItems[li.SubItems.Count - 1].BackColor = Color.MediumSpringGreen;
                            if (isBestScore)
                            {
                                //取最大值
                                if (MaxScore < resultInfo.Result) MaxScore = resultInfo.Result;
                            }
                            else
                            {
                                //取最小值
                                if (MaxScore > resultInfo.Result) MaxScore = resultInfo.Result;
                            }
                        }
                        li.SubItems[li.SubItems.Count - 1].Font = f;
                        if (resultInfo.uploadState == 0)
                        {
                            li.SubItems.Add("未上传");
                            li.SubItems[li.SubItems.Count - 1].ForeColor = Color.Red;
                        }
                        else if (resultInfo.uploadState == 1)
                        {
                            li.SubItems.Add("已上传");
                            li.SubItems[li.SubItems.Count - 1].ForeColor = Color.MediumSpringGreen;
                            li.SubItems[li.SubItems.Count - 1].Font = f;
                        }
                        resultRound++;
                    }
                    for (int i = resultRound; i < sportProjectInfos.RoundCount; i++)
                    {
                        li.SubItems.Add("未测试");
                        li.SubItems.Add("未上传");
                    }
                    if (getScore)
                    { li.SubItems[5].Text = MaxScore.ToString(); }
                    step++;
                    listView1.Items.Insert(listView1.Items.Count, li);
                }
                ListViewHelper.AutoResizeColumnWidth(listView1);
                listView1.EndUpdate();
            }
            catch (Exception ex)
            {
                listView1.Items.Clear();
                dbPersonInfos.Clear();
                // LoggerHelper.Debug(ex);
            }
            return dbPersonInfos;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userControl1S"></param>
        public void ClearMatchStudent(List<UserControl1> userControl1S)
        {
            int len = userControl1S.Count;
            for (int i = 0; i < len; i++)
            {
                userControl1S[i].p_IdNumber = "未分配";
                userControl1S[i].p_Name = "未分配";
                userControl1S[i].p_Score = "0";
                userControl1S[i].p_roundCbx_selectIndex = 0;
                userControl1S[i].p_stateCbx_selectIndex = 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dpi"></param>
        /// <param name="nowRound"></param>
        /// <returns></returns>
        public List<ResultInfos> GetResultInfos(DbPersonInfos dpi, int nowRound)
        {
            return freeSql.Select<ResultInfos>().Where(a => a.PersonId == dpi.Id.ToString() && a.IsRemoved == 0 && a.RoundId == nowRound + 1).OrderBy(a => a.Id).ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ResultInfos> GetResultInfos(string id)
        {
            return freeSql.Select<ResultInfos>()
                .Where(a => a.PersonIdNumber == id && a.IsRemoved == 0).OrderBy(a => a.Id).ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<DbPersonInfos> GetPersonInfo(string text)
        {
            return freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == text).ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userControl1s"></param>
        /// <param name="groupCombox"></param>
        /// <param name="sportProjectInfos"></param>
        /// <param name="listView1"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void WriteScoreIntoDb(List<UserControl1> userControl1s, UIComboBox groupCombox, SportProjectInfos sportProjectInfos, ListView listView1)
        {
            try
            {
                freeSql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
                List<ResultInfos> insertResults = new List<ResultInfos>();
                List<DbPersonInfos> dbPersonInfos =
                    freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == groupCombox.Text).ToList();
                StringBuilder errorsb = new StringBuilder();
                foreach (var user in userControl1s)
                {
                    if (string.IsNullOrEmpty(user.p_IdNumber) || user.p_IdNumber == "未分配") continue;
                    string idNumber = user.p_IdNumber;
                    int state = user.p_stateCbx_selectIndex;
                    int roundId = user.p_roundCbx_selectIndex;
                    double.TryParse(user.p_Score, out double score);
                    DbPersonInfos df = dbPersonInfos.Find(a => a.IdNumber == idNumber);
                    if (state == 0)
                    {
                        errorsb.AppendLine($"{df.IdNumber},{df.Name}未完成测试!!!");
                        continue;
                    }
                    bool t_flag = false;
                    //检查轮次
                    for (int i = roundId; i < sportProjectInfos.RoundCount; i++)
                    {
                        List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>().Where(a => a.PersonIdNumber == idNumber && a.IsRemoved == 0 && a.RoundId == i + 1).OrderBy(a => a.Id).ToList();
                        if (resultInfos.Count == 0)
                        {
                            t_flag = true;
                            maxSortId++;
                            ResultInfos rinfo = new ResultInfos();
                            rinfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            rinfo.SortId = maxSortId;
                            rinfo.IsRemoved = 0;
                            rinfo.PersonId = df.Id.ToString();
                            rinfo.SportItemType = 0;
                            rinfo.PersonName = df.Name;
                            rinfo.PersonIdNumber = df.IdNumber;
                            rinfo.RoundId = i + 1;
                            rinfo.Result = score;
                            rinfo.State = state;
                            insertResults.Add(rinfo);
                            break;
                        }
                    }
                    if (!t_flag)
                    {
                        errorsb.AppendLine($"{df.IdNumber},{df.Name}轮次已满");
                    }
                }
                int result = freeSql.InsertOrUpdate<ResultInfos>().SetSource(insertResults).IfExistsDoNothing().ExecuteAffrows();
                if (errorsb.Length != 0) MessageBox.Show(errorsb.ToString());
                if (result > 0) UpDataListView(listView1, groupCombox, sportProjectInfos);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        public bool UpLoadStuGroupScore(object obj)
        {
            try
            {
                List<Dictionary<string, string>> successList = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> errorList = new List<Dictionary<string, string>>();
                Dictionary<string, string> localInfos = new Dictionary<string, string>();
                List<LocalInfos> list0 = freeSql.Select<LocalInfos>().ToList();
                string cpuid = CPUHelper.GetCpuID();
                foreach (var item in list0)
                {
                    localInfos.Add(item.key, item.value);
                } //组
                string groupName = obj as string;
                SportProjectInfos sportProjectInfos = freeSql.Select<SportProjectInfos>().ToOne();
                List<DbGroupInfos> dbGroupInfos = new List<DbGroupInfos>();
                ///查询本项目已考组
                if (!string.IsNullOrEmpty(groupName))
                {
                    //sql0 += $" AND Name = '{groupName}'";
                    dbGroupInfos = freeSql.Select<DbGroupInfos>().Where(a => a.Name == groupName).ToList();
                }

                UploadResultsRequestParameter urrp = new UploadResultsRequestParameter();
                urrp.AdminUserName = localInfos["AdminUserName"];
                urrp.TestManUserName = localInfos["TestManUserName"];
                urrp.TestManPassword = localInfos["TestManPassword"];
                string MachineCode = localInfos["MachineCode"];
                string ExamId = localInfos["ExamId"];
                if (MachineCode.IndexOf('_') != -1)
                {
                    MachineCode = MachineCode.Substring(MachineCode.IndexOf('_') + 1);
                }

                if (ExamId.IndexOf('_') != -1)
                {
                    ExamId = ExamId.Substring(ExamId.IndexOf('_') + 1);
                }

                urrp.MachineCode = MachineCode;
                urrp.ExamId = ExamId;
                StringBuilder messageSb = new StringBuilder();
                StringBuilder logWirte = new StringBuilder();

                ///按组上传
                foreach (var gInfo in dbGroupInfos)
                {
                    List<DbPersonInfos> dbPersonInfos =
                        freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == gInfo.Name).ToList();
                    StringBuilder resultSb = new StringBuilder();
                    List<SudentsItem> sudentsItems = new List<SudentsItem>();
                    //IdNumber 对应Id
                    Dictionary<string, string> map = new Dictionary<string, string>();
                    //取值模式
                    bool isBestScore = sportProjectInfos.BestScoreMode == 0;
                    foreach (var stu in dbPersonInfos)
                    {
                        List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>()
                            .Where(a => a.PersonIdNumber == stu.IdNumber).ToList();
                        //无成绩的跳过
                        if (resultInfos.Count == 0) continue;
                        int state = -1;
                        string examTime = string.Empty;
                        double MaxScore = 99999;
                        if (isBestScore) MaxScore = 0;
                        foreach (var ri in resultInfos)
                        {
                            if (ri.State <= 0) continue;
                            ///异常状态
                            if (ri.State != 1)
                            {
                                if (isBestScore && MaxScore < 0)
                                {
                                    //取最大值
                                    MaxScore = 0;
                                    state = ri.State;
                                }
                                else if (!isBestScore && MaxScore > 99999)
                                {
                                    //取最小值
                                    MaxScore = 99999;
                                    state = ri.State;
                                }
                            }
                            else
                            {
                                if (isBestScore && MaxScore < ri.Result)
                                {
                                    //取最大值
                                    MaxScore = ri.Result;
                                    state = ri.State;
                                }
                                else if (!isBestScore && MaxScore > ri.Result)
                                {
                                    //取最小值
                                    MaxScore = ri.Result;
                                    state = ri.State;
                                }
                            }

                            examTime = ri.CreateTime;
                        }

                        if (state < 0) continue;
                        if (state != 1)
                        {
                            MaxScore = 0;
                        }
                        List<RoundsItem> roundsItems = new List<RoundsItem>();
                        RoundsItem rdi = new RoundsItem();
                        rdi.RoundId = 1;
                        rdi.State = ResultStateType.Match(state);
                        rdi.Time = examTime;
                        StringBuilder logSb = new StringBuilder();
                        List<LogInfos> logInfos = freeSql.Select<LogInfos>()
                                .Where(a => a.IdNumber == stu.IdNumber && a.State != -404)
                                .ToList();
                        logInfos.ForEach(item =>
                        {
                            string sbtxt = $"时间：{item.CreateTime},考号:{item.IdNumber},{item.Remark};";
                            logSb.Append(sbtxt);
                        });
                        rdi.Memo = logSb.ToString();
                        rdi.Ip = cpuid;
                        ///可以处理成绩
                        rdi.Result = MaxScore;

                        #region 查询文件

                        //成绩根目录
                        Dictionary<string, string> dic_images = new Dictionary<string, string>();
                        Dictionary<string, string> dic_viedos = new Dictionary<string, string>();
                        Dictionary<string, string> dic_texts = new Dictionary<string, string>();
                        string scoreRoot = Application.StartupPath +
                                           $"\\Scores\\{sportProjectInfos.Name}\\{stu.GroupName}\\";
                        DateTime.TryParse(examTime, out DateTime examTime_dt);
                        string dateStr = examTime_dt.ToString("yyyyMMdd");
                        string GroupNo = $"{dateStr}_{stu.GroupName}_{stu.IdNumber}_1";
                        if (Directory.Exists(scoreRoot))
                        {
                            List<DirectoryInfo> rootDirs = new DirectoryInfo(scoreRoot).GetDirectories().ToList();
                            string dirEndWith = $"_{stu.IdNumber}_{stu.Name}";
                            DirectoryInfo directoryInfo = rootDirs.Find(a => a.Name.EndsWith(dirEndWith));
                            if (directoryInfo != null)
                            {
                                string stuDir = Path.Combine(scoreRoot, directoryInfo.Name);
                                GroupNo = $"{dateStr}_{stu.GroupName}_{directoryInfo.Name}_1";
                                if (Directory.Exists(stuDir))
                                {
                                    int step = 1;
                                    FileInfo[] files = new DirectoryInfo(stuDir).GetFiles("*.jpg");
                                    if (files.Length > 0)
                                    {
                                        foreach (var item in files)
                                        {
                                            dic_images.Add(step + "", item.Name);
                                            step++;
                                        }
                                    }
                                    step = 1;
                                    files = new DirectoryInfo(stuDir).GetFiles("*.txt");
                                    if (files.Length > 0)
                                    {
                                        foreach (var item in files)
                                        {
                                            dic_texts.Add(step + "", item.Name);
                                            step++;
                                        }
                                    }
                                    step = 1;
                                    files = new DirectoryInfo(stuDir).GetFiles("*.mp4");
                                    if (files.Length > 0)
                                    {
                                        foreach (var item in files)
                                        {
                                            dic_viedos.Add(step + "", item.Name);
                                            step++;
                                        }
                                    }
                                }
                            }
                        }

                        #endregion 查询文件

                        rdi.GroupNo = GroupNo;
                        rdi.Text = dic_texts;
                        rdi.Images = dic_images;
                        rdi.Videos = dic_viedos;
                        roundsItems.Add(rdi);
                        SudentsItem ssi = new SudentsItem();
                        ssi.SchoolName = stu.SchoolName;
                        ssi.GradeName = stu.GradeName;
                        ssi.ClassNumber = stu.ClassNumber;
                        ssi.Name = stu.Name;
                        ssi.IdNumber = stu.IdNumber;
                        ssi.Rounds = roundsItems;
                        sudentsItems.Add(ssi);
                        map.Add(stu.IdNumber, stu.Id.ToString());
                    }
                    if (sudentsItems.Count == 0) continue;
                    urrp.Sudents = sudentsItems;
                    //序列化json
                    string JsonStr = JsonConvert.SerializeObject(urrp);
                    string url = localInfos["Platform"] + RequestUrl.UploadResults;
                    var httpUpload = new HttpUpload();
                    var formDatas = new List<FormItemModel>();
                    //添加其他字段
                    formDatas.Add(new FormItemModel()
                    {
                        Key = "data",
                        Value = JsonStr
                    });
                    logWirte.AppendLine();
                    logWirte.AppendLine();
                    logWirte.AppendLine(JsonStr);
                    //上传学生成绩
                    string result = HttpUpload.GetInstance().PostForm(url, formDatas);
                    upload_Result upload_Result = JsonConvert.DeserializeObject<upload_Result>(result);
                    string errorStr = "null";
                    List<Dictionary<string, int>> result1 = upload_Result.Result;
                    foreach (var item in sudentsItems)
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        //map
                        dic.Add("Id", map[item.IdNumber]);
                        dic.Add("IdNumber", item.IdNumber);
                        dic.Add("Name", item.Name);
                        dic.Add("uploadGroup", item.Rounds[0].GroupNo);
                        var value = 0;
                        result1.Find(a => a.TryGetValue(item.IdNumber, out value));
                        if (value == 1 || value == -4)
                        {
                            successList.Add(dic);
                        }
                        else if (value != 0)
                        {
                            errorStr = uploadResult.Match(value);
                            dic.Add("error", errorStr);
                            errorList.Add(dic);
                            messageSb.AppendLine(
                                $"{gInfo.Name}组 考号:{item.IdNumber} 姓名:{item.Name}上传失败,错误内容:{errorStr}");
                        }
                    }
                    //写入失败log
                    WriteLogWithFauilt(errorList, gInfo);
                }
                //写入成功写入日志
                WriteLogWriteSucess(successList);
                //LoggerHelper.Monitor(logWirte.ToString());
                string outpitMessage = messageSb.ToString();
                return true;
            }
            catch (Exception exception)
            {
                //LoggerHelper.Debug(exception);
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="successList"></param>
        private void WriteLogWriteSucess(List<Dictionary<string, string>> successList)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"成功:{successList.Count}");
            sb.AppendLine("******************success*********************");
            foreach (var item in successList)
            {
                freeSql.Update<DbPersonInfos>()
                    .Set(a => a.uploadGroup == "1")
                    .Where(a => a.Id == Convert.ToInt32(item["Id"]))
                    .ExecuteAffrows();
                freeSql.Update<ResultInfos>()
                    .Set(a => a.uploadState == 1)
                    .Where(a => a.PersonId == item["Id"])
                    .ExecuteAffrows();
                ;
                sb.AppendLine($"考号:{item["IdNumber"]} 姓名:{item["Name"]}");
            }
            sb.AppendLine("*******************success********************");
            if (successList.Count != 0)
            {
                string txtpath = Application.StartupPath + $"\\Log\\upload\\";
                txtpath = Path.Combine(txtpath, $"upload_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
                File.WriteAllText(txtpath, sb.ToString());
                successList.Clear();
            }
        }

        /// <summary>
        /// 写入失败log
        /// </summary>
        /// <param name="errorList"></param>
        /// <param name="gInfo"></param>
        private void WriteLogWithFauilt(List<Dictionary<string, string>> errorList, DbGroupInfos gInfo)
        {
            string txtpath = Application.StartupPath + $"\\Log\\upload\\";
            if (!Directory.Exists(txtpath))
            {
                Directory.CreateDirectory(txtpath);
            }
            StringBuilder errorsb = new StringBuilder();
            errorsb.AppendLine($"失败:{errorList.Count}");
            errorsb.AppendLine("****************error***********************");
            foreach (var item in errorList)
            {
                errorsb.AppendLine($"考号:{item["IdNumber"]} 姓名:{item["Name"]} 错误:{item["error"]}");
            }
            errorsb.AppendLine("*****************error**********************");
            if (errorList.Count != 0)
            {
                string txtpath1 = Path.Combine(txtpath,
                    $"error_{gInfo.Name}_upload_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
                File.WriteAllText(txtpath1, errorsb.ToString());
                errorList.Clear();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="group"></param>
        /// <param name="sportProjectInfos"></param>
        public void PrintScore(string groupName, SportProjectInfos sportProjectInfos)
        {
            try
            {
                if (string.IsNullOrEmpty(groupName)) throw new Exception("未选择组");
                string path = Application.StartupPath + "\\Data\\PrintExcel\\";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path = Path.Combine(path, $"PrintExcel_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
                List<Dictionary<string, string>> ldic = new List<Dictionary<string, string>>();
                //序号 项目名称    组别名称 姓名  准考证号 考试状态    第1轮 第2轮 最好成绩
                List<DbPersonInfos> dbPersonInfos = new List<DbPersonInfos>();
                dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == groupName).ToList();
                List<OutPutPrintExcelData> outPutExcelDataList = new List<OutPutPrintExcelData>();
                int step = 1;
                bool isBestScore = sportProjectInfos.BestScoreMode == 0;
                foreach (var dpInfo in dbPersonInfos)
                {
                    List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>().Where(a => a.PersonId == dpInfo.Id.ToString() && a.IsRemoved == 0).ToList();
                    OutPutPrintExcelData opd = new OutPutPrintExcelData();
                    opd.Id = step;
                    opd.examTime = dpInfo.CreateTime;
                    opd.School = dpInfo.SchoolName;
                    opd.Name = dpInfo.Name;
                    opd.Sex = dpInfo.Sex == 0 ? "男" : "女";
                    opd.IdNumber = dpInfo.IdNumber;
                    opd.GroupName = dpInfo.GroupName;
                    int state = 0;
                    double MaxScore = 99999;
                    if (isBestScore) MaxScore = 0;
                    foreach (var ri in resultInfos)
                    {
                        ///异常状态
                        if (ri.State != 1)
                        {
                            if (isBestScore && MaxScore < 0)
                            {
                                //取最大值
                                MaxScore = 0;
                                state = ri.State;
                            }
                            else if (!isBestScore && MaxScore > 99999)
                            {
                                //取最小值
                                MaxScore = 99999;
                                state = ri.State;
                            }
                        }
                        else if (ri.State > 0)
                        {
                            if (isBestScore && MaxScore < ri.Result)
                            {
                                //取最大值
                                MaxScore = ri.Result;
                                state = ri.State;
                            }
                            else if (!isBestScore && MaxScore > ri.Result)
                            {
                                //取最小值
                                MaxScore = ri.Result;
                                state = ri.State;
                            }
                        }
                    }
                    if (state < 0) continue;
                    if (state != 1)
                    {
                        MaxScore = 0;
                        opd.Result = ResultStateType.Match(state);
                    }
                    else
                    {
                        opd.Result = MaxScore.ToString();
                    }
                    outPutExcelDataList.Add(opd);
                    step++;
                }

                MiniExcel.SaveAs(path, outPutExcelDataList);
                PrintCurrentStudentData(path);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private string GetLocalDefaultPrinter()
        {
            PrintDocument document = new PrintDocument();
            return document.PrinterSettings.PrinterName;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        private void PrintCurrentStudentData(string path)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(path);
            Worksheet sheet = workbook.Worksheets[0];
            sheet.PageSetup.Orientation = PageOrientationType.Portrait;
            sheet.PageSetup.PaperSize = PaperSizeType.PaperA4;
            sheet.PageSetup.HeaderMarginInch = 2;
            sheet.PageSetup.TopMargin = 0.5;
            sheet.PageSetup.BottomMargin = 0;
            ///页边距_左
            sheet.PageSetup.LeftMargin = 0.5;
            ///页边距_右
            sheet.PageSetup.RightMargin = 0.5;
            PrintDialog dialog = new PrintDialog();
            dialog.AllowPrintToFile = true;
            dialog.AllowCurrentPage = true;
            dialog.AllowSomePages = true;
            dialog.AllowSelection = true;
            dialog.UseEXDialog = true;
            dialog.PrinterSettings.Duplex = Duplex.Simplex;
            dialog.PrinterSettings.FromPage = 0;
            dialog.PrinterSettings.ToPage = 8;
            dialog.PrinterSettings.PrintRange = PrintRange.SomePages;
            dialog.PrinterSettings.PrinterName = GetLocalDefaultPrinter();
            workbook.PrintDialog = dialog;
            PrintDocument pd = workbook.PrintDocument;
            if (dialog.ShowDialog() == DialogResult.OK)
            { pd.Print(); }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listView1"></param>
        /// <param name="nowRound"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool SetErrorState(ListView listView1, int nowRound, string state)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0)
                {
                    UIMessageBox.ShowWarning("请先选择学生数据！！");
                    return false;
                }
                else
                {
                    string idnumber = listView1.SelectedItems[0].SubItems[3].Text;
                    int states = ResultStateType.ResultState2Int(state);
                    int result = freeSql.Update<ResultInfos>().Set(a => a.State == states).Where(a => a.PersonIdNumber == idnumber && a.RoundId == nowRound + 1).ExecuteAffrows();
                    if (result == 0)
                    {
                        freeSql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
                        List<ResultInfos> insertResults = new List<ResultInfos>();
                        DbPersonInfos dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.IdNumber == idnumber).ToOne();
                        if (dbPersonInfos != null)
                        {
                            maxSortId++;
                            ResultInfos rinfo = new ResultInfos();
                            rinfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            rinfo.SortId = maxSortId;
                            rinfo.IsRemoved = 0;
                            rinfo.PersonId = dbPersonInfos.Id.ToString();
                            rinfo.SportItemType = 0;
                            rinfo.PersonName = dbPersonInfos.Name;
                            rinfo.PersonIdNumber = dbPersonInfos.IdNumber;
                            rinfo.RoundId = nowRound + 1;
                            rinfo.Result = 0;
                            rinfo.State = states;
                            insertResults.Add(rinfo);
                            var res = freeSql.InsertOrUpdate<ResultInfos>().SetSource(insertResults).IfExistsDoNothing().ExecuteAffrows();
                            if (res > 0) return true;
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void LoadingChipInfos(UIComboBox uiComboBox1)
        {
            uiComboBox1.Items.Clear();
            var data = freeSql.Select<ChipInfos>().ToList();
            foreach (var item in data)
            {
                if (!uiComboBox1.Items.Contains(item.GroupName))
                    uiComboBox1.Items.Add(item.GroupName);
                else continue;
            }
            if (uiComboBox1.Items.Count > 0)
            {
                uiComboBox1.SelectedIndex = 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        internal List<string> LoadingCurrentProjectGroups(int projectID)
        {
            List<string> li = new List<string>();
            var da = freeSql.Select<DbGroupInfos>().Where(a => a.ProjectId.Equals(projectID)).ToList();
            if (da.Count > 0)
            {
                foreach (DbGroupInfos group in da)
                {
                    li.Add(group.Name);
                }
                return li;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="groups"></param>
        /// <param name="currentRound"></param>
        /// <returns></returns>
        public List<StudentDataModel> LoadCurreentStudentData(int projectID, string groups, int currentRound)
        {
            List<StudentDataModel> studentDataModels = new List<StudentDataModel>();
            var data = freeSql.Select<DbPersonInfos>().Where(a => a.ProjectId.Equals(projectID) && a.GroupName.Equals(groups)).OrderBy(a => a.Id).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var row in data)
                {
                    StudentDataModel StudentDataModel = new StudentDataModel();
                    StudentDataModel.Name = row.Name;
                    StudentDataModel.Id = row.Id;
                    StudentDataModel.IDNumber = row.IdNumber;
                    StudentDataModel.GradeName = row.GradeName;
                    StudentDataModel.ClassName = row.ClassNumber;
                    StudentDataModel.Round = currentRound;
                    var res = freeSql.Select<ResultInfos>().Where(A => A.PersonName.Equals(row.Name) && A.PersonIdNumber.Equals(row.IdNumber) && A.RoundId.Equals(currentRound)).ToOne();
                    if (res != null)
                    {
                        StudentDataModel.IsTest = true;
                        var result = res.Result;
                        var state = res.State;
                        int upLoadState = res.uploadState;
                        if (state != 1)
                        {
                            string rt = ResultStateType.Match(state);
                            StudentDataModel.Score = rt;
                        }
                        else
                        {
                            StudentDataModel.Score = result.ToString();
                        }
                        StudentDataModel.UpLoadState = upLoadState;
                        //studentDataModels.Add(StudentDataModel);
                    }
                    else
                    {
                        StudentDataModel.Score = "未测试";
                        StudentDataModel.IsTest = false;
                        StudentDataModel.UpLoadState = 0;
                    }
                    studentDataModels.Add(StudentDataModel);
                }
                return studentDataModels;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="iDNum"></param>
        /// <returns></returns>
        public SelectStudentDataModel SelectStudentDataByIDNumber(int projectID, string iDNum)
        {
            SelectStudentDataModel SelectStudentDataModel = new SelectStudentDataModel();
            try
            {
                var data = freeSql.Select<DbPersonInfos>().Where(A => A.ProjectId.Equals(projectID) && A.IdNumber.Equals(iDNum)).ToOne();
                if (data != null)
                {
                    SelectStudentDataModel.Id = data.Id.ToString();
                    SelectStudentDataModel.Name = data.Name;
                    SelectStudentDataModel.Sex = data.Sex == 0 ? "男" : "女";
                    SelectStudentDataModel.idNumber = iDNum;
                    SelectStudentDataModel.groupName = data.GroupName;
                    var res = freeSql.Select<ResultInfos>().Where(a => a.PersonName.Equals(data.Name) && a.PersonIdNumber.Equals(iDNum)).ToList();
                    if (res.Count > 0)
                    {
                        if (res.Count == 2)
                        {
                            foreach (var item in res)
                            {
                                var round = item.RoundId;
                                var state = item.State;
                                var ress = item.Result;
                                if (state != 1)
                                {
                                    var ds = ResultStateType.Match(state);
                                    if (round == 1) SelectStudentDataModel.score = ds;
                                    else SelectStudentDataModel.score1 = ds;
                                }
                                else
                                {
                                    if (round == 1) SelectStudentDataModel.score = ress.ToString();
                                    else SelectStudentDataModel.score1 = ress.ToString();
                                }
                            }
                            SelectStudentDataModel.IsAllTest = true;
                        }
                        else if (res.Count == 1)
                        {
                            var round = res[0].RoundId;
                            var state = res[0].State;
                            var ress = res[0].Result;
                            if (state != 1)
                            {
                                var ds = ResultStateType.Match(state);
                                if (round == 1) { SelectStudentDataModel.score = ds; SelectStudentDataModel.score1 = "未测试"; }
                                else { SelectStudentDataModel.score1 = ds; SelectStudentDataModel.score = "未测试"; }
                            }
                            else
                            {
                                if (round == 1) { SelectStudentDataModel.score = ress.ToString(); SelectStudentDataModel.score1 = "未测试"; }
                                else { SelectStudentDataModel.score1 = ress.ToString(); SelectStudentDataModel.score = "未测试"; }
                            }
                            SelectStudentDataModel.IsAllTest = false;
                        }
                    }
                    else
                    {
                        SelectStudentDataModel.score = "未测试";
                        SelectStudentDataModel.score1 = "未测试";
                        SelectStudentDataModel.IsAllTest = false;
                    }
                    return SelectStudentDataModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="sl"></param>
        /// <returns></returns>
        public List<SelectStudentDataModel> LoadingStudentChipData(string sl)
        {
            List<SelectStudentDataModel> selectStudentDataModels = new List<SelectStudentDataModel>();

            var chips = freeSql.Select<ChipInfos>().Where(A => A.GroupName == sl).ToList();
            if (chips != null)
            {
                foreach (var chip in chips)
                {
                    int projectID = chip.ProjectID;
                    var idnum = chip.ChipLabel.ToString();
                    var data = SelectStudentDataByIDNumber(projectID, idnum);
                    if (data.IsAllTest)
                    {
                        data.round = -1;
                    }
                    else
                    {
                        if (data.score == "未测试")
                        {
                            data.round = 1;
                        }
                        else
                        {
                            if (data.score1 != "未测试")
                            {
                                data.round = -1;
                                data.IsAllTest = false;
                            }
                            else
                            {
                                data.round = 2;
                            }
                        }
                    }
                    selectStudentDataModels.Add(data);
                }
            }

            return selectStudentDataModels;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tempstudentDataModels"></param>
        /// <param name="projectID"></param>
        /// <param name="name"></param>
        public void SaveChipInfosData(List<SelectStudentDataModel> tempstudentDataModels, int projectID, string name)
        {
            freeSql.Select<ChipInfos>().Aggregate(x => x.Max(x.Key.ChipSort), out int maxID);
            List<ChipInfos> list = new List<ChipInfos>();
            var ck = freeSql.Select<ChipInfos>().Where(A => A.ProjectID.Equals(projectID) && A.GroupName.Equals(name)).ToList();
            if (ck != null)
            {
                foreach (var ch in ck)
                {
                    freeSql.Delete<ChipInfos>(ch).ExecuteAffrows();
                }
            }
            foreach (var chip in tempstudentDataModels)
            {
                ChipInfos chips = new ChipInfos()
                {
                    ChipSort = maxID,
                    ChipLabel = chip.idNumber,
                    GroupName = name,
                    ProjectID = projectID,
                };
                list.Add(chips);
            }
            if (list.Count > 0)
            {
                freeSql.InsertOrUpdate<ChipInfos>().SetSource(list).IfExistsDoNothing().ExecuteAffrows();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentTestingStudents"></param>
        /// <param name="groupName"></param>
        public void WriteScoreToDataBase(List<StudentDataModel> currentTestingStudents, string groupName)
        {
            freeSql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxID);
            List<DbPersonInfos> dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == groupName).ToList();
            StringBuilder error = new StringBuilder();
            foreach (var student in currentTestingStudents)
            {
                var round = student.Round;
                var score = student.Score;
                var state = student.State;
                string name = student.Name;
                string idnum = student.IDNumber;
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(idnum) || name == "0" || idnum == "0")
                {
                    continue;
                }
                DbPersonInfos dbPersonInfo = dbPersonInfos.Find(A => A.Name == name && A.IdNumber == student.IDNumber);
                if (state == 0)
                {
                    error.AppendLine($"{dbPersonInfo.IdNumber},{dbPersonInfo.Name}未完成测试!!!");
                    continue;
                }
                bool flag = false;
                List<ResultInfos> resultInfosList = freeSql.Select<ResultInfos>().Where(a => a.PersonName == name && a.PersonIdNumber == student.IDNumber && a.IsRemoved == 0 & a.RoundId == round).OrderBy(a => a.Id).ToList();
                if (resultInfosList == null || resultInfosList.Count == 0)
                {
                    flag = true;

                    ResultInfos resultInfos = new ResultInfos()
                    {
                        CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        SortId = maxID,
                        IsRemoved = 0,
                        PersonId = dbPersonInfo.Id.ToString(),
                        SportItemType = 0,
                        PersonName = dbPersonInfo.Name,
                        RoundId = round,
                        Result = double.Parse(score.ToString()),
                        State = state,
                        PersonIdNumber = student.IDNumber,
                    };
                    var ress = freeSql.InsertOrUpdate<ResultInfos>().SetSource(resultInfos).IfExistsDoNothing().ExecuteAffrows();
                    if (ress > 0)
                    {
                        maxID++;
                        flag = true;
                    }
                    else { flag = false; }
                }
                else
                {
                    if (resultInfosList.Count == 2) flag = false;
                    else
                    {
                        foreach (var res in resultInfosList)
                        {
                            if (res.RoundId != round)
                            {
                                flag = true;
                            }
                            else
                            { flag = false; }
                        }
                        if (flag)
                        {
                            flag = false;
                            ResultInfos resultInfos = new ResultInfos()
                            {
                                CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                SortId = maxID,
                                IsRemoved = 0,
                                PersonId = dbPersonInfo.Id.ToString(),
                                SportItemType = 0,
                                PersonName = dbPersonInfo.Name,
                                RoundId = round,
                                Result = double.Parse(score.ToString()),
                                State = state,
                                PersonIdNumber = student.IDNumber,
                            };
                            var ress = freeSql.InsertOrUpdate<ResultInfos>().SetSource(resultInfos).IfExistsDoNothing().ExecuteAffrows();
                            if (ress > 0)
                            {
                                maxID++;
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                    }

                    if (flag)
                    {
                        freeSql.Update<DbPersonInfos>().Set(a => a.State == 1).Where(a => a.Name == name && a.IdNumber == student.IDNumber).ExecuteAffrows();
                        LogInfos logInfos = new LogInfos()
                        {
                            CreateTime = DateTime.Now.ToString("yyyy-dd-MM HH:ss:dd"),

                            IdNumber = student.IDNumber,
                            Name = student.Name,
                            Remark = $"第{round}轮测试成绩",
                            State = state,
                        };
                        freeSql.InsertOrUpdate<LogInfos>().SetSource(logInfos).IfExistsDoNothing().ExecuteAffrows();
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentTestingStudents"></param>
        public void WriteScoreToDataBase(List<StudentDataModel> currentTestingStudents)
        {
            freeSql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxID);
            if (currentTestingStudents.Count > 0)
            {
                foreach (var student in currentTestingStudents)
                {
                    string name = student.Name;
                    string IDNumber = student.IDNumber;
                    int round = student.Round;
                    string score = student.Score;
                    if (name == "" || IDNumber == "" || score == "") continue;
                    var da = freeSql.Select<ResultInfos>().Where(a => a.PersonIdNumber.Equals(IDNumber) && a.PersonName.Equals(name) && a.RoundId.Equals(round)).ToOne();
                    if (da != null)
                    {
                        continue;
                    }
                    else
                    {
                        ResultInfos resultInfos = new ResultInfos()
                        {
                            CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            SortId = maxID,
                            IsRemoved = 0,
                            PersonId = student.Id.ToString(),
                            SportItemType = 0,
                            PersonName = name,
                            RoundId = round,
                            Result = double.Parse(score.ToString()),
                            State = 1,
                            PersonIdNumber = student.IDNumber,
                        };
                        var ress = freeSql.InsertOrUpdate<ResultInfos>().SetSource(resultInfos).IfExistsDoNothing().ExecuteAffrows();
                        if (ress > 0)
                        {
                            maxID++;
                            freeSql.Update<DbPersonInfos>().Set(a => a.State == 1).Where(a => a.Name == name && a.IdNumber == student.IDNumber).ExecuteAffrows();
                        }
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="idnumber"></param>
        /// <param name="name"></param>
        /// <param name="v"></param>
        /// <param name="currentRound"></param>
        public void SetStudentErrorData(string projectId, string idnumber, string name, string v, int currentRound)
        {
            int state = ResultStateType.ResultState2Int(v);
            var ds = freeSql.Select<DbPersonInfos>().Where(a => a.Name.Equals(name) && a.IdNumber.Equals(idnumber) && a.ProjectId.Equals(projectId)).ToOne();
            if (ds != null)
            {
                var res = freeSql.Select<ResultInfos>().Where(a => a.PersonName.Equals(name) && a.PersonIdNumber.Equals(idnumber) && a.RoundId.Equals(currentRound)).ToOne();
                if (res != null)
                {
                    if (UIMessageBox.ShowAsk("当前考生成绩已经存在,是否覆盖？？"))
                    {
                        freeSql.Update<ResultInfos>().Set(a => a.State == state && a.Result == 0).Where(a => a.Id.Equals(res.Id)).ExecuteAffrows();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    freeSql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxID);
                    ResultInfos infos = new ResultInfos()
                    {
                        SortId = maxID,
                        SportItemType = 0,
                        CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        State = state,
                        uploadState = 0,
                        IsRemoved = 0,
                        PersonId = ds.Id.ToString(),
                        PersonIdNumber = idnumber,
                        PersonName = name,
                        Result = 0,
                        RoundId = currentRound,
                    };
                    freeSql.InsertOrUpdate<ResultInfos>().SetSource(infos).IfExistsDoNothing().ExecuteAffrows();
                }
            }
        }
    }
}