using CDS.Logic;
using CDS.Models;
using LEAF_Logic;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace CDS.Manager
{
    public class TemplateManager
    {

        CommonLogic objlogic = new CommonLogic();
        public List<LessonTemplate> GetLessonTemplates(int LessonID,int UserID)
        {
            List<LessonTemplate> lst = new List<LessonTemplate>();
            SqlConnection Connection = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_CDS_LESSON_Templates";

            SqlParameter[] aParam = new SqlParameter[] {
                 new SqlParameter("@LessonID", LessonID),
            new SqlParameter("@USERID", UserID),
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, aParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    lst = new List<LessonTemplate>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        LessonTemplate obj = new LessonTemplate();
                        obj.Name = dt.Rows[i]["Name"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["Name"]);
                        obj.Title = dt.Rows[i]["LessonTitle"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["LessonTitle"]);
                        obj.TemplateID = dt.Rows[i]["VerifiedID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["VerifiedID"]);
                        obj.Rating = dt.Rows[i]["Rating"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Rating"]);
                        obj.Email = dt.Rows[i]["Email"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["Email"]);
                        obj.Phone = dt.Rows[i]["Phone"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["Phone"]);
                        obj.TotalLessons = dt.Rows[i]["TotalLessons"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["TotalLessons"]);
                        obj.VerifiedLessons = dt.Rows[i]["VerifiedLessons"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["VerifiedLessons"]);
                        obj.TotalVotes = dt.Rows[i]["Votes"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Votes"]);
                        obj.TempLessonPlanID = dt.Rows[i]["LessonPlanID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LessonPlanID"]);
                        lst.Add(obj);
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
            return lst;
        }
        public DataSet GetTemplateDetails(int TemplateID,int LessonID)
        {
            SqlConnection Connection = null;
            DataSet ds = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_CDS_GET_TEMPLATE_DETAILS";

            SqlParameter[] aParam = new SqlParameter[] {
                 new SqlParameter("@TemplateID", TemplateID),
                 new SqlParameter("@LessonID", LessonID)
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
        public int SaveTemplate(int TemplateID, int LessonID,int UserID)
        {
            SqlConnection Connection = null;
            DataTable dt = null;
            int res = 0;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_CDS_SAVE_TEMPLATE";

            SqlParameter[] aParam = new SqlParameter[] {
                 new SqlParameter("@TempID", TemplateID),
                 new SqlParameter("@UserId", UserID),
                  new SqlParameter("@LessonID", LessonID)
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                dt =SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, aParam);
                res = 1;
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (TemplateID==-1)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int OldImageID = Convert.ToInt32(dt.Rows[i]["FileID"]);
                            int BookID = Convert.ToInt32(dt.Rows[i]["BookIDFK"]);
                            int extid = Convert.ToInt32(dt.Rows[i]["ExtensionTypeIDFK"]);
                            int imgfor = Convert.ToInt32(dt.Rows[i]["ImageForIDFK"]);
                            string imgNickName = Convert.ToString(dt.Rows[i]["NickName"]);
                            string oldextension = Convert.ToString(dt.Rows[i]["ExtensionName"]);
                            int ChapterId = new Mngr_ScopeSequence().BookDetails(BookID).Where(x => x.LessonID == LessonID).FirstOrDefault().ChapterID;
                            int NewID = new ImagesManager().ImagesInsert(BookID, LessonID, extid, ChapterId, imgNickName, imgfor, 1, 0);

                            var oldmainpath = System.Configuration.ConfigurationSettings.AppSettings["OLDMediaPath"];
                            var mainpath = System.Configuration.ConfigurationSettings.AppSettings["MediaPath"];
                            string oldpath = oldmainpath + "B_" + BookID + "\\L_" + LessonID + "\\" + OldImageID + "." + oldextension;
                            string newpath = mainpath + "B_" + BookID + "\\L_" + LessonID + "\\" + NewID + "." + oldextension;
                            string newdirc = mainpath + "B_" + BookID + "\\L_" + LessonID;
                            DirectoryInfo dir = new DirectoryInfo(newdirc);
                            if (dir.Exists == false)
                            {
                                Directory.CreateDirectory(newdirc);
                            }
                            FileInfo file = new FileInfo(oldpath);
                            if (file.Exists == true)
                            {
                                System.IO.File.Copy(oldpath, newpath, true);
                                byte[] oldimgEncrypt = UTF8Encoding.UTF8.GetBytes("B_" + BookID + "\\L_" + LessonID + "\\" + OldImageID + "." + oldextension);
                                byte[] newimgEncrypt = UTF8Encoding.UTF8.GetBytes("B_" + BookID + "\\L_" + LessonID + "\\" + NewID + "." + oldextension);
                                string oldimgpath = Convert.ToBase64String(oldimgEncrypt);
                                string newimgpath = Convert.ToBase64String(newimgEncrypt);
                                string abc = ReplaceImagePath(oldimgpath, newimgpath, LessonID,SessionManager.Current.UserID);
                                if (abc!="Done")
                                {
                                    res = 0;
                                    return res;
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
            return res;
        }
        public List<TemplateRating> GetTemplateRating(int TemplateID)
        {
            List<TemplateRating> lst = new List<TemplateRating>();
            SqlConnection Connection = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_GET_TEMPLATE_RATING_INFO";

            SqlParameter[] aParam = new SqlParameter[] {
                 new SqlParameter("@TemplateID", TemplateID)
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, aParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    lst = new List<TemplateRating>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TemplateRating obj = new TemplateRating();
                        obj.Rating = dt.Rows[i]["Rating"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Rating"]);
                        obj.TotalVotes = dt.Rows[i]["Votes"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Votes"]);
                        obj.FiveVotes = dt.Rows[i]["FiveVotes"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["FiveVotes"]);
                        obj.FourVotes = dt.Rows[i]["FourVotes"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["FourVotes"]);
                        obj.ThreeVotes = dt.Rows[i]["ThreeVotes"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["ThreeVotes"]);
                        obj.TwoVotes = dt.Rows[i]["TwoVotes"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["TwoVotes"]);
                        obj.OneVotes = dt.Rows[i]["OneVotes"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["OneVotes"]);
                        obj.PersonName = dt.Rows[i]["PersonName"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["PersonName"]);
                        obj.PersonRating = dt.Rows[i]["PersonRating"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["PersonRating"]);
                        obj.RatedBy = dt.Rows[i]["Ratedby"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Ratedby"]);
                        obj.Date = dt.Rows[i]["RatedDate"] == DBNull.Value ? null : Convert.ToString(dt.Rows[i]["RatedDate"]);
                        lst.Add(obj);
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
            return lst;
        }
        public string ReplaceImagePath(string oldpath, string newpath, int DesLessonid,int UserID)
        {
            SqlConnection Connection = null;
            string result = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_CDS_REPLACE_IMAGE_PATH";
            SqlParameter[] prams = new SqlParameter[] {

                    new SqlParameter("@OldPath" , oldpath),
                     new SqlParameter("@NewPath" , newpath),
                      new SqlParameter("@DesLesoonID" , DesLessonid),
                      new SqlParameter("@UserID" , UserID),
                                                        };
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, prams);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = Convert.ToString(dt.Rows[0]["msg"]);
                }
            }
            catch (Exception ex)
            {
                new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);

            }

            return result;
        }
    }
}