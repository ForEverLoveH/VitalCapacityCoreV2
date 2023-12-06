using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitalCapacityV2.Summer.GameSystem.GameModel
{
    public class upload_Result
    {
        /// <summary>
        ///
        /// </summary>
        public List<Dictionary<string, int>> Result { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Error { get; set; }
    }

    public class uploadResult
    {
        public static string Match(int index)
        {
            string result = "";
            switch (index)
            {
                case 1:
                    result = "成功";
                    break;

                case -1:
                    result = "学生数据有误";
                    break;

                case -2:
                    result = "报项数据有误";
                    break;

                case -3:
                    result = "轮次数据有误";
                    break;

                case -4:
                    result = "轮次已经上报过了";
                    break;

                case -5:
                    result = "未检录";
                    break;

                default:
                    result = "未解析错误";
                    break;
            }
            return result;
        }
    }
}