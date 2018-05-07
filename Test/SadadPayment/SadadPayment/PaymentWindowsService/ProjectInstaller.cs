using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using DALSADADPayment;

namespace PaymentWindowsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        clsDatabase objClsDatabase = new clsDatabase();

        string methodName = string.Empty;
        public ProjectInstaller()
        {
            try
            {
                InitializeComponent();
            }

            catch (Exception ex)
            {
                methodName = "ProjectInstaller_ProjectInstaller";

                objClsDatabase.LogError(methodName, ex.Message);
            }
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            try
            {
                new ServiceController(this.serviceInstaller1.ServiceName).Start();
            }

            catch (Exception ex)
            {
                methodName = "ProjectInstaller_serviceInstaller1_AfterInstall";

                objClsDatabase.LogError(methodName, ex.Message);
            }
        }      
    }
}
