using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SadadInsertPayment
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IIjarahSadadPaymentInsert" in both code and config file together.
    [ServiceContract]
    public interface IIjarahSadadPaymentInsert
    {
        [OperationContract(IsOneWay = true, IsInitiating = true, IsTerminating = false)]
        void SadadPaymentInsert();
    }
}
