using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitalCapacityV2.Summer.GameSystem.GameHelper
{
    public class StudentDataModel
    {
        public int Id { get; set; }
        public string GradeName { get; set; }
        public string ClassName { get; set; }
        public string Name { get; set; }
        public string IDNumber { get; set; }
        public int Round { get; set; }
        public int State { get; set; }

        public string Score { get; set; }
        public bool IsTest { get; set; }
        public int UpLoadState { get; set; }
    }

    public class SelectStudentDataModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        public string Sex { get; set; }

        /// <summary>
        /// 考号
        /// </summary>
        public string idNumber { get; set; }

        /// <summary>
        /// 组号
        /// </summary>
        public string groupName { get; set; }

        /// <summary>
        /// 第一轮成绩
        /// </summary>
        public string score { get; set; }

        /// <summary>
        /// 第二轮成绩
        /// </summary>
        public string score1 { get; set; }

        public bool IsAllTest { get; set; }
        public int round { get; set; }
    }
}