/* DEVELOPER NOTES
--> CREATED BY : SATISH.M -- KISL
--> CREATED ON : 15-05-2017
--> PURPOSE : TRANSACTION FUNCTIONS AND MAIL SERVICE
*/

using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Net;
using DALSADADPayment;
using System.Text;
using System.ServiceModel.Web;
using System.Reflection;
using System.Timers;
using PaymentWindowsService.ServiceReference1;

namespace PaymentWindowsService
{
    public class ClsPaymentScheduler
    {
        private Timer paymentTimer;

        private double paymentInterval = Convert.ToDouble(ConfigurationManager.AppSettings["TIMEINTERVAL"]);

        clsDatabase objClsDatabase = new clsDatabase();

        string methodName = string.Empty;

        /// <summary>
        /// To start the schedule
        /// </summary>
        public void ScheduleStart()
        {
            try
            {
                this.paymentTimer = new Timer(this.paymentInterval);
                this.paymentTimer.AutoReset = true;
                this.paymentTimer.Enabled = true;
                this.paymentTimer.Start();
                this.paymentTimer.Elapsed += new ElapsedEventHandler(this.paymentTimerElapsed);
            }

            catch (Exception ex)
            {
                methodName = "ClsPaymentScheduler_ScheduleStart";

                objClsDatabase.LogError(methodName, ex.Message);

            }
        }

        /// <summary>
        /// Call the required function on timer elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paymentTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.paymentTimer.Stop();
                objClsDatabase.LogError("Windows Service triggered", DateTime.Now.ToString() );
                this.InsertPaymentInfo();
                objClsDatabase.LogError("Windows Service finished", DateTime.Now.ToString());
                this.paymentTimer.Start();
            }

            catch (Exception ex)
            {
                methodName = "ClsPaymentScheduler_paymentTimerElapsed";

                objClsDatabase.LogError(methodName, ex.Message);
            }
        }

        /// <summary>
        /// To Send the payment info and to update the status transaction
        /// </summary>
        /// 

        public void InsertPaymentInfo()
        {
            try
            {
                ServiceReference1.IjarahSadadPaymentInsertClient objInsertPayment = new ServiceReference1.IjarahSadadPaymentInsertClient();
                objInsertPayment.SadadPaymentInsert();
            }
            catch (Exception ex)
            {
                methodName = "ClsPaymentScheduler_InsertPaymentInfo";

                objClsDatabase.LogError(methodName, ex.Message);
            }
        }

      
    }
}

