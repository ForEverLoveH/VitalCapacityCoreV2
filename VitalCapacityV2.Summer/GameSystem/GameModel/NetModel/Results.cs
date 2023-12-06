using System.Collections.Generic;

namespace VitalCapacityV2.Summer.GameSystem.GameModel
{
    public class Results
    {
        /// <summary>
        ///
        /// </summary>
        public List<GroupsItem> groups { get; set; }
    }

    public class GroupsItem
    {
        /// <summary>
        ///
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<StudentInfosItem> StudentInfos { get; set; }
    }

    public class StudentInfosItem
    {
        /// <summary>
        /// 高小雅
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// 女
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 淮安市文通中学
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 初三
        /// </summary>
        public string GradeName { get; set; }

        /// <summary>
        /// 1班
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ClassNumber { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string GroupIndex { get; set; }

        public string examTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<string> Projects { get; set; }
    }
}