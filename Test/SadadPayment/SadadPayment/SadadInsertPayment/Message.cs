using System;
using System.Runtime.Serialization;

namespace SadadInsertPayment
{
    [DataContract(Name = "Message", Namespace = "")]
    public class Message
    {
        [DataMember(Name = "Header", Order = 0)]
        public Header objHeader
        {
            get;
            set;
        }

        [DataMember(Name = "Body", Order = 1)]
        public Body objBody
        {
            get;
            set;
        }
    }
}