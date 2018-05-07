using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SADADPayment
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IIjarahSadadPaymentAlert
    {

        [OperationContract, WebInvoke(Method = "POST", UriTemplate = "epayalert", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        Stream sadadpaymentalert(Message MessageObject);

        [OperationContract, WebInvoke(Method = "POST", UriTemplate = "comment", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        Comment MyComment(Comment comment);

        //[OperationContract, WebInvoke(Method = "POST", UriTemplate = "PaymentInsert", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        //string SadadPaymentInsert(Message MessageObject);
    }
   
}
