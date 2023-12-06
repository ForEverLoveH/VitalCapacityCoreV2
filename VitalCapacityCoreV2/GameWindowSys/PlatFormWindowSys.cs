using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VitalCapacityCoreV2.GameWindow;
using VitalCapacityV2.Summer.GameSystem.FreeSqlHelper;
using VitalCapacityV2.Summer.GameSystem.GameModel;

namespace VitalCapacityCoreV2.GameWindowSys
{
    public class PlatFormWindowSys
    {
        /// <summary>
        ///
        /// </summary>
        private PlatFormWindow PlatFormWindow = null;

        private readonly IFreeSql freeSql = FreeSqlHelper.Sqlite;

        /// <summary>
        ///
        /// </summary>
        public void ShowPlatFormWindow()
        {
            PlatFormWindow = new PlatFormWindow();
            PlatFormWindow.ShowDialog();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="machineCode"></param>
        /// <param name="examId"></param>
        /// <param name="platforms"></param>
        /// <param name="platform"></param>
        /// <param name="uiComboBox1"></param>
        /// <param name="uiComboBox2"></param>
        /// <param name="uiComboBox3"></param>
        /// <param name="localValues"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void LoadingInitData(ref string machineCode, ref string examId, ref string platforms, ref string platform, UIComboBox uiComboBox1, UIComboBox uiComboBox2, UIComboBox uiComboBox3, ref Dictionary<string, string> localValues)
        {
            List<LocalInfos> localInfos = freeSql.Select<LocalInfos>().ToList();
            localValues = new Dictionary<string, string>();
            foreach (var localInfo in localInfos)
            {
                localValues.Add(localInfo.key, localInfo.value);
                switch (localInfo.key)
                {
                    case "MachineCode":
                        machineCode = localInfo.value;
                        break;

                    case "ExamId":
                        examId = localInfo.value;
                        break;

                    case "Platform":
                        platform = localInfo.value;
                        break;

                    case "Platforms":
                        platforms = localInfo.value;
                        break;
                }
            }
            if (string.IsNullOrEmpty(machineCode))
            {
                UIMessageBox.ShowError("设备码为空！！");
            }
            else
            {
                uiComboBox1.Text = machineCode;
            }
            if (string.IsNullOrEmpty(examId))
            {
                UIMessageBox.ShowError("考试id为空！！");
            }
            else
            {
                uiComboBox3.Text = examId;
            }
            if (string.IsNullOrEmpty(platforms))
            {
                UIMessageBox.ShowError("平台码为空！！");
            }
            else
            {
                string[] ps = platforms.Split(';');
                uiComboBox2.Items.Clear();
                foreach (string p in ps)
                {
                    uiComboBox2.Items.Add(p);
                }
            }
            if (string.IsNullOrEmpty(platform))
            {
                UIMessageBox.ShowError("平台码为空");
            }
            else
            {
                uiComboBox2.Text = platform;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="uiComboBox3"></param>
        /// <param name="url"></param>
        /// <param name="localValues"></param>
        public void GetExamNum(UIComboBox uiComboBox3, string url, Dictionary<string, string> localValues)
        {
            uiComboBox3.Items.Clear();
            if (string.IsNullOrEmpty(url))
            {
                UIMessageBox.ShowWarning("网址为空!");
                return;
            }
            url += RequestUrl.GetExamListUrl;
            RequestParameter RequestParameter = new RequestParameter();
            RequestParameter.AdminUserName = localValues["AdminUserName"];
            RequestParameter.TestManUserName = localValues["TestManUserName"];
            RequestParameter.TestManPassword = localValues["TestManPassword"];
            //序列化
            string JsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(RequestParameter);

            // string url = localValues["Platform"] + RequestUrl.GetExamListUrl;

            var formDatas = new List<FormItemModel>();
            //添加其他字段
            formDatas.Add(new FormItemModel()
            {
                Key = "data",
                Value = JsonStr
            });
            var httpUpload = new HttpUpload();
            string result = HttpUpload.GetInstance().PostForm(url, formDatas);
            GetExamList upload_Result = JsonConvert.DeserializeObject<GetExamList>(result);

            if (upload_Result == null || upload_Result.Results == null || upload_Result.Results.Count == 0)
            {
                string error = string.Empty;
                try
                {
                    error = upload_Result.Error;
                }
                catch (Exception)
                {
                    error = string.Empty;
                }
                UIMessageBox.ShowWarning($"提交错误,错误码:[{error}]");
                return;
            }
            foreach (var item in upload_Result.Results)
            {
                string str = $"{item.title}_{item.exam_id}";
                uiComboBox3.Items.Add(str);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="examId"></param>
        /// <param name="uiComboBox2"></param>
        /// <param name="uiComboBox1"></param>
        /// <param name="localValues"></param>
        public void GetCode(string examId, UIComboBox uiComboBox2, UIComboBox uiComboBox1, Dictionary<string, string> localValues)
        {
            uiComboBox1.Items.Clear();

            if (string.IsNullOrEmpty(examId))
            {
                UIMessageBox.ShowWarning("考试id为空!");
                return;
            }
            if (examId.IndexOf('_') != -1)
            {
                examId = examId.Substring(examId.IndexOf('_') + 1);
            }
            string url = uiComboBox2.Text;
            if (string.IsNullOrEmpty(url))
            {
                UIMessageBox.ShowWarning("网址为空!");
                return;
            }
            url += RequestUrl.GetMachineCodeListUrl;

            RequestParameter RequestParameter = new RequestParameter();
            RequestParameter.AdminUserName = localValues["AdminUserName"];
            RequestParameter.TestManUserName = localValues["TestManUserName"];
            RequestParameter.TestManPassword = localValues["TestManPassword"];
            RequestParameter.ExamId = examId;
            //序列化
            string JsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(RequestParameter);

            var formDatas = new List<FormItemModel>();
            //添加其他字段
            formDatas.Add(new FormItemModel()
            {
                Key = "data",
                Value = JsonStr
            });
            var httpUpload = new HttpUpload();
            string result = HttpUpload.GetInstance().PostForm(url, formDatas);
            GetMachineCodeList upload_Result = JsonConvert.DeserializeObject<GetMachineCodeList>(result);
            if (upload_Result == null || upload_Result.Results == null || upload_Result.Results.Count == 0)
            {
                string error = string.Empty;
                try
                {
                    error = upload_Result.Error;
                }
                catch (Exception)
                {
                    error = string.Empty;
                }
                UIMessageBox.ShowWarning($"提交错误,错误码:[{error}]");
                return;
            }

            foreach (var item in upload_Result.Results)
            {
                string str = $"{item.title}_{item.MachineCode}";
                uiComboBox1.Items.Add(str);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="uiComboBox1"></param>
        /// <param name="uiComboBox2"></param>
        /// <param name="uiComboBox3"></param>
        /// <exception cref="NotImplementedException"></exception>

        public bool SaveData(UIComboBox uiComboBox1, UIComboBox uiComboBox2, UIComboBox uiComboBox3)
        {
            try
            {
                string Platform = uiComboBox2.Text;
                string ExamId = uiComboBox3.Text;
                string MachineCode = uiComboBox1.Text;
                int sum = 0;
                int result = freeSql.Update<LocalInfos>().Set(a => a.value == Platform).Where(a => a.key == "Platform").ExecuteAffrows();
                sum += result;
                result = freeSql.Update<LocalInfos>().Set(a => a.value == ExamId).Where(a => a.key == "ExamId").ExecuteAffrows();
                sum += result;
                result = freeSql.Update<LocalInfos>().Set(a => a.value == MachineCode).Where(a => a.key == "MachineCode").ExecuteAffrows();
                sum += result;
                if (sum == 3)
                {
                    UIMessageBox.ShowSuccess("保存成功");
                    return true;
                }
                else
                {
                    UIMessageBox.ShowError("更新失败");
                    return false;
                }
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError("更新失败");
                return false;
            }
        }
    }
}