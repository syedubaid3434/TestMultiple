using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALSADADPayment
{
    public class RBHeaderReply
    {
        public string Sender
        {
            get;
            set;
        }

        public string Receiver
        {
            get;
            set;
        }

        public string MessageType
        {
            get;
            set;
        }

        public string TimeStamp
        {
            get;
            set;
        }
    }
}
