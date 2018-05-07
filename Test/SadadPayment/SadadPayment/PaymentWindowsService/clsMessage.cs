using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentWindowsService
{
  public  class clsMessage
    {
        public string AccountNo { get; set; }
        public string Amount { get; set; }
        public string CustomerRefNo { get; set; }
        public string Description { get; set; }
        public string TransType { get; set; }
        public string MessageType { get; set; }
        public string Receiver { get; set; }
        public string Sender { get; set; }
        public string TimeStamp { get; set; }
        
    }
}
