using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VitalCapacityV2.Summer.GameSystem.FreeSqlHelper;
using VitalCapacityV2.Summer.GameSystem.GameModel;

namespace VitalCapacityCoreV2.GameWindowSys
{
    public class TreeViewHelper
    {
        private IFreeSql freeSql = FreeSqlHelper.Sqlite;

        /// <summary>
        ///
        /// </summary>
        /// <param name="treeView1"></param>
        public void UpdataTreeview(UITreeView treeView1, string project)
        {
            List<TreeViewModel> treeViewModels = new List<TreeViewModel>();

            treeView1.Nodes.Clear();
            //listView1.Items.Clear();
            //List<DbGroupInfos> dbGroupInfos = fsql.Select<DbGroupInfos>().ToList();
            var lists = freeSql.Select<DbPersonInfos>().Distinct().ToList(a => new { a.CreateTime, a.SchoolName, a.GroupName });
            Console.WriteLine();
            foreach (var item in lists)
            {
                TreeViewModel treeViewModel = treeViewModels.Find(a => a.CreateTime == item.CreateTime);
                if (treeViewModel == null)
                {
                    treeViewModels.Add(new TreeViewModel { CreateTime = item.CreateTime, schoolModels = new List<TreeViewSchoolModel>() });
                }
                treeViewModel = treeViewModels.Find(a => a.CreateTime == item.CreateTime);
                if (treeViewModel != null)
                {
                    TreeViewSchoolModel treeViewSchoolsModel = treeViewModel.schoolModels.Find(a => a.schoolName == item.SchoolName);
                    if (treeViewSchoolsModel == null)
                    {
                        treeViewModel.schoolModels.Add(new TreeViewSchoolModel()
                        {
                            schoolName = item.SchoolName,
                            Groups = new List<string> { item.GroupName }
                        });
                    }
                    else
                    {
                        treeViewSchoolsModel.Groups.Add(item.GroupName);
                    }
                }
            }
            for (int i = 0; i < treeViewModels.Count; i++)
            {
                TreeNode ts = new TreeNode(project);
                TreeNode tn1 = new TreeNode(treeViewModels[i].CreateTime);
                List<TreeViewSchoolModel> treeViewSchoolsModel = treeViewModels[i].schoolModels;
                for (int j = 0; j < treeViewSchoolsModel.Count; j++)
                {
                    TreeNode tn2 = new TreeNode(treeViewSchoolsModel[j].schoolName);
                    foreach (var group in treeViewSchoolsModel[j].Groups)
                    {
                        tn2.Nodes.Add(group);
                    }
                    tn1.Nodes.Add(tn2);
                }

                ts.Nodes.Add(tn1);
                treeView1.Nodes.Add(ts);
            }
        }

        /// <summary>
        /// 自动调整ListView的列宽的方法
        /// </summary>
        /// <param name="lv"></param>
        public static void AutoResizeColumnWidth(ListView lv)
        {
            int count = lv.Columns.Count;
            int MaxWidth = 0;
            Graphics graphics = lv.CreateGraphics();
            int width;
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            for (int i = 0; i < count; i++)
            {
                string str = lv.Columns[i].Text;
                MaxWidth = lv.Columns[i].Width;

                foreach (ListViewItem item in lv.Items)
                {
                    str = item.SubItems[i].Text;
                    width = (int)graphics.MeasureString(str, lv.Font).Width;
                    if (width > MaxWidth)
                    {
                        MaxWidth = width;
                    }
                }
                if (MaxWidth <= 150)
                {
                    lv.Columns[i].Width = MaxWidth;
                }
                else
                {
                    lv.Columns[i].Width = 100;
                }
            }
        }
    }
}