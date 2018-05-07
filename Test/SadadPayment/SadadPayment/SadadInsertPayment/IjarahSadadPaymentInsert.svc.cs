using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using DALSADADPayment;
using System.Configuration;
using System.IO;
using System.Data;
using System.Net;
using SadadInsertPayment.ServiceReference1;
namespace SadadInsertPayment
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "IjarahSadadPaymentInsert" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select IjarahSadadPaymentInsert.svc or IjarahSadadPaymentInsert.svc.cs at the Solution Explorer and start debugging.
    public class IjarahSadadPaymentInsert : IIjarahSadadPaymentInsert
    {
        string smsUrl = ConfigurationManager.AppSettings["SMSURL"];

        clsDatabase objClsDatabase = new clsDatabase();

        string methodName;

        public void SadadPaymentInsert()
        {
            string retValue = string.Empty;
            string txtNotProcessRecords = string.Empty;
            string txtSadadAlert = string.Empty;
            string txtSadadAlertdup = string.Empty;
            string txtSADADID = string.Empty;
            string txtCustomerRefNo = string.Empty;
            string txtDateTime = string.Empty;
            string txtCustRefNo = string.Empty;
            string txtCustomerReferenceNo = string.Empty;
            string txtAmount = string.Empty;
            string txtDescription = string.Empty;
            string contractNo = string.Empty;
            object objCheckData = null;
            DataSet objResult = new DataSet();

            try
            {
                Message MessageObject;
                Body objBody;
                Header objHeader;

                txtNotProcessRecords = "select * from xxi_sadad_alert_log_fr where STATUS is null";

                objResult = objClsDatabase.ExecuteDatamySql(txtNotProcessRecords);
                if (objResult != null && objResult.ToString() != string.Empty)
                {
                    if (objResult.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in objResult.Tables[0].Rows)
                        {
                            objHeader = new Header();
                            objHeader.Sender = dr["SENDERID"].ToString();
                            objHeader.Receiver = dr["RECEIVERID"].ToString();
                            objHeader.MessageType = dr["MESSAGETYPE"].ToString();
                            objHeader.TimeStamp = dr["TIMESTAMP"].ToString();

                            objBody = new Body();
                            objBody.AccountNo = dr["ACCOUNTNO"].ToString();
                            objBody.Amount = dr["AMOUNT"].ToString();
                            objBody.CustomerRefNo = dr["CUSTOMERREFNO"].ToString();
                            objBody.Description = dr["DESCRIPTION"].ToString();
                            objBody.TransType = dr["TRANSTYPE"].ToString();

                            MessageObject = new Message();
                            MessageObject.objHeader = objHeader;
                            MessageObject.objBody = objBody;

                            if (MessageObject != null)
                            {
                                txtSadadAlert = "select apps.xxi_sadad_alert_s.nextval from dual";
                                object obj = objClsDatabase.ExecuteScalar(txtSadadAlert);
                                if (obj != null && obj != DBNull.Value && obj.ToString().Trim() != string.Empty)
                                {
                                    txtSADADID = obj.ToString().Trim();
                                }
                                string txtStringSADADID = Convert.ToString(txtSADADID);

                                if ((MessageObject.objBody.CustomerRefNo.ToString() != null) && (MessageObject.objBody.CustomerRefNo.ToString() != string.Empty))
                                {
                                    txtCustomerRefNo = MessageObject.objBody.CustomerRefNo.ToString().Trim();
                                    txtCustomerRefNo = txtCustomerRefNo.Substring(5).Trim();
                                }

                                txtSadadAlert = "select count(*) as num from XXI_SADAD_ALERT_LOG where DESCRIPTION='" + MessageObject.objBody.Description.ToString() + "'";
                                objCheckData = objClsDatabase.ExecuteScalar(txtSadadAlert);

                                if ((objCheckData != null) && (objCheckData != DBNull.Value) && (objCheckData.ToString() != "0"))
                                {
                                    txtSadadAlert = "select STATUS as smscount from XXI_SADAD_ALERT_LOG where DESCRIPTION='" + MessageObject.objBody.Description + "'";
                                    object objcheckStatus = objClsDatabase.ExecuteScalar(txtSadadAlert);
                                    if ((objcheckStatus.ToString() == "PROCESSED") || (objcheckStatus.ToString() == "NEW") || (objcheckStatus.ToString() == "PROCESS_DP") || (objcheckStatus.ToString() == "DUPLICATE"))
                                    {
                                        txtSadadAlertdup = string.Concat(new string[]
                            {
                                        "insert into xxi_sadad_alert_log_dup(SENDERID,RECEIVERID,MESSAGETYPE,TIMESTAMP,ACCOUNTNO,AMOUNT,CUSTOMERREFNO,TRANSTYPE,DESCRIPTION,SADADID)values('",
                                        MessageObject.objHeader.Sender.ToString().Trim(),
                                        "','",
                                        MessageObject.objHeader.Receiver.Trim(),
                                        "','",
                                        MessageObject.objHeader.MessageType.ToString().Trim(),
                                        "','",
                                        MessageObject.objHeader.TimeStamp.ToString().Trim(),
                                        "','",
                                        MessageObject.objBody.AccountNo.ToString().Trim(),
                                        "','",
                                        MessageObject.objBody.Amount.ToString().Trim(),
                                        "','",
                                        txtCustomerRefNo,
                                        "','",
                                        MessageObject.objBody.TransType.ToString().Trim(),
                                        "','",
                                        MessageObject.objBody.Description.ToString().Trim(),
                                        "','",
                                        txtSADADID,
                                        "')"
                            });
                                        objClsDatabase.ExecuteNonQuerymySql(txtSadadAlertdup);

                                        txtSadadAlert = "Update xxi_sadad_alert_log_fr set STATUS = '" + objcheckStatus.ToString() + "', CreatedOn = NOW() where DESCRIPTION='" + dr["DESCRIPTION"].ToString().Trim() + "'";
                                        objClsDatabase.ExecuteNonQuerymySql(txtSadadAlert.ToString());

                                        return;

                                    }
                                    txtSadadAlert = "Update xxi_sadad_alert_log_fr set STATUS = 'Exists', CreatedOn = NOW() where DESCRIPTION='" + dr["DESCRIPTION"].ToString().Trim() + "'";
                                    objClsDatabase.ExecuteNonQuerymySql(txtSadadAlert.ToString());
                                }
                                else
                                {
                                    //If new record found in the table, the details to be transferred to Sybase and Oracle table (table : XXI_SADAD_ALERT_LOG)

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

                                    objClsDatabase.ExecuteNonQuery(txtSadadAlert.ToString());




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
                                    objClsDatabase.ExecuteNonSybaseQuery(txtSadadAlert.ToString());


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
                                //"call apps.xxi_sadad_recappl.create_receipt_and_apply('",
                                //txtCustomerReferenceNo.Trim(),
                                //"','",
                                //txtAmount.Trim(),
                                //"','",
                                //txtDescription.Trim(),
                                //"','",
                                //txtDateTime.Trim(),
                                //"')"


                                 //Added on 04-12-2017 for new package update : Satish.M
                                "call APPS.ijr_payments_import_pkg.create_sadad_receipts('",
                                txtCustomerReferenceNo.Trim(),
                                "','",
                                txtAmount.Trim(),
                                "','",
                                txtDescription.Trim(),
                                "','",
                                txtDateTime.Trim(),
                                "')"
                });
                                    objClsDatabase.ExecuteNonQuery(txtSadadAlert.Trim());  //Query (Package) which updates the status accordingly.
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
                                    txtPaymentMessage = txtPaymentMessage.Replace("#AMT#", txtCustomerRefNo.Substring(5).Trim());
                                    txtPaymentMessage = txtPaymentMessage.Replace("\n", Environment.NewLine); //added by KISL : Satish.M on 20072017 for new sms url
                                    txtPaymentReceivedMessage = txtPaymentReceivedMessage.Replace("\n", Environment.NewLine);

                                    byte[] bytes_txtPaymentMessage = Encoding.UTF8.GetBytes(txtPaymentMessage); //edited by Satish.M on 05-12-2017 for sms template issue
                                    txtPaymentMessage = Encoding.UTF8.GetString(bytes_txtPaymentMessage);

                                    byte[] bytes_txtPaymentReceivedMessage = Encoding.UTF8.GetBytes(txtPaymentReceivedMessage);
                                    txtPaymentReceivedMessage = Encoding.UTF8.GetString(bytes_txtPaymentReceivedMessage);

                                    contractNo = txtCustomerRefNo.Substring(5);

                                    txtSadadAlert = "select count(*) as num from XXI_SADAD_ALERT_LOG where DESCRIPTION='" + MessageObject.objBody.Description.ToString().Trim() + "'";
                                    object obj2 = objClsDatabase.ExecuteScalar(txtSadadAlert);
                                    if (obj2 != null && obj2 != DBNull.Value && int.Parse(obj2.ToString().Trim()) == 1)
                                    {
                                        txtSadadAlert = "select STATUS as smscount from XXI_SADAD_ALERT_LOG where DESCRIPTION='" + MessageObject.objBody.Description.ToString().Trim() + "'";
                                        object obj3 = objClsDatabase.ExecuteScalar(txtSadadAlert);
                                        if (obj3 != null && obj3 != DBNull.Value)
                                        {
                                            if (obj3.ToString() == "PROCESSED")
                                            {
                                                this.SendSMStest(txtPaymentMessage.Trim(), txtCustomerReferenceNo.Trim(), txtSADADID.Trim() , txtPreferredLanguage, contractNo.Trim());
                                            }
                                            else if (obj3.ToString() == "PROCESS_DP")
                                            {
                                                email = this.GetCustomerEmailAddressFromSiebel(txtCustomerReferenceNo.Trim());
                                                this.SendSMS(txtPaymentReceivedMessage.Trim(), txtCustomerReferenceNo.Trim(), txtSADADID.Trim(), txtPreferredLanguage, txtPreferredFirstName, txtPreferredGender, num.ToString("N2"), email, contractNo.Trim());
                                            }

                                            txtSadadAlert = "Update xxi_sadad_alert_log_fr set STATUS = '" + obj3.ToString() + "', CreatedOn = NOW() where DESCRIPTION='" + dr["DESCRIPTION"].ToString().Trim() + "'";
                                            objClsDatabase.ExecuteNonQuerymySql(txtSadadAlert.ToString());
                                            retValue = obj3.ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                methodName = "IjarahSadadPaymentAlert_SadadPaymentInsert";

                objClsDatabase.LogError(methodName, ex.Message);
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
        private void SendSMS(string SMSTemplate, string CustomerAccNo, string SADADID, string Language, string Name, string Gender, string PaymentAmount, string Email, string ContractNo)
        {
            string text = string.Empty;
            try
            {
                IijaraMessagingGatewayClient iijaraMessagingGatewayClient = new IijaraMessagingGatewayClient();
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
                iijaraMessagingGatewayClient.Open();
                iijaraMessagingGatewayClient.SendSaleEmail(sendSaleEmail);
                iijaraMessagingGatewayClient.Close();

                CommonSMS(CustomerAccNo, SMSTemplate, SADADID, Language, ContractNo);

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
        private void SendSMStest(string SMSTemplate, string CustomerAccNo, string SADADID , string Language, string ContractNo)
        {
            string text = string.Empty;
            try
            {
                CommonSMS(CustomerAccNo, SMSTemplate, SADADID, Language, ContractNo);
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
        private void CommonSMS(string CustomerAccNo, string SMSTemplate, string SADADID, string Language, string ContactNo)
        {
            try
            {
                string text = string.Empty;

                text = this.GetCustomerContactNumberFromSiebel(CustomerAccNo.Trim());

                if (text.Trim() != string.Empty)
                {

                    string MessageType = string.Empty;

                    if (Language == "ARA")
                    {
                        MessageType = "U";
                    }
                    else if (Language == "ENU")
                    {
                        MessageType = "N";
                    }

                    clsSMSLogInfo sentLogInfo = new clsSMSLogInfo
                    {
                        MESSAGETYPE = MessageType,
                        MESSAGE = SMSTemplate,
                        MESSAGECODE = "SADADPAYSMS",  //Added by kisl on 14-03-2018
                        SERVER = "IJARAH-SADADPAYMENT",
                        REFERENCENO = "",
                        RECIPIENTS = text.Trim(),
                        DATE = DateTime.Now,
                        STATUS = 1
                    };

                    decimal sMSLOGID = new clsCommonSP().SMSLogAdd(sentLogInfo);

                    //string text2 = this.ConvertTextTohex(SMSTemplate.Trim());

                    string text2 = SMSTemplate.Trim();

                    string requestUriString = string.Empty;

                    //Added by for new SMS URL change request : KISL-SATISH.M 18062017

                    //Test for URI
                    //objClsDatabase.LogError("SMSTemplate", text2.ToString());

                    requestUriString = string.Concat(new string[]
                        {
                            //smsUrl,
                            //text.Trim(),
                            //"&from=IJARAH&message=",
                            //text2,
                            //"&lang=8&action=send"

                            //smsUrl,
                            //text.Trim(),
                            //"&Text=",
                            //SMSTemplate.Trim(),
                            //"&lang=1&sender=IJARAH"

                            smsUrl, //changed by KISL on 10-07-2017 for new sms url
                            text.Trim(),
                            "&sender=ijarah",
                            "&msg=",
                            text2.Trim()
                        });

                   
                    //Test for URI
                    //objClsDatabase.LogError("requestUriString", requestUriString.ToString());

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
                    if (text3.Contains("Sent"))
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
                    else if (text3.Contains("Invalid"))
                    {
                        text4 = "403";
                        text5 = "ERROR";
                    }
                    else if (text3.Contains("invalid"))
                    {
                        text4 = "403";
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
                    "' where SADADID='",
                    SADADID.Trim(),
                    "'"
                });
                    clsDatabase.ExecuteNonQuery(text6.ToString());

                    clsSMSLogDetailsInfo sentLogDetailInfo = new clsSMSLogDetailsInfo()
                    {
                        SMSLOGID = sMSLOGID,
                        ERRORLOGID = 0,
                        MOBILENOS = text.Trim(),
                        DELIVERYSTAUSCODE = text4,
                        DATE = DateTime.Now,
                        CONTRACTNO = ContactNo
                    };
                    new clsCommonSP().SMSLOGDetailsAdd(sentLogDetailInfo);
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
