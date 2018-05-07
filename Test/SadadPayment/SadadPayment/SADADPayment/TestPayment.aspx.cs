using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using PaymentWindowsService;
/// <summary>
/// Added by satish KISL on 15-Feb-2017
/// </summary>
namespace SADADPayment
{
    public partial class TestPayment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            

            btnTest.Enabled = false;
            Message objMessage = new Message();
            Header objHeader = new Header();
            Body objBody = new Body();
            objBody.AccountNo = "2010358209941";
            objBody.Amount = txtAmount.Text.Trim();
            objBody.CustomerRefNo = txtRefNumber.Text.Trim();
            objBody.Description = txtDescription.Text.Trim();
            objBody.TransType = "2";
            objHeader.MessageType = "ACNSND";
            objHeader.Receiver = "IJARAH";
            objHeader.Sender = "RYBK";
            objHeader.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH':'mm':'ss");
            objMessage.objHeader = objHeader;
            objMessage.objBody = objBody;

            //ClsPaymentScheduler paymentObj = new ClsPaymentScheduler();
            //clsMessage obj = new clsMessage();
            //obj.AccountNo = "2010358209941";
            //obj.Amount = "1200,00";
            //obj.CustomerRefNo = txtRefNumber.Text.Trim();
            //obj.Description = txtDescription.Text.Trim();
            //obj.TransType = "2";
            //obj.MessageType = "ACNSND";
            //obj.Receiver = "IJARAH";
            //obj.Sender = "RYBK";
            //obj.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH':'mm':'ss");
            //paymentObj.PaymentAlert(obj);

            IjarahSadadPaymentAlert obj = new IjarahSadadPaymentAlert();
            obj.sadadpaymentalert(objMessage);
            btnTest.Enabled = true;


        }
    }
}