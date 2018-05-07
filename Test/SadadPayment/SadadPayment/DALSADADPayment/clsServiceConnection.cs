using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Configuration;

namespace DALSADADPayment
{
    public class clsServiceConnection
    {

       

        /// <summary>
        /// Method to post data with signature
        /// </summary>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static string PostDataWithSignature(string postdata)
        {
            string result = string.Empty;
            string fileName = string.Empty;
            string password = string.Empty;
            string requestUriString = string.Empty;

            string checkTest = ConfigurationManager.AppSettings["CHECKTEST"];
            try
            {
                postdata = "strXmldata=" + postdata;

                if (checkTest == "1")
                {
                    fileName = ConfigurationManager.AppSettings["SSLFILENAMETEST"];
                    password = ConfigurationManager.AppSettings["SSLPASSWORDTEST"];
                    requestUriString = ConfigurationManager.AppSettings["SADADPAYMENTTEST"];
                }
                else if (checkTest == "0")
                {
                    fileName = ConfigurationManager.AppSettings["SSLFILENAME"];
                    password = ConfigurationManager.AppSettings["SSLPASSWORD"];
                    requestUriString = ConfigurationManager.AppSettings["SADADPAYMENT"];
                }
                
                IWebProxy proxy = null;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Proxy = proxy;
                X509Certificate2 x509Certificate = new X509Certificate2(fileName, password);
                string result2;
                if (!x509Certificate.HasPrivateKey)
                {
                    result2 = "Error:(httpMethodPost): The certificate does not have a private key";
                    return result2;
                }
                httpWebRequest.ClientCertificates.Add(x509Certificate);
                ServicePointManager.ServerCertificateValidationCallback = ((object s, X509Certificate cert, X509Chain chain, SslPolicyErrors ssl) => true);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                byte[] bytes = Encoding.UTF8.GetBytes(postdata.ToString());
                httpWebRequest.ContentLength = (long)bytes.Length;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding(1256));
                result2 = streamReader.ReadToEnd();
                return result2;
            }
            catch (WebException ex)
            {
                result = "Error:" + ex.Message;

                clsDatabase objClsDatabase = new clsDatabase();

                objClsDatabase.LogError("clsServiceConnection_PostDataWithSignature", ex.Message);          
            }
            catch (Exception ex2)
            {
                result = "Error:(httpMethodPost):" + ex2.Message;

                clsDatabase objClsDatabase = new clsDatabase();

                objClsDatabase.LogError("clsServiceConnection_PostDataWithSignature", ex2.Message);          
            }
            return result;
        }

        /// <summary>
        /// Method for Digital sign Message
        /// </summary>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static string DigitalSignMessage(string postdata)
        {
            string result;
            string fileName = string.Empty;
            string password = string.Empty;
            string checkTest = ConfigurationManager.AppSettings["CHECKTEST"];
            try
            {
                if (checkTest == "1")
                {
                    fileName = ConfigurationManager.AppSettings["DIGITALFILENAMETEST"];
                    password = ConfigurationManager.AppSettings["DIGITALPASSWORDTEST"];
                }
                else if(checkTest == "0")
                {
                    fileName = ConfigurationManager.AppSettings["DIGITALFILENAME"];
                    password = ConfigurationManager.AppSettings["DIGITALPASSWORD"];
                }
               
                X509Certificate2 certificate = new X509Certificate2(fileName, password);
                string arg_17_0 = string.Empty;
                string text = string.Empty;
                string arg_23_0 = string.Empty;
                string text2 = string.Empty;
                Convert.ToBase64String(Encoding.ASCII.GetBytes(postdata));
                text2 = postdata.ToString();
                byte[] bytes = Encoding.ASCII.GetBytes(text2.ToString());
                SignedCms signedCms = new SignedCms();
                signedCms = new SignedCms(new ContentInfo(bytes), true);
                CmsSigner cmsSigner = new CmsSigner();
                signedCms.ComputeSignature(new CmsSigner(certificate)
                {
                    IncludeOption = X509IncludeOption.EndCertOnly
                });
                signedCms.CheckSignature(true);
                text = Convert.ToBase64String(signedCms.Encode());
                result = text;
            }
            catch (Exception ex)
            {
                result = "Error:(httpMethodPost):" + ex.Message;

                clsDatabase objClsDatabase = new clsDatabase();

                objClsDatabase.LogError("clsServiceConnection_DigitalSignMessage", ex.Message);          
            }
            return result;
        }
    }
}
