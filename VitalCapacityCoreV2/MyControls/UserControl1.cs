using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static VitalCapacityCoreV2.MyControls.UserControl1;

namespace VitalCapacityCoreV2.MyControls
{
    public delegate void CallSwitchCallback(studentPanelControlPojo data);
    public delegate void UploadSwitchCallback(studentPanelControlPojo data);
    public delegate void ErrorCallback(studenterrorMsg data);
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }
        public string CPortName = string.Empty;

        [Description("标题"), Category("自定义属性")]
        public string p_title
        {
            get
            {
                return this.mtitle.Text;
            }
            set
            {
                this.mtitle.Text = value;
            }
        }
        [Description("标题颜色"), Category("自定义属性")]
        public Color p_title_Color
        {
            get
            {
                return this.mtitle.BackColor;
            }
            set
            {
                this.mtitle.BackColor = value;
            }
        }
        [Description("考号"), Category("自定义属性")]
        public string p_IdNumber
        {
            get
            {
                return this.mIdNumber.Text;
            }
            set
            {
                this.mIdNumber.Text = value;
            }
        }
        [Description("姓名"), Category("自定义属性")]
        public string p_Name
        {
            get
            {
                return this.mName.Text;
            }
            set
            {
                this.mName.Text = value;
            }
        }
        [Description("成绩"), Category("自定义属性")]
        public string p_Score
        {
            get
            {
                return this.mScore.Text;
            }
            set
            {
                this.mScore.Text = value;
            }
        }
        [Description("设备状态"), Category("自定义属性")]
        public string p_toolState
        {
            get
            {
                return this.toolState.Text;
            }
            set
            {
                this.toolState.Text = value;
            }
        }
        [Description("设备状态颜色"), Category("自定义属性")]
        public Color p_toolState_color
        {
            get
            {
                return this.toolState.ForeColor;
            }
            set
            {
                this.toolState.ForeColor = value;
            }
        }
        [Description("轮次"), Category("自定义属性")]
        public int p_roundCbx_selectIndex
        {
            get
            {
                return this.roundCbx.SelectedIndex;
            }
            set
            {
                roundCbx.SelectedIndex = value;
            }
        }

        [Description("轮次items"), Category("自定义属性")]
        public List<string> p_roundCbx_items
        {
            get
            {
                List<string> items = new List<string>();
                foreach (var item in roundCbx.Items)
                {
                    items.Add(item.ToString());
                }
                return items;
            }
            set
            {
                roundCbx.Items.Clear();
                foreach (var item in value)
                {
                    roundCbx.Items.Add(item);

                }
            }
        }

        [Description("状态"), Category("自定义属性")]
        public int p_stateCbx_selectIndex
        {
            get
            {
                return this.stateCbx.SelectedIndex;
            }
            set
            {
                stateCbx.SelectedIndex = value;
            }
        }


        public class studentPanelControlPojo
        {
            public string IdNumber;
            public int State;
        }
        public class studenterrorMsg
        {
            public string IdNumber;
            public Bitmap img;
            public string log;
        }

        
    }
}
