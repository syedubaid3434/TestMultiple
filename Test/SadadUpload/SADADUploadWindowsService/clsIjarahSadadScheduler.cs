using System;
using System.Timers;
using SADADUploadWindowsService.SADADUploadWebService;

namespace SADADUploadWindowsService
{
    class clsIjarahSadadScheduler
    {
        private int SchedulePeriodCounter;

        private Timer scheduleTimer;

        private double sadadInterval = 60000.0;

        /// <summary>
        /// Method to Schedule start
        /// </summary>
        public void ScheduleStart()
        {
            this.scheduleTimer = new Timer(this.sadadInterval);
            this.scheduleTimer.AutoReset = true;
            this.scheduleTimer.Enabled = true;
            this.scheduleTimer.Start();
            this.scheduleTimer.Elapsed += new ElapsedEventHandler(this.scheduleTimer_Elapsed);
        }

        /// <summary>
        /// method for Scheduler Timer Elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scheduleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (clsSadadScheduler.EveryPeriod)
            {
                this.SchedulePeriodCounter++;
                if (clsSadadScheduler.SchedulePeriod > 0 && this.SchedulePeriodCounter >= clsSadadScheduler.SchedulePeriod)
                {
                    this.CallSadadIjarahGateway();
                    this.SchedulePeriodCounter = 0;
                    return;
                }
            }
            else
            {
                this.SchedulePeriodCounter = 0;
                if (clsSadadScheduler.EveryDay)
                {
                    this.CallSadadIjarahGatewayOnDayWise();
                }
                if (clsSadadScheduler.EveryDayTwice)
                {
                    this.CallSadadIjarahGatewayOnTwice_A_Day();
                }
                if (clsSadadScheduler.EveryWeek)
                {
                    this.CallSadadIjarahGatewayOnWeekly();
                }
            }
        }

        /// <summary>
        /// Method to call Sadad Ijarah Gateway on Day Wise
        /// </summary>
        private void CallSadadIjarahGatewayOnDayWise()
        {
            if (clsSadadScheduler.ScheduleTime != null && clsSadadScheduler.ScheduleTime.ToString().Trim() != string.Empty)
            {
                int num = (int)Convert.ToInt16(clsSadadScheduler.ScheduleTime.ToString().Trim().Split(new char[]
				{
					'|'
				})[0].Trim());
                int num2 = (int)Convert.ToInt16(clsSadadScheduler.ScheduleTime.ToString().Trim().Split(new char[]
				{
					'|'
				})[1].Trim());
                if (num > 23)
                {
                    num = 23;
                }
                if (num2 > 59)
                {
                    num2 = 59;
                }
                if (DateTime.Now.Hour == num && DateTime.Now.Minute == num2)
                {
                    this.CallSadadIjarahGateway();
                }
            }
        }

        /// <summary>
        /// Method to call Sadad Ijarah Gateway on Twice a day
        /// </summary>
        private void CallSadadIjarahGatewayOnTwice_A_Day()
        {
            if (clsSadadScheduler.ScheduleTimeFirst != null && clsSadadScheduler.ScheduleTimeFirst.ToString().Trim() != string.Empty)
            {
                int num = (int)Convert.ToInt16(clsSadadScheduler.ScheduleTimeFirst.ToString().Trim().Split(new char[]
				{
					'|'
				})[0].Trim());
                int num2 = (int)Convert.ToInt16(clsSadadScheduler.ScheduleTimeFirst.ToString().Trim().Split(new char[]
				{
					'|'
				})[1].Trim());
                if (num > 23)
                {
                    num = 23;
                }
                if (num2 > 59)
                {
                    num2 = 59;
                }
                if (DateTime.Now.Hour == num && DateTime.Now.Minute == num2)
                {
                    this.CallSadadIjarahGateway();
                }
            }
            if (clsSadadScheduler.ScheduleTimeSecond != null && clsSadadScheduler.ScheduleTimeSecond.ToString().Trim() != string.Empty)
            {
                int num3 = (int)Convert.ToInt16(clsSadadScheduler.ScheduleTimeSecond.ToString().Trim().Split(new char[]
				{
					'|'
				})[0].Trim());
                int num4 = (int)Convert.ToInt16(clsSadadScheduler.ScheduleTimeSecond.ToString().Trim().Split(new char[]
				{
					'|'
				})[1].Trim());
                if (num3 > 23)
                {
                    num3 = 23;
                }
                if (num4 > 59)
                {
                    num4 = 59;
                }
                if (DateTime.Now.Hour == num3 && DateTime.Now.Minute == num4)
                {
                    this.CallSadadIjarahGateway();
                }
            }
        }

        /// <summary>
        /// Method to call Sadad Ijarah Gateway on Weekly
        /// </summary>
        private void CallSadadIjarahGatewayOnWeekly()
        {
            if (clsSadadScheduler.ScheduleDay != null && clsSadadScheduler.ScheduleTimeSecond != null && clsSadadScheduler.ScheduleDay.ToString().Trim() != string.Empty && clsSadadScheduler.ScheduleTimeSecond.ToString().Trim() != string.Empty)
            {
                int num = (int)Convert.ToInt16(clsSadadScheduler.ScheduleTime.ToString().Trim().Split(new char[]
				{
					'|'
				})[0].Trim());
                int num2 = (int)Convert.ToInt16(clsSadadScheduler.ScheduleTime.ToString().Trim().Split(new char[]
				{
					'|'
				})[1].Trim());
                if (num > 23)
                {
                    num = 23;
                }
                if (num2 > 59)
                {
                    num2 = 59;
                }
                if (DateTime.Now.DayOfWeek.ToString().Trim().ToUpper() == clsSadadScheduler.ScheduleDay.ToString().Trim() && DateTime.Now.Hour == num && DateTime.Now.Minute == num2)
                {
                    this.CallSadadIjarahGateway();
                }
            }
        }

        /// <summary>
        /// Method to call Sadad Ijarah Gateway
        /// </summary>
        private void CallSadadIjarahGateway()
        {
            IJARAHSADADUPLOADClient objIjarahSadadUpload = new IJARAHSADADUPLOADClient();
            objIjarahSadadUpload.GetAndUploadAccountReceivables();
            objIjarahSadadUpload.Close();
        }
    }
}
