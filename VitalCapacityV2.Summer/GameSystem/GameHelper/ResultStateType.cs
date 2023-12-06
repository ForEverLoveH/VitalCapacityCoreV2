﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitalCapacityV2.Summer.GameSystem.GameHelper
{
    public class ResultStateType
    {
        public static int NoTest = 0;//未测试
        public static int Test = 1;//已测试
        public static int Withdrawal = 2;//中退
        public static int MissTest = 3;//缺考
        public static int Foul = 4;//犯规
        public static int Waiver = 5;//弃权

        public static string Match(int state)
        {
            switch (state)
            {
                case 0:
                    return "未测试";

                case 1:
                    return "已测试";

                case 2:
                    return "中退";

                case 3:
                    return "缺考";

                case 4:
                    return "犯规";

                case 5:
                    return "弃权";

                default:
                    return "";
            }
        }

        public static string Match(string state0)
        {
            int.TryParse(state0, out int state);
            return Match(state);
        }

        public static int ResultState2Int(string state)
        {
            switch (state)
            {
                case "未测试":
                    return NoTest;

                case "已测试":
                    return Test;

                case "中退":
                    return Withdrawal;

                case "缺考":
                    return MissTest;

                case "犯规":
                    return Foul;

                case "弃权":
                    return Waiver;

                default:
                    return 0;
            }
        }
    }
}