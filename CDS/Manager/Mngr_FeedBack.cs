using CDS.Logic;
using CDS.Models;
using LEAF_Logic;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CDS.Manager
{
    public class Mngr_FeedBack
    {
        CommonLogic objlogic = new CommonLogic();

        #region FeedBack

        public List<mdl_FeedBack> AddFeedBack(string source,string remarks, int userID)
        {
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            System.Data.DataTable dt = null;
            List<mdl_FeedBack> _Select = new List<mdl_FeedBack>();
            try
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "USP_CDS_ADD_FEEDBACK";
                Connection = DBConnection.GetDBConn();
                SqlParameter[] aParam = new SqlParameter[] {
                new SqlParameter("@Source" , source) ,
                new SqlParameter("@Remarks" , remarks),
                new SqlParameter("@Status" , 1),
                new SqlParameter("@UserIDFK" , userID)
               };
               dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, aParam);
            }
            catch (Exception ex)
            {
                objlogic.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            if (dt != null || dt.Rows.Count > 0) {
                for (int i = 0; i < dt.Rows.Count; i++) {
                    mdl_FeedBack objbll = new mdl_FeedBack();
                    objbll.FeedBackID = EncyptionDcryption.GetEncryptedText(dt.Rows[i]["ID"].ToString());
                    objbll.Title = Convert.ToString(dt.Rows[i]["Title"]);
                    objbll.Source = Convert.ToString(dt.Rows[i]["Source"]);
                    objbll.Remarks = Convert.ToString(dt.Rows[i]["Remarks"]);
                    objbll.Date = Convert.ToDateTime(dt.Rows[i]["Date"]);
                    objbll.Status = Convert.ToString(dt.Rows[i]["FeedBackStatus"]);
                    objbll.UserID = Convert.ToInt32(dt.Rows[i]["UserId"]);
                    objbll.UserName = Convert.ToString(dt.Rows[i]["UserName"]);
                    objbll.FeedbackStatusID = Convert.ToInt32(dt.Rows[i]["FeedBackStatusID"]);
                    objbll.ScopeIdentity = Convert.ToInt32(dt.Rows[i]["ScopeIdentity"]);
                    _Select.Add(objbll);
                }
            }
            return _Select;
        }

        public bool AddFeedBackImages(string imgList, int id)
        {
            object i = null;
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            try
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "USP_CDS_ADD_FEEDBACK_IMAGE";
                Connection = DBConnection.GetDBConn();
                SqlParameter[] aParam = new SqlParameter[] {
                new SqlParameter("@FeedBackID" , id) ,
                new SqlParameter("@Image" , imgList)
               };

                i = SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText, aParam);
            }
            catch (Exception ex)
            {
                objlogic.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            return Convert.ToBoolean(i);
        }

        public bool UpdateFeedBack(int status, string comment, int userID, int feedBackID)
        {
            object i = null;
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            try
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "USP_CDS_UPDATE_FEEDBACK";
                Connection = DBConnection.GetDBConn();
                SqlParameter[] aParam = new SqlParameter[] {
                new SqlParameter("@FeedBackID" , feedBackID) ,
                new SqlParameter("@StatusID" , status),
                new SqlParameter("@Comment" , comment) ,
                new SqlParameter("@UserIDFK" , userID)
               };
               i = SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText, aParam);
            }
            catch (Exception ex)
            {
                objlogic.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            return Convert.ToBoolean(i);
        }

        public bool AddFeedBackComment(string comment, int userID, int feedBackID)
        {
            object i = null;
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            try
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "USP_ADD_CDS_FEEDBACK_COMMENT";
                Connection = DBConnection.GetDBConn();
                SqlParameter[] aParam = new SqlParameter[] {
                new SqlParameter("@FeedBackID" , feedBackID) ,
                new SqlParameter("@Comment" , comment) ,
                new SqlParameter("@UserIDFK" , userID)
               };
               i = SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText, aParam);
            }
            catch (Exception ex)
            {
                objlogic.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            return Convert.ToBoolean(i);
        }

        public bool ReAllocateFeedBack(int departmentID, string comment, int feebbackId, int prevDeptID, int userID)
        {
            object i = null;
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            try
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "USP_REALLOCATE_FEEDBACK";
                Connection = DBConnection.GetDBConn();
                SqlParameter[] aParam = new SqlParameter[] {
                new SqlParameter("@FeedBackID" , feebbackId) ,
                new SqlParameter("@DepartmentID" , departmentID),
                new SqlParameter("@Comment" , comment) ,
                new SqlParameter("@PrevDeptID" , prevDeptID),
                 new SqlParameter("@UserIDFK" , userID)
               };
               i = SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText, aParam);
            }
            catch (Exception ex)
            {
                objlogic.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            return Convert.ToBoolean(i);
        }

        public bool DeleteFeedBackComment(int commentID)
        {
            object i = null;
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            try
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "USP_DELETE_CDS_FEEDBACK_COMMENT";
                Connection = DBConnection.GetDBConn();
                SqlParameter[] aParam = new SqlParameter[] {
                new SqlParameter("@FeedBackCommentID" , commentID)
               };
               i = SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText, aParam);
            }
            catch (Exception ex)
            {
                objlogic.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            return Convert.ToBoolean(i);
        }

        public List<mdl_FeedBack> GetFeedBackList(string userID = null)
        {
            SqlConnection Connection = null;
            List<mdl_FeedBack> _select = null;
            System.Data.DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_GET_CDS_FEEDBACK_LIST";
            //userID = EncyptionDcryption.GetDecryptedText(userID);
            SqlParameter[] aParam = new SqlParameter[] { new SqlParameter("@Id", userID) };
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, aParam);
            }
            catch (Exception ex)
            {
                objlogic.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                _select = new List<mdl_FeedBack>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    mdl_FeedBack objbll = new mdl_FeedBack();
                    objbll.FeedBackID = EncyptionDcryption.GetEncryptedText(dt.Rows[i]["ID"].ToString());
                    objbll.Title = Convert.ToString(dt.Rows[i]["Title"]);
                    objbll.Source = Convert.ToString(dt.Rows[i]["Source"]);
                    objbll.Remarks = Convert.ToString(dt.Rows[i]["Remarks"]);
                    objbll.Date = Convert.ToDateTime(dt.Rows[i]["Date"]);
                    objbll.Status = Convert.ToString(dt.Rows[i]["FeedBackStatus"]);
                    objbll.UserID = Convert.ToInt32(dt.Rows[i]["UserId"]);
                    objbll.UserName = Convert.ToString(dt.Rows[i]["UserName"]);
                    objbll.FeedbackStatusID = Convert.ToInt32(dt.Rows[i]["FeedBackStatusID"]);
                    _select.Add(objbll);
                }
            }
            return _select;
        }

        public mdl_FeedBack GetFeedBackProcess(int id)
        {
            SqlConnection Connection = null;
            mdl_FeedBack _select = null;
            DataSet ds = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_GET_CDS_FEEDBACK_PROCESS";
            SqlParameter[] aParam = new SqlParameter[] 
            {
                new SqlParameter("@Id" , id)
            };

            try
            {
                Connection = DBConnection.GetDBConn();

                ds = SqlHelper.ExecuteDataset(Connection, Command.CommandType, Command.CommandText, aParam);
            }
            catch (Exception ex)
            {
                objlogic.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
                // strErrMsg = ex.Message;
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            if (ds != null)
            {
                _select = new mdl_FeedBack();
                _select.FeedBackComment = new List<mdl_FeedBackComment>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                    {
                        if (i == 0)
                        {
                            _select.ID = Convert.ToInt32(ds.Tables[i].Rows[j]["ID"]);
                            _select.FeedBackID = EncyptionDcryption.GetEncryptedText(ds.Tables[i].Rows[j]["ID"].ToString());
                            _select.Source = Convert.ToString(ds.Tables[i].Rows[j]["Title"]);
                            _select.Remarks = Convert.ToString(ds.Tables[i].Rows[j]["Remarks"]);
                            _select.Date = Convert.ToDateTime(ds.Tables[i].Rows[j]["Date"]);
                            _select.Status = Convert.ToString(ds.Tables[i].Rows[j]["FeedBackStatus"]);
                            _select.StatusID = Convert.ToInt32(ds.Tables[i].Rows[j]["FeedBackStatusID"]);
                            _select.UserName = Convert.ToString(ds.Tables[i].Rows[j]["UserId"]);
                            _select.image = Convert.ToString(ds.Tables[i].Rows[j]["ImageList"]);
                            _select.imageList = new List<string>();
                            _select.UserID = Convert.ToInt32(ds.Tables[i].Rows[j]["UserIDFK"]);

                            if (!string.IsNullOrEmpty(_select.image))
                            {
                                var fol = "Picture\\FeedBack";
                                string[] imagePath = _select.image.Split(',');
                                for (int count = 0; count < imagePath.Count(); count++)
                                {
                                    string img = imagePath.ToList()[count];
                                    string all = string.Concat(fol, "\\", img, ".jpg");
                                    _select.imageList.Add(all);
                                }
                            }
                        }

                        if (i == 1)
                        {
                            if (!string.IsNullOrEmpty(ds.Tables[i].Rows[j]["CommentDetail"].ToString()))
                            {
                                mdl_FeedBackComment fbc = new mdl_FeedBackComment();
                                fbc.CommentDetail = Convert.ToString(ds.Tables[i].Rows[j]["CommentDetail"]);
                                fbc.CommentDate = Convert.ToDateTime(ds.Tables[i].Rows[j]["CommentDate"]);
                                if (!string.IsNullOrEmpty(ds.Tables[i].Rows[j]["CommentTime"].ToString()))
                                {
                                    fbc.CommentTime = (Convert.ToDateTime(ds.Tables[i].Rows[j]["CommentTime"])).ToString("HH:mm:ss tt");
                                }
                                else
                                {
                                    fbc.CommentTime = null;
                                }
                                fbc.CommentUserName = Convert.ToString(ds.Tables[i].Rows[j]["CommentUserName"]);
                                fbc.CommentID = Convert.ToInt32(ds.Tables[i].Rows[j]["CommentID"]);
                                fbc.CommentUserID = Convert.ToInt32(ds.Tables[i].Rows[j]["CommentUserID"]);
                                _select.FeedBackComment.Add(fbc);
                            }

                        }
                    }
                }
            }

            return _select;
        }

        #endregion FeedBack

        #region FeedBackType

        public List<mdl_FeedBackType> DropDownType(string FeedBackGroupID = null)
        {
            SqlConnection Connection = null;
            List<mdl_FeedBackType> _select = null;
            System.Data.DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_FEEDBACKTYPE_SELECT_DROPDOWN";
            SqlParameter[] aParam = new SqlParameter[] { new SqlParameter("@FeedBackGroupID", FeedBackGroupID), };
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, aParam);
            }
            catch (Exception ex)
            {
                objlogic.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                _select = new List<mdl_FeedBackType>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    mdl_FeedBackType objbll = new mdl_FeedBackType();
                    objbll.FeedBackTypeID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    objbll.TypeName = Convert.ToString(dt.Rows[i]["TypeName"]);
                    objbll.IsSelected = Convert.ToBoolean(dt.Rows[i]["IsDefault"]);
                    _select.Add(objbll);
                }
            }
            return _select;
        }

        #endregion FeedBackType

        #region FeedBackStatus
        public List<mdl_FeedBackStatus> DropDownStatus()
        {
            List<mdl_FeedBackStatus> fbsList = new List<mdl_FeedBackStatus>();
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    mdl_FeedBackStatus fbs = new mdl_FeedBackStatus();
                    fbs.FeedBackStatusID = 1;
                    fbs.StatusName = "InProcess";
                    fbsList.Add(fbs);
                }
                else if (i == 1)
                {
                    mdl_FeedBackStatus fbs = new mdl_FeedBackStatus();
                    fbs.FeedBackStatusID = 2;
                    fbs.StatusName = "Pending";
                    fbsList.Add(fbs);
                }
                else if (i == 2)
                {
                    mdl_FeedBackStatus fbs = new mdl_FeedBackStatus();
                    fbs.FeedBackStatusID = 4;
                    fbs.StatusName = "Complete";
                    fbsList.Add(fbs);
                }
            }
            return fbsList;
        }
        #endregion FeedBackStatus

        #region FeedBackFrequency

        public List<mdl_FeedBackFrequency> DropDownFrequency()
        {
            SqlConnection Connection = null;
            List<mdl_FeedBackFrequency> _select = null;
            System.Data.DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_FEEDBACKFREQUENCY_SELECT_DROPDOWN";
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText);
            }
            catch (Exception ex)
            {
                objlogic.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                _select = new List<mdl_FeedBackFrequency>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    mdl_FeedBackFrequency objbll = new mdl_FeedBackFrequency();
                    objbll.FeedBackFrequencyID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    objbll.FrequencyName = Convert.ToString(dt.Rows[i]["FrequencyName"]);
                    _select.Add(objbll);
                }
            }
            return _select;
        }

        #endregion FeedBackFrequency

        #region mdl_FeedBackDepartment

        public List<mdl_FeedBackDepartment> DropDownDepartment()
        {
            SqlConnection Connection = null;
            List<mdl_FeedBackDepartment> _select = null;
            System.Data.DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_FEEDBACKDEPARTMENT_SELECT_DROPDOWN";
            try
            {
                Connection = DBConnection.GetDBConn();

                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText);
            }
            catch (Exception ex)
            {
                objlogic.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
                // strErrMsg = ex.Message;
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                _select = new List<mdl_FeedBackDepartment>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    mdl_FeedBackDepartment objbll = new mdl_FeedBackDepartment();
                    objbll.FeedBackDepartmentID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    objbll.DepartmentName = Convert.ToString(dt.Rows[i]["DepartmentName"]);
                    _select.Add(objbll);
                }
            }
            return _select;
        }

        #endregion mdl_FeedBackDepartment
    }
}