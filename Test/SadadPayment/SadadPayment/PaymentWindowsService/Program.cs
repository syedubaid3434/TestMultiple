using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace PaymentWindowsService
{
     static class Program
    {
          [STAThread]
         static void Main()
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[] 
            //{ 
            //    new PaymentService() 
            //};
            //ServiceBase.Run(ServicesToRun);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PaymentForm());
        }
    }
}





