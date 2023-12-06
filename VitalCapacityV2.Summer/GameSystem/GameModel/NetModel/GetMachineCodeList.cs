using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitalCapacityV2.Summer.GameSystem.GameModel
{
    public class GetMachineCodeList
    {
        public List<GetMachineCodeListResults> Results;

        public String Error;
    }

    public class GetMachineCodeListResults
    {
        public String title { get; set; }

        public String MachineCode { get; set; }
    }
}