using System;
using System.Data;
using System.IO;
using System.ServiceModel;
using System.Xml;
using DALSADADUpload;
using System.Configuration;

namespace SADADUploadWebService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class IJARAHSADADUPLOAD : IIJARAHSADADUPLOAD
    {

        public const string vbLf = "\n";

        private string QueryString = string.Empty;

        clsDatabase objDatabase = new clsDatabase();

        /// <summary>
        /// Method to get and upload account receivables
        /// </summary>
        public void GetAndUploadAccountReceivables()
        {
            try
            {
                string empty = string.Empty;
                string txtIJR = string.Empty;
                string txtAccountReceivables = string.Empty;
                string txtRequest = string.Empty;
                string txtResponse = string.Empty;
                string chk = ConfigurationManager.AppSettings["TESTUPLOAD"];
                bool testUpload = true;
                
                //1 For UAT
                if (chk == "1")
                {
                    testUpload = true;
                }
                else if (chk == "0")
                {
                    testUpload = false;
                }
                //int c, aa = 10, b = 0;
                //c = aa / b;


                //                txtAccountReceivables = "H1000000002281 180411 180411 " +
                //" D100202180300084MAY ABU DUJAYN ACTIVE  0000000007972502018 - 03 - 202018 - 03 - 202018 - 04 - 202018 - 04 - 20000000025000000000000000045000000000000045000" +
                //"T                 0000001";
                //  this.FrameAccountReceivables(out empty);

                txtAccountReceivables = this.FrameAccountReceivables(out empty);

                if (txtAccountReceivables.Trim() != string.Empty)
                {
                    txtIJR = "IJR" + empty.ToString();
                    txtRequest = "<?xml version=\"1.0\"?>";
                    txtRequest += "<FileMessage>";
                    txtRequest += "<Header>";
                    txtRequest += "<Sender>IJARAH</Sender>";
                    txtRequest += "<FileType>SADADBillerUploadMSG</FileType>";
                    txtRequest = txtRequest + "<FileID>" + txtIJR + "</FileID>";
                    txtRequest += "</Header>";
                    txtRequest = txtRequest + "<Body>" + txtAccountReceivables + "</Body>";
                    txtRequest = txtRequest + "<Signature>" +(testUpload ? clsServiceConnection.TestDigitalSignMessage(txtAccountReceivables) : clsServiceConnection.DigitalSignMessage(txtAccountReceivables)) + "</Signature>";
                    txtRequest += "</FileMessage>";
                    this.CreateTextFile("REQUEST", empty.ToString(), txtRequest.ToString());
                    txtResponse = (testUpload ?  clsServiceConnection.TestPostDataWithSignature(txtRequest.ToString()) : clsServiceConnection.PostDataWithSignature(txtRequest.ToString()));
                    txtResponse = txtResponse.Replace("&", "&amp;");
                    this.CreateTextFile("REPLY", empty.ToString(), txtResponse.ToString());
                    clsDatabase clsDatabase = new clsDatabase();
                    RBFileMessageReply rBFileMessageReply = clsSerializationDeserialization<RBFileMessageReply>.Deserialize<RBFileMessageReply>(txtResponse);
                    if (rBFileMessageReply != null)
                    {
                        string txtFileID = rBFileMessageReply.HeaderRep.FileID.ToString();
                        string a = rBFileMessageReply.Body.ToString().Trim().ToUpper();
                        if (a == "FILE RECEIVED SUCCESSFULLY.")
                        {
                            this.QueryString = string.Concat(new string[]
						{
							"UPDATE XXI_SADAD_OUT_FILE_HEADER_T SET PAYMENTSTATUS='P',LAST_UPDATE_DATE=SYSDATE,FILESTATUSCODE='OK',FILESTATUSDETAIL='",
							rBFileMessageReply.Body.ToString().Trim(),
							"' WHERE FILEID='",
							txtFileID.ToString().Trim(),
							"'"
						});
                            clsDatabase.ExecuteNonQuery(this.QueryString);
                            return;
                        }
                        this.QueryString = string.Concat(new string[]
					{
						"UPDATE XXI_SADAD_OUT_FILE_HEADER_T SET LAST_UPDATE_DATE=SYSDATE,FILESTATUSCODE='FAILED',FILESTATUSDETAIL='",
						rBFileMessageReply.Body.ToString().Trim(),
						"' WHERE FILEID='",
						txtFileID.ToString().Trim(),
						"'"
					});
                        clsDatabase.ExecuteNonQuery(this.QueryString);
                        return;
                    }
                    else if (txtResponse != null && txtResponse.Trim() != string.Empty)
                    {
                        this.QueryString = string.Concat(new string[]
					{
						"UPDATE XXI_SADAD_OUT_FILE_HEADER_T SET LAST_UPDATE_DATE=SYSDATE,FILESTATUSCODE='FAILED',FILESTATUSDETAIL='",
						txtResponse.Trim(),
						"' WHERE FILEID='",
						txtIJR.Trim(),
						"'"
					});
                        clsDatabase.ExecuteNonQuery(this.QueryString);
                    }
                }
            }
            catch (Exception ex)
            {
                objDatabase.LogError("IJARAHSADAD_GetAndUploadAccountReceivables", ex.Message);
            }
        }

        /// <summary>
        /// Method to frame acount receivables
        /// </summary>
        /// <param name="FileSequenceID"></param>
        /// <returns></returns>
        private string FrameAccountReceivables(out string FileSequenceID)
        {
            try
            {
                string txtResult = string.Empty;
                clsDatabase clsDatabase = new clsDatabase();
                FileSequenceID = string.Empty;
                this.QueryString = "SELECT (SELECT SYSDATE FROM DUAL) AS SERVERDATE,RECORDID,H_RECORDTYPE,H_BILLERID,FILESEQUENCE,BILLDATE,BILLTHROUGHDATE,D_RECORDTYPE,D_BILLERID,EXACTFLAG,CUSTOMERACCOUNT,CUSTOMERNAME,STATUSFLAG,AMOUNTDUE,BILLOPENDATE,BILLDUEDATE,BILLEXPIRYDATE,BILLCLOSEDATE,MAXADVANCEAMOUNT,MINADVANCEAMOUNT,MINPARTIALAMOUNT,T_RECORDTYPE,PAYMENTSTATUS FROM XXI_SADAD_OUT_FILE_HEADER_T WHERE PAYMENTSTATUS='U' AND FILESEQUENCE IS NULL";
                DataSet dataSet = clsDatabase.ExecuteDataset(this.QueryString.Trim(), "AR");
                if (dataSet != null && dataSet.Tables["AR"].Rows.Count > 0)
                {
                    string txtData = string.Empty;
                    string txtDetails = string.Empty;
                    string txtRecordDetails = string.Empty;
                    string txtFirstString_1 = new string(' ', 1);
                    string txtSecondString_1 = new string(' ', 1);
                    string txtString_224 = new string(' ', 224);
                    string txtString_92 = new string(' ', 92);
                    string txtString_17 = new string(' ', 17);
                    string txtString_231 = new string(' ', 231);
                    string txt_H_RecordType = "H";
                    string txt_H_BillerID = "100";
                    string txt_D_RECORDTYPE = "D";
                    string txt_D_BILLERID = "100";
                    string txt_ExactFlag = "1";
                    string txt_CustomerAccount = string.Empty;
                    string txt_CustomerName = string.Empty;
                    string txt_StatusFlag = string.Empty;
                    string txt_T_RecordType = "T";
                    string txt_SequenceNo = string.Empty;
                    string txtFileSequenceID = string.Empty;
                    decimal amount = 0m;
                    decimal amountDue = 0m;
                    decimal maxAdvanceAmount = 0m;
                    decimal minAdvanceAmount = 0m;
                    decimal minPartialAmount = 0m;
                    int sequenceNumber = 1;
                    int number = 0;
                    DateTime dateTime = DateTime.Now;
                    DateTime dateTime2 = DateTime.Now;
                    DateTime dateTime3 = DateTime.Now;
                    DateTime dateTime4 = DateTime.Now;
                    DateTime dateTime5 = DateTime.Now;
                    DateTime dateTime6 = DateTime.Now;
                    this.QueryString = "SELECT SEQUENCENUMBER FROM XXI_SADAD_BTOB_CONFIG";
                    object obj = clsDatabase.ExecuteScalar(this.QueryString);
                    if (obj != null && obj != DBNull.Value && obj.ToString().Trim() != string.Empty)
                    {
                        sequenceNumber = Convert.ToInt32(obj.ToString().Trim());
                    }
                    sequenceNumber++;
                    this.QueryString = "UPDATE XXI_SADAD_BTOB_CONFIG SET SEQUENCENUMBER=" + sequenceNumber.ToString();
                    clsDatabase.ExecuteNonQuery(this.QueryString);
                    DataRow dataRow = dataSet.Tables[0].Rows[0];
                    if (dataRow["H_RECORDTYPE"] != DBNull.Value && dataRow["H_RECORDTYPE"] != null && dataRow["H_RECORDTYPE"].ToString().Trim() != string.Empty)
                    {
                        txt_H_RecordType = dataRow["H_RECORDTYPE"].ToString().Trim();
                    }
                    if (dataRow["H_BILLERID"] != DBNull.Value && dataRow["H_BILLERID"] != null && dataRow["H_BILLERID"].ToString().Trim() != string.Empty)
                    {
                        txt_H_BillerID = dataRow["H_BILLERID"].ToString().Trim();
                    }
                    txt_SequenceNo = sequenceNumber.ToString("0000000000");
                    FileSequenceID = txt_SequenceNo.ToString();
                    txtFileSequenceID = "IJR" + FileSequenceID.ToString() + ".txt";
                    DateTime dateTime7;
                    if (dataRow["SERVERDATE"] != DBNull.Value && dataRow["SERVERDATE"] != null && DateTime.TryParse(dataRow["SERVERDATE"].ToString().Trim(), out dateTime7))
                    {
                        dateTime = DateTime.Parse(dataRow["SERVERDATE"].ToString().Trim());
                        dateTime2 = DateTime.Parse(dataRow["SERVERDATE"].ToString().Trim());
                    }
                    txtData = string.Concat(new string[]
				{
					txt_H_RecordType.Trim(),
					txt_H_BillerID.Trim(),
					txt_SequenceNo.Trim(),
					txtFirstString_1.ToString(),
					dateTime.ToString("yyMMdd"),
					txtSecondString_1.ToString(),
					dateTime2.ToString("yyMMdd"),
					txtString_224.ToString()
				});
                    foreach (DataRow dataRow2 in dataSet.Tables[0].Rows)
                    {
                        number++;
                        if (dataRow2["D_RECORDTYPE"] != DBNull.Value && dataRow2["D_RECORDTYPE"] != null && dataRow2["D_RECORDTYPE"].ToString().Trim() != string.Empty)
                        {
                            txt_D_RECORDTYPE = dataRow2["D_RECORDTYPE"].ToString().Trim();
                        }
                        if (dataRow2["D_BILLERID"] != DBNull.Value && dataRow2["D_BILLERID"] != null && dataRow2["D_BILLERID"].ToString().Trim() != string.Empty)
                        {
                            txt_D_BILLERID = dataRow2["D_BILLERID"].ToString().Trim();
                        }
                        if (dataRow2["EXACTFLAG"] != DBNull.Value && dataRow2["EXACTFLAG"] != null && dataRow2["EXACTFLAG"].ToString().Trim() != string.Empty)
                        {
                            txt_ExactFlag = dataRow2["EXACTFLAG"].ToString().Trim();
                        }
                        if (dataRow2["CUSTOMERACCOUNT"] != DBNull.Value && dataRow2["CUSTOMERACCOUNT"] != null && dataRow2["CUSTOMERACCOUNT"].ToString().Trim() != string.Empty)
                        {
                            txt_CustomerAccount = dataRow2["CUSTOMERACCOUNT"].ToString().Trim();
                        }
                        if (dataRow2["CUSTOMERNAME"] != DBNull.Value && dataRow2["CUSTOMERNAME"] != null && dataRow2["CUSTOMERNAME"].ToString().Trim() != string.Empty)
                        {
                            txt_CustomerName = dataRow2["CUSTOMERNAME"].ToString().Trim();
                        }
                        if (txt_CustomerName.Length > 40)
                        {
                            txt_CustomerName = txt_CustomerName.Substring(1, 40).Trim();
                        }
                        if (dataRow2["STATUSFLAG"] != DBNull.Value && dataRow2["STATUSFLAG"] != null && dataRow2["STATUSFLAG"].ToString().Trim() != string.Empty)
                        {
                            txt_StatusFlag = dataRow2["STATUSFLAG"].ToString().Trim();
                        }
                        if (dataRow2["AMOUNTDUE"] != DBNull.Value && dataRow2["AMOUNTDUE"] != null && decimal.TryParse(dataRow2["AMOUNTDUE"].ToString().Trim(), out amount))
                        {
                            amountDue = decimal.Parse(dataRow2["AMOUNTDUE"].ToString().Trim());
                        }
                        string txt_AmountDue = amountDue.ToString("N2").Replace(",", "").Replace(".", "").PadLeft(15, '0');
                        if (dataRow2["BILLOPENDATE"] != DBNull.Value && dataRow2["BILLOPENDATE"] != null && DateTime.TryParse(dataRow2["BILLOPENDATE"].ToString().Trim(), out dateTime7))
                        {
                            dateTime3 = DateTime.Parse(dataRow2["BILLOPENDATE"].ToString().Trim());
                        }
                        if (dataRow2["BILLDUEDATE"] != DBNull.Value && dataRow2["BILLDUEDATE"] != null && DateTime.TryParse(dataRow2["BILLDUEDATE"].ToString().Trim(), out dateTime7))
                        {
                            dateTime4 = DateTime.Parse(dataRow2["BILLDUEDATE"].ToString().Trim());
                        }
                        if (dataRow2["BILLEXPIRYDATE"] != DBNull.Value && dataRow2["BILLEXPIRYDATE"] != null && DateTime.TryParse(dataRow2["BILLEXPIRYDATE"].ToString().Trim(), out dateTime7))
                        {
                            dateTime5 = DateTime.Parse(dataRow2["BILLEXPIRYDATE"].ToString().Trim());
                        }
                        if (dataRow2["BILLCLOSEDATE"] != DBNull.Value && dataRow2["BILLCLOSEDATE"] != null && DateTime.TryParse(dataRow2["BILLCLOSEDATE"].ToString().Trim(), out dateTime7))
                        {
                            dateTime6 = DateTime.Parse(dataRow2["BILLCLOSEDATE"].ToString().Trim());
                        }
                        if (dataRow2["MAXADVANCEAMOUNT"] != DBNull.Value && dataRow2["MAXADVANCEAMOUNT"] != null && decimal.TryParse(dataRow2["MAXADVANCEAMOUNT"].ToString().Trim(), out amount))
                        {
                            maxAdvanceAmount = decimal.Parse(dataRow2["MAXADVANCEAMOUNT"].ToString().Trim());
                        }
                        string txt_MaxAdvanceAmount = maxAdvanceAmount.ToString("N2").Replace(",", "").Replace(".", "").PadLeft(15, '0');
                        if (dataRow2["MINADVANCEAMOUNT"] != DBNull.Value && dataRow2["MINADVANCEAMOUNT"] != null && decimal.TryParse(dataRow2["MINADVANCEAMOUNT"].ToString().Trim(), out amount))
                        {
                            minAdvanceAmount = decimal.Parse(dataRow2["MINADVANCEAMOUNT"].ToString().Trim());
                        }
                        string txt_MinAdvanceAmount = minAdvanceAmount.ToString("N2").Replace(",", "").Replace(".", "").PadLeft(15, '0');
                        if (dataRow2["MINPARTIALAMOUNT"] != DBNull.Value && dataRow2["MINPARTIALAMOUNT"] != null && decimal.TryParse(dataRow2["MINPARTIALAMOUNT"].ToString().Trim(), out amount))
                        {
                            minPartialAmount = decimal.Parse(dataRow2["MINPARTIALAMOUNT"].ToString().Trim());
                        }
                        string txtMinPartialAmount = minPartialAmount.ToString("N2").Replace(",", "").Replace(".", "").PadLeft(15, '0');
                        string txtSampleData = txtDetails;
                        txtDetails = string.Concat(new string[]
					{
						txtSampleData,
						txt_D_RECORDTYPE.ToString(),
						txt_D_BILLERID.ToString(),
						txt_ExactFlag.ToString(),
						txt_CustomerAccount.ToString().PadLeft(11, '0'),
						txt_CustomerName.ToString().PadRight(40, ' '),
						txt_StatusFlag.ToString().PadRight(8, ' '),
						txt_AmountDue.ToString(),
						dateTime3.ToString("yyyy-MM-dd"),
						dateTime4.ToString("yyyy-MM-dd"),
						dateTime5.ToString("yyyy-MM-dd"),
						dateTime6.ToString("yyyy-MM-dd"),
						txt_MaxAdvanceAmount.ToString(),
						txt_MinAdvanceAmount.ToString(),
						txtMinPartialAmount.ToString(),
						txtString_92.ToString()
					});
                        txtDetails = txtDetails.Trim();
                        if (dataSet.Tables[0].Rows.Count != number)
                        {
                            txtDetails += "\n";
                        }
                        this.QueryString = string.Concat(new string[]
					{
						"UPDATE XXI_SADAD_OUT_FILE_HEADER_T SET FILESEQUENCE='",
						txt_SequenceNo.ToString(),
						"',FILEID='",
						txtFileSequenceID.ToString(),
						"',PAYMENTSTATUS='L',LAST_UPDATE_DATE=SYSDATE WHERE RECORDID=",
						dataRow2["RECORDID"].ToString().Trim()
					});
                        clsDatabase.ExecuteNonQuery(this.QueryString);
                     //   break;
                    }
                    if (dataRow["T_RECORDTYPE"] != DBNull.Value && dataRow["T_RECORDTYPE"] != null && dataRow["T_RECORDTYPE"].ToString().Trim() != string.Empty)
                    {
                        txt_T_RecordType = dataRow["T_RECORDTYPE"].ToString().Trim();
                    }
                    txtRecordDetails = txt_T_RecordType.ToString() + txtString_17.ToString() + number.ToString().PadLeft(7, '0') + txtString_231.ToString();
                    txtResult = string.Concat(new string[]
				{
					txtData.ToString().Trim(),
					"\n",
					txtDetails.ToString().Trim(),
					"\n",
					txtRecordDetails.ToString().Trim()
				});
                }
                return txtResult.ToString();
            }

            catch (Exception ex)
            {
                objDatabase.LogError("IJARAHSADAD_FrameAccountReceivables", ex.Message);

                FileSequenceID = string.Empty;

                return string.Empty;
            }

        }

        /// <summary>
        /// Method to get signature value
        /// </summary>
        /// <param name="strFileID"></param>
        /// <param name="BodyValue"></param>
        /// <returns></returns>
        private string GetForSignatureValue(string strFileID, string BodyValue)
        {
            try
            {
                string txtValueSerialization = string.Empty;
                RBFileMessageRequest value = new RBFileMessageRequest
                {
                    HeaderReq = new RBHeaderRequest
                    {
                        Sender = ConfigurationManager.AppSettings["SENDER"],
                        FileType = ConfigurationManager.AppSettings["FILETYPE"],
                        FileID = strFileID.ToString()
                    },
                    Body = BodyValue.ToString(),
                    Signature = ConfigurationManager.AppSettings["SIGNATURE"]
                };
                txtValueSerialization = clsSerializationDeserialization<RBFileMessageRequest>.Serialize<RBFileMessageRequest>(value);
                txtValueSerialization = txtValueSerialization.Remove(0, txtValueSerialization.IndexOf('>') + 1).Trim();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(txtValueSerialization.ToString());
                XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Body");
                return elementsByTagName.Item(0).InnerXml.ToString();
            }

            catch (Exception ex)
            {
                objDatabase.LogError("IJARAHSADAD_GetForSignatureValue", ex.Message);

                return string.Empty;
            }
        }

        /// <summary>
        /// Method to create text file
        /// </summary>
        /// <param name="TranType"></param>
        /// <param name="FileSequenceID"></param>
        /// <param name="FileData"></param>
        private void CreateTextFile(string TranType, string FileSequenceID, string FileData)
        {
            try
            {
                string fileName = string.Empty;
                string folderPath = ConfigurationManager.AppSettings["FolderPath"];
                Random random = new Random();
                try
                {
                    this.CreateFolder(folderPath);
                    fileName = string.Concat(new string[]
				{
					"\\",
					TranType,
					FileSequenceID,
					"_",
					random.Next(0, 1000).ToString(),
					".txt"
				});
                    File.WriteAllText(folderPath + fileName, FileData);
                }
                catch (Exception ex)
                {
                    FileData = FileData + Environment.NewLine + Environment.NewLine + ex.Message.ToString();
                    fileName = string.Concat(new string[]
				{
					"\\",
					TranType,
					FileSequenceID,
					"_",
					random.Next(0, 1000).ToString(),
					".txt"
				});
                    File.WriteAllText(folderPath + fileName, FileData);
                }
            }

            catch (Exception ex)
            {
                objDatabase.LogError("IJARAHSADAD_CreateTextFile", ex.Message);
            }
        }

        /// <summary>
        /// Method to create folder
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
                objDatabase.LogError("IJARAHSADAD_CreateFolder", ex.Message);
            }
        }
    }
}
