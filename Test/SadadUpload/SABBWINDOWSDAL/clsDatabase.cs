using System;
using System.Data;
using System.Data.OracleClient;
using ErrorLogger;


namespace SABBWINDOWSDAL
{
    public class clsDatabase
    {
        private string ConnectionString = string.Empty;

        string methodName;

        /// <summary>
        /// Method for connection string
        /// </summary>
        public clsDatabase()
        {
            this.ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=ijr-erp-db-p-1.ijarah.loc)(PORT=1521))(CONNECT_DATA=(SERVER = DEDICATED)(SERVICE_NAME=prod.ijarah.loc)));User Id=apps;Password=apps;Workaround Oracle Bug 914652=true;";
        }


        /// <summary>
        /// Method to get connection status
        /// </summary>
        /// <returns></returns>
        public ConnectionState ConnectionStatus()
        {
                OracleConnection oracleConnection = new OracleConnection(this.ConnectionString);
                oracleConnection.Open();
                ConnectionState state = oracleConnection.State;
                oracleConnection.Close();
                return state;                     
        }

        /// <summary>
        /// Method to Execute Data Set
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
        /// Method to Execute Scalar
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
        /// Method to Execute Non Query
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
