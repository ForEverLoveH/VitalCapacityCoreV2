using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitalCapacityV2.Summer.GameSystem.GameModel
{
    public class TreeViewModel
    {
        public string CreateTime { get; set; }
        public List<TreeViewSchoolModel> schoolModels { get; set; }
    }

    public class TreeViewSchoolModel
    {
        public string schoolName { get; set; }
        public List<string> Groups { get; set; }
    }
}