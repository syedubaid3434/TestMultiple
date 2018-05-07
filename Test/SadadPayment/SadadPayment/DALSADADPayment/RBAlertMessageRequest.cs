using System;
using System.Xml.Serialization;

namespace DALSADADPayment
{
    [XmlRoot(ElementName = "Message", Namespace = "", IsNullable = false)]
    [Serializable]
    public class RBAlertMessageRequest
    {
        [XmlElement(ElementName = "Header")]
        public RBHeaderRequest HeaderReq
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Body")]
        public RBBodyRequest BodyReq
        {
            get;
            set;
        }
    }
}
