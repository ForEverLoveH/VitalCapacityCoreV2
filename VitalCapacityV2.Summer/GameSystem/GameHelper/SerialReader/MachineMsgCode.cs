namespace VitalCapacityV2.Summer.GameSystem.GameHelper
{
    public class MachineMsgCode
    {
        public int type { get; set; }
        public string mac { get; set; }
        public string itemtype { get; set; }
        public int code { get; set; }
        /// <summary>
        /// 身高
        /// </summary>
        public double fhl_result{
            get;
            set;
        }
        /// <summary>
        /// 肺活量成绩
        /// </summary>
        public double sg_result { get; set; }
        /// <summary>
        /// 体重成绩
        /// </summary>
        public double tz_result { get; set; }
        /// <summary>
        /// bmi
        /// </summary>
        public double bmi_result { get; set; }
        public  double wl_result { get; set; }
    }

    public class SerialMsg
    {
        public  MachineMsgCode MsgCode { get; set; }    
        public  int num { get; set; }

        public SerialMsg(MachineMsgCode code, int num)
        {
            this.num = num;
            this.MsgCode = code;
        }
    }
    
}