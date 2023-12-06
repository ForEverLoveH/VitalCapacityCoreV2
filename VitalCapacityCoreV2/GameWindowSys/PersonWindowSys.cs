using MiniExcelLibs;
using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VitalCapacityCoreV2.GameWindow;
using VitalCapacityV2.Summer.GameSystem;
using VitalCapacityV2.Summer.GameSystem.FreeSqlHelper;
using VitalCapacityV2.Summer.GameSystem.GameModel;

namespace VitalCapacityCoreV2.GameWindowSys
{
    public class PersonWindowSys
    {
        private readonly IFreeSql freeSql = FreeSqlHelper.Sqlite;

        /// <summary>
        ///
        /// </summary>
        /// <param name="listView1"></param>
        public void InitListViewHeader(ListView listView1)
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
            Header[sp].Width = 220;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "年级";
            Header[sp].Width = 80;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "班级";
            Header[sp].Width = 80;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "准考证号";
            Header[sp].Width = 200;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "姓名";
            Header[sp].Width = 150;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "性别";
            Header[sp].Width = 80;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "组别名称";
            Header[sp].Width = 150;
            sp++;

            ColumnHeader[] Header1 = new ColumnHeader[sp];
            listView1.Columns.Clear();
            for (int i = 0; i < Header1.Length; i++)
            {
                Header1[i] = Header[i];
            }
            listView1.Columns.AddRange(Header1);
        }

        private PlatFormWindowSys PlatFormWindowSys = new PlatFormWindowSys();

        /// <summary>
        ///
        /// </summary>
        public void ShowPlatFormWindow()
        {
            PlatFormWindowSys.ShowPlatFormWindow();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="uiTextBox1"></param>
        public void OpenLocalExcelFile(UITextBox uiTextBox1)
        {
            string path = string.Empty;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;      //该值确定是否可以选择多个文件
            dialog.Title = "请选择文件";     //弹窗的标题
            dialog.InitialDirectory = Application.StartupPath + "\\";    //默认打开的文件夹的位置
            dialog.Filter = "MicroSoft Excel文件(*.xlsx)|*.xlsx";       //筛选文件
            dialog.ShowHelp = true;     //是否显示“帮助”按钮
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = dialog.FileName;
            }
            if (!String.IsNullOrEmpty(path))
            {
                uiTextBox1.Text = path;
            }
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
        /// <param name="obj"></param>
        /// <param name="isDeleteBeforeImport"></param>
        /// <param name="uiLabel5"></param>
        /// <returns></returns>
        public bool ExcelInputDataBase(object obj, bool isDeleteBeforeImport, UILabel uiLabel5)
        {
            bool isRes = false;
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                if (isDeleteBeforeImport)
                {
                    string[] datas = new string[] { "DbGroupInfos", "DbPersonInfos", "ResultInfos", "LogInfos" };
                    int result1 = freeSql.Delete<DbGroupInfos>().Where("1=1").ExecuteAffrows();
                    int result2 = freeSql.Delete<DbPersonInfos>().Where("1=1").ExecuteAffrows();
                    int result3 = freeSql.Delete<ResultInfos>().Where("1=1").ExecuteAffrows();
                    int result4 = freeSql.Update<LogInfos>().Set(a => a.State == -404).Where("1=1").ExecuteAffrows();
                    int result5 = freeSql.Delete<ChipInfos>().Where("1=1").ExecuteAffrows();
                }
                string path = obj as string;
                if (!String.IsNullOrEmpty(path))
                {
                    SportProjectInfos sportProjectInfos = freeSql.Select<SportProjectInfos>().ToOne();
                    var rows = MiniExcel.Query<InputData>(path).ToList();
                    HashSet<string> set = new HashSet<string>();
                    for (int i = 0; i < rows.Count; i++)
                    {
                        string[] examTime = rows[i].examTime.Split(' ');
                        set.Add(rows[i].GroupName + "#" + examTime[0]);
                    }
                    List<string> rolesList = new List<string>();
                    rolesList.AddRange(set);
                    freeSql.Select<DbGroupInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
                    List<DbGroupInfos> insertDbGroupInfosList = new List<DbGroupInfos>();
                    for (int i = 0; i < rolesList.Count; i++)
                    {
                        maxSortId++;
                        string role = rolesList[i];
                        string[] roles = role.Split("#");
                        string groupName = roles[0];
                        string examTime = roles[1];
                        DbGroupInfos dbGroupInfos = new DbGroupInfos();
                        dbGroupInfos.Name = groupName;
                        dbGroupInfos.CreateTime = examTime;
                        dbGroupInfos.SortId = maxSortId;
                        dbGroupInfos.IsRemoved = 0;
                        dbGroupInfos.ProjectId = 0.ToString();
                        dbGroupInfos.IsAllTested = 0;
                        insertDbGroupInfosList.Add(dbGroupInfos);
                    }
                    int sy = freeSql.InsertOrUpdate<DbGroupInfos>().SetSource(insertDbGroupInfosList).IfExistsDoNothing().ExecuteAffrows();
                    freeSql.Select<DbPersonInfos>().Aggregate(x => x.Max(x.Key.SortId), out maxSortId);
                    List<DbPersonInfos> personInfos = new List<DbPersonInfos>();
                    foreach (var row in rows)
                    {
                        maxSortId++;
                        string personID = row.IdNumber.ToString();
                        string name = row.Name.ToString();
                        int sex = row.Sex == "男" ? 0 : 1;
                        string SchoolName = row.School;
                        string GradeName = row.GradeName;
                        string classNumber = row.ClassName;
                        string GroupName = row.GroupName;
                        string[] examTimes = row.examTime.Split(' ');
                        string examTime = examTimes[0];
                        DbPersonInfos dbPersonInfos = new DbPersonInfos();
                        dbPersonInfos.CreateTime = examTime;
                        dbPersonInfos.SortId = maxSortId;
                        dbPersonInfos.ProjectId = "0";
                        dbPersonInfos.SchoolName = SchoolName;
                        dbPersonInfos.GradeName = GradeName;
                        dbPersonInfos.ClassNumber = classNumber;
                        dbPersonInfos.GroupName = GroupName;
                        dbPersonInfos.Name = name;
                        dbPersonInfos.IdNumber = personID;
                        dbPersonInfos.Sex = sex;
                        dbPersonInfos.State = 0;
                        dbPersonInfos.FinalScore = -1;
                        dbPersonInfos.uploadState = 0;
                        personInfos.Add(dbPersonInfos);
                    }
                    int reslut = freeSql.InsertOrUpdate<DbPersonInfos>().SetSource(personInfos).IfExistsDoNothing().ExecuteAffrows();
                    if (reslut == 0) { isRes = false; }
                    else { isRes = true; }
                    sw.Stop();
                    string time = (sw.ElapsedMilliseconds / 1000).ToString("0.000") + "秒";
                    UpDataCurrentView(uiLabel5, () =>
                    {
                        uiLabel5.Text = $"耗时：{time},实际插入:{reslut},重复：{rows.Count - reslut}";
                    });
                }
                if (isRes)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="localValues"></param>
        /// <param name="listView1"></param>
        /// <param name="uiLabel5"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public bool LoadingServerData(string name, ref Dictionary<string, string> localValues, ListView listView1, UILabel uiLabel5, ref string paths)
        {
            try
            {
                List<LocalInfos> localInfos = freeSql.Select<LocalInfos>().ToList();
                localValues = new Dictionary<string, string>();
                foreach (LocalInfos info in localInfos)
                {
                    localValues.Add(info.key, info.value);
                }
                RequestParameter RequestParameter = new RequestParameter();
                RequestParameter.AdminUserName = localValues["AdminUserName"];
                RequestParameter.TestManUserName = localValues["TestManUserName"];
                RequestParameter.TestManPassword = localValues["TestManPassword"];
                string ExamId0 = localValues["ExamId"];
                ExamId0 = ExamId0.Substring(ExamId0.IndexOf('_') + 1);
                string MachineCode0 = localValues["MachineCode"];
                MachineCode0 = MachineCode0.Substring(MachineCode0.IndexOf('_') + 1);
                RequestParameter.ExamId = ExamId0;
                RequestParameter.MachineCode = MachineCode0;
                RequestParameter.GroupNums = name + "";
                //序列化
                string JsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(RequestParameter);
                string url = localValues["Platform"] + RequestUrl.GetGroupStudentUrl;
                var formDatas = new List<FormItemModel>();
                //添加其他字段
                formDatas.Add(new FormItemModel()
                {
                    Key = "data",
                    Value = JsonStr
                });

                string result = string.Empty;
                try
                {
                    result = HttpUpload.GetInstance().PostForm(url, formDatas);
                }
                catch (Exception ex)
                {
                    UIMessageBox.ShowError("请检查网络");
                }
                //GetGroupStudent upload_Result = JsonConvert.DeserializeObject<GetGroupStudent>(result);
                string[] strs = GetGroupStudent.CheckJson(result);
                GetGroupStudent upload_Result = null;
                if (strs[0] == "1")
                {
                    upload_Result = JsonConvert.DeserializeObject<GetGroupStudent>(result);
                }
                else
                {
                    upload_Result = new GetGroupStudent();
                    upload_Result.Error = strs[1];
                }
                bool bFlag = false;
                if (upload_Result == null || upload_Result.Results == null || upload_Result.Results.groups.Count == 0)
                {
                    string error = string.Empty;
                    try
                    { error = upload_Result.Error; }
                    catch (Exception)
                    { error = string.Empty; }
                    UIMessageBox.ShowError($"提交错误,错误码:[{error}]");
                }
                else
                {
                    bFlag = true;
                }
                if (bFlag)
                {
                    DownlistOutputExcel(upload_Result, listView1, uiLabel5, ref paths);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 下载名单
        /// </summary>
        /// <param name="upload_Result"></param>
        /// <param name="listView1"></param>
        /// <param name="msglabel"></param>
        private void DownlistOutputExcel(GetGroupStudent upload_Result, ListView listView1, UILabel msglabel, ref String importpath)
        {
            List<GroupsItem> Groups = upload_Result.Results.groups;
            List<InputData> doc = new List<InputData>();
            int step = 1;
            listView1.BeginUpdate();
            listView1.Items.Clear();
            //序号	学校	 年级	班级 	姓名	 性别	准考证号	 组别名称
            foreach (var Group in Groups)
            {
                string groupId = Group.GroupId;
                string groupName = Group.GroupName;
                foreach (var StudentInfo in Group.StudentInfos)
                {
                    InputData idata = new InputData();
                    idata.Id = step;
                    idata.examTime = StudentInfo.examTime;
                    idata.School = StudentInfo.SchoolName;
                    idata.GradeName = StudentInfo.GradeName;
                    idata.ClassName = StudentInfo.ClassName;
                    idata.Name = StudentInfo.Name;
                    idata.Sex = StudentInfo.Sex;
                    idata.IdNumber = StudentInfo.IdNumber;
                    idata.GroupName = groupId;
                    ListViewItem li = new ListViewItem();
                    li.Text = step.ToString();
                    li.SubItems.Add(StudentInfo.examTime);
                    li.SubItems.Add(StudentInfo.SchoolName);
                    li.SubItems.Add(StudentInfo.GradeName);
                    li.SubItems.Add(StudentInfo.ClassName);
                    li.SubItems.Add(StudentInfo.IdNumber);
                    li.SubItems.Add(StudentInfo.Name);
                    li.SubItems.Add(StudentInfo.Sex);
                    li.SubItems.Add(groupId);
                    listView1.Items.Insert(listView1.Items.Count, li);
                    doc.Add(idata);
                    step++;
                }
            }
            listView1.EndUpdate();
            importpath = Application.StartupPath + $"\\模板\\下载名单\\";
            if (!Directory.Exists(importpath))
            {
                Directory.CreateDirectory(importpath);
            }
            importpath = Path.Combine(importpath, $"downList{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
            MiniExcel.SaveAsAsync(importpath, doc);
            UpDataCurrentView(msglabel, () => { msglabel.Text = $"下载名单成功,共{step - 1}人"; });
        }

        private ImportStudentWindow ImportStudentWindow;

        /// <summary>
        ///
        /// </summary>
        public bool ShowPersonWindow(string projectName)
        {
            ImportStudentWindow = new ImportStudentWindow();
            ImportStudentWindow.ProjectName = projectName;
            return ImportStudentWindow.ShowDialog() == DialogResult.OK ? true : false;
        }
    }
}