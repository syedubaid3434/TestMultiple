using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace ErrorLogger
{
    public class clsErrorLogger
    {
        #region Constructor
        /// <summary>
        /// Errorlog Path
        /// </summary>        
        private string _strErrorLogPath = String.Empty;
        
        public clsErrorLogger()
        {
            try
            {
                string logPath = ConfigurationManager.AppSettings["ERRORLOG"];

                //_strErrorLogPath = EncryptDecrypt(logPath);
                _strErrorLogPath = logPath;

                if (!Directory.Exists(_strErrorLogPath))
                {
                    Directory.CreateDirectory(_strErrorLogPath);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
            }
        }

        #endregion     

        #region Log error in file
        /// <summary>
        /// Insert Error Log information into File
        /// </summary>
        /// <param name="errLocation">Error Location</param>
        /// <param name="errDescription">Error Description</param>
        /// <param name="errNumber">Error Number</param>
        /// <returns>True/False</returns>
        public Boolean LogErrorInFile(string errLocation, string errDescription, long errNumber = 0)
        {
            StreamWriter sw;
            try
            {
                LoggerClass.instance.Error(errDescription + ", Error No:" + errNumber, errLocation);
                string errTime = String.Empty;
                DateTime CD = DateTime.Now;
                string Year = DateTime.Now.Year.ToString();
                string Month = DateTime.Now.Month.ToString();
                string Day = DateTime.Now.Day.ToString();
                string FileName = String.Empty;
                errTime = DateTime.Now.ToString();
                string strDate = String.Format("{0:dd/MM/yyyy}", CD);
                strDate = strDate.Replace("-", "/").Replace(":","/").Replace(" ", "/");

                string[] strDateSplit = strDate.Split('/');

                FileName = strDateSplit[1] + "_" + strDateSplit[0] + "_" + strDateSplit[2] + ".txt";
                FileName = _strErrorLogPath + FileName;

                sw = new StreamWriter(FileName, true);
                sw.WriteLine("******************************************************************************");
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("DATE:- " + DateTime.Now);
                sw.WriteLine("TIME:- " + CD.TimeOfDay.Hours.ToString() + ":" + CD.TimeOfDay.Minutes.ToString() + ":" + CD.TimeOfDay.Seconds.ToString());
                sw.WriteLine();
                sw.WriteLine("Error Number:- " + errNumber.ToString());
                sw.WriteLine();
                sw.WriteLine("Description:- " + errDescription);
                sw.WriteLine();
                sw.WriteLine("Location:- " + errLocation);
                sw.WriteLine();
                sw.WriteLine("******************************************************************************");
                sw.WriteLine();
                sw.Flush();
                sw.Close();


                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
            finally
            {

            }
        }
        #endregion

        #region Function for to do the Encryption and Decryption
        /// <summary>
        /// Do the Encription and Decryption
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string EncryptDecrypt(string Text)
        {
            try
            {
                string strTempChar = string.Empty;
                int i = 0;
                for (i = 1; i <= Text.Length; i++)
                {
                    if (System.Convert.ToInt32(Text[i - 1]) < 128)
                    {
                        strTempChar = System.Convert.ToString(System.Convert.ToInt32(Text[i - 1]) + 128);
                    }
                    else if (System.Convert.ToInt32(Text[i - 1]) > 128)
                    {
                        strTempChar = System.Convert.ToString(System.Convert.ToInt32(Text[i - 1]) - 128);
                    }
                    Text = Text.Remove(i - 1, 1).Insert(i - 1, ((char)(System.Convert.ToInt32(strTempChar))).ToString());
                }              
            }

            catch (Exception ex)
            {
                ex.Message.ToString();              
            }
            return Text;
        }

        #endregion
    }
}
