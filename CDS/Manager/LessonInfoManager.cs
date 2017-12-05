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
    public class LessonInfoManager
    {
        CommonLogic objlogic = new CommonLogic();
        public DataSet GetLessonDataForEdit(int LessonId, int USERID,int isInvitationID=0)
        {
            SqlConnection Connection = null;
            DataSet ds = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_CDS_GET_LESSON_DATA_FOR_Editor";

            SqlParameter[] aParam = new SqlParameter[] {
                 new SqlParameter("@LessonID", LessonId),
            new SqlParameter("@USERID", USERID),
            new SqlParameter("@IsIvitationID", isInvitationID),
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                ds = SqlHelper.ExecuteDataset(Connection, Command.CommandType, Command.CommandText, aParam);
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
            return ds;
        }
        public LessonInfo GetLessonHeader(DataTable dt)
        {
            LessonInfo lf = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                    lf = new LessonInfo();
                    int i = 0;
                    lf.LessonPlanID = dt.Rows[i]["LessonPlanID"] == DBNull.Value ? null : EncyptionDcryption.GetEncryptedText(dt.Rows[i]["LessonPlanID"].ToString());
                    lf.LessonPlanIDDecr = dt.Rows[i]["LessonPlanID"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["LessonPlanID"]);
                    lf.LessonTitle = dt.Rows[i]["LessonTitle"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["LessonTitle"]);
                    lf.ClassName = dt.Rows[i]["Grade"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["Grade"]);
                    lf.SubjectName = dt.Rows[i]["SubjectName"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["SubjectName"]);
                    lf.Duration = dt.Rows[i]["Duration"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Duration"]);
                    lf.BookID = dt.Rows[i]["BookID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["BookID"]);
                    lf.EncBookID = EncyptionDcryption.GetEncryptedText(Convert.ToString(lf.BookID));
                    lf.SubjectID = dt.Rows[i]["SubjectID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["SubjectID"]);
                    lf.ClassID = dt.Rows[i]["ClassID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["ClassID"]);
                    lf.LessonCode = dt.Rows[i]["LessonCode"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["LessonCode"]);
                    lf.ChapterID = dt.Rows[i]["ChapterID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["ChapterID"]);
                    lf.LessonPlanStatus = dt.Rows[i]["LessonPlanStatus"].ToString() == "" ? 1 : Convert.ToInt32(dt.Rows[i]["LessonPlanStatus"]);
                    lf.UnitNumber = dt.Rows[i]["UnitNo"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["UnitNo"]);
                    lf.UnitName = dt.Rows[i]["UnitName"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["UnitName"]);
                    lf.LessonNumber = dt.Rows[i]["LessonNo"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonNo"]);
                    lf.BookName = dt.Rows[i]["BookName"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["BookName"]);
                    lf.LessonType = dt.Rows[i]["LessonType"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonType"]);
                    lf.LanguageID = dt.Rows[i]["LanguageID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LanguageID"]);
                    lf.LessonID = dt.Rows[i]["LessonID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonID"]);
                    lf.EncryptLessonID = EncyptionDcryption.GetEncryptedText(lf.LessonID.ToString());
                    lf.AutoSaveTime = dt.Rows[i]["LessonSaveTime"] == DBNull.Value ? 15 : Convert.ToInt32(dt.Rows[i]["LessonSaveTime"]);
                    lf.OwnerId = dt.Rows[i]["LessonOwnerID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonOwnerID"]);
                    lf.VerifiedID = dt.Rows[i]["VerifiedID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["VerifiedID"]);
                    //lf.LockedBy = dt.Rows[i]["LockedBy"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["LockedBy"]);
                    //lf.IsLockByVerifier = dt.Rows[i]["LockByVerifier"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LockByVerifier"]);
                    lf.AllScript = dt.Rows[i]["AllScript"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["AllScript"]);
            }
            return lf;
        }
        public LessonInfo GetPDfLessonHeader(DataTable dt)
        {
            LessonInfo lf = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                lf = new LessonInfo();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lf.LessonPlanID = dt.Rows[i]["LessonPlanID"] == DBNull.Value ? null : EncyptionDcryption.GetEncryptedText(dt.Rows[i]["LessonPlanID"].ToString());
                    lf.LessonPlanIDDecr = dt.Rows[i]["LessonPlanID"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["LessonPlanID"]);
                    lf.LessonTitle = dt.Rows[i]["LessonTitle"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["LessonTitle"]);
                    lf.ClassName = dt.Rows[i]["Grade"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["Grade"]);
                    lf.SubjectName = dt.Rows[i]["SubjectName"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["SubjectName"]);
                    lf.Duration = dt.Rows[i]["Duration"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Duration"]);
                    lf.BookID = dt.Rows[i]["BookID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["BookID"]);
                    lf.EncBookID = EncyptionDcryption.GetEncryptedText(Convert.ToString(lf.BookID));
                    lf.SubjectID = dt.Rows[i]["SubjectID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["SubjectID"]);
                    lf.ClassID = dt.Rows[i]["ClassID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["ClassID"]);
                    lf.LessonCode = dt.Rows[i]["LessonCode"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["LessonCode"]);
                    lf.ChapterID = dt.Rows[i]["ChapterID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["ChapterID"]);
                    lf.LessonPlanStatus = dt.Rows[i]["LessonPlanStatus"].ToString() == "" ? 1 : Convert.ToInt32(dt.Rows[i]["LessonPlanStatus"]);
                    lf.UnitNumber = dt.Rows[i]["UnitNo"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["UnitNo"]);
                    lf.UnitName = dt.Rows[i]["UnitName"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["UnitName"]);
                    lf.LessonNumber = dt.Rows[i]["LessonNo"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonNo"]);
                    lf.BookName = dt.Rows[i]["BookName"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["BookName"]);
                    lf.LessonType = dt.Rows[i]["LessonType"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonType"]);
                    lf.LanguageID = dt.Rows[i]["LanguageID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LanguageID"]);
                    lf.LessonID = dt.Rows[i]["LessonID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonID"]);
                    lf.EncryptLessonID = EncyptionDcryption.GetEncryptedText(lf.LessonID.ToString());
                    lf.AutoSaveTime = dt.Rows[i]["LessonSaveTime"] == DBNull.Value ? 15 : Convert.ToInt32(dt.Rows[i]["LessonSaveTime"]);
                    lf.OwnerId = dt.Rows[i]["LessonOwnerID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonOwnerID"]);
                }
            }
            return lf;
        }
        public List<Section> GetLessonSections(DataTable dt)
        {
            List<Section> sections = new List<Section>();
            if (dt != null && dt.Rows.Count > 0)
            {
                sections = new List<Section>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Section section = new Section();
                    section.Name = dt.Rows[i]["SectionName"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["SectionName"]);
                    section.Duration = dt.Rows[i]["SectionDuration"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["SectionDuration"]);
                    section.Id = dt.Rows[i]["SectionIDFK"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["SectionIDFK"]);
                    section.SubsectionCount = dt.Rows[i]["SubsectionCount"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["SubsectionCount"]);
                    section.SingleSubsectionId = dt.Rows[i]["SingelSubsectionId"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[i]["SingelSubsectionId"]);
                    sections.Add(section);
                }
            }
            return sections;
        }
        public List<SubSection> GetLessonSubSections(DataTable dt)
        {
            List<SubSection> subsections1 = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                subsections1 = new List<SubSection>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubSection subsection = new SubSection();
                    subsection.LessonPlanScriptId = dt.Rows[i]["LessonPlanSectionID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonPlanSectionID"]);
                    subsection.SectionId = dt.Rows[i]["SectionIDFK"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["SectionIDFK"]);
                    subsection.SubSectionId = dt.Rows[i]["SubSectionIDFK"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["SubSectionIDFK"]);
                    subsection.SubSectionName = dt.Rows[i]["SubSectionName"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["SubSectionName"]);
                    subsection.LessonPlanScript = dt.Rows[i]["Script"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["Script"]);
                    subsections1.Add(subsection);
                }
            }
            return subsections1;
        }
        public List<BookLessoninfo> GetTotalBookLesson(int BookId, string Condition, int UserID, int BooktypeId = 1)
        {
            SqlConnection Connection = null;
            List<BookLessoninfo> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_CDS_GETBOOKLESSONINFO";
            SqlParameter[] aParam = new SqlParameter[] {
                 new SqlParameter("@Bookid", BookId),
                 new SqlParameter("@Condition", Condition),
                 new SqlParameter("@BookType", BooktypeId),
                 new SqlParameter("@UserID", UserID),
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                _select = new List<BookLessoninfo>();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, aParam);
                if (dt.Rows.Count != 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        BookLessoninfo obj = new BookLessoninfo();
                        int ID = dt.Rows[i]["LessonID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonID"]);
                        obj.intLessonID = ID;
                        if (ID != 0)
                        {
                            string LID = EncyptionDcryption.GetEncryptedText(ID.ToString());
                            obj.LessonID = LID;
                        }
                        obj.LessonName = dt.Rows[i]["LessonTitle"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["LessonTitle"]);
                        int CID = dt.Rows[i]["ChapterIDFK"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["ChapterIDFK"]);
                        if (CID != 0)
                        {
                            string cID = EncyptionDcryption.GetEncryptedText(CID.ToString());
                            obj.ChapterID = cID;
                        }
                        obj.ChapterNumber = dt.Rows[i]["ChapterNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["ChapterNumber"]);
                        obj.ChapterName = dt.Rows[i]["ChapterName"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["ChapterName"]);
                        obj.Status = dt.Rows[i]["LessonStatus"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonStatus"]);
                        obj.LessonType = dt.Rows[i]["LessonType"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonType"]);
                        obj.LessonNumber = dt.Rows[i]["LessonNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonNumber"]);
                        _select.Add(obj);
                    }
                }
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

            return _select;
        }
        public List<LessonImages> ImagesSelect(int id, int groupid, int userID, bool isAll = false, int ImageFor = 1)
        {
            SqlConnection Connection = null;
            List<LessonImages> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_WEB_LES_IMAGES_SELECT";

            SqlParameter[] aParam;
            if (isAll)
            {
                aParam = new SqlParameter[] { new SqlParameter("@LessonIDFK", id), new SqlParameter("@MediaGroupType", groupid), new SqlParameter("@ImageForIDFK", 2), new SqlParameter("@UserID", userID)};
            }
            else
            {
                aParam = new SqlParameter[] { new SqlParameter("@LessonIDFK", id), new SqlParameter("@MediaGroupType", groupid), new SqlParameter("@ImageForIDFK", ImageFor), new SqlParameter("@UserID", userID)};
            }
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
                _select = new List<LessonImages>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    LessonImages objbll = new LessonImages();
                    objbll.FileID = Convert.ToInt32(dt.Rows[i]["FileID"]);
                    objbll.BookIDFK = Convert.ToInt32(dt.Rows[i]["BookIDFK"]);
                    objbll.LessonIDFK = Convert.ToInt32(dt.Rows[i]["LessonIDFK"]);
                    objbll.ImageExt = Convert.ToString(dt.Rows[i]["ImageExt"]);
                    objbll.ImageName = Convert.ToString(dt.Rows[i]["ImageName"]);
                    objbll.NickName = Convert.ToString(dt.Rows[i]["NickName"]);
                    objbll.UserID = Convert.ToInt32(dt.Rows[i]["CDSUserID"]);

                    byte[] imgEncrypt = UTF8Encoding.UTF8.GetBytes(Convert.ToString(dt.Rows[i]["ImageName"]));
                    objbll.EncName = Convert.ToBase64String(imgEncrypt);

                    _select.Add(objbll);
                }
            }
            return _select;
        }
        public string CheckScriptAudio(string LessonPlanID)
        {
            object count = null;
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "select ActualName from LEAF_LOG.dbo.ScriptAudioLog where LessonPlanID =" + LessonPlanID + "and FileName='S_" + LessonPlanID + "'";
            try
            {
                Connection = DBConnection.GetDBConn();

                count = SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText);
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
            if (count == null)
            {
                return null;
            }
            else
            {
                return count.ToString();
            }
        }
        public int UpdateLessonPalnScript(int ID, string data, int UserID, Int64 LessonPalnID, int LoginTrackingId)
        {
            int status = 0;
            string ss = null;
            SqlConnection Connection = null;
            SqlCommand cmd = new SqlCommand();
            DataTable dt = null;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "USP_WEB_CDS_Update_LessonPlanData";
            SqlParameter[] prams = new SqlParameter[] {


                    new SqlParameter("@LessonPlanSectionId" , ID),
                    new SqlParameter("@Data" , data),
                    new SqlParameter("@userid" , UserID),
                       new SqlParameter("@LessonPlanId" ,LessonPalnID),
                       new SqlParameter("@LogTrackingID" ,LoginTrackingId)
                                                        };
            try
            {
                Connection = DBConnection.GetDBConn();


                dt = SqlHelper.ExecuteDataTable(Connection, cmd.CommandType, cmd.CommandText, prams);
                if (dt.Rows.Count != 0)
                {
                    status = Convert.ToInt32(dt.Rows[0]["LogTrackingID"]);
                }

            }
            catch (Exception ex)
            {
                ss = ex.Message;
                new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), cmd.CommandText);
                status = -1;
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            return status;
        }
        public int ManageLessonTime(Int64 LogID, int UserID, Int64 LessonPalnID, int LesPlansecID)
        {
            int status = 0;
            string ss = null;
            SqlConnection Connection = null;
            SqlCommand cmd = new SqlCommand();
            DataTable dt = null;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "USP_WEB_CDS_SET_LESSONTIMELINE";
            SqlParameter[] prams = new SqlParameter[] {
                     new SqlParameter("@LogID" , LogID),
                    new SqlParameter("@userid" , UserID),
                       new SqlParameter("@CDSLessonPlanID" ,LessonPalnID),
                       new SqlParameter("@LesPlanSectionId" ,LesPlansecID)
                                                        };
            try
            {
                Connection = DBConnection.GetDBConn();


                dt = SqlHelper.ExecuteDataTable(Connection, cmd.CommandType, cmd.CommandText, prams);
                if (dt.Rows.Count != 0)
                {
                    status = Convert.ToInt32(dt.Rows[0]["LogID"]);
                }

            }
            catch (Exception ex)
            {
                ss = ex.Message;
                new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), cmd.CommandText);
                status = -1;
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            return status;
        }
        public List<SectionHistory> LessonPlanUserWork(int LessonPlanSectionID,int LessonPlanID, int Num = 5)
        {
            SqlConnection Connection = null;
            List<SectionHistory> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP__CDS_GET_LOG_SECTION_USERS_WORK";
            SqlParameter[] aparam = new SqlParameter[] {

                    new SqlParameter("@LessonPlanSectionId" , LessonPlanSectionID),
                      new SqlParameter("@LessonPlanID" , LessonPlanID),
                     new SqlParameter("@Num" , Num),
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
                _select = new List<SectionHistory>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SectionHistory objbll = new SectionHistory();
                    objbll.LogID = Convert.ToInt32(dt.Rows[i]["LogID"]);
                    objbll.LessonPlanSectionID = Convert.ToInt32(dt.Rows[i]["LessonPlanSectionID"]);
                    objbll.LessonPlanScript = Convert.ToString(dt.Rows[i]["LessonPlanScript"]);
                    objbll.UpdateDateTime = Convert.ToDateTime(dt.Rows[i]["DateTimeStamp"]);
                    objbll.UserID = Convert.ToInt32(dt.Rows[i]["USERID"]);
                    objbll.FirstName = Convert.ToString(dt.Rows[i]["FIRSTNAME"]);
                    objbll.LastName = Convert.ToString(dt.Rows[i]["LASTNAME"]);
                    _select.Add(objbll);
                }
            }
            return _select;
        }
        public string RestoreSectionData(Int64 LogId, int USERID)
        {
            SqlConnection Connection = null;
            DataTable dt = null;
            string Data = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_Restore_SectionHistory";
            SqlParameter[] aparam = new SqlParameter[]
            {
                    new SqlParameter("@LogId" , LogId),
                      new SqlParameter("@USerID" , USERID),
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
                Data = Convert.ToString(dt.Rows[0]["LessonPlanScript"]);
            }

            return Data;
        }

        public string CompleteLessonPlanMaster(int LessonId, int UserId, int BooktypeID, int isIndexPage)
        {
            string result = null;
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            try
            {
                Connection = DBConnection.GetDBConn();
                Command = Connection.CreateCommand();
                Command.Connection = Connection;

                //-------------Lesson details---
                Command.CommandType = CommandType.Text;
                Command.CommandText = "exec USP_WEB_COMPLETE_LESSON " + LessonId + "," + UserId;
                SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText);
                result = "Done";
            }

            catch (Exception ex)
            {
                //strErrMsg = ex.Message;
                new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            return result;
        }

        public List<BLL_ChatHistory> CommentsSectionByLessonID(int LessonID,int LessonPlanID = 0,int UserID = 0)
        {
            SqlConnection Connection = null;
            List<BLL_ChatHistory> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();


            SqlParameter[] aParam = new SqlParameter[]
           { new SqlParameter("@LessonID" , LessonID) ,
               new SqlParameter("@LessonPlanID" , LessonPlanID) ,
               new SqlParameter("@UserID" , UserID) ,
          };

            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_GET_COMMENTS_BY_LESSONID";

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
                _select = new List<BLL_ChatHistory>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BLL_ChatHistory objbll = new BLL_ChatHistory();
                    objbll.Message_ID = Convert.ToInt32(dt.Rows[i]["MessageID"]);
                    objbll.sectionID = Convert.ToInt32(dt.Rows[i]["SectionId"]);
                    objbll.EncMessage_ID = EncyptionDcryption.GetEncryptedText(dt.Rows[i]["MessageID"].ToString());
                    objbll.UserID_A = Convert.ToInt32(dt.Rows[i]["SenderID"]);
                    objbll.EncUserID_A = EncyptionDcryption.GetEncryptedText(dt.Rows[i]["SenderID"].ToString());
                    objbll.MessageDateTime = Convert.ToDateTime(dt.Rows[i]["MessageDateTime"]);
                    objbll.MessageDateTimeStr = dt.Rows[i]["MessageDateTime"].ToString();
                    objbll.LessonID = dt.Rows[i]["LessonID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonID"]);
                    objbll.Comment = Convert.ToString(dt.Rows[i]["Comment"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["ReceiverID"])))
                    {
                        if (Convert.ToInt32(dt.Rows[i]["ReceiverID"]) == UserID)
                        {
                            if (objbll.Comment.Contains("</b>"))
                            {
                                int index = objbll.Comment.LastIndexOf("</b>");
                                int length = index + 3;
                                objbll.Comment = objbll.Comment.Substring(length + 1).Trim();
                            }
                        }

                    }
                    objbll.FirstName = Convert.ToString(dt.Rows[i]["FIRSTNAME"]);
                    objbll.LastName = Convert.ToString(dt.Rows[i]["LASTNAME"]);
                    if (string.IsNullOrEmpty(dt.Rows[i]["IsViewed"].ToString()))
                    {
                        objbll.isRead = true;
                    }
                    else
                    {
                        objbll.isRead = Convert.ToBoolean(dt.Rows[i]["IsViewed"].ToString() == "False" ? false : true);
                    }
                    objbll.CommentTimeAgo = GetCommentTimeAgo(objbll.MessageDateTime);
                    objbll.ReceiverUnreadCount = Convert.ToInt32(dt.Rows[i]["UnreadCount"]);
                    _select.Add(objbll);

                }

            }


            return _select;
        }

        public int CheckLessonUser(int LessonId = 0,int LessonPlanID = 0, int UserID = 0)
        {
            SqlConnection Connection = null;
            int Status = 0;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "select LessonOwnerID from cds_LessonPlanMaster where LessonPlanID = " + LessonPlanID + " and LessonIDFK = " + LessonId + "";
            try
            {
                Connection = DBConnection.GetDBConn();
                Status =  Convert.ToInt32(SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText));
                if (Status == UserID)
                {
                    Status = 2;
                }
                else
                {
                    Command.CommandText = "select count(*) from CDS_User_Lesson_Rights where UserIDFK = " + UserID + " and LessonPlanID = " + LessonPlanID + " ";
                    try
                    {
                        Connection = DBConnection.GetDBConn();
                        Status = Convert.ToInt32(SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText));
                    }
                    catch (Exception ex)
                    {
                        objlogic.InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
                    }
                }
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
            return Status;
        }

        public LessonUser GetUserWorkingOnLesson(int LessonID, int LessonPlanID = 0, int UserID = 0)
        {
            SqlConnection Connection = null;
            LessonUser _select = null;
            DataSet ds = null;
            SqlCommand Command = new SqlCommand();


            SqlParameter[] aParam = new SqlParameter[]
           { new SqlParameter("@LessonID" , LessonID) ,
               new SqlParameter("@LessonPlanID" , LessonPlanID) ,
               new SqlParameter("@UserID" , UserID) ,
          };

            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_GET_USER_WORKING_ON_LESSON";

            try
            {
                Connection = DBConnection.GetDBConn();

                ds = SqlHelper.ExecuteDataset(Connection, Command.CommandType, Command.CommandText, aParam);

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


            if (ds != null)
            {
                _select = new LessonUser();
                _select.LessonUserRightList = new List<LessonUserRight>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                    {
                        if (i == 0)
                        {
                            LessonUserRight objbll = new LessonUserRight();
                            objbll.UserID = Convert.ToInt32(ds.Tables[i].Rows[j]["UserID"]);
                            objbll.UserName = Convert.ToString(ds.Tables[i].Rows[j]["UserName"]);
                            objbll.Rights = Convert.ToString(ds.Tables[i].Rows[j]["Rights"]);
                            objbll.UserEmail = Convert.ToString(ds.Tables[i].Rows[j]["Email"]);
                            objbll.UserContactNo     = Convert.ToString(ds.Tables[i].Rows[j]["ContactNo"]);
                            objbll.Icon = "View";
                            objbll.LessonRightList = new List<string>();

                            if (!string.IsNullOrEmpty(objbll.Rights))
                            {
                                if (objbll.Rights.Contains(','))
                                {
                                    string[] RightsArray = objbll.Rights.Split(',');

                                    foreach (string item in RightsArray)
                                    {
                                        objbll.LessonRightList.Add(item);
                                    }
                                    objbll.Icon = "Edit";
                                }
                                else
                                {
                                    objbll.LessonRightList.Add(objbll.Rights);
                                }
                            }
                            _select.LessonUserRightList.Add(objbll);
                        }
                        else if (i == 1)
                        {
                            LessonOwner objbll = new LessonOwner();
                            objbll.OwnerName = Convert.ToString(ds.Tables[i].Rows[j]["OwnerName"]);
                            objbll.OwnerID = Convert.ToInt32(ds.Tables[i].Rows[j]["OwnerID"]);
                            if (Convert.ToInt32(ds.Tables[i].Rows[j]["InHour"]) <= 0)
                            {
                                objbll.TimeConsumed = Convert.ToString(ds.Tables[i].Rows[j]["InHour"]) + ":" + Convert.ToString(ds.Tables[i].Rows[j]["InMinute"]) + " mins";

                            }
                            else {
                                objbll.TimeConsumed = Convert.ToString(ds.Tables[i].Rows[j]["InHour"]) + ":" + Convert.ToString(ds.Tables[i].Rows[j]["InMinute"]) + " hrs";

                            }
                            objbll.CompleteDate = Convert.ToString(ds.Tables[i].Rows[j]["CompleteDate"]);
                            objbll.EndTime = Convert.ToString(ds.Tables[i].Rows[j]["EndDateTime"]);
                            objbll.StartTime = Convert.ToString(ds.Tables[i].Rows[j]["CreateDate"]);
                            objbll.OwnerEmail = Convert.ToString(ds.Tables[i].Rows[j]["Email"]);
                            objbll.ContactNo = Convert.ToString(ds.Tables[i].Rows[j]["ContactNo"]);
                            objbll.LastVerifiyBy = Convert.ToString(ds.Tables[i].Rows[j]["LastVerifyBy"]);
                            objbll.Rating = Convert.ToInt32(ds.Tables[i].Rows[j]["Rating"]);

                            _select.LessonOwner = objbll;
                        }
                    }
                }



            }


            return _select;
        }

        public List<LessonUser> GetLessonRights(int userID, int lessonPlanID,int lessonID = 0)
        {
            SqlConnection Connection = null;
            List<LessonUser> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();


            SqlParameter[] aParam = new SqlParameter[]
           {
               new SqlParameter("@UserID" , userID),
               new SqlParameter("@lessonPlanID" , lessonPlanID),
               new SqlParameter("@lessonID" , lessonID),
          };

            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_GET_LESSON_RIGHTS";

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
                _select = new List<LessonUser>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    LessonUser objbll = new LessonUser();
                    objbll.LessonRightsID = Convert.ToInt32(dt.Rows[i]["LessonRightsID"]);
                    if (lessonID != 0)
                    {
                        objbll.LessonOwnerID = Convert.ToInt32(dt.Rows[i]["OwnerID"]);
                    }
                    objbll.LessonRightsName = Convert.ToString(dt.Rows[i]["LessonRightsName"]);
                    if (objbll.LessonRightsName.Contains("Media"))
                    {
                        objbll.LessonRightsName += " Files";
                    }
                    objbll.UserName = Convert.ToString(dt.Rows[i]["UserName"]);
                    objbll.HasRights = Convert.ToBoolean(dt.Rows[i]["HasRights"].ToString() == "0" ? false : true);
                    _select.Add(objbll);
                }

            }


            return _select;
        }

        public int UpdateUserLessonRights(int item, int userID,int a,int lessonPlanID,string queryFor,bool isRevoke = false)
        {
            SqlConnection Connection = null;
            int Status = 0;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            if (isRevoke)
            {
                Command.CommandText = "delete from CDS_User_Lesson_Rights where UserIDFK = " + userID + " and LessonPlanID = " + lessonPlanID + "";
            }
            else
            {
                if (queryFor == "selection")
                {
                    if (a == 1)
                    {
                        Command.CommandText = "delete from CDS_User_Lesson_Rights where UserIDFK = " + userID + " and LessonRightIDFK <> 6 and LessonPlanID = " + lessonPlanID + "";
                        Command.CommandText += " insert into dbo.CDS_User_Lesson_Rights values (" + userID + "," + item + "," + lessonPlanID + ") ";
                    }
                    else
                    {
                        Command.CommandText += " insert into dbo.CDS_User_Lesson_Rights values (" + userID + "," + item + "," + lessonPlanID + ") ";
                    }
                }
                else
                {
                    Command.CommandText = "delete from CDS_User_Lesson_Rights where UserIDFK = " + userID + " and LessonRightIDFK <> 6 and LessonPlanID = " + lessonPlanID + "";
                }
            }
            try
            {
                Connection = DBConnection.GetDBConn();
                SqlHelper.ExecuteNonQuery(Connection, Command.CommandType, Command.CommandText);
                Status = 1;
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
            return Status;
        }

        public string GetCommentTimeAgo(DateTime commentDate)
        {
            TimeSpan span = DateTime.Now - commentDate;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("{0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("{0} {1} ago",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("{0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("{0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("{0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "min" : "mins");
            if (span.Seconds > 5)
                return String.Format("{0} sec ago", span.Seconds);
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }

        public void ModifyLesson(int LessonPlanID,int RejectBy,string Reason)
        {
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_CDS_ADD_LESSON_RejctDetails";
            SqlParameter[] aParam = new SqlParameter[]
              {
                   new SqlParameter("@LessonPlanID" , LessonPlanID),
                   new SqlParameter("@RejectBy" , RejectBy),
                   new SqlParameter("@Reason" , Reason),
             };

            try
            {
                Connection = DBConnection.GetDBConn();

               SqlHelper.ExecuteNonQuery(Connection, Command.CommandType, Command.CommandText,aParam);
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
        }

        public int VerifyLesson(int LessonPlanID, int UserID)
        {
            SqlConnection Connection = null;
            int VerifiedID = 0;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_CDS_VERIFY_LESSON";
            SqlParameter[] aParam = new SqlParameter[]
           {
                new SqlParameter("@LessonPlanID",LessonPlanID),
           new SqlParameter("@VerifyBy", UserID) ,

          };
            try
            {
                Connection = DBConnection.GetDBConn();
                VerifiedID=Convert.ToInt32(SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText,aParam));
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
            return VerifiedID;
        }

        public int GetLessonRating(int LessonPlanID = 0, int UserID = 0)
        {
            SqlConnection Connection = null;
            int Rating = 0;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "select top 1 isnull(Rating,0) Rating from dbo.CDS_LessonPlanRating R where LessonPlanID= " + LessonPlanID + " and Ratedby = " + UserID + " and VerifiedID=isnull((select VerifiedID from cds_LessonPlanMaster where LessonPlanID=R.LessonPlanID),0) order by RatedDate desc";
            try
            {
                Connection = DBConnection.GetDBConn();
                Rating = Convert.ToInt32(SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText));
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
            return Rating;
        }

        public void RateLesson(int LessonPlanID,int Rating, int UserID,int VerifiedID)
        {
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_CDS_RATE_LESSON";
            SqlParameter[] aParam = new SqlParameter[]
           {
                new SqlParameter("@LessonPlanID",LessonPlanID),
                 new SqlParameter("@Rating",Rating),
           new SqlParameter("@RateBy", UserID) ,
           new SqlParameter("@VerifiedID", VerifiedID) ,
          };
            try
            {
                Connection = DBConnection.GetDBConn();

                SqlHelper.ExecuteNonQuery(Connection, Command.CommandType, Command.CommandText, aParam);
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
        }
        public LessonLock ResetLessonPlanLock(int LessonPlanID, int UserId, int LockStatus)
        {
            SqlConnection con = null;
            LessonLock obj = new LessonLock();
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "USP_WEB_CDS_ResetLessonPlanLock";
            SqlParameter[] aParam = new SqlParameter[] {
                new SqlParameter("@UserID", UserId),
                new SqlParameter("@LessonPlanID", LessonPlanID),
                new SqlParameter("@LockStatus", LockStatus)};


            try
            {
                con = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(con, cmd.CommandType, cmd.CommandText, aParam);
                if (dt!=null && dt.Rows.Count!=0)
                {
                    obj = new LessonLock();
                    obj.LockedBy = dt.Rows[0]["MSG"] == DBNull.Value ? null : Convert.ToString(dt.Rows[0]["MSG"]);
                    obj.VerifiedLockTrackId = dt.Rows[0]["LockTrackID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["LockTrackID"]);
                    obj.IsLockByVerifier = dt.Rows[0]["LockByVerifier"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["LockByVerifier"]);
                }
            }

            catch (Exception ex)
            {
                //throw;
                new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), cmd.CommandText);
            }
            finally
            {
                con.Close();
            }
            return obj;
        }

        public void updateLock(int LessonPlanID,int UserID,int TrackID)
        {
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "update cds_LessonPlanMaster set LockedDateTime=GETDATE() where LessonPlanID="+LessonPlanID+" and LockedBy="+UserID+";";
            Command.CommandText += " update LEAF_LOG.dbo.CDS_Verified_LessonTracking set EndTime=getdate() where TrackID=" + TrackID;
            try
            {
                Connection = DBConnection.GetDBConn();
                SqlHelper.ExecuteNonQuery(Connection, Command.CommandType, Command.CommandText);
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
        }

        public void RealeseLessonLock(int LessonPlanID)
        {
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "update cds_LessonPlanMaster set LockedDateTime=null,LockedBy=null where LessonPlanID="+LessonPlanID;
            try
            {
                Connection = DBConnection.GetDBConn();
                SqlHelper.ExecuteNonQuery(Connection, Command.CommandType, Command.CommandText);
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
        }

        public LessonSourceInformation GetLessonSourceInfo(int LessonPlanID = 0)
        {
            SqlConnection Connection = null;
            LessonSourceInformation obj = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            SqlParameter[] aParam = new SqlParameter[]
            {
               new SqlParameter("@LessonPlanID" , LessonPlanID)
            };
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_GET_LESSON_SOURCE_DETAILS";

            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, aParam);
                if (dt != null && dt.Rows.Count!=0)
                {
                    obj = new LessonSourceInformation();
                    obj.Name = Convert.ToString(dt.Rows[0]["Name"]);
                    obj.Email = Convert.ToString(dt.Rows[0]["Email"]);
                    obj.Phone = Convert.ToString(dt.Rows[0]["Phone"]);
                    obj.Rating = Convert.ToString(dt.Rows[0]["Rating"]);
                    obj.LessonVotes = Convert.ToString(dt.Rows[0]["LessonVotes"]);
                    obj.ToatlLessons = Convert.ToString(dt.Rows[0]["TotalLessons"]);
                    obj.ToatlVotes = Convert.ToString(dt.Rows[0]["TotalVotes"]);
                }
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
            return obj;
        }
        public void UpdateVerifiedTracID(int TrackID)
        {
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "update LEAF_LOG.dbo.CDS_Verified_LessonTracking set EndTime=getdate() where TrackID=" + TrackID;
            try
            {
                Connection = DBConnection.GetDBConn();
                SqlHelper.ExecuteNonQuery(Connection, Command.CommandType, Command.CommandText);
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
        }
        #region LessonPlanList
        public ListLessonInfo GetLessonPlanList(int UserID,int SelectedTab)
        {
            SqlConnection Connection = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_GET_LESSONSCRIPT_LIST";
            SqlParameter[] aParam = new SqlParameter[] { new SqlParameter("@UserID", UserID), new SqlParameter("@SelectedTab", SelectedTab) };

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

            List<LessonInfo> _select = null;

            if (dt != null && dt.Rows.Count > 0)
            {
                _select = new List<LessonInfo>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    LessonInfo objbll = new LessonInfo();
                    objbll.LessonID = Convert.ToInt32(dt.Rows[i]["LessonID"]);
                    objbll.EncryptLessonID = EncyptionDcryption.GetEncryptedText(objbll.LessonID.ToString());
                    objbll.EncryptBookID = EncyptionDcryption.GetEncryptedText(objbll.BookID.ToString());
                    //objbll.BookID = Convert.ToInt32(dt.Rows[i]["BookIDFK"]);
                    //objbll.LessonType = Convert.ToInt32(dt.Rows[i]["LessonType"]);
                    objbll.ChapterNumber = Convert.ToInt32(dt.Rows[i]["ChapterNumber"]);
                    objbll.LessonPlanID = Convert.ToString(dt.Rows[i]["LessonPlanID"]);
                    objbll.LessonNumber = Convert.ToInt32(dt.Rows[i]["LessonNumber"]);
                    objbll.LessonTitle = Convert.ToString(dt.Rows[i]["LessonTitle"]);
                    objbll.LessonPlanStatus = Convert.ToInt32(dt.Rows[i]["LessonPlanStatus"]);
                    objbll.ClassName = Convert.ToString(dt.Rows[i]["ClassName"]);
                    objbll.SubjectName = dt.Rows[i]["StartDateTime"] != null ? Convert.ToString(dt.Rows[i]["SubjectName"]) : "";
                    objbll.StartTime = dt.Rows[i]["StartDateTime"] != null ? Convert.ToString(dt.Rows[i]["StartDateTime"]) : "";
                    objbll.EndTime = Convert.ToString(dt.Rows[i]["EndDateTime"]);
                    objbll.CompleteDate = Convert.ToString(dt.Rows[i]["CompleteDate"]);
                    objbll.VerificationDate = Convert.ToString(dt.Rows[i]["VerificationDate"]);
                    objbll.VerifiedBy = Convert.ToString(dt.Rows[i]["VerifiedBy"]);
                    if (Convert.ToInt32(dt.Rows[i]["InHour"]) <= 0)
                    {
                        objbll.TimeConsumed = Convert.ToString(dt.Rows[i]["InHour"]) + ":" + Convert.ToString(dt.Rows[i]["InMinute"]) + " mins";
                    }
                    else {
                        objbll.TimeConsumed = Convert.ToString(dt.Rows[i]["InHour"]) + ":" + Convert.ToString(dt.Rows[i]["InMinute"]) + " hrs";
                    }
                    objbll.Rating = Convert.ToInt32(dt.Rows[i]["Rating"]);
                    _select.Add(objbll);
                }

            }

            ListLessonInfo info = new ListLessonInfo();
            info.lstLessonInfo = _select;
            return info;
        }

        public string DeleteLesson(string val)
        {
            SqlConnection Connection = null;
            string result = "";
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_Delete_LESSONSCRIPT";
            SqlParameter[] aParam = new SqlParameter[] { new SqlParameter("@LessonPlanID", val), };

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

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["msg"].ToString();
                }
            }
            return result;
        }

        public ListLessonInfo UnderVerificationLessons(int UserID)
        {
            SqlConnection Connection = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_UnderVerification_LESSON";
            SqlParameter[] aparm = new SqlParameter[] {
                new SqlParameter("@UserID",UserID)
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText,aparm);
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

            List<LessonInfo> _select = null;

            if (dt != null && dt.Rows.Count > 0)
            {
                _select = new List<LessonInfo>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    LessonInfo objbll = new LessonInfo();
                    objbll.LessonID = Convert.ToInt32(dt.Rows[i]["LessonID"]);
                    objbll.OwnerId = Convert.ToInt32(dt.Rows[i]["LessonOwnerID"]);
                    objbll.EncryptLessonID = EncyptionDcryption.GetEncryptedText(objbll.LessonID.ToString());
                    objbll.EncryptBookID = EncyptionDcryption.GetEncryptedText(objbll.BookID.ToString());
                    objbll.EncLessonOwnerID = EncyptionDcryption.GetEncryptedText(objbll.OwnerId.ToString());
                    objbll.LessonPlanID = Convert.ToString(dt.Rows[i]["LessonPlanID"]);
                    objbll.LessonNumber = Convert.ToInt32(dt.Rows[i]["LessonNumber"]);
                    objbll.LessonTitle = Convert.ToString(dt.Rows[i]["LessonTitle"]);
                    objbll.LessonPlanStatus = Convert.ToInt32(dt.Rows[i]["LessonPlanStatus"]);
                    objbll.ClassName = Convert.ToString(dt.Rows[i]["ClassName"]);
                    objbll.SubjectName = dt.Rows[i]["SubjectName"] != null ? Convert.ToString(dt.Rows[i]["SubjectName"]) : "";
                    objbll.CompleteDate = Convert.ToString(dt.Rows[i]["CompleteDate"]);
                    objbll.CompleteDateTime = dt.Rows[i]["CompleteDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dt.Rows[i]["CompleteDate"]);
                    objbll.VerificationDate = Convert.ToString(dt.Rows[i]["VerificationDate"]);
                    objbll.VerifiedBy = Convert.ToString(dt.Rows[i]["VerifiedBy"]);
                    objbll.Rating = Convert.ToInt32(dt.Rows[i]["Rating"]);
                    objbll.CreatedBy = Convert.ToString(dt.Rows[i]["CreatedBy"]);
                    objbll.RejectCount = Convert.ToInt32(dt.Rows[i]["Reject"]);
                    objbll.VerifyCount = Convert.ToInt32(dt.Rows[i]["Verify"]);
                    if (Convert.ToInt32(dt.Rows[i]["InHour"]) <= 0)
                    {
                        objbll.TimeConsumed = Convert.ToString(dt.Rows[i]["InHour"]) + ":" + Convert.ToString(dt.Rows[i]["InMinute"]) + " mins";
                    }
                    else
                    {
                        objbll.TimeConsumed = Convert.ToString(dt.Rows[i]["InHour"]) + ":" + Convert.ToString(dt.Rows[i]["InMinute"]) + " hrs";
                    }
                    objbll.TimeConsumedAgo=GetCommentTimeAgo(objbll.CompleteDateTime);
                    _select.Add(objbll);
                }

            }

            ListLessonInfo info = new ListLessonInfo();
            info.lstLessonInfo = _select;
            return info;
        }

        public ListLessonInfo VerifiedLessonList(int SelectedTab,int UserID) {
            SqlConnection Connection = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_VERIFIED_LESSON_LIST";
            SqlParameter[] apram = new SqlParameter[] {
                new SqlParameter("@SelectedTab",SelectedTab),
                new SqlParameter("@UserID",UserID)
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText,apram);
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

            List<LessonInfo> _select = null;

            if (dt != null && dt.Rows.Count > 0)
            {
                _select = new List<LessonInfo>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    LessonInfo objbll = new LessonInfo();
                    objbll.LessonID = Convert.ToInt32(dt.Rows[i]["LessonID"]);
                    objbll.OwnerId = Convert.ToInt32(dt.Rows[i]["LessonOwnerID"]);
                    objbll.EncryptLessonID = EncyptionDcryption.GetEncryptedText(objbll.LessonID.ToString());
                    objbll.EncLessonOwnerID = EncyptionDcryption.GetEncryptedText(objbll.OwnerId.ToString());
                    objbll.LessonPlanID = Convert.ToString(dt.Rows[i]["LessonPlanID"]);
                    objbll.StartTime = Convert.ToString(dt.Rows[i]["CreationDate"]);
                    objbll.LessonNumber = Convert.ToInt32(dt.Rows[i]["LessonNumber"]);
                    objbll.LessonTitle = Convert.ToString(dt.Rows[i]["LessonTitle"]);
                    objbll.LessonPlanStatus = Convert.ToInt32(dt.Rows[i]["LessonPlanStatus"]);
                    objbll.ClassName = Convert.ToString(dt.Rows[i]["ClassName"]);
                    objbll.SubjectName = dt.Rows[i]["SubjectName"] != null ? Convert.ToString(dt.Rows[i]["SubjectName"]) : "";
                    objbll.CompleteDate = Convert.ToString(dt.Rows[i]["CompleteDate"]);
                    objbll.VerificationDate = Convert.ToString(dt.Rows[i]["VerificationDate"]);
                    objbll.VerifiedBy = Convert.ToString(dt.Rows[i]["VerifiedBy"]);
                    objbll.Rating = Convert.ToInt32(dt.Rows[i]["Rating"]);
                    objbll.CreatedBy = Convert.ToString(dt.Rows[i]["CreatedBy"]);
                    _select.Add(objbll);
                }

            }

            ListLessonInfo info = new ListLessonInfo();
            info.lstLessonInfo = _select;
            return info;
        }

        public ListLessonInfo VerifiedLesson(int UserID)
        {
            SqlConnection Connection = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_VERIFIED_LESSON_LIST";
            SqlParameter[] apram = new SqlParameter[] {
                new SqlParameter("@SelectedTab",2),
                new SqlParameter("@UserID",UserID)
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, apram);
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

            List<LessonInfo> _select = null;

            if (dt != null && dt.Rows.Count > 0)
            {
                _select = new List<LessonInfo>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    LessonInfo objbll = new LessonInfo();
                    objbll.LessonID = Convert.ToInt32(dt.Rows[i]["LessonID"]);
                    objbll.OwnerId = Convert.ToInt32(dt.Rows[i]["LessonOwnerID"]);
                    objbll.EncryptLessonID = EncyptionDcryption.GetEncryptedText(objbll.LessonID.ToString());
                    objbll.EncLessonOwnerID = EncyptionDcryption.GetEncryptedText(objbll.OwnerId.ToString());
                    objbll.LessonPlanID = Convert.ToString(dt.Rows[i]["LessonPlanID"]);
                    objbll.StartTime = Convert.ToString(dt.Rows[i]["CreationDate"]);
                    objbll.LessonNumber = Convert.ToInt32(dt.Rows[i]["LessonNumber"]);
                    objbll.LessonTitle = Convert.ToString(dt.Rows[i]["LessonTitle"]);
                    objbll.LessonPlanStatus = Convert.ToInt32(dt.Rows[i]["LessonPlanStatus"]);
                    objbll.ClassName = Convert.ToString(dt.Rows[i]["ClassName"]);
                    objbll.SubjectName = dt.Rows[i]["SubjectName"] != null ? Convert.ToString(dt.Rows[i]["SubjectName"]) : "";
                    objbll.CompleteDate = Convert.ToString(dt.Rows[i]["CompleteDate"]);
                    objbll.VerificationDate = Convert.ToString(dt.Rows[i]["VerificationDate"]);
                    objbll.VerifiedBy = Convert.ToString(dt.Rows[i]["VerifiedBy"]);
                    objbll.Rating = Convert.ToInt32(dt.Rows[i]["Rating"]);
                    objbll.CreatedBy = Convert.ToString(dt.Rows[i]["CreatedBy"]);
                    _select.Add(objbll);
                }

            }

            ListLessonInfo info = new ListLessonInfo();
            info.lstLessonInfo = _select;
            return info;
        }

        public int ModifyApprovedLesson(int LessonPlanID, int LessonID, int UserID) {
            int message = 0;
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_Modify_Approved_Lessons";
            SqlParameter[] apram = new SqlParameter[] {
                new SqlParameter("@LessonPlanID",LessonPlanID),
                new SqlParameter("@LessonID",LessonID),
                new SqlParameter("@UserID",UserID)
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                SqlHelper.ExecuteNonQuery(Connection, Command.CommandType, Command.CommandText, apram);
                message = 1;
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
            return message;
        }

        public ListLessonInfo GetLessonHistory(int LessonPLanID)
        {
            SqlConnection Connection = null;
            DataSet ds = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_GET_LESSON_VERIFIED_HISTORY";
            SqlParameter[] apram = new SqlParameter[] {
                new SqlParameter("@LessonPLanID",LessonPLanID),
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                ds = SqlHelper.ExecuteDataset(Connection, Command.CommandType, Command.CommandText, apram);
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



            List<LessonVerifiedHistory> _rej = null;
            List<LessonVerifiedHistory> _verify = null;

            if (ds != null && ds.Tables.Count > 0)
            {
                _rej = new List<LessonVerifiedHistory>();
                _verify = new List<LessonVerifiedHistory>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    LessonVerifiedHistory objbll = new LessonVerifiedHistory();
                    objbll.CompletionDate = Convert.ToString(ds.Tables[0].Rows[i]["CompletationDate"]);
                    objbll.Date = Convert.ToString(ds.Tables[0].Rows[i]["RejectOn"]);
                    objbll.Reason = Convert.ToString(ds.Tables[0].Rows[i]["Reason"]);
                    objbll.UserName = Convert.ToString(ds.Tables[0].Rows[i]["UserName"]);
                    _rej.Add(objbll);
                }
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    LessonVerifiedHistory objbll = new LessonVerifiedHistory();
                    objbll.CompletionDate = Convert.ToString(ds.Tables[1].Rows[i]["CompletationDate"]);
                    objbll.Date = Convert.ToString(ds.Tables[1].Rows[i]["VerifiedOn"]);
                    objbll.UserName = Convert.ToString(ds.Tables[1].Rows[i]["UserName"]);
                    _verify.Add(objbll);
                }

            }
            ListLessonInfo obj = new ListLessonInfo();
            obj.RejectionHistory = _rej;
            obj.VerificationHistory = _verify;
            return obj;
        }
        #endregion LessonPlanList
    }
}
