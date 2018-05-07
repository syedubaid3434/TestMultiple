using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using DALSADADPayment;

namespace PaymentWindowsService
{
    partial class PaymentService : ServiceBase
    {
        clsDatabase objClsDatabase = new clsDatabase();

        string methodName = string.Empty;
        public PaymentService()
        {
            try
            {
                InitializeComponent();
                this.InitializePaymentScheduler();
            }

            catch (Exception ex)
            {
                methodName = "PaymentService_PaymentService";

                objClsDatabase.LogError(methodName, ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

        private void InitializePaymentScheduler()
        {
            try
            {
                ClsPaymentScheduler objClsPaymentScheduler = new ClsPaymentScheduler();
                objClsPaymentScheduler.ScheduleStart();
            }

            catch (Exception ex)
            {
                methodName = "PaymentService_InitializePaymentScheduler";

                objClsDatabase.LogError(methodName, ex.Message);
            }
        }
    }
}
