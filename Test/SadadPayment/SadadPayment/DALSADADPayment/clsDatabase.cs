using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.OracleClient;
using ErrorLogger;
using System.Data.SqlClient;
using System.Configuration;

namespace DALSADADPayment
{
    public class clsDatabase
    {
        private string ConnectionString = string.Empty;

        string checkTest = ConfigurationManager.AppSettings["CHECKTEST"];

        public SqlConnection connection; //Added by satish KISL on 20-Feb-2017

        /// <summary>
        /// Method for Connection
        /// </summary>
        public clsDatabase()
        {
            try
            {
                if (checkTest == "1")
                {
                    this.ConnectionString = ConfigurationManager.ConnectionStrings["oracleConnTest"].ConnectionString;
                }
                else if (checkTest == "0")
                {
                    this.ConnectionString = ConfigurationManager.ConnectionStrings["oracleConn"].ConnectionString;
                }

            }

            catch (Exception ex)
            {
                LogError("clsDatabase_clsDatabase", ex.Message);
            }
        }


        /// <summary>
        /// Method for Connection status
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
        /// Method to Execute DataSet
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
        /// Method to Execute Scalar From Siebel
        /// </summary>
        /// <param name="QueryString"></param>
        /// <returns></returns>
        public object ExecuteScalarFromSiebel(string QueryString)
        {
            try
            {
                string connectionString = string.Empty;

                if (checkTest == "1")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["siebelConnTest"].ConnectionString;
                }
                else if (checkTest == "0")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["siebelConn"].ConnectionString;
                }

                OracleConnection oracleConnection = new OracleConnection(connectionString);
                oracleConnection.Open();
                OracleCommand oracleCommand = new OracleCommand(QueryString, oracleConnection);
                object result = oracleCommand.ExecuteOracleScalar();
                oracleConnection.Close();
                oracleConnection.Dispose();
                return result;
            }

            catch (Exception ex)
            {
                LogError("clsDatabase_ExecuteScalarFromSiebel", ex.Message);

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
        /// Execute Non Sybase Query
        /// </summary>
        /// <param name="QueryString"></param>
        public void ExecuteNonSybaseQuery(string QueryString)
        {
            try
            {
                string connectionString = string.Empty;

                if (checkTest == "1")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["mySqlConnTest"].ConnectionString;
                }
                else if (checkTest == "0")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["mySqlConn"].ConnectionString;
                }

               


                MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(QueryString, mySqlConnection);
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                mySqlConnection.Dispose();
            }

            catch (Exception ex)
            {
                LogError("clsDatabase_ExecuteNonSybaseQuery", ex.Message);
            }
        }

        /// <summary>
        /// Execute MySql Non  Query
        /// </summary>
        /// <param name="QueryString"></param>
   
        //added by Zubair for MySqlDatabase Inserting the data --23-Apr-2017
        public void ExecuteNonQuerymySql(string QueryString)
        {
            try
            {
               
                string connectionString = string.Empty;

                if (checkTest == "1")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["mySqlConnTest"].ConnectionString;
                }
                else if (checkTest == "0")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["mySqlConn"].ConnectionString;
                }

                //int result = 0;
                MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(QueryString, mySqlConnection);
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                mySqlConnection.Dispose();
            }

            catch (Exception ex)
            {
                LogError("clsDatabase_ExecuteNonQuery", ex.Message);
            }
        }


        /// <summary>
        /// Execute MySql Non  Query
        /// </summary>
        /// <param name="QueryString"></param>

        // added by Zubair for selecting data from MySql 23-Apr-2017
        public object ExecuteScalarmySql(string Querystring)
        {

            try
            {
                string connectionString = string.Empty;

                if (checkTest == "1")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["mySqlConnTest"].ConnectionString;
                }
                else if (checkTest == "0")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["mySqlConn"].ConnectionString;
                }



                MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(Querystring, mySqlConnection);

                object result = mySqlCommand.ExecuteScalar();

                mySqlConnection.Close();
                mySqlConnection.Dispose();
                return result;
            }


            catch (Exception ex)
            {
                LogError("clsDatabase_ExecuteScalar", ex.Message);

                return null;
            }

        }

        /// <summary>
        /// Execute MySql Non  Query
        /// </summary>
        /// <param name="QueryString"></param>

        // added by Zubair for selecting data from MySql 23-Apr-2017
        public DataSet ExecuteDatamySql(string Querystring, string DataSet_TableName = "MySqlTable")
        {

            try
            {
                string connectionString = string.Empty;
                DataSet dataSet = new DataSet();

                if (checkTest == "1")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["mySqlConnTest"].ConnectionString;
                }
                else if (checkTest == "0")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["mySqlConn"].ConnectionString;
                }

                MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(Querystring, mySqlConnection);
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
                mySqlDataAdapter.Fill(dataSet, DataSet_TableName);

                mySqlConnection.Close();
                mySqlConnection.Dispose();
                return dataSet;
            }


            catch (Exception ex)
            {
                LogError("clsDatabase_ExecuteScalar", ex.Message);

                return null;
            }

        }




        /// <summary>
        /// Execute SQL Query 
        /// </summary>
        /// <param name="QueryString"></param>
        //public void ExecuteNonSqlQuery(string QueryString) //Added by satish on 20-Feb-2017
        //{

        //    try
        //    {
        //        string connectionString = ConfigurationManager.ConnectionStrings["TestConn"].ConnectionString;
        //        this.connection = new SqlConnection(connectionString);
        //        if (this.connection.State == ConnectionState.Closed)
        //        {
        //            this.connection.Open();
        //        }
        //        SqlCommand sqlCommand = new SqlCommand(QueryString, this.connection);
        //        sqlCommand.CommandType = CommandType.Text;
        //        sqlCommand.ExecuteNonQuery();
        //        this.connection.Close();
        //        this.connection.Dispose();
        //    }

        //    catch (Exception ex)
        //    {
        //        LogError("clsDatabase_ExecuteNonSqlQuery", ex.Message);
        //    }
        //}

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
