using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VitalCapacityCoreV2.GameWindow;
using VitalCapacityV2.Summer.GameSystem.FreeSqlHelper;
using VitalCapacityV2.Summer.GameSystem.GameHelper;
using VitalCapacityV2.Summer.GameSystem.GameModel;

namespace VitalCapacityCoreV2.GameWindowSys
{
    public class MainWindowSys
    {
        private TreeViewHelper treeViewHelper = new TreeViewHelper();
        private PlatFormWindowSys PlatFormWindowSys = new PlatFormWindowSys();
        private PersonWindowSys StudentWindowSys = new PersonWindowSys();
        private SystemSettingWindowSys SystemSettingWindow = new SystemSettingWindowSys();
        private ExportGradeWindowSys ExportGradeWindowSys = new ExportGradeWindowSys();
        private readonly IFreeSql freeSql = FreeSqlHelper.Sqlite;

        /// <summary>
        ///
        /// </summary>
        /// <param name="uiTreeView1"></param>
        public void LoadingProject(UITreeView uiTreeView1, string projectName)
        {
            treeViewHelper.UpdataTreeview(uiTreeView1, projectName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="createTime"></param>
        /// <param name="schoolName"></param>
        /// <param name="groupName"></param>
        /// <param name="listView1"></param>
        public void UpdataGroupDataView(string createTime, string schoolName, string groupName, ListView listView1)
        {
            listView1.Items.Clear();
            if (String.IsNullOrEmpty(schoolName) || String.IsNullOrEmpty(groupName) || String.IsNullOrEmpty(createTime))
            {
                return;
            }
            SportProjectInfos sportProjectInfos = freeSql.Select<SportProjectInfos>().ToOne();
            List<DbPersonInfos> dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.CreateTime == createTime && a.SchoolName == schoolName && a.GroupName == groupName).ToList();
            if (dbPersonInfos.Count == 0)
            {
                return;
            }
            int step = 1;
            listView1.BeginUpdate();
            InitListViewHeader(listView1, sportProjectInfos.RoundCount);
            Font font = new Font(Control.DefaultFont, FontStyle.Bold);
            bool st = sportProjectInfos.BestScoreMode == 0 ? true : false;
            if (sportProjectInfos.BestScoreMode == 0) st = true;
            foreach (var dbPersonInfo in dbPersonInfos)
            {
                ListViewItem li = new ListViewItem();
                li.UseItemStyleForSubItems = false;
                li.Text = step.ToString();
                li.SubItems.Add(dbPersonInfo.CreateTime);
                li.SubItems.Add(dbPersonInfo.SchoolName);
                li.SubItems.Add(dbPersonInfo.GroupName);
                li.SubItems.Add(dbPersonInfo.Name);
                li.SubItems.Add(dbPersonInfo.IdNumber);
                List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>().Where(a => a.PersonId == dbPersonInfo.Id.ToString() && a.IsRemoved == 0).OrderBy(a => a.Id).ToList();
                int res = 0;
                double maxScore = 1000;
                if (st) maxScore = 0;
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
                        if (st)
                        {
                            //取最大值
                            if (maxScore < resultInfo.Result) maxScore = resultInfo.Result;
                        }
                        else
                        {
                            //取最小值
                            if (maxScore > resultInfo.Result) maxScore = resultInfo.Result;
                        }
                    }
                    li.SubItems[li.SubItems.Count - 1].Font = font;
                    if (resultInfo.uploadState == 0)
                    {
                        li.SubItems.Add("未上传");
                        li.SubItems[li.SubItems.Count - 1].ForeColor = Color.Red;
                    }
                    else if (resultInfo.uploadState == 1)
                    {
                        li.SubItems.Add("已上传");
                        li.SubItems[li.SubItems.Count - 1].ForeColor = Color.MediumSpringGreen;
                        li.SubItems[li.SubItems.Count - 1].Font = font;
                    }
                    res++;
                }
                for (int i = res; i < sportProjectInfos.RoundCount; i++)
                {
                    li.SubItems.Add("未测试");
                    li.SubItems.Add("未上传");
                }
                if (getScore)
                { li.SubItems.Add(maxScore.ToString()); }
                else
                { li.SubItems.Add("未测试"); }
                step++;
                listView1.Items.Insert(listView1.Items.Count, li);
            }
            ListViewHelper.AutoResizeColumnWidth(listView1);
            listView1.EndUpdate();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listView1"></param>
        /// <param name="roundCount"></param>
        private void InitListViewHeader(ListView listView1, int roundCount)
        {
            listView1.View = View.Details;
            ColumnHeader[] Header = new ColumnHeader[100];
            int sp = 0;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "序号";
            Header[sp].Width = 40;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "时间";
            Header[sp].Width = 200;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "学校";
            Header[sp].Width = 250;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "组别名称";
            Header[sp].Width = 150;

            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "姓名";
            Header[sp].Width = 180;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "准考证号";
            Header[sp].Width = 200;
            sp++;
            for (int i = 1; i <= roundCount; i++)
            {
                Header[sp] = new ColumnHeader();
                Header[sp].Text = $"第{i}轮";
                Header[sp].Width = 100;
                sp++;

                Header[sp] = new ColumnHeader();
                Header[sp].Text = "上传状态";
                Header[sp].Width = 100;
                sp++;
            }
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "最好成绩";
            Header[sp].Width = 100;
            sp++;

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
        /// <param name="con"></param>
        /// <param name="action"></param>
        private void UpDataCurrentView(Control con, Action action)
        {
            if (con.InvokeRequired)
            {
                con?.Invoke(action);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool ShowPersonWindow(string projectName)
        {
            return StudentWindowSys.ShowPersonWindow(projectName);
        }

        /// <summary>
        ///
        /// </summary>
        public void ShowPlatFormSetting()
        {
            PlatFormWindowSys.ShowPlatFormWindow();
        }

        private RunningWindowSys RunningWindowSys = new RunningWindowSys();

        /// <summary>
        ///
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="createTime"></param>
        /// <param name="schoolName"></param>
        public void ShowRunningWindow(string projectName)
        {
            RunningWindowSys.ShowRunningWindow(projectName);
        }

        /// <summary>
        ///
        /// </summary>
        public void ShowSystemSetting()
        {
            SystemSettingWindow.ShowSystemSettingWindow();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public SportProjectInfos LoadingSportProjectInfos()
        {
            return freeSql.Select<SportProjectInfos>().ToOne();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="groupName"></param>
        public void SetExporWindowData(string groupName, string projectName)
        {
            ExportGradeWindowSys.SetExporWindowData(groupName, projectName);
        }

        /// <summary>
        /// 上传成绩
        /// </summary>
        public string UpLoadingGradeToServer(object obj, ref int proMax, ref int proVal, UIProcessBar ucProcessLine1, Timer timer1)
        {
            try
            {
                string cpuid = CPUHelper.GetCpuID();
                List<Dictionary<string, string>> successList = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> errorList = new List<Dictionary<string, string>>();
                Dictionary<string, string> localInfos = new Dictionary<string, string>();
                List<LocalInfos> list0 = freeSql.Select<LocalInfos>().ToList();
                foreach (var item in list0)
                {
                    localInfos.Add(item.key, item.value);
                }
                //组
                string groupName = obj as string;
                SportProjectInfos sportProjectInfos = freeSql.Select<SportProjectInfos>().ToOne();
                List<DbGroupInfos> dbGroupInfos = new List<DbGroupInfos>();
                ///查询本项目已考组
                if (!string.IsNullOrEmpty(groupName))
                {
                    //sql0 += $" AND Name = '{groupName}'";
                    dbGroupInfos = freeSql.Select<DbGroupInfos>().Where(a => a.Name == groupName).ToList();
                }
                else
                {
                    dbGroupInfos = freeSql.Select<DbGroupInfos>().ToList();
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
                proMax = dbGroupInfos.Count;
                proVal = 0;
                UpDataCurrentView(ucProcessLine1, () =>
                {
                    ucProcessLine1.Visible = true;
                    ucProcessLine1.Value = 0;
                    //timer1.Start();
                });
                ///按组上传
                foreach (var gInfo in dbGroupInfos)
                {
                    proVal++;
                    List<DbPersonInfos> dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == gInfo.Name).ToList();
                    StringBuilder resultSb = new StringBuilder();
                    List<SudentsItem> sudentsItems = new List<SudentsItem>();
                    //IdNumber 对应Id
                    Dictionary<string, string> map = new Dictionary<string, string>();
                    //取值模式
                    bool isBestScore = sportProjectInfos.BestScoreMode == 0;
                    foreach (var stu in dbPersonInfos)
                    {
                        List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>().Where(a => a.PersonIdNumber == stu.IdNumber).ToList();
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
                        try
                        {
                            List<LogInfos> logInfos = freeSql.Select<LogInfos>()
                                .Where(a => a.IdNumber == stu.IdNumber && a.State != -404)
                                .ToList();
                            logInfos.ForEach(item =>
                            {
                                string sbtxt = $"时间：{item.CreateTime},考号:{item.IdNumber},{item.Remark};";
                                logSb.Append(sbtxt);
                            });
                        }
                        catch (Exception)
                        {
                            //logSb.Clear();
                        }
                        rdi.Memo = logSb.ToString();
                        rdi.Ip = cpuid;
                        ///可以处理成绩
                        rdi.Result = MaxScore;
                        //string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);

                        #region 查询文件

                        //成绩根目录
                        Dictionary<string, string> dic_images = new Dictionary<string, string>();
                        Dictionary<string, string> dic_viedos = new Dictionary<string, string>();
                        Dictionary<string, string> dic_texts = new Dictionary<string, string>();
                        string scoreRoot = Application.StartupPath + $"\\Scores\\{sportProjectInfos.Name}\\{stu.GroupName}\\";
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
                            messageSb.AppendLine($"{gInfo.Name}组 考号:{item.IdNumber} 姓名:{item.Name}上传失败,错误内容:成绩已上传0000000000000000000000000000000000");
                        }
                        else if (value != 0)
                        {
                            errorStr = uploadResult.Match(value);
                            dic.Add("error", errorStr);
                            errorList.Add(dic);
                            messageSb.AppendLine($"{gInfo.Name}组 考号:{item.IdNumber} 姓名:{item.Name}上传失败,错误内容:{errorStr}");
                        }
                    }

                    #region 失败写入日志

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
                        string txtpath1 = Path.Combine(txtpath, $"error_{gInfo.Name}_upload_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
                        File.WriteAllText(txtpath1, errorsb.ToString());
                        errorList.Clear();
                    }

                    #endregion 失败写入日志
                }

                #region 成功写入日志

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
                        .ExecuteAffrows(); ;
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

                #endregion 成功写入日志

                // LoggerHelper.Monitor(logWirte.ToString());
                string outpitMessage = messageSb.ToString();
                return outpitMessage;
            }
            catch (Exception ex)
            {
                // LoggerHelper.Debug(ex);
                return ex.Message;
            }
            finally
            {
                UpDataCurrentView(ucProcessLine1, () =>
                {
                    ucProcessLine1.Visible = false;
                    ucProcessLine1.Value = 0;
                });
            }
        }
    }
}