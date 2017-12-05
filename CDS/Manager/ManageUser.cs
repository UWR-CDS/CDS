using CDS.Logic;
using CDS.Models;
using LEAF_Logic;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CDS.Manager
{
    public class ManageUser
    {
        CommonLogic objlog = new CommonLogic();
        public UserData CheckUserCredentionals(string Email, string Password)
        {
            string strErrMsg;
            SqlConnection Connection = null;
            DataTable dt = null;
            UserData objUser = new UserData();
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_CheckUserCredentioanls";
            try
            {
                Connection = DBConnection.GetDBConn();
                SqlParameter[] prams = new SqlParameter[] {

                    new SqlParameter("@Email" , Email),
                    new SqlParameter("@Password" , Password)
                                                        };
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, prams);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objUser.UserID = Convert.ToInt32(dt.Rows[0]["USERID"]);
                    objUser.UserFirstName = Convert.ToString(dt.Rows[0]["FIRSTNAME"]);
                    objUser.UserLastName = Convert.ToString(dt.Rows[0]["LASTNAME"]);
                    objUser.UserEmail = Convert.ToString(dt.Rows[0]["EMAIL"]);
                    objUser.Password = Convert.ToString(dt.Rows[0]["secret"]);
                    objUser.UserPhone = Convert.ToString(dt.Rows[0]["ContactNo"]);
                    objUser.DefaultPage = Convert.ToString(dt.Rows[0]["DefaultPage"]);
                    objUser.LoginAs = Convert.ToInt32(dt.Rows[0]["PriviligeEntIDFK"]);
                    objUser.isMaster = Convert.ToInt32(dt.Rows[0]["isMaster"]);
                    objUser.EntityID = Convert.ToInt32(dt.Rows[0]["EntityID"]);
                    objUser.EntityName = Convert.ToString(dt.Rows[0]["EntityName"]);
                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                objlog.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

            return objUser;

        }
        public UserData CheckUserEmail(string Email)
        {

            string ss = null;
            SqlConnection Connection = null;
            UserData objUser = new UserData();
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select USERID,EMAIL,secret from CDS_Users where EMAIL='" + Email + "' and Status!=3";
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, cmd.CommandType, cmd.CommandText);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objUser.UserID = Convert.ToInt32(dt.Rows[i]["USERID"]);
                        objUser.UserEmail = Convert.ToString(dt.Rows[i]["EMAIL"]);
                        objUser.Password =EncyptionDcryption.GetDecryptedText(Convert.ToString(dt.Rows[i]["secret"]));
                    }
                }
            }
            catch (Exception ex)
            {
                ss = ex.Message;
                objlog.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), cmd.CommandText);

            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

            return objUser;
        }
        public void UpdateUserPAssword(string Password, string Email)
        {
            SqlConnection con = null;
            try
            {
                con = DBConnection.GetDBConn();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "update CDS_Users set secret='" + Password + "' where EMAIL='" + Email + "'";
                cmd.ExecuteNonQuery();

            }

            catch (Exception)
            {

                throw;
            }
            finally
            {

                con.Close();

            }

        }
        public string LogoutBy(Int64 LoginTrackid, int UserID)
        {
            string v = "";
            SqlConnection con = null;
            con = DBConnection.GetDBConn();
            LoginTracking objUser = new LoginTracking();
            try
            {
                LoginTracking obtrack = new LoginTracking();
                SqlCommand cmd6 = new SqlCommand("SELECT top 1 IP FROM Login_Tracking WHERE UserID=" + UserID + " and logouttime is null order by LoginTrackID desc", con);
                SqlDataReader dr = cmd6.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    v = dr["IP"].ToString();
                    if ((v == null) || (v == ""))
                    {

                    }
                    else
                    {
                        v = dr["IP"].ToString();
                    }
                }

                return v;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        public string RemoveUserConnectionID(int UserID)
        {
            string msg = "";
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_REMOVE_CONNECTIONID";
            SqlParameter[] aParam = new SqlParameter[] { new SqlParameter("@UserID", UserID) };
            try
            {
                msg = SqlHelper.ExecuteScalar(DBConnection.GetDBConn(), Command.CommandType, Command.CommandText, aParam).ToString();
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
            return msg;
        }
        public LoginTracking LoginTracking(object id)
        {
            string ip = HttpContext.Current.Request.UserHostAddress;
            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }
            SqlConnection con = null;
            con = DBConnection.GetDBConn();
            LoginTracking obtrack = new LoginTracking();
            try
            {
                int f = 1;
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand("[USP_Login_Tracking_History]", con);
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter[] aParam = new SqlParameter[]
                {
                         new SqlParameter("@UserID", id),
                         new SqlParameter("@IP", ip),
                         new SqlParameter("@ForceFullyLogout", f)
                };

                dt = SqlHelper.ExecuteDataTable(con, command.CommandType, command.CommandText, aParam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        obtrack.LoginTrackID = Convert.ToInt32(dt.Rows[i]["LoginTrackID"]);
                    }
                }
                new ActivityLog().GenActivitylog(Convert.ToInt64(obtrack.LoginTrackID), Convert.ToInt32(id), 1, "successfully Loged in.", ip);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                    con.Close();
            }
            return obtrack;
        }
        public LoginModel ManageSocialAccount(string Email, string Id, string For, string FirstName, string LastName, string Password)
        {
            LoginModel obj = new LoginModel();
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            DataTable dt = null;
            Command.CommandText = "USB_WEB_MANAGE_SOCAILACCOUNT_ID";
            SqlParameter[] aParam = new SqlParameter[]
            {
                new SqlParameter("@Email", Email),
                new SqlParameter("@ID", Id),
                new SqlParameter("@For", For),
                new SqlParameter("@FirstName", FirstName),
                new SqlParameter("@LaseName", LastName),
                new SqlParameter("@Password", Password),
            };
            try
            {
                dt = SqlHelper.ExecuteDataTable(DBConnection.GetDBConn(), Command.CommandType, Command.CommandText, aParam);
                if (dt!=null && dt.Rows.Count!=0)
                {
                    obj.Email = Convert.ToString(dt.Rows[0]["EMAIL"]);
                    obj.Password = EncyptionDcryption.GetDecryptedText(Convert.ToString(dt.Rows[0]["secret"]));
                }
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
            return obj;
        }
        public void RealseAllLessonLockBYUser(int UserID)
        {
            string ss = null;
            SqlConnection Connection = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update  cds_LessonPlanMaster  set [LockedBy]=null,[LockedDateTime]=null where LockedBy=" + UserID;
            try
            {
                Connection = DBConnection.GetDBConn();
                SqlHelper.ExecuteNonQuery(Connection, cmd.CommandType, cmd.CommandText).ToString();
            }
            catch (Exception ex)
            {
                ss = ex.Message;
                new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), cmd.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
        }
        public UserData GetUserById(int ID)
        {
            string strErrMsg;
            SqlConnection Connection = null;
            DataTable dt = null;
            UserData objUser = new UserData();
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "select * from dbo.CDS_Users where UserID="+ID;
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objUser.UserID = Convert.ToInt32(dt.Rows[0]["USERID"]);
                    objUser.UserFirstName = Convert.ToString(dt.Rows[0]["FIRSTNAME"]);
                    objUser.UserLastName = Convert.ToString(dt.Rows[0]["LASTNAME"]);
                    objUser.UserEmail = Convert.ToString(dt.Rows[0]["EMAIL"]);
                    objUser.Password = Convert.ToString(dt.Rows[0]["secret"]);
                    objUser.UserPhone= Convert.ToString(dt.Rows[0]["ContactNo"]);
                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                objlog.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

            return objUser;

        }
        public int ManageUsers(UserData obj)
        {
            string strErrMsg;
            SqlConnection Connection = null;
            DataTable dt = null;
            int UserID = 0;
            if (obj.Password==null || obj.Password=="")
            {
                obj.Password = null;
            }
            else
            {
                obj.Password = EncyptionDcryption.GetEncryptedText(obj.Password);
            }
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_MANAGE_USER";
            try
            {

                Connection = DBConnection.GetDBConn();
                SqlParameter[] prams = new SqlParameter[] {

                    new SqlParameter("@UserID" , obj.UserID),
                    new SqlParameter("@FirstName" , obj.UserFirstName),
                    new SqlParameter("@LastName" , obj.UserLastName),
                    new SqlParameter("@Email" , obj.UserEmail),
                    new SqlParameter("@Password" ,obj.Password),
                     new SqlParameter("@Phone" , obj.UserPhone),
                                                        };
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, prams);
                if (dt != null && dt.Rows.Count > 0)
                {
                    UserID = Convert.ToInt32(dt.Rows[0]["USERID"]);
                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                objlog.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            return UserID;
        }
        public int UpdateStatus(string Email,int UserID)
        {
            string strErrMsg;
            SqlConnection Connection = null;
            int Status = 0;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_Update_User_Status";
            SqlParameter[] apram = new SqlParameter[] {
                new SqlParameter("@UserID",UserID),
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                SqlHelper.ExecuteNonQuery(Connection, Command.CommandType, Command.CommandText,apram);
                Status = 1;
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                objlog.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            return Status;
        }
        public int UpdateUserSubject(int a,int UserID,int ClassId,int SubjectId)
        {
            string strErrMsg;
            SqlConnection Connection = null;
            int Status = 0;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            if (a==1)
            {
                Command.CommandText = "delete from dbo.CDS_UserClassSubject where UserID=" + UserID;
                Command.CommandText += " insert into dbo.CDS_UserClassSubject ";
                Command.CommandText += " select " + UserID + "," + ClassId + "," + SubjectId;
            }
            else
            {
                Command.CommandText = " insert into dbo.CDS_UserClassSubject ";
                Command.CommandText += " select " + UserID + "," + ClassId + "," + SubjectId;
            }
            try
            {
                Connection = DBConnection.GetDBConn();
                SqlHelper.ExecuteNonQuery(Connection, Command.CommandType, Command.CommandText);
                Status = 1;
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                objlog.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            return Status;
        }
        public string GetVerifyTempalte()
        {
            string msg = "";
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "select TemplateScript from LEAF_Main.dbo.Con_Email_Template where TamplateID=10";
            try
            {
                Connection = DBConnection.GetDBConn();
                msg = SqlHelper.ExecuteScalar(DBConnection.GetDBConn(), Command.CommandType, Command.CommandText).ToString();
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
            return msg;
        }
        public string GetResetPasswordTempalte()
        {
            string msg = "";
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "select TemplateScript from LEAF_Main.dbo.Con_Email_Template where TamplateID=12";
            try
            {
                Connection = DBConnection.GetDBConn();
                msg = SqlHelper.ExecuteScalar(DBConnection.GetDBConn(), Command.CommandType, Command.CommandText).ToString();
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
            return msg;
        }
        public List<UserData> UserList(int UserID) {

            string strErrMsg;
            SqlConnection Connection = null;
            DataTable dt = null;
            List<UserData> _Select = new List<UserData>();
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_GET_USER_LIST";
            SqlParameter[] aparam = new SqlParameter[] {
                new SqlParameter("UserID",UserID)
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText,aparam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++) {
                        UserData objUser = new UserData();
                        objUser.UserID = Convert.ToInt32(dt.Rows[i]["USERID"]);
                        objUser.UserFirstName = Convert.ToString(dt.Rows[i]["FIRSTNAME"]);
                        objUser.UserLastName = Convert.ToString(dt.Rows[i]["LASTNAME"]);
                        objUser.UserEmail = Convert.ToString(dt.Rows[i]["EMAIL"]);
                        objUser.UserPhone = Convert.ToString(dt.Rows[i]["ContactNo"]);
                        objUser.Status = Convert.ToInt32(dt.Rows[i]["Status"]);
                        objUser.LastActivityTime = dt.Rows[i]["Time"]==DBNull.Value ? 120 : Convert.ToInt32(dt.Rows[i]["Time"]);
                        _Select.Add(objUser);
                    }

                }
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                objlog.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

            return _Select;
        }
        public string SetDefaultPage(string value)
        {
            string msg = "";
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "update dbo.CDS_Users set DefaultPage='"+value+"' where UserID="+SessionManager.Current.UserID;
            try
            {
                Connection = DBConnection.GetDBConn();
                SqlHelper.ExecuteNonQuery(DBConnection.GetDBConn(), Command.CommandType, Command.CommandText).ToString();
                msg = "1";
            }
            catch (Exception ex)
            {
                msg = "0";
                new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            return msg;
        }

    }
}