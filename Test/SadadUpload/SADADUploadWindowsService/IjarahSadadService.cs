using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SADADUploadWindowsService
{
    public partial class IjarahSadadService : ServiceBase
    {
        public IjarahSadadService()
        {
            this.InitializeComponent();
            this.InitializeOracleScheduler();
            this.InitializeSadadScheduler();
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

        public void OnDebug()
        {
            this.InitializeOracleScheduler();
            this.InitializeSadadScheduler();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
