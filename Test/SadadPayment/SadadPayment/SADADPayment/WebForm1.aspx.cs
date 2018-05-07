using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SADADPayment
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Message objMessage = new Message();
            Header objHeader = new Header();
            Body objBody = new Body();
            objBody.AccountNo = "2010358209941";
            objBody.Amount = "1000,00";
            objBody.CustomerRefNo = "SUBS 100201160500042";
            objBody.Description = "SPTN 1729859975";
            objBody.TransType = "2";
            objHeader.MessageType = "ACNSND";
            objHeader.Receiver = "IJARAH";
            objHeader.Sender = "RYBK";
            objHeader.TimeStamp = "2017-01-03T09:30:44";
            objMessage.objHeader = objHeader;
            objMessage.objBody = objBody;
            IjarahSadadPaymentAlert testing = new IjarahSadadPaymentAlert();
            testing.sadadpaymentalert(objMessage);
        }
    }
}