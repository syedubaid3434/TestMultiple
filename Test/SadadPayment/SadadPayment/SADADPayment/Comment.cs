using System;
using System.Runtime.Serialization;

namespace SADADPayment
{
    [DataContract(Namespace = "")]
    public class Comment
    {
        [DataMember]
        public string Body
        {
            get;
            set;
        }
    }
}