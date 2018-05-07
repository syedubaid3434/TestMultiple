using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DALSADADPayment
{
    public class clsSMSLogDetailsInfo
    {
        public decimal SMSLOGDETAILSID
        {
            get;
            set;
        }

        public decimal SMSLOGID
        {
            get;
            set;
        }

        public string MOBILENOS
        {
            get;
            set;
        }

        public int ERRORLOGID
        {
            get;
            set;
        }

        public string DELIVERYSTAUSCODE
        {
            get;
            set;
        }

        public DateTime DATE
        {
            get;
            set;
        }

        public string CONTRACTNO
        {
            get;
            set;
        }
    }
}
