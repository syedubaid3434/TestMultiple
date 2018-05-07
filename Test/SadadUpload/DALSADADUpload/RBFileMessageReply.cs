using System;
using System.Xml.Serialization;

namespace DALSADADUpload
{
	[XmlRoot(ElementName = "FileMessage", Namespace = "", IsNullable = false)]
	[Serializable]
	public class RBFileMessageReply
	{
		[XmlElement(ElementName = "Header")]
		public RBHeaderReply HeaderRep
		{
			get;
			set;
		}

		public string Body
		{
			get;
			set;
		}
	}
}
