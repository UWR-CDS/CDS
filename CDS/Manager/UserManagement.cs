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
    public class UserManagement
    {
        CommonLogic objlog = new CommonLogic();
        public UserData GetUserPrivilige(int UserID) {

            string strErrMsg;
            SqlConnection Connection = null;
            DataSet ds = null;
            UserData obj = new UserData();
            List<Privilige> _Select = new List<Privilige>();
            List<UserData> _SelectUser = new List<UserData>();
            List<Les_Subject> _Selectsub = new List<Les_Subject>();
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_GET_USER_PRIVILIGES";
            SqlParameter[] apram = new SqlParameter[] {
                new SqlParameter("@UserID",UserID)
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                ds = SqlHelper.ExecuteDataset(Connection, Command.CommandType, Command.CommandText,apram);
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Privilige pri = new Privilige();
                        pri.PriviligeId = Convert.ToString(ds.Tables[0].Rows[i]["PriviligeID"]);
                        pri.PriviligeName = Convert.ToString(ds.Tables[0].Rows[i]["PriviligeName"]);
                        pri.IsActive = Convert.ToInt32(ds.Tables[0].Rows[i]["isActive"]);
                        pri.IsAllow = Convert.ToBoolean(ds.Tables[0].Rows[i]["Allow"]);
                        pri.EntityName = Convert.ToString(ds.Tables[0].Rows[i]["Entity"]);
                        pri.EntityIdFk = Convert.ToString(ds.Tables[0].Rows[i]["EntityID"]);
                        _Select.Add(pri);
                    }

                }
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        UserData user = new UserData();
                        user.UserID = Convert.ToInt32(ds.Tables[1].Rows[i]["UserID"]);
                        user.UserFirstName = Convert.ToString(ds.Tables[1].Rows[i]["FirstName"]);
                        user.UserLastName = Convert.ToString(ds.Tables[1].Rows[i]["LastName"]);
                        user.UserPhone = Convert.ToString(ds.Tables[1].Rows[i]["ContactNo"]);
                        user.UserEmail = Convert.ToString(ds.Tables[1].Rows[i]["Email"]);
                        user.Status = Convert.ToInt32(ds.Tables[1].Rows[i]["Status"]);
                        user.LoginAs = Convert.ToInt32(ds.Tables[1].Rows[i]["PriviligeEntIDFK"]);
                        _SelectUser.Add(user);
                    }

                }
                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        Les_Subject sub = new Les_Subject();
                        sub.SubjectName = Convert.ToString(ds.Tables[2].Rows[i]["SubjectName"]);
                        sub.ClassName = Convert.ToString(ds.Tables[2].Rows[i]["ClassName"]);
                        _Selectsub.Add(sub);
                    }

                }

                obj.priviligesList = _Select;
                obj.UserList = _SelectUser;
                obj.lstUserSubjects = _Selectsub;
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

            return obj;
        }

        public List<BLL_ChatHistory> GetMessageNotification(int userID,int EntityID)
        {
            SqlConnection Connection = null;
            List<BLL_ChatHistory> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();

            SqlParameter[] aParam = new SqlParameter[]
           { new SqlParameter("@UserID" , userID),
            new SqlParameter("@Mode" , "Desktop"),
            new SqlParameter("@EntityID" , EntityID)
          };

            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_GET_COMMENTS_BY_USERID";

            try
            {
                Connection = DBConnection.GetDBConn();

                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, aParam);

            }
            catch (Exception ex)
            {
                objlog.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);

            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }


            if (dt != null && dt.Rows.Count > 0)
            {
                _select = new List<BLL_ChatHistory>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BLL_ChatHistory objbll = new BLL_ChatHistory();
                    objbll.Message_ID = Convert.ToInt32(dt.Rows[i]["MessageID"]);
                    objbll.OwnerID = Convert.ToInt32(dt.Rows[i]["OwnerID"]);
                    objbll.EncMessage_ID = EncyptionDcryption.GetEncryptedText(dt.Rows[i]["MessageID"].ToString());
                    objbll.UserName = Convert.ToString(dt.Rows[i]["UserName"]);
                    objbll.EncUserID_A = EncyptionDcryption.GetEncryptedText(dt.Rows[i]["SenderID"].ToString());
                    objbll.EncLessonID = EncyptionDcryption.GetEncryptedText(dt.Rows[i]["LessonID"].ToString());
                    objbll.MessageDateTime = Convert.ToDateTime(dt.Rows[i]["MessageDateTime"]);
                    objbll.MessageDateTimeStr = dt.Rows[i]["MessageDateTime"].ToString();
                    objbll.LessonID = dt.Rows[i]["LessonID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonID"]);
                    objbll.Comment = Convert.ToString(dt.Rows[i]["Comment"]);
                    objbll.WhereFrom = Convert.ToString(dt.Rows[i]["WhereFrom"]);
                    objbll.isRead = Convert.ToBoolean(Convert.ToInt32(dt.Rows[i]["IsViewed"]) == 0 ? false : true);

                    //if (string.Equals(objbll.WhereFrom, "Inbox"))
                    //{
                    //    if (!objbll.Comment.Contains("</b>"))
                    //    {
                    //        objbll.WhereFrom = "Verifier";
                    //    }
                    //}

                    if (objbll.Comment.Contains("</b>"))
                    {
                        int index = objbll.Comment.LastIndexOf("</b>");
                        int length = index + 3;
                        objbll.Comment = objbll.Comment.Substring(length + 1).Trim();
                    }
                    objbll.CommentTimeAgo = new LessonInfoManager().GetCommentTimeAgo(objbll.MessageDateTime);
                    _select.Add(objbll);

                }

            }
           
            return _select;
        }

        public void SavePriviliges(int UserID, string PriviligeID, string UserFirstName, string UserLastName, string ContactNo, int Status,int LoginAs)
        {
            string strErrMsg;
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_ADDUPDATE_USER_PRIVILIGES";
            SqlParameter[] apram = new SqlParameter[] {
                new SqlParameter("@UserID",UserID),
                new SqlParameter("@PriviligeID",PriviligeID),
                new SqlParameter("@UserFirstName",UserFirstName),
                new SqlParameter("@UserLastName",UserLastName),
                new SqlParameter("@ContactNo",ContactNo),
                new SqlParameter("@Status",Status),
                new SqlParameter("@LoginAs",LoginAs)
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, apram);
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

        }

        public List<Privilige> UserLoginAs() {
            string strErrMsg;
            SqlConnection Connection = null;
            DataTable dt = null;
            List<Privilige> _Select = new List<Privilige>();
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "SELECT EntityID,EntityName FROM dbo.CDS_Entity WHERE isActive=1";
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Privilige pri = new Privilige();
                        pri.EntityIdFk = Convert.ToString(dt.Rows[i]["EntityID"]);
                        pri.EntityName = Convert.ToString(dt.Rows[i]["EntityName"]);
                        _Select.Add(pri);
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


    }
}