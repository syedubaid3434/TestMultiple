using System;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using ErrorLogger;

namespace DALSADADUpload
{
	public class clsDatabase
	{
		private string ConnectionString = string.Empty;
        private string checkTest = ConfigurationManager.AppSettings["TESTUPLOAD"];

        /// <summary>
        /// Method for database connectivity
        /// </summary>
		public clsDatabase()
		{
            try
            {
                if(checkTest == "1")
                {
                    this.ConnectionString = ConfigurationManager.ConnectionStrings["TestSADADUploadConn"].ConnectionString;
                }
                else if(checkTest == "0")
                {
                    this.ConnectionString = ConfigurationManager.ConnectionStrings["SadadUploadConnectionString"].ConnectionString;
                }
            }

            catch (Exception ex)
            {
                LogError("clsDatabase_clsDatabase", ex.Message);          
            }
		}

        /// <summary>
        /// Method for connecion Status
        /// </summary>
        /// <returns></returns>
		public ConnectionState ConnectionStatus()
		{
            try
            {
                OracleConnection oracleConnection = new OracleConnection(this.ConnectionString);
                oracleConnection.Open();
                ConnectionState state = oracleConnection.State;
                oracleConnection.Close();
                return state;
            }

            catch (Exception ex)
            {
                LogError("clsDatabase_ConnectionStatus", ex.Message); 
    
                return ConnectionState.Closed;
            }
		}

        /// <summary>
        /// Method to execute data set
        /// </summary>
        /// <param name="Querystring"></param>
        /// <param name="DataSet_TableName"></param>
        /// <returns></returns>
		public DataSet ExecuteDataset(string Querystring, string DataSet_TableName = "Temp1")
		{
            try
            {
                DataSet dataSet = new DataSet();
                OracleConnection oracleConnection = new OracleConnection(this.ConnectionString);
                OracleCommand selectCommand = new OracleCommand(Querystring, oracleConnection);
                OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(selectCommand);
                oracleDataAdapter.Fill(dataSet, DataSet_TableName);
                oracleConnection.Close();
                oracleConnection.Dispose();
                return dataSet;
            }

            catch (Exception ex)
            {
                LogError("clsDatabase_ExecuteDataset", ex.Message); 

                return null;
            }
		}

        /// <summary>
        /// Method to execute scalar
        /// </summary>
        /// <param name="QueryString"></param>
        /// <returns></returns>
		public object ExecuteScalar(string QueryString)
		{
            try
            {
                OracleConnection oracleConnection = new OracleConnection(this.ConnectionString);
                oracleConnection.Open();
                OracleCommand oracleCommand = new OracleCommand(QueryString, oracleConnection);
                object result = oracleCommand.ExecuteOracleScalar();
                oracleConnection.Close();
                oracleConnection.Dispose();
                return result;
            }

            catch (Exception ex)
            {
                LogError("clsDatabase_ExecuteScalar", ex.Message); 

                return null;
            }
		}

        /// <summary>
        /// Method to execute Non query
        /// </summary>
        /// <param name="QueryString"></param>
		public void ExecuteNonQuery(string QueryString)
		{
            try
            {
                OracleString oracleString = string.Empty;
                OracleConnection oracleConnection = new OracleConnection(this.ConnectionString);
                oracleConnection.Open();
                OracleCommand oracleCommand = new OracleCommand(QueryString, oracleConnection);
                oracleCommand.ExecuteOracleNonQuery(out oracleString);
                oracleConnection.Close();
                oracleConnection.Dispose();
            }

            catch (Exception ex)
            {
                LogError("clsDatabase_ExecuteNonQuery", ex.Message);  
            }
		}

        /// <summary>
        /// Method to log error
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="errorMessage"></param>
        public void LogError(string methodName, string errorMessage)
        {
            try
            {
                clsErrorLogger objError = new clsErrorLogger();
                objError.LogErrorInFile("Exception(" + methodName + ") Common: {0}", errorMessage, 0);
            }

            catch (Exception ex)
            {
                methodName = "clsDatabase_LogError";

                LogError(methodName, ex.Message);
            }
        }   
	}
}
