using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SADADUploadWindowsService
{
    public partial class Frm_Upload : Form
    {
        public Frm_Upload()
        {
            InitializeComponent();
        }

        private void Frm_Upload_Load(object sender, EventArgs e)
        {
            InitializeOracleScheduler();
            InitializeSadadScheduler();
        }


        private void InitializeOracleScheduler()
        {
            clsOracleScheduler clsOracleScheduler = new clsOracleScheduler();
            clsOracleScheduler.ScheduleStart();
        }

        private void InitializeSadadScheduler()
        {
            clsIjarahSadadScheduler clsIjarahSadadScheduler = new clsIjarahSadadScheduler();
            clsIjarahSadadScheduler.ScheduleStart();
        }
    }
}
