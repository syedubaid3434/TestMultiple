using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALSADADPayment
{
    public class RBBodyRequest
    {
        public string AccountNo
        {
            get;
            set;
        }

        public string Amount
        {
            get;
            set;
        }

        public string CustomerRefNo
        {
            get;
            set;
        }

        public string TransType
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }
    }
}
