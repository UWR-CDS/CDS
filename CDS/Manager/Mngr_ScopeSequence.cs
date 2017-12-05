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
    public class Mngr_ScopeSequence
    {
        CommonLogic objlogic = new CommonLogic();
        public List<ScopeSequence> UserScopeSequence(int UserId)
        {
            SqlConnection Connection = null;
            List<ScopeSequence> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_CDS_SCOPESEQUENCE";
            SqlParameter[] aparam = new SqlParameter[] {
                    new SqlParameter("@UserID" , UserId),
                                                        };
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, aparam);
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
                _select = new List<ScopeSequence>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ScopeSequence objbll = new ScopeSequence();
                    objbll.ClassID = Convert.ToInt32(dt.Rows[i]["ClassID"]);
                    objbll.ClassName = Convert.ToString(dt.Rows[i]["ClassName"]);
                    objbll.SubjectID = Convert.ToInt32(dt.Rows[i]["SubjectID"]);
                    objbll.SubjectName = Convert.ToString(dt.Rows[i]["SubjectName"]);
                    objbll.BookID = Convert.ToInt32(dt.Rows[i]["BookID"]);
                    objbll.BookName = Convert.ToString(dt.Rows[i]["BookName"]);
                    objbll.TotalChapters = Convert.ToInt32(dt.Rows[i]["TotalChapters"]);
                    objbll.TotalLessons = Convert.ToInt32(dt.Rows[i]["TotalLessons"]);
                    _select.Add(objbll);
                }
            }
            return _select;
        }
        public List<BookDetail> BookDetails(int BookId)
        {
            SqlConnection Connection = null;
            List<BookDetail> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_CDS_GET_BOOKINFORMATION";
            SqlParameter[] aparam = new SqlParameter[] {
                    new SqlParameter("@BookID" , BookId),
                                                        };
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, aparam);
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
                _select = new List<BookDetail>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BookDetail objbll = new BookDetail();
                    objbll.BookID = Convert.ToInt32(dt.Rows[i]["BookIDFK"]);
                    objbll.BookName = Convert.ToString(dt.Rows[i]["BookName"]);
                    objbll.ChapterID = Convert.ToInt32(dt.Rows[i]["ChapterID"]);
                    objbll.ChapterName = Convert.ToString(dt.Rows[i]["ChapterName"]);
                    objbll.UnitNumber = Convert.ToInt32(dt.Rows[i]["UnitNumber"]);
                    objbll.LessonID = Convert.ToInt32(dt.Rows[i]["LessonID"]);
                    objbll.LessonTitle = Convert.ToString(dt.Rows[i]["LessonTitle"]);
                    objbll.KeyVocablory = Convert.ToString(dt.Rows[i]["KeyVocabulary"]);
                    objbll.Objectives = Convert.ToString(dt.Rows[i]["Objective"]);
                    _select.Add(objbll);
                }
            }
            return _select;
        }

        public List<ScopeSequence> GetUnitLessonInformation(int BookId,int userID)
        {
            List<ScopeSequence> _select = null;
            SqlConnection Connection = null;
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[USP_CDS_WEB_GETALLCHAPTER_LESSON_BYBOOKID]";
            SqlParameter[] aParam = new SqlParameter[]
            {
                new SqlParameter("@BookID", BookId),
                new SqlParameter("@userID", userID),
            };


            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, cmd.CommandType, cmd.CommandText, aParam);

            }
            catch (Exception ex)
            {
                new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), cmd.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                _select = new List<ScopeSequence>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ScopeSequence objbll = new ScopeSequence();
                    objbll.ChapterID = Convert.ToInt32(dt.Rows[i]["ChapterIDFK"]);
                    objbll.UnreadCount = Convert.ToInt32(dt.Rows[i]["UnreadCount"]);
                    objbll.LessonID = Convert.ToInt32(dt.Rows[i]["LessonID"]);
                    objbll.IsExist = Convert.ToInt32(dt.Rows[i]["isExist"]);
                    objbll.EncLessonID = EncyptionDcryption.GetEncryptedText(dt.Rows[i]["LessonID"].ToString());
                    objbll.LessonTitle = Convert.ToString(dt.Rows[i]["LessonTitle"]);
                    objbll.LessonNumber = Convert.ToInt32(dt.Rows[i]["LessonNumber"]);
                    objbll.UnitNumber = Convert.ToInt32(dt.Rows[i]["UnitNumber"]);
                    objbll.Duration = Convert.ToInt32(dt.Rows[i]["Duration"]);
                    objbll.Vocabulary = Convert.ToString(dt.Rows[i]["Vocabulary"]);
                    objbll.Objective = Convert.ToString(dt.Rows[i]["Objective"]);
                    objbll.HaveTemplate = Convert.ToInt32(dt.Rows[i]["ReadyForCDS"]);
                    _select.Add(objbll);
                }
            }
            return _select;

        }

        public int IsUserLessonExists(string LesPlanID, int UserID)
        {
            object result = null;
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            LesPlanID = CDS.Logic.EncyptionDcryption.GetDecryptedText(LesPlanID);
            Command.CommandText = string.Format("SELECT count(0) FROM dbo.cds_LessonPlanMaster m WHERE m.LessonIDFK = {0} AND m.LessonOwnerID = {1}", LesPlanID, UserID);
            try
            {
                Connection = DBConnection.GetDBConn();
                result = SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText);
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

            int res = 0;
            if (result != null)
                int.TryParse(result.ToString(), out res);

            return res;
        }
    }
}