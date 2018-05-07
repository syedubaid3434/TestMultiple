using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Configuration;

namespace DALSADADUpload
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
            try
            {
                postdata = "strXmldata=" + postdata;
                string requestUriString = ConfigurationManager.AppSettings["SADADUPLOADURL"];
                string fileName = ConfigurationManager.AppSettings["FILENAME"];
                string password = ConfigurationManager.AppSettings["PASSWORD"];

                IWebProxy proxy = null;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Proxy = proxy;
                X509Certificate2 value = new X509Certificate2(fileName, password);
                httpWebRequest.ClientCertificates.Add(value);
                ServicePointManager.ServerCertificateValidationCallback = ((object s, X509Certificate cert, X509Chain chain, SslPolicyErrors ssl) => true);
                //Modified by meera 10-Mar-2017 for TLS 1.2 - Start
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 9999;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                //Modified by meera 10-Mar-2017 for TLS 1.2 - End

                byte[] bytes = Encoding.UTF8.GetBytes(postdata.ToString());
                httpWebRequest.ContentLength = (long)bytes.Length;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                MemoryStream memoryStream = new MemoryStream();
                using (Stream stream = new GZipStream(responseStream, CompressionMode.Decompress))
                {
                    byte[] array = new byte[1024];
                    int count;
                    while ((count = stream.Read(array, 0, array.Length)) > 0)
                    {
                        memoryStream.Write(array, 0, count);
                    }
                    byte[] array2 = memoryStream.ToArray();
                    result = Encoding.UTF8.GetString(array2, 0, array2.Length);
                }
            }
            catch (WebException ex)
            {
                result = "Error:" + ex.StackTrace;

                clsDatabase objDatabase = new clsDatabase();

                objDatabase.LogError("clsServiceConnection_PostDataWithSignature", ex.Message);

            }
            catch (Exception ex2)
            {
                result = "Error:" + ex2.StackTrace;

                clsDatabase objDatabase = new clsDatabase();

                objDatabase.LogError("clsServiceConnection_PostDataWithSignature", ex2.Message);
            }
            return result;
        }

        /// <summary>
        /// Method to Test post data with signature -- Added on 6th-Mar-2017 KISL
        /// </summary>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static string TestPostDataWithSignature(string postdata)
        {
            string result = string.Empty;
            try
            {
                postdata = "strXmldata=" + postdata;
                string requestUriString = ConfigurationManager.AppSettings["TESTSADADUPLOADURL"];
                string fileName = ConfigurationManager.AppSettings["TESTFILENAME"];
                string password = ConfigurationManager.AppSettings["TESTPASSWORD"];
                IWebProxy proxy = null;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Proxy = proxy;
                X509Certificate2 value = new X509Certificate2(fileName, password);
                httpWebRequest.ClientCertificates.Add(value);
                ServicePointManager.ServerCertificateValidationCallback = ((object s, X509Certificate cert, X509Chain chain, SslPolicyErrors ssl) => true);
                //Modified by meera 10-Mar-2017 for TLS 1.2 - Start
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 9999;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                //Modified by meera 10-Mar-2017 for TLS 1.2 - END

                byte[] bytes = Encoding.UTF8.GetBytes(postdata.ToString());
                httpWebRequest.ContentLength = (long)bytes.Length;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                MemoryStream memoryStream = new MemoryStream();
                using (Stream stream = new GZipStream(responseStream, CompressionMode.Decompress))
                {
                    byte[] array = new byte[1024];
                    int count;
                    while ((count = stream.Read(array, 0, array.Length)) > 0)
                    {
                        memoryStream.Write(array, 0, count);
                    }
                    byte[] array2 = memoryStream.ToArray();
                    result = Encoding.UTF8.GetString(array2, 0, array2.Length);
                }
            }
            catch (WebException ex)
            {
                result = "Error:" + ex.StackTrace;

                clsDatabase objDatabase = new clsDatabase();

                objDatabase.LogError("clsServiceConnection_TestPostDataWithSignature", ex.Message);

            }
            catch (Exception ex2)
            {
                result = "Error1:" + ex2.StackTrace;

                clsDatabase objDatabase = new clsDatabase();

                objDatabase.LogError("clsServiceConnection_TestPostDataWithSignature", ex2.Message);
            }
            return result;
        }

        /// <summary>
        /// Method for Digital signature
        /// </summary>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static string DigitalSignMessage(string postdata)
        {
            string result;
            try
            {
                string fileName = ConfigurationManager.AppSettings["FILENAME"];
                string password = ConfigurationManager.AppSettings["PASSWORD"];


                X509Certificate2 certificate = new X509Certificate2(fileName, password);
                string arg_17_0 = string.Empty;
                string txtSignedCms = string.Empty;
                string arg_23_0 = string.Empty;
                string txtPostData = string.Empty;
                Convert.ToBase64String(Encoding.ASCII.GetBytes(postdata));
                txtPostData = postdata.ToString();
                byte[] bytes = Encoding.ASCII.GetBytes(txtPostData.ToString());
                SignedCms signedCms = new SignedCms();
                signedCms = new SignedCms(new ContentInfo(bytes), true);
                CmsSigner cmsSigner = new CmsSigner();
                signedCms.ComputeSignature(new CmsSigner(certificate)
                {
                    IncludeOption = X509IncludeOption.EndCertOnly
                });
                signedCms.CheckSignature(true);
                txtSignedCms = Convert.ToBase64String(signedCms.Encode());
                result = txtSignedCms;
            }
            catch (Exception ex)
            {
                result = "Error:(httpMethodPost):" + ex.Message;

                clsDatabase objDatabase = new clsDatabase();

                objDatabase.LogError("clsServiceConnection_DigitalSignMessage", ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Method for Test Digital signature -- Added on 6th-Mar-2017 KISL
        /// </summary>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static string TestDigitalSignMessage(string postdata)
        {
            string result;
            try
            {
                string fileName = ConfigurationManager.AppSettings["TESTFILENAME"];
                string password = ConfigurationManager.AppSettings["TESTPASSWORD"];


                X509Certificate2 certificate = new X509Certificate2(fileName, password);
                string arg_17_0 = string.Empty;
                string txtSignedCms = string.Empty;
                string arg_23_0 = string.Empty;
                string txtPostData = string.Empty;
                Convert.ToBase64String(Encoding.ASCII.GetBytes(postdata));
                txtPostData = postdata.ToString();
                byte[] bytes = Encoding.ASCII.GetBytes(txtPostData.ToString());
                SignedCms signedCms = new SignedCms();
                signedCms = new SignedCms(new ContentInfo(bytes), true);
                CmsSigner cmsSigner = new CmsSigner();
                signedCms.ComputeSignature(new CmsSigner(certificate)
                {
                    IncludeOption = X509IncludeOption.EndCertOnly
                });
                signedCms.CheckSignature(true);
                txtSignedCms = Convert.ToBase64String(signedCms.Encode());
                result = txtSignedCms;
            }
            catch (Exception ex)
            {
                result = "Error:(httpMethodPost):" + ex.Message;

                clsDatabase objDatabase = new clsDatabase();

                objDatabase.LogError("clsServiceConnection_TestDigitalSignMessage", ex.Message);
            }
            return result;
        }
    }
}
