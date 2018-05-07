using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DALSADADUpload
{
	public class clsSerializationDeserialization<T> where T : class
	{        

        /// <summary>
        /// Method for Serialization
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
		public static string Serialize<t>(T value)
		{
			string result;
			try
			{
				Encoding uTF = Encoding.UTF8;
				XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
				xmlWriterSettings.Indent = true;
				xmlWriterSettings.IndentChars = "";
				xmlWriterSettings.NewLineOnAttributes = true;
				xmlWriterSettings.Encoding = uTF;
				xmlWriterSettings.NewLineChars = "\n";
				xmlWriterSettings.NewLineHandling = NewLineHandling.Replace;
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
				xmlSerializerNamespaces.Add("", "");
				MemoryStream memoryStream = new MemoryStream();
				XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
				xmlSerializer.Serialize(xmlWriter, value, xmlSerializerNamespaces);
				result = uTF.GetString(memoryStream.ToArray());
			}
			catch (Exception ex)
			{
				result = string.Empty;
                clsDatabase objDatabase = new clsDatabase();
                objDatabase.LogError("clsSerializationDeserialization_Serialize",ex.Message);
                
			}
			return result;
		}

        /// <summary>
        /// Method for Deserialization
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="XMLstring"></param>
        /// <returns></returns>
		public static T Deserialize<t>(string XMLstring)
		{
			if (string.IsNullOrEmpty(XMLstring))
			{
				return default(T);
			}
			T result;
			try
			{
				Encoding uTF = Encoding.UTF8;
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				MemoryStream memoryStream = new MemoryStream();
				StreamWriter streamWriter = new StreamWriter(memoryStream, uTF);
				streamWriter.Write(XMLstring);
				streamWriter.Flush();
				memoryStream.Position = 0L;
				T ta = (T)((object)xmlSerializer.Deserialize(memoryStream));
				memoryStream.Close();
				result = ta;
			}
			catch (Exception ex)
			{
				result = default(T);
                clsDatabase objDatabase = new clsDatabase();
                objDatabase.LogError("clsSerializationDeserialization_Deserialize", ex.Message);
			}
			return result;
		}
	}
}
