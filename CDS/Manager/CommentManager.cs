using CDS.Logic;
using CDS.Models;
using LEAF_Logic;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Web;

namespace CDS.Manager
{
    public class CommentManager
    {
        public int CommentInsert(int LessonID, int SenderID, string Comment,string URL, string InvitationTo, string EmailInvitation, int LessonPlanID,int isverified,int isReject, int secId, string slectedtext)
        {
            object i = 0;
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_CDS_INSERT_COMMENTS";
            SqlParameter[] aParam = new SqlParameter[]
            {
                new SqlParameter("@LessonID" , LessonID) ,
                new SqlParameter("@SenderID" , SenderID) ,
                new SqlParameter("@Comment" ,Comment),
                new SqlParameter("@URL" ,URL),
                new SqlParameter("@InvitationIDs" ,InvitationTo),
                new SqlParameter("@InvitationEmails" ,EmailInvitation),
                new SqlParameter("@LessonPlanID" ,LessonPlanID),
                   new SqlParameter("@ISVerified" ,isverified),
                   new SqlParameter("@IsRejectLesson" ,isReject),
                new SqlParameter("@SectionId" ,secId),
                new SqlParameter("@SelectedText" ,slectedtext)
           };

            try
            {
                Connection = DBConnection.GetDBConn();
                i = SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText, aParam);
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
            return Convert.ToInt32(i);

        }
        public int ScopeCommentInsert(int LessonID, string Comment,int UserID, int ReceiverID)
        {
            object i = 0;
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_CDS_INSERT_SCOPE_AND_SEQUENCE_COMMENTS";
            SqlParameter[] aParam = new SqlParameter[]
            {
                new SqlParameter("@LessonID" , LessonID) ,
                new SqlParameter("@UserID" , UserID) ,
                new SqlParameter("@Comment" ,Comment),
                new SqlParameter("@ReceiverID" ,ReceiverID)
           };

            try
            {
                Connection = DBConnection.GetDBConn();
                i = SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText, aParam);
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
            return Convert.ToInt32(i);

        }

        public List<UserTag> Get_AllUserListForTagging(int UserID, string onlineids = "")
        {
            SqlConnection Connection = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_GET_ALLUSER_LIST";
            SqlParameter[] aParam = null;
            aParam = new SqlParameter[] { new SqlParameter("@UserID", UserID), new SqlParameter("@OnlineIDs", onlineids), };
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, aParam);
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

            List<UserTag> _select = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                _select = new List<UserTag>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UserTag objbll = new UserTag();
                    objbll.id = Convert.ToInt32(dt.Rows[i]["UserID"]);
                    objbll.sid = EncyptionDcryption.GetEncryptedText(dt.Rows[i]["UserID"].ToString());
                    //objbll.name = Convert.ToString(dt.Rows[i]["FirstName"]);
                    objbll.LastName = Convert.ToString(dt.Rows[i]["LastName"]);
                    objbll.name = Convert.ToString(dt.Rows[i]["FullName"]);
                    objbll.isLogedIn = Convert.ToInt32(dt.Rows[i]["isLogedIn"]);
                    objbll.IsNotViewedCount = Convert.ToInt32(dt.Rows[i]["IsNotViewedCount"]);

                    //objbll.avatar = "../Handler/UserImageHandler.ashx?key=" + objbll.sid + "&&vuser=" + objbll.sid + "";

                    _select.Add(objbll);
                }
            }
            return _select;
        }

        public BLL_ChatHistory ChangeReadStatus(int LessonPlanID,int UserID)
        {
            SqlConnection Connection = null;
            BLL_ChatHistory result = null;
            DataSet ds = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_UPDATE_COMMENT_STATUS";
            SqlParameter[] aParam = null;
            aParam = new SqlParameter[]
            { new SqlParameter("@LessonPlanID", LessonPlanID),
             new SqlParameter("@UserID", UserID) };
            try
            {
                Connection = DBConnection.GetDBConn();
                ds = SqlHelper.ExecuteDataset(Connection, Command.CommandType, Command.CommandText,aParam);
                if (ds != null)
                {
                    result = new BLL_ChatHistory();
                    result.unReadObj = new Unread();
                    result.unReadObj.MessageList = new List<int>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int val = Convert.ToInt32(ds.Tables[0].Rows[i]["CommentIDFK"]);
                        result.unReadObj.MessageList.Add(val);
                    }
                    result.unReadObj.UnreadCount = Convert.ToInt32(ds.Tables[1].Rows[0]["Column1"]);
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
            return result;
        }

        public List<BLL_ChatHistory> GetScopeAndSequenceComments(int LessonID,int userID,int messageID = 0)
        {
            SqlConnection Connection = null;
            List<BLL_ChatHistory> result = null;
            DataSet ds = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_GET_SCOPE_AND_SEQUENCE_COMMENT";
            SqlParameter[] aParam = null;
            aParam = new SqlParameter[]
            {
                new SqlParameter("@LessonID", LessonID),
                new SqlParameter("@userID", userID),
                new SqlParameter("@messageID", messageID)
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                ds = SqlHelper.ExecuteDataset(Connection, Command.CommandType, Command.CommandText, aParam);
                if (ds != null)
                {
                    result = new List<BLL_ChatHistory>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                        {
                            if (i == 0)
                            {
                                BLL_ChatHistory objbll = new BLL_ChatHistory();
                                objbll.Message_ID = Convert.ToInt32(ds.Tables[i].Rows[j]["Message_ID"]);
                                objbll.EncMessage_ID = EncyptionDcryption.GetEncryptedText(ds.Tables[i].Rows[j]["Message_ID"].ToString());
                                objbll.UserName = Convert.ToString(ds.Tables[i].Rows[j]["UserName"]);
                                objbll.BookName = Convert.ToString(ds.Tables[i].Rows[j]["BookName"]);
                                objbll.ClassName = Convert.ToString(ds.Tables[i].Rows[j]["ClassName"]);
                                objbll.SubjectName = Convert.ToString(ds.Tables[i].Rows[j]["SubjectName"]);
                                objbll.LessonName = Convert.ToString(ds.Tables[i].Rows[j]["LessonName"]);
                                objbll.EncUserID_A = EncyptionDcryption.GetEncryptedText(ds.Tables[i].Rows[j]["Sender_UserID"].ToString());
                                if (messageID != 0)
                                {
                                    objbll.SenderID = Convert.ToInt32(ds.Tables[i].Rows[j]["SenderID"]);
                                }
                                else
                                {
                                    objbll.SenderID = 0;
                                }
                                objbll.EncLessonID = EncyptionDcryption.GetEncryptedText(ds.Tables[i].Rows[j]["LessonID"].ToString());
                                objbll.MessageDateTime = Convert.ToDateTime(ds.Tables[i].Rows[j]["MessageDateTime"]);
                                objbll.MessageDateTimeStr = ds.Tables[i].Rows[j]["MessageDateTime"].ToString();
                                objbll.LessonID = ds.Tables[i].Rows[j]["LessonID"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[i].Rows[j]["LessonID"]);
                                objbll.Comment = Convert.ToString(ds.Tables[i].Rows[j]["Comment"]);
                                objbll.isRead = Convert.ToBoolean(ds.Tables[i].Rows[j]["IsRead"].ToString() == "False" ? false : true);
                                objbll.CommentTimeAgo = new LessonInfoManager().GetCommentTimeAgo(objbll.MessageDateTime);

                                result.Add(objbll);
                            }
                            else
                            {
                                if (result != null && result.Count > 0)
                                {
                                    result.ToList()[0].InboxCount = Convert.ToInt32(ds.Tables[i].Rows[j]["Unread"]);
                                }
                            }
                        }
                    }
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
            return result;
        }

        public List<BLL_ChatHistory> CommentInbox(int UserID,int EntityID) {

            SqlConnection Connection = null;
            List<BLL_ChatHistory> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            int UnreadCount = 0;

            SqlParameter[] aParam = new SqlParameter[]
           {
               new SqlParameter("@UserID" , UserID),
               new SqlParameter("@Mode" , "MessageBox"),
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
                new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);

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
                    objbll.EncMessage_ID = EncyptionDcryption.GetEncryptedText(dt.Rows[i]["MessageID"].ToString());
                    objbll.UserName = Convert.ToString(dt.Rows[i]["UserName"]);
                    objbll.EncUserID_A = EncyptionDcryption.GetEncryptedText(dt.Rows[i]["SenderID"].ToString());
                    objbll.EncLessonID = EncyptionDcryption.GetEncryptedText(dt.Rows[i]["LessonID"].ToString());
                    objbll.MessageDateTime = Convert.ToDateTime(dt.Rows[i]["MessageDateTime"]);
                    objbll.MessageDateTimeStr = dt.Rows[i]["MessageDateTime"].ToString();
                    objbll.LessonID = dt.Rows[i]["LessonID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonID"]);
                    objbll.Comment = Convert.ToString(dt.Rows[i]["Comment"]);
                    objbll.WhereFrom = Convert.ToString(dt.Rows[i]["WhereFrom"]);
                    if (string.IsNullOrEmpty(dt.Rows[i]["IsViewed"].ToString()))
                    {
                        objbll.isRead = true;
                    }
                    else
                    {
                        objbll.isRead = Convert.ToBoolean(dt.Rows[i]["IsViewed"].ToString() == "False" ? false : true);
                    }

                    if (Convert.ToBoolean(dt.Rows[i]["IsViewed"]) == false)
                    {
                        UnreadCount++;
                    }
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
            if (_select != null)
            {
                _select.ToList()[0].UnreadCount = UnreadCount;
            }


            return _select;
        }
    }
}