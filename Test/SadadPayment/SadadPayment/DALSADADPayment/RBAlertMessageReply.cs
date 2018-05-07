using System;
using System.Xml.Serialization;

namespace DALSADADPayment
{
    [XmlRoot(ElementName = "Message", Namespace = "", IsNullable = false)]
    [Serializable]
    public class RBAlertMessageReply
    {
        [XmlElement(ElementName = "Header")]
        public RBHeaderReply HeaderRep
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }
    }
}
