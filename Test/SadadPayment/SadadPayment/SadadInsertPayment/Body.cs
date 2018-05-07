using System;
using System.Runtime.Serialization;

namespace SadadInsertPayment
{
    [DataContract(Name = "Body", Namespace = "")]
    public class Body
    {
        [DataMember(Name = "Description", Order = 0)]
        public string Description
        {
            get;
            set;
        }

        [DataMember(Name = "AccountNo", Order = 1)]
        public string AccountNo
        {
            get;
            set;
        }

        [DataMember(Name = "Amount", Order = 2)]
        public string Amount
        {
            get;
            set;
        }

        [DataMember(Name = "CustomerRefNo", Order = 3)]
        public string CustomerRefNo
        {
            get;
            set;
        }

        [DataMember(Name = "TransType", Order = 4)]
        public string TransType
        {
            get;
            set;
        }
    }
}