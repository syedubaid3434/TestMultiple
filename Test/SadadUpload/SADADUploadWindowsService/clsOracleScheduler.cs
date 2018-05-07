using System;
using System.Data;
using System.Timers;
using SABBWINDOWSDAL;

namespace SADADUploadWindowsService
{
    class clsOracleScheduler
    {
        private Timer oracleTimer;

        private double oracleInterval = 600000.0;

        /// <summary>
        /// Method to Schedule Start
        /// </summary>
        public void ScheduleStart()
        {
            this.oracleTimer = new Timer(this.oracleInterval);
            this.oracleTimer.AutoReset = true;
            this.oracleTimer.Enabled = true;
            this.oracleTimer.Start();
            this.oracleTimer.Elapsed += new ElapsedEventHandler(this.oracleTimer_Elapsed);
        }

        /// <summary>
        /// Method for Oracle Timer Elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void oracleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.UpdateSchedulerTimerSettings();
        }

        /// <summary>
        /// Method to Update Scheduler Timer Settings
        /// </summary>
        private void UpdateSchedulerTimerSettings()
        {
            string querystring = "select everyperiod,everyday,everydaytwice,everyweek,scheduleperiod,scheduletime,scheduletimefirst,scheduletimesecond,scheduleday from XXI_SADAD_BTOB_CONFIG";
            DataSet dataSet = new clsDatabase().ExecuteDataset(querystring, "sConfig");
            if (dataSet != null && dataSet.Tables["sConfig"].Rows.Count > 0)
            {
                DataRow dataRow = dataSet.Tables["sConfig"].Rows[0];
                clsSadadScheduler.EveryPeriod = false;
                if (dataRow["everyperiod"] != null && dataRow["everyperiod"] != DBNull.Value && dataRow["everyperiod"].ToString().Trim() != string.Empty && dataRow["everyperiod"].ToString().Trim().ToUpper() == "ACTIVE")
                {
                    clsSadadScheduler.EveryPeriod = true;
                }
                clsSadadScheduler.EveryDay = false;
                if (dataRow["everyday"] != null && dataRow["everyday"] != DBNull.Value && dataRow["everyday"].ToString().Trim() != string.Empty && dataRow["everyday"].ToString().Trim().ToUpper() == "ACTIVE")
                {
                    clsSadadScheduler.EveryDay = true;
                }
                clsSadadScheduler.EveryDayTwice = false;
                if (dataRow["everydaytwice"] != null && dataRow["everydaytwice"] != DBNull.Value && dataRow["everydaytwice"].ToString().Trim() != string.Empty && dataRow["everydaytwice"].ToString().Trim().ToUpper() == "ACTIVE")
                {
                    clsSadadScheduler.EveryDayTwice = true;
                }
                clsSadadScheduler.EveryWeek = false;
                if (dataRow["everyweek"] != null && dataRow["everyweek"] != DBNull.Value && dataRow["everyweek"].ToString().Trim() != string.Empty && dataRow["everyweek"].ToString().Trim().ToUpper() == "ACTIVE")
                {
                    clsSadadScheduler.EveryWeek = true;
                }
                clsSadadScheduler.SchedulePeriod = 1;
                if (dataRow["scheduleperiod"] != null && dataRow["scheduleperiod"] != DBNull.Value && dataRow["scheduleperiod"].ToString().Trim() != string.Empty && Convert.ToInt16(dataRow["scheduleperiod"].ToString().Trim()) >= 1)
                {
                    clsSadadScheduler.SchedulePeriod = (int)Convert.ToInt16(dataRow["scheduleperiod"].ToString().Trim());
                }
                clsSadadScheduler.ScheduleTimeFirst = "06|00|00";
                if (dataRow["scheduletimefirst"] != null && dataRow["scheduletimefirst"] != DBNull.Value && dataRow["scheduletimefirst"].ToString().Trim() != string.Empty)
                {
                    clsSadadScheduler.ScheduleTimeFirst = dataRow["scheduletimefirst"].ToString().Trim();
                }
                clsSadadScheduler.ScheduleTimeSecond = "13|30|00";
                if (dataRow["scheduletimesecond"] != null && dataRow["scheduletimesecond"] != DBNull.Value && dataRow["scheduletimesecond"].ToString().Trim() != string.Empty)
                {
                    clsSadadScheduler.ScheduleTimeSecond = dataRow["scheduletimesecond"].ToString().Trim();
                }
                dataSet.Dispose();
            }
        }
    }
}
