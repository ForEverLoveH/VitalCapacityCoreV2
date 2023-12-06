using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitalCapacityV2.Summer.GameSystem
{
    public class ChipInfos
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }

        [Column(IsNullable = false)]
        public int ProjectID { get; set; }

        public string ChipLabel { get; set; }

        [Column(IsNullable = true)]
        public string ColorGroupId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string GroupName { get; set; }

        public int ChipSort { get; set; }
    }
}