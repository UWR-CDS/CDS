using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using System.Net.Mail;
using System.Net;
using CDS.Logic;
using CDS.Manager;

namespace LEAF_Logic
{
    public class CommonLogic
    {
        public string strErrMsg = "";

        public void InsertError(string sErrMsg, string sSource, string sSP_Name)
        {
            SqlConnection Connection = null;
            System.Diagnostics.StackTrace CurrentStack = new System.Diagnostics.StackTrace();
            /////////////////////////////////////////////////////////////////
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_INSERT_Error";

            SqlParameter[] aParam = new SqlParameter[] { new SqlParameter("@ErrorMsg" , sErrMsg) ,
                new SqlParameter("@ScreenName" , sSource) ,
               new SqlParameter("@FunctionName" ,CurrentStack.GetFrame(1).GetMethod().Name),
                new SqlParameter("@SP_Name" ,(sSP_Name == "") ? System.DBNull.Value : (object)sSP_Name) };

            try
            {
                Connection = DBConnection.GetDBConn();
                int i = SqlHelper.ExecuteNonQuery(Connection, Command.CommandType, Command.CommandText, aParam);
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
        }

       

        public bool CanAccessPrivilige(int PriviligeID, int UserID)
        {
            //return true;
            int Result = 0;
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_USER_PRIVILIGE_ALLOWED";

            SqlParameter[] aParam = new SqlParameter[] {
                new SqlParameter("@PriviligeID" ,PriviligeID),
                new SqlParameter("@UserID" ,UserID) };

            try
            {
                Connection = DBConnection.GetDBConn();
                Result = Convert.ToInt32(SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText, aParam).ToString());
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "USP_USER_PRIVILIGE_ALLOWED");
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            return Result == 1 ? true : false;
        }
        public static int SendGnericEmail(string email, string Masg, string subject)
        {
            int result = 0;
            using (MailMessage mm = new MailMessage("lpes@unitedwereach.org", email))
            {
                try
                {
                    SmtpClient ss = new SmtpClient();
                    ss.Host = "smtp.gmail.com";
                    ss.Port = 587;
                    ss.Timeout = 10000;
                    ss.DeliveryMethod = SmtpDeliveryMethod.Network;
                    ss.UseDefaultCredentials = false;
                    ss.EnableSsl = true;
                    ss.Credentials = new NetworkCredential("lpes@unitedwereach.org", "Lp3sUn!t3dw3r3acH");
                    mm.Subject = subject;
                    string body = Masg;
                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    ss.Send(mm);
                    result = 1;
                }
                catch (Exception ex)
                {
                    new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "Send Email");
                    SaveEmailLog(email,subject,Masg);
                }

            }
            return result;
        }
        public static void SaveEmailLog(string EmailTo, string EmailSubject, string EmailBody)
        {

            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();

            Command.CommandType = CommandType.Text;
            Command.CommandText = "INSERT INTO LEAF_LOG.dbo.Comment_Email_History( EmailFrom,EmailTo,EmailSubject,EmailBody ,ScheduleDate ,SentDate)VALUES('lpes@unitedwereach.org','" + EmailTo + "','" + EmailSubject + "','" + EmailBody + "',GETDATE(),NULL)";
            try
            {
                Connection = DBConnection.GetDBConn();

                SqlHelper.ExecuteNonQuery(Connection, Command.CommandType, Command.CommandText);
            }
            catch (Exception ex)
            {
                new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

        }
        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

    }
}
