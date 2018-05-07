using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace DALSADADPayment
{
    public class clsCommonSP : clsDatabase
    {
        string checkTest = ConfigurationManager.AppSettings["CHECKTEST"];

        string methodName = string.Empty;

        public decimal SMSLogAdd(clsSMSLogInfo SentLogInfo)
        {
            decimal result = 0m;
            try
            {
                string sqlConnectionString = string.Empty;

                if (checkTest.ToString() == "1")
                {
                    sqlConnectionString = ConfigurationManager.ConnectionStrings["sqlTestConn"].ConnectionString;
                }
                else
                {
                    sqlConnectionString = ConfigurationManager.ConnectionStrings["sqlConn"].ConnectionString;
                }

                this.connection = new SqlConnection(sqlConnectionString);

                if (this.connection.State == ConnectionState.Closed)
                {
                    this.connection.Open();
                }
                SqlCommand sqlCommand = new SqlCommand("sp_SMSLogAdd", this.connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter = sqlCommand.Parameters.Add("@SERVER", SqlDbType.VarChar);
                sqlParameter.Value = SentLogInfo.SERVER;
                sqlParameter = sqlCommand.Parameters.Add("@REFERENCENO", SqlDbType.VarChar);
                sqlParameter.Value = SentLogInfo.REFERENCENO;
                sqlParameter = sqlCommand.Parameters.Add("@MESSAGECODE", SqlDbType.NVarChar);
                sqlParameter.Value = SentLogInfo.MESSAGECODE;
                sqlParameter = sqlCommand.Parameters.Add("@MESSAGETYPE", SqlDbType.VarChar);
                sqlParameter.Value = SentLogInfo.MESSAGETYPE;
                sqlParameter = sqlCommand.Parameters.Add("@MESSAGE", SqlDbType.NVarChar);
                sqlParameter.Value = SentLogInfo.MESSAGE;
                sqlParameter = sqlCommand.Parameters.Add("@DATE", SqlDbType.DateTime);
                sqlParameter.Value = SentLogInfo.DATE;
                sqlParameter = sqlCommand.Parameters.Add("@STATUS", SqlDbType.Int);
                sqlParameter.Value = SentLogInfo.STATUS;
                result = decimal.Parse(sqlCommand.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                methodName = "clsCommonSP_SMSLogAdd";
                LogError(methodName, ex.Message);
            }
            finally
            {
                this.connection.Close();
            }
            return result;
        }

        public void SMSLOGDetailsAdd(clsSMSLogDetailsInfo SentLogInfo)
        {
            try
            {
                string sqlConnectionString = string.Empty;

                if (checkTest.ToString() == "1")
                {
                    sqlConnectionString = ConfigurationManager.ConnectionStrings["sqlTestConn"].ConnectionString;
                }
                else
                {
                    sqlConnectionString = ConfigurationManager.ConnectionStrings["sqlConn"].ConnectionString;
                }

                this.connection = new SqlConnection(sqlConnectionString);

                if (this.connection.State == ConnectionState.Closed)
                {
                    this.connection.Open();
                }
                SqlCommand sqlCommand = new SqlCommand("sp_smsLogDetailsAdd", this.connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter = sqlCommand.Parameters.Add("@SMSLOGID", SqlDbType.Decimal);
                sqlParameter.Value = SentLogInfo.SMSLOGID;
                sqlParameter = sqlCommand.Parameters.Add("@MOBILENOS", SqlDbType.VarChar);
                sqlParameter.Value = SentLogInfo.MOBILENOS;
                sqlParameter = sqlCommand.Parameters.Add("@ERRORLOGID", SqlDbType.Int);
                sqlParameter.Value = SentLogInfo.ERRORLOGID;
                sqlParameter = sqlCommand.Parameters.Add("@DELIVERYSTAUSCODE", SqlDbType.VarChar);
                sqlParameter.Value = SentLogInfo.DELIVERYSTAUSCODE;
                sqlParameter = sqlCommand.Parameters.Add("@DATE", SqlDbType.DateTime);
                sqlParameter.Value = SentLogInfo.DATE;
                sqlParameter = sqlCommand.Parameters.Add("@CONTRACTNO", SqlDbType.NVarChar);
                sqlParameter.Value = SentLogInfo.CONTRACTNO;
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                methodName = "clsCommonSP_SMSLOGDetailsAdd";
                LogError(methodName, ex.Message);
            }
            finally
            {
                this.connection.Close();
            }
        }
    }
}
