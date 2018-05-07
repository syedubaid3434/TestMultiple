using System;
using System.Xml.Serialization;

namespace DALSADADUpload
{
	[XmlRoot(ElementName = "FileMessage", Namespace = "", IsNullable = false)]
	[Serializable]
	public class RBFileMessageRequest
	{
		[XmlElement(ElementName = "Header")]
		public RBHeaderRequest HeaderReq
		{
			get;
			set;
		}

		public string Body
		{
			get;
			set;
		}

		public string Signature
		{
			get;
			set;
		}
	}
}
