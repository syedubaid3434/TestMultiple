using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Net;
using DALSADADPayment;
using SADADPayment.ServiceReference1;
using System.Configuration;

namespace SADADPayment
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class IjarahSadadPaymentAlert : IIjarahSadadPaymentAlert
    {
        string smsUrl = ConfigurationManager.AppSettings["SMSURL"];

        clsDatabase objClsDatabase = new clsDatabase();

        string methodName;


        /// <summary>
        /// Method for SADAD Payment Alert
        /// </summary>
        /// <param name="MessageObject"></param>
        /// <returns></returns>
        public Stream sadadpaymentalert(Message MessageObject)
        {
            try
            {
                string text = "OK";
                if (MessageObject != null)
                {
                    clsDatabase clsDatabase = new clsDatabase();
                    string txtCustomerReferenceNo = string.Empty;
                    string txtAmount = string.Empty;
                    string txtDescription = string.Empty;
                    string txtCustomerRefNo = string.Empty;
                    string arg_31_0 = string.Empty;
                    string txtCustRefNo = string.Empty;
                    string txtSadadAlert = string.Empty;
                    string txtSADADID = "0";
                    string txtDateTime = string.Empty;
                    txtSadadAlert = "select apps.xxi_sadad_alert_s.nextval from dual";
                    object obj = clsDatabase.ExecuteScalar(txtSadadAlert);
                    if (obj != null && obj != DBNull.Value && obj.ToString().Trim() != string.Empty)
                    {
                        txtSADADID = obj.ToString().Trim();
                    }
                    string txtStringSADADID = Convert.ToString(txtSADADID);
                    if (MessageObject.objBody.CustomerRefNo != null && MessageObject.objBody.CustomerRefNo.Trim() != string.Empty)
                    {
                        MessageObject.objBody.CustomerRefNo.Trim();
                        txtCustomerRefNo = MessageObject.objBody.CustomerRefNo.Trim();
                        txtCustomerRefNo = txtCustomerRefNo.Substring(5).Trim();
                    }
                    txtSadadAlert = string.Concat(new string[]
			{
				"insert into XXI_SADAD_ALERT_LOG(SENDERID,RECEIVERID,MESSAGETYPE,TIMESTAMP,ACCOUNTNO,AMOUNT,CUSTOMERREFNO,TRANSTYPE,DESCRIPTION,SADADID)values('",
				MessageObject.objHeader.Sender.ToString(),
				"','",
				MessageObject.objHeader.Receiver.ToString(),
				"','",
				MessageObject.objHeader.MessageType.ToString(),
				"','",
				MessageObject.objHeader.TimeStamp.ToString(),
				"','",
				MessageObject.objBody.AccountNo.ToString(),
				"','",
				MessageObject.objBody.Amount.ToString(),
				"','",
				txtCustomerRefNo,
				"','",
				MessageObject.objBody.TransType.ToString(),
				"','",
				MessageObject.objBody.Description.ToString(),
				"','",
				txtStringSADADID,
				"')"
			});
                    clsDatabase.ExecuteNonSybaseQuery(txtSadadAlert.ToString());
                    txtSadadAlert = string.Concat(new string[]
			{
				"insert into XXI_SADAD_ALERT_LOG(SENDERID,RECEIVERID,MESSAGETYPE,TIMESTAMP,ACCOUNTNO,AMOUNT,CUSTOMERREFNO,TRANSTYPE,DESCRIPTION,SADADID)values('",
				MessageObject.objHeader.Sender.ToString(),
				"','",
				MessageObject.objHeader.Receiver.ToString(),
				"','",
				MessageObject.objHeader.MessageType.ToString(),
				"','",
				MessageObject.objHeader.TimeStamp.ToString(),
				"','",
				MessageObject.objBody.AccountNo.ToString(),
				"','",
				MessageObject.objBody.Amount.ToString(),
				"','",
				txtCustomerRefNo,
				"','",
				MessageObject.objBody.TransType.ToString(),
				"','",
				MessageObject.objBody.Description.ToString(),
				"',",
				txtSADADID.Trim(),
				")"
			});
                    clsDatabase.ExecuteNonQuery(txtSadadAlert.ToString());
                    txtDateTime = DateTime.Now.ToString("dd/MMM/yyyy");
                    DateTime dateTime;
                    if (DateTime.TryParse(MessageObject.objHeader.TimeStamp.ToString().Trim(), out dateTime))
                    {
                        txtDateTime = DateTime.Parse(MessageObject.objHeader.TimeStamp.ToString().Trim()).ToString("dd/MMM/yyyy");
                    }
                    if (MessageObject.objBody.CustomerRefNo != null && MessageObject.objBody.CustomerRefNo.Trim() != string.Empty)
                    {
                        txtCustRefNo = MessageObject.objBody.CustomerRefNo.Trim();
                        txtCustomerReferenceNo = MessageObject.objBody.CustomerRefNo.Trim();
                        txtCustomerReferenceNo = txtCustomerReferenceNo.Substring(9).Trim();
                        if (txtCustomerReferenceNo.Trim().Length > 10 && txtCustomerReferenceNo.Substring(0, 1).Trim() == "0")
                        {
                            txtCustomerReferenceNo = txtCustomerReferenceNo.Substring(1).Trim();
                        }
                    }
                    if (MessageObject.objBody.Amount != null && MessageObject.objBody.Amount.Trim() != string.Empty)
                    {
                        txtAmount = MessageObject.objBody.Amount.Trim();
                        txtAmount = txtAmount.Replace(',', '.').Trim();
                    }
                    if (MessageObject.objBody.Description != null && MessageObject.objBody.Description.Trim() != string.Empty)
                    {
                        txtDescription = MessageObject.objBody.Description.Trim();
                        txtDescription = txtDescription.Substring(txtDescription.ToUpper().IndexOf('N') + 1, txtDescription.Length - (txtDescription.ToUpper().IndexOf('N') + 1)).Trim();
                    }
                    txtSadadAlert = string.Concat(new string[]
			{
				"call apps.xxi_sadad_recappl.create_receipt_and_apply('",
				txtCustomerReferenceNo.Trim(),
				"','",
				txtAmount.Trim(),
				"','",
				txtDescription.Trim(),
				"','",
				txtDateTime.Trim(),
				"')"
			});
                    clsDatabase.ExecuteNonQuery(txtSadadAlert.Trim());
                    string txtPreferredLanguage = string.Empty;
                    string txtPreferredFirstName = string.Empty;
                    string txtPreferredGender = string.Empty;
                    string txtPaymentMessage = string.Empty;
                    string txtPaymentReceivedMessage = string.Empty;
                    string email = string.Empty;
                    txtPreferredLanguage = this.GetPreferredLanguage(txtCustomerReferenceNo.Trim());
                    txtPreferredFirstName = this.GetPreferredFirstName(txtCustRefNo.Trim(), txtPreferredLanguage);
                    txtPreferredGender = this.GetPreferredGender(txtCustRefNo.Trim());
                    if (txtPreferredGender == "M")
                    {
                        txtPreferredGender = "عزيزي";
                    }
                    else if (txtPreferredGender == "F")
                    {
                        txtPreferredGender = "عزيزتي";
                    }
                    decimal num = Convert.ToDecimal(txtAmount.Trim());
                    if (txtPreferredLanguage == "ENU")
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append("Dear Customer, \n");
                        stringBuilder.Append("Your Payment of ×12× SAR has been processed for Contract Number #AMT#.\n");
                        stringBuilder.Append("Thank you for choosing Ijarah");
                        string txtEngPaymentMsg = stringBuilder.ToString();
                        txtPaymentMessage = txtEngPaymentMsg;
                    }
                    else if (txtPreferredLanguage == "ARA")
                    {
                        StringBuilder stringBuilder2 = new StringBuilder();
                        stringBuilder2.Append("عزيزي العميل \n");
                        stringBuilder2.Append("لقد تم استلام مبلغ ×12× ريال لحساب عقدكم رقم #AMT#\n");
                        stringBuilder2.Append("شكرا لإختياركم إيجارة");
                        string txtArabicPaymentMsg = stringBuilder2.ToString();
                        txtPaymentMessage = txtArabicPaymentMsg;
                    }
                    if (txtPreferredLanguage == "ENU")
                    {
                        StringBuilder stringBuilder3 = new StringBuilder();
                        stringBuilder3.Append("Dear #REFNAME#, \n");
                        stringBuilder3.Append("Your payment of #REFAMT# SAR has been received.\n");
                        stringBuilder3.Append("Thank you.");
                        string txtEngPaymentReceived = stringBuilder3.ToString();
                        txtPaymentReceivedMessage = txtEngPaymentReceived;
                        txtPaymentReceivedMessage = txtPaymentReceivedMessage.Replace("#REFNAME#", txtPreferredFirstName);
                        txtPaymentReceivedMessage = txtPaymentReceivedMessage.Replace("#REFAMT#", num.ToString("N2"));
                    }
                    else if (txtPreferredLanguage == "ARA")
                    {
                        StringBuilder stringBuilder4 = new StringBuilder();
                        stringBuilder4.Append("#REFGENDER#/#REFNAME#\n");
                        stringBuilder4.Append("تم إستلام مبلغ #REFAMT# ريال\n");
                        stringBuilder4.Append("شكرا");
                        string txtArabicPaymentReceived = stringBuilder4.ToString();
                        txtPaymentReceivedMessage = txtArabicPaymentReceived;
                        txtPaymentReceivedMessage = txtPaymentReceivedMessage.Replace("#REFGENDER#", txtPreferredGender);
                        txtPaymentReceivedMessage = txtPaymentReceivedMessage.Replace("#REFNAME#", txtPreferredFirstName);
                        txtPaymentReceivedMessage = txtPaymentReceivedMessage.Replace("#REFAMT#", num.ToString("N2"));
                    }
                    txtPaymentMessage = txtPaymentMessage.Replace("×12×", num.ToString("N2"));
                    txtPaymentMessage = txtPaymentMessage.Replace("#AMT#", txtCustomerRefNo.Trim());
                    txtSadadAlert = "select count(*) as num from XXI_SADAD_ALERT_LOG where DESCRIPTION='" + MessageObject.objBody.Description + "'";
                    object obj2 = clsDatabase.ExecuteScalar(txtSadadAlert);
                    if (obj2 != null && obj2 != DBNull.Value && int.Parse(obj2.ToString().Trim()) == 1)
                    {
                        txtSadadAlert = "select STATUS as smscount from XXI_SADAD_ALERT_LOG where DESCRIPTION='" + MessageObject.objBody.Description + "'";
                        object obj3 = clsDatabase.ExecuteScalar(txtSadadAlert);
                        if (obj3 != null && obj3 != DBNull.Value)
                        {
                            if (obj3.ToString() == "PROCESSED")
                            {
                                this.SendSMStest(txtPaymentMessage.Trim(), txtCustomerReferenceNo.Trim(), txtSADADID.Trim());
                            }
                            else if (obj3.ToString() == "PROCESS_DP")
                            {
                                email = this.GetCustomerEmailAddressFromSiebel(txtCustomerReferenceNo.Trim());
                                this.SendSMS(txtPaymentReceivedMessage.Trim(), txtCustomerReferenceNo.Trim(), txtSADADID.Trim(), txtPreferredLanguage, txtPreferredFirstName, txtPreferredGender, num.ToString("N2"), email);
                            }
                        }
                    }
                }
                else
                {
                    text = "ERR";
                }
                RBAlertMessageReply value = new RBAlertMessageReply
                {
                    HeaderRep = new RBHeaderReply
                    {
                        Sender = "IJARAH",
                        Receiver = "RYBK",
                        MessageType = "ACNRPLY",
                        TimeStamp = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss")
                    },
                    Status = text.ToString()
                };
                string s = clsSerializationDeserialization<RBAlertMessageReply>.Serialize<RBAlertMessageReply>(value);
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                OutgoingWebResponseContext outgoingResponse = WebOperationContext.Current.OutgoingResponse;
                outgoingResponse.ContentType = "text/plain";
                return new MemoryStream(bytes);
            }

            catch (Exception ex)
            {
                methodName = "IjarahSadadPaymentAlert_sadadpaymentalert";

                objClsDatabase.LogError(methodName, ex.Message);

                return Stream.Null;
            }
        }

        /// <summary>
        /// Method to Send SMS
        /// </summary>
        /// <param name="SMSTemplate"></param>
        /// <param name="CustomerAccNo"></param>
        /// <param name="SADADID"></param>
        /// <param name="Language"></param>
        /// <param name="Name"></param>
        /// <param name="Gender"></param>
        /// <param name="PaymentAmount"></param>
        /// <param name="Email"></param>
        private void SendSMS(string SMSTemplate, string CustomerAccNo, string SADADID, string Language, string Name, string Gender, string PaymentAmount, string Email)
        {
            string text = string.Empty;
            try
            {
                string arg_0B_0 = string.Empty;
                string arg_11_0 = string.Empty;
                SendSaleEmail sendSaleEmail = new SendSaleEmail();
                sendSaleEmail.RecipientsTo = Email;
                sendSaleEmail.ApplicationNum = CustomerAccNo.Trim();
                sendSaleEmail.Totalamount = PaymentAmount;
                sendSaleEmail.Firstname = Name;
                sendSaleEmail.MessageCode = "ENDP0104";
                if (Language == "ARA")
                {
                    sendSaleEmail.MessageType = "U";
                }
                else
                {
                    sendSaleEmail.MessageType = "N";
                }
                sendSaleEmail.Gender = Gender;
                IijaraMessagingGatewayClient iijaraMessagingGatewayClient = new IijaraMessagingGatewayClient();
                iijaraMessagingGatewayClient.Open();
                iijaraMessagingGatewayClient.SendSaleEmail(sendSaleEmail);
                iijaraMessagingGatewayClient.Close();

                CommonSMS(CustomerAccNo, SMSTemplate, SADADID);

            }
            catch (Exception ex)
            {
                text = ex.Message.ToString().Trim();

                methodName = "IjarahSadadPaymentAlert_SendSMS";

                objClsDatabase.LogError(methodName, ex.Message);
            }
        }

        /// <summary>
        /// Method to Send SMS Test
        /// </summary>
        /// <param name="SMSTemplate"></param>
        /// <param name="CustomerAccNo"></param>
        /// <param name="SADADID"></param>
        private void SendSMStest(string SMSTemplate, string CustomerAccNo, string SADADID)
        {
            string text = string.Empty;
            try
            {
                CommonSMS(CustomerAccNo, SMSTemplate, SADADID);
            }
            catch (Exception ex)
            {
                text = ex.Message.ToString().Trim();

                methodName = "IjarahSadadPaymentAlert_SendSMStest";

                objClsDatabase.LogError(methodName, ex.Message);
            }
        }

        /// <summary>
        /// Method to Convert Text to Hex
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string ConvertTextTohex(string message)
        {
            try
            {
                string text = "";
                for (int i = 0; i < message.Length; i++)
                {
                    char c = message[i];
                    int num = (int)c;
                    string str = num.ToString("X4");
                    text += str;
                }
                return text;
            }

            catch (Exception ex)
            {
                methodName = "IjarahSadadPaymentAlert_ConvertTextTohex";

                objClsDatabase.LogError(methodName, ex.Message);

                return string.Empty;
            }
        }

        /// <summary>
        /// Method to get Customer Email Address from Siebel
        /// </summary>
        /// <param name="CustomerAccountNumber"></param>
        /// <returns></returns>
        private string GetCustomerEmailAddressFromSiebel(string CustomerAccountNumber)
        {
            try
            {
                string result = string.Empty;
                clsDatabase clsDatabase = new clsDatabase();
                string queryString = "SELECT con.email_address FROM siebel.XXI_SEIBEL_OPP_INFO_V opp, siebel.XXI_SIEBEL_CONTACTS_V con WHERE opp.CONTACT_ID = con.CONTACT_ID and opp.opportunity_name='" + CustomerAccountNumber.Trim() + "'";
                object obj = clsDatabase.ExecuteScalarFromSiebel(queryString);
                if (obj != null && obj != DBNull.Value && obj.ToString().Trim() != string.Empty)
                {
                    result = obj.ToString();
                }
                return result;
            }

            catch (Exception ex)
            {
                methodName = "IjarahSadadPaymentAlert_GetCustomerEmailAddressFromSiebel";

                objClsDatabase.LogError(methodName, ex.Message);

                return string.Empty;
            }
        }

        /// <summary>
        /// Method to get Customer Contact Number from Siebel
        /// </summary>
        /// <param name="CustomerAccountNumber"></param>
        /// <returns></returns>
        private string GetCustomerContactNumberFromSiebel(string CustomerAccountNumber)
        {
            try
            {
                string text = string.Empty;
                clsDatabase clsDatabase = new clsDatabase();
                string queryString = "SELECT con.mobile_number,con.pref_lang FROM siebel.XXI_SEIBEL_OPP_INFO_V opp,siebel.XXI_SIEBEL_CONTACTS_V con where opp.CONTACT_ID=con.CONTACT_ID and opp.opportunity_name='" + CustomerAccountNumber.Trim() + "'";
                object obj = clsDatabase.ExecuteScalarFromSiebel(queryString);
                if (obj != null && obj != DBNull.Value && obj.ToString().Trim() != string.Empty)
                {
                    if (obj.ToString().Trim().Length > 9)
                    {
                        text = obj.ToString().Trim().Substring(obj.ToString().Trim().Length - 9);
                    }
                    text = "966" + text.Trim();
                }
                return text;
            }

            catch (Exception ex)
            {
                methodName = "IjarahSadadPaymentAlert_GetCustomerContactNumberFromSiebel";

                objClsDatabase.LogError(methodName, ex.Message);

                return string.Empty;
            }
        }

        /// <summary>
        /// Method to get Preferred Language
        /// </summary>
        /// <param name="CustomerAccountNumber"></param>
        /// <returns></returns>
        private string GetPreferredLanguage(string CustomerAccountNumber)
        {
            try
            {
                string result = string.Empty;
                clsDatabase clsDatabase = new clsDatabase();
                string queryString = "SELECT con.pref_lang FROM siebel.XXI_SEIBEL_OPP_INFO_V opp,siebel.XXI_SIEBEL_CONTACTS_V con where opp.CONTACT_ID=con.CONTACT_ID and opp.opportunity_name='" + CustomerAccountNumber.Trim() + "'";
                object obj = clsDatabase.ExecuteScalarFromSiebel(queryString);
                if (obj != null && obj != DBNull.Value)
                {
                    result = obj.ToString();
                }
                else
                {
                    result = "ARA";
                }
                return result;
            }

            catch (Exception ex)
            {
                methodName = "IjarahSadadPaymentAlert_GetPreferredLanguage";

                objClsDatabase.LogError(methodName, ex.Message);

                return string.Empty;
            }
        }

        /// <summary>
        /// Method to get Preferred First Name
        /// </summary>
        /// <param name="CustomerAccountNumber"></param>
        /// <param name="Language"></param>
        /// <returns></returns>
        private string GetPreferredFirstName(string CustomerAccountNumber, string Language)
        {
            try
            {
                string result = string.Empty;
                clsDatabase clsDatabase = new clsDatabase();
                string queryString = string.Empty;
                if (Language == "ENU")
                {
                    queryString = "SELECT con.first_name_english FROM siebel.XXI_SEIBEL_OPP_INFO_V opp,siebel.XXI_SIEBEL_CONTACTS_V con where opp.CONTACT_ID=con.CONTACT_ID and opp.opportunity_name='" + CustomerAccountNumber.Trim() + "'";
                }
                else if (Language == "ARA")
                {
                    queryString = "SELECT con.first_name FROM siebel.XXI_SEIBEL_OPP_INFO_V opp,siebel.XXI_SIEBEL_CONTACTS_V con where opp.CONTACT_ID=con.CONTACT_ID and opp.opportunity_name='" + CustomerAccountNumber.Trim() + "'";
                }
                object obj = clsDatabase.ExecuteScalarFromSiebel(queryString);
                if (obj != null && obj != DBNull.Value)
                {
                    result = obj.ToString();
                }
                return result;
            }

            catch (Exception ex)
            {
                methodName = "IjarahSadadPaymentAlert_GetPreferredFirstName";

                objClsDatabase.LogError(methodName, ex.Message);

                return string.Empty;
            }
        }

        /// <summary>
        /// Method to get Preferred Gender
        /// </summary>
        /// <param name="CustomerAccountNumber"></param>
        /// <returns></returns>
        private string GetPreferredGender(string CustomerAccountNumber)
        {
            try
            {
                string result = string.Empty;
                clsDatabase clsDatabase = new clsDatabase();
                string queryString = string.Empty;
                queryString = "SELECT con.gender FROM siebel.XXI_SEIBEL_OPP_INFO_V opp,siebel.XXI_SIEBEL_CONTACTS_V con where opp.CONTACT_ID=con.CONTACT_ID and opp.opportunity_name='" + CustomerAccountNumber.Trim() + "'";
                object obj = clsDatabase.ExecuteScalarFromSiebel(queryString);
                if (obj != null && obj != DBNull.Value)
                {
                    result = obj.ToString();
                }
                return result;
            }

            catch (Exception ex)
            {
                methodName = "IjarahSadadPaymentAlert_GetPreferredGender";

                objClsDatabase.LogError(methodName, ex.Message);

                return string.Empty;
            }
        }

        Comment IIjarahSadadPaymentAlert.MyComment(Comment comment)
        {
            return comment;
        }

        /// <summary>
        /// Method to create Text File
        /// </summary>
        /// <param name="TranType"></param>
        /// <param name="FileSequenceID"></param>
        /// <param name="FileData"></param>
        private void CreateTextFile(string TranType, string FileSequenceID, string FileData)
        {
            string str = string.Empty;
            string text = ConfigurationManager.AppSettings["TEXTFILEPATH"];

            Random random = new Random();
            try
            {
                this.CreateFolder(text);
                str = string.Concat(new string[]
			{
				"\\",
				TranType,
				FileSequenceID,
				"_",
				random.Next(0, 1000).ToString(),
				".txt"
			});
                File.WriteAllText(text + str, FileData);
            }
            catch (Exception ex)
            {
                FileData = FileData + Environment.NewLine + Environment.NewLine + ex.Message.ToString();
                str = string.Concat(new string[]
			{
				"\\",
				TranType,
				FileSequenceID,
				"_",
				random.Next(0, 1000).ToString(),
				".txt"
			});
                File.WriteAllText(text + str, FileData);

                methodName = "IjarahSadadPaymentAlert_CreateTextFile";

                objClsDatabase.LogError(methodName, ex.Message);
            }
        }

        /// <summary>
        /// Method to Create Folder
        /// </summary>
        /// <param name="path"></param>
        private void CreateFolder(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            catch (Exception ex)
            {
                methodName = "IjarahSadadPaymentAlert_CreateFolder";

                objClsDatabase.LogError(methodName, ex.Message);
            }
        }

        /// <summary>
        /// Method for Common SMS send functionality
        /// </summary>
        /// <param name="CustomerAccNo"></param>
        /// <param name="SMSTemplate"></param>
        /// <param name="SADADID"></param>
        private void CommonSMS(string CustomerAccNo, string SMSTemplate, string SADADID)
        {
            try
            {
                string text = string.Empty;

                text = this.GetCustomerContactNumberFromSiebel(CustomerAccNo.Trim());
                if (text.Trim() != string.Empty)
                {
                    string text2 = this.ConvertTextTohex(SMSTemplate.Trim());
                    string requestUriString = string.Concat(new string[]
				{
					smsUrl,
					text.Trim(),
					"&from=IJARAH&message=",
					text2,
					"&lang=8&action=send"
				});
                    CookieContainer cookieContainer = new CookieContainer();
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                    httpWebRequest.Proxy = null;
                    httpWebRequest.UseDefaultCredentials = true;
                    httpWebRequest.KeepAlive = false;
                    httpWebRequest.ProtocolVersion = HttpVersion.Version10;
                    httpWebRequest.CookieContainer = cookieContainer;
                    WebResponse response = httpWebRequest.GetResponse();
                    string arg_14F_0 = ((HttpWebResponse)response).StatusDescription;
                    Stream responseStream = response.GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                    string text3 = streamReader.ReadToEnd();
                    string text4 = string.Empty;
                    string text5 = string.Empty;
                    if (text3.Contains("Status : 200"))
                    {
                        text4 = "200";
                        text5 = "SUCCESS";
                    }
                    else if (text3.Contains("Status : 201"))
                    {
                        text4 = "201";
                        text5 = "USER_ID IS NOT PASSED";
                    }
                    else if (text3.Contains("Status : 202"))
                    {
                        text4 = "202";
                        text5 = "PASSWORD IS NOT PASSED";
                    }
                    else if (text3.Contains("Status : 207"))
                    {
                        text4 = "207";
                        text5 = "ACTION IS NOT PASSED";
                    }
                    else if (text3.Contains("Status : -998"))
                    {
                        text4 = "-998";
                        text5 = "APPLICATION ERROR";
                    }
                    else if (text3.Contains("Status : -999"))
                    {
                        text4 = "-999";
                        text5 = "DATABASE ERROR";
                    }
                    else if (text3.Contains("Status : 224"))
                    {
                        text4 = "224";
                        text5 = "ERROR";
                    }
                    clsDatabase clsDatabase = new clsDatabase();
                    string text6 = string.Concat(new string[]
				{
					"update XXI_SADAD_ALERT_LOG set MOBILE_NUMBER='",
					text.Trim(),
					"',SMSREPLYCODE='",
					text4.Trim(),
					"',SMSREPLYDESC='",
					text5.Trim(),
					"' where DESCRIPTION='",
					SADADID.Trim(),
					"'"
				});
                    clsDatabase.ExecuteNonQuery(text6.ToString());
                }
            }
            catch (Exception ex)
            {
                methodName = "IjarahSadadPaymentAlert_CommonSMS";

                objClsDatabase.LogError(methodName, ex.Message);
            }
        }
    }
}
