using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADADUploadWindowsService
{
    public class clsSadadScheduler
    {
        public static bool EveryPeriod
        {
            get;
            set;
        }

        public static bool EveryDay
        {
            get;
            set;
        }

        public static bool EveryDayTwice
        {
            get;
            set;
        }

        public static bool EveryWeek
        {
            get;
            set;
        }

        public static int SchedulePeriod
        {
            get;
            set;
        }

        public static string ScheduleTime
        {
            get;
            set;
        }

        public static string ScheduleTimeFirst
        {
            get;
            set;
        }

        public static string ScheduleTimeSecond
        {
            get;
            set;
        }

        public static string ScheduleDay
        {
            get;
            set;
        }
    }
}
