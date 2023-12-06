using Spire.Xls.Core;
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
using VitalCapacityV2.Summer.GameSystem.FreeSqlHelper;
using VitalCapacityV2.Summer.GameSystem.GameModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace VitalCapacityCoreV2.GameWindow
{
    public partial class ChooseRoundWindow : UIForm
    {
        public ChooseRoundWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///
        /// </summary>
        public int mode = -1;

        /// <summary>
        ///
        /// </summary>
        private readonly IFreeSql freeSql = FreeSqlHelper.Sqlite;

        /// <summary>
        ///
        /// </summary>
        private SportProjectInfos sportProjectInfos = null;

        /// <summary>
        ///
        /// </summary>
        private DbPersonInfos dbPersonInfos = null;

        /// <summary>
        ///
        /// </summary>
        public string _idNumber = "";

        /// <summary>
        ///
        /// </summary>
        private bool isNoExam = false;

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseRoundWindow_Load(object sender, EventArgs e)
        {
            if (mode == 0) uiTitlePanel1.Text = this.Text = "轮次清空";
            else if (mode == 1) uiTitlePanel1.Text = this.Text = "修正成绩";
            if (!string.IsNullOrEmpty(_idNumber))
            {
                dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.IdNumber == _idNumber).ToOne();
            }
            sportProjectInfos = freeSql.Select<SportProjectInfos>().ToOne();
            if (sportProjectInfos != null)
            {
                int roundTotal = sportProjectInfos.RoundCount;
                for (int i = 0; i < roundTotal; i++)
                {
                    uiComboBox1.Items.Add($"第{i + 1}轮");
                }
                if (roundTotal > 0)
                    uiComboBox1.SelectedIndex = 0;
            }
            if (dbPersonInfos != null)
            {
                uiTextBox1.Text = dbPersonInfos.IdNumber;
                uiTextBox2.Text = dbPersonInfos.Name;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int roundid = uiComboBox1.SelectedIndex + 1;
            if (roundid <= 0) return;
            List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>().Where(a => a.PersonIdNumber == _idNumber).Where(a => a.RoundId == roundid).Where(a => a.IsRemoved == 0).ToList();
            isNoExam = false;
            uiTextBox3.Text = "";
            if (resultInfos.Count == 0)
            {
                isNoExam = true;
                MessageBox.Show("该学生本轮未参加考试");
                return;
            }
            foreach (var ri in resultInfos)
            {
                if (ri.IsRemoved == 0)
                {
                    uiTextBox3.Text = ri.Result.ToString();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            int roundid = uiComboBox1.SelectedIndex + 1;
            if (mode == 0)
            {
                int result = freeSql.Delete<ResultInfos>()
                   .Where(a => a.PersonIdNumber == _idNumber)
                   .Where(a => a.RoundId == roundid)
                   .ExecuteAffrows();
                if (result == 1) UIMessageBox.ShowSuccess("删除成功");
            }
            else if (mode == 1)
            {
                double.TryParse(uiTextBox3.Text, out double fhl);

                if (isNoExam)
                {
                    List<ResultInfos> insertResults = new List<ResultInfos>();
                    freeSql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
                    maxSortId++;
                    ResultInfos rinfo = new ResultInfos();
                    rinfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    rinfo.SortId = maxSortId;
                    rinfo.PersonId = dbPersonInfos.Id.ToString();
                    rinfo.SportItemType = 0;
                    rinfo.PersonName = dbPersonInfos.Name;
                    rinfo.PersonIdNumber = dbPersonInfos.IdNumber;
                    rinfo.RoundId = roundid;
                    rinfo.State = 1;
                    rinfo.IsRemoved = 0;
                    rinfo.Result = fhl;
                    insertResults.Add(rinfo);
                    int result = freeSql.InsertOrUpdate<ResultInfos>().SetSource(insertResults).IfExistsDoNothing().ExecuteAffrows();
                    if (result == 1) UIMessageBox.ShowSuccess("修改成功");
                }
                else
                {
                    int result = freeSql.Update<ResultInfos>().Set(a => a.Result == fhl).Where(a => a.PersonIdNumber == _idNumber).Where(a => a.RoundId == roundid).Where(a => a.IsRemoved == 0).ExecuteAffrows();
                    if (result == 1) UIMessageBox.ShowSuccess("修改成功");
                }
            }
        }
    }
}