using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PaymentWindowsService
{
    public partial class PaymentForm : Form
    {
       
        public PaymentForm()
        {
            InitializeComponent();
        }

        private void PaymentForm_Load(object sender, EventArgs e)
        {
            this.InitializePaymentScheduler();
        }

        private void InitializePaymentScheduler()
        {
            ClsPaymentScheduler objClsPaymentScheduler = new ClsPaymentScheduler();
            objClsPaymentScheduler.InsertPaymentInfo();
        }

        
    }
}
