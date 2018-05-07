using System;
using System.Runtime.Serialization;

namespace SADADPayment
{
    [DataContract(Name = "Header", Namespace = "")]
    public class Header
    {
        [DataMember(Name = "Sender", Order = 0)]
        public string Sender;

        [DataMember(Name = "Receiver", Order = 1)]
        public string Receiver;

        [DataMember(Name = "MessageType", Order = 2)]
        public string MessageType;

        [DataMember(Name = "TimeStamp", Order = 3)]
        public string TimeStamp;
    }
}