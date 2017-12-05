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
    public class ImagesManager
    {

        public List<ImagesModel> GetImages(int bkid, int pageNumber, string searchKeyWord,int userId = 0, string imageType = "Lesson", int lid = 0)
        {
            SqlConnection Connection = null;
            List<ImagesModel> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_GETUWRIMAGESBYSCROLL";
            //Command.CommandText = "USP_SCH_GET_IMAGES";

            SqlParameter[] aParam = new SqlParameter[] { new SqlParameter("@BookID", bkid),
            new SqlParameter("@PageNumber", pageNumber),
            new SqlParameter("@SearchKeyWord", searchKeyWord),
            new SqlParameter("@userID", userId),
            new SqlParameter("@imageType", imageType),
            new SqlParameter("@LessonID", lid)
            };


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
                _select = new List<ImagesModel>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ImagesModel objbll = new ImagesModel();
                    objbll.ImageFolderPath = Convert.ToString(dt.Rows[i]["ImageFolderPath"]);
                    objbll.NickName = Convert.ToString(dt.Rows[i]["NickName"]);
                    objbll.FileID = Convert.ToInt32(dt.Rows[i]["FileID"]);
                    _select.Add(objbll);
                }

            }

            return _select;
        }

        public List<ImagesModel> GetAudios(int bkid, int pageNumber, string searchKeyWord, int userId = 0, string imageType = "Lesson", int lid = 0)
        {
            SqlConnection Connection = null;
            List<ImagesModel> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_GETUWRAUDIOSBYSCROLL";
            //Command.CommandText = "USP_SCH_GET_IMAGES";

            SqlParameter[] aParam = new SqlParameter[] { new SqlParameter("@BookID", bkid),
            new SqlParameter("@PageNumber", pageNumber),
            new SqlParameter("@SearchKeyWord", searchKeyWord),
            new SqlParameter("@userID", userId),
            new SqlParameter("@imageType", imageType),
            new SqlParameter("@LessonID", lid)
            };


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
                _select = new List<ImagesModel>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ImagesModel objbll = new ImagesModel();
                    objbll.ImageFolderPath = Convert.ToString(dt.Rows[i]["ImageFolderPath"]);
                    objbll.NickName = Convert.ToString(dt.Rows[i]["NickName"]);
                    objbll.FileID = Convert.ToInt32(dt.Rows[i]["FileID"]);
                    _select.Add(objbll);
                }

            }

            return _select;
        }

        public int ImagesInsert(int bookidfk, int lessonidfk, int extensiontypeidfk, int chapteridfk, string NickName,int UserId, int ImageFor = 1, int BookTypeId = 1, int isindexpage = 0)
        {
            object i = null;
            SqlConnection Connection = null;
            // System.Diagnostics.StackTrace CurrentStack = new System.Diagnostics.StackTrace();
            // Warning!!! Optional parameters not supported
            SqlCommand Command = new SqlCommand();
            try
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "USP_CDS_IMAGES_INSERT";
                Connection = DBConnection.GetDBConn();
                SqlParameter[] aParam = new SqlParameter[] { new SqlParameter("@BookIDFK" , bookidfk) ,
                new SqlParameter("@LessonIDFK" , lessonidfk) ,
                new SqlParameter("@ChapterIDFK" , chapteridfk) ,
                new SqlParameter("@ExtensionTypeIDFK" ,extensiontypeidfk),
                new SqlParameter("@NickName" ,NickName),
                new SqlParameter("@ImageFor" ,ImageFor),
                new SqlParameter("@BookTypeID" ,BookTypeId),
                 new SqlParameter("@IsindexPage" ,isindexpage),
                 new SqlParameter("@UserId" ,UserId)
               };


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
        public string SaveAudioFiles(string LessonPlanID, int userID, string FileName)
        {
            object i = null;
            SqlConnection Connection = null;
            // System.Diagnostics.StackTrace CurrentStack = new System.Diagnostics.StackTrace();
            // Warning!!! Optional parameters not supported
            SqlCommand Command = new SqlCommand();
            try
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "USP_ADD_SCRIPT_AUDIO";
                Connection = DBConnection.GetDBConn();
                SqlParameter[] aParam = new SqlParameter[] { new SqlParameter("@LessonPlanID" , LessonPlanID) ,
                new SqlParameter("@UserID" , userID),
                new SqlParameter("@FileName",FileName)
               };


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
            return Convert.ToString(i);
        }

        public bool SaveImageName(int imageID, string NickName)
        {
            bool isSaved = false;
            SqlConnection Connection = null;
            // System.Diagnostics.StackTrace CurrentStack = new System.Diagnostics.StackTrace();
            // Warning!!! Optional parameters not supported
            SqlCommand Command = new SqlCommand();
            try
            {
                SqlParameter[] aParam = new SqlParameter[]
           {
                    new SqlParameter("@FileID" , imageID) ,

                new SqlParameter("@NickName" ,NickName)
          };


                Command.CommandType = CommandType.Text;
                Command.CommandText = "UPDATE CDS_Les_MediaFiles SET NickName = @NickName where FileID = @FileID";
                Connection = DBConnection.GetDBConn();
                SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText, aParam);

                isSaved = true;
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
            return isSaved;
        }

        public ImagesModel ImagesInsertQuestionnire(int bookidfk, int lessonidfk, int extensiontypeidfk, int chapteridfk, string NickName, int ImageFor)
        {
            object i = null;
            DataTable dt = null;
            SqlConnection Connection = null;
            // System.Diagnostics.StackTrace CurrentStack = new System.Diagnostics.StackTrace();
            // Warning!!! Optional parameters not supported
            SqlCommand Command = new SqlCommand();
            ImagesModel img = new ImagesModel();
            try
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "USP_LES_IMAGES_INSERT_QUESTIONNAIRE";
                Connection = DBConnection.GetDBConn();
                SqlParameter[] aParam = new SqlParameter[] { new SqlParameter("@BookIDFK" , bookidfk) ,
                new SqlParameter("@LessonIDFK" , lessonidfk) ,
                new SqlParameter("@ChapterIDFK" , chapteridfk) ,
                new SqlParameter("@ExtensionTypeIDFK" ,extensiontypeidfk),
                new SqlParameter("@NickName" ,NickName),
                new SqlParameter("@ImageFor" ,ImageFor)
               };


                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, aParam);

                if (dt != null && dt.Rows.Count > 0)
                {

                    img.imageID = Convert.ToInt32(dt.Rows[0]["ImageId"]);
                    string s = Convert.ToString((dt.Rows[0]["ImageSrc"]));
                    byte[] imgEncrypt = UTF8Encoding.UTF8.GetBytes(s);
                    img.ImageSrc = Convert.ToBase64String(imgEncrypt);
                    img.NickName = Convert.ToString((dt.Rows[0]["NickName"]));
                }
                else
                {
                    img.imageID = 0;
                }
                return img;

            }
            catch (Exception ex)
            {
                new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), Command.CommandText);
                return img;
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

        }
        public List<ImagesModel> ImagesSelect(int id, int groupid, bool isAll = false, int ImageFor = 1)
        {
            SqlConnection Connection = null;
            List<ImagesModel> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_LES_IMAGES_SELECT";

            SqlParameter[] aParam;
            if (isAll)
            {
                aParam = new SqlParameter[] { new SqlParameter("@LessonIDFK", id), new SqlParameter("@MediaGroupType", groupid), new SqlParameter("@ImageForIDFK", 2) };
            }
            else
            {
                aParam = new SqlParameter[] { new SqlParameter("@LessonIDFK", id), new SqlParameter("@MediaGroupType", groupid), new SqlParameter("@ImageForIDFK", ImageFor) };
            }


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
                _select = new List<ImagesModel>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    ImagesModel objbll = new ImagesModel();
                    objbll.FileID = Convert.ToInt32(dt.Rows[i]["FileID"]);
                    objbll.BookIDFK = Convert.ToInt32(dt.Rows[i]["BookIDFK"]);
                    objbll.LessonIDFK = Convert.ToInt32(dt.Rows[i]["LessonIDFK"]);
                    //objbll.ChapterIDFK = Convert.ToInt32(dt.Rows[i]["ChapterIDFK"]);
                    objbll.ImageExt = Convert.ToString(dt.Rows[i]["ImageExt"]);
                    objbll.ImageName = Convert.ToString(dt.Rows[i]["ImageName"]);
                    objbll.NickName = Convert.ToString(dt.Rows[i]["NickName"]);

                    byte[] imgEncrypt = UTF8Encoding.UTF8.GetBytes(objbll.ImageName);
                    objbll.EncName = Convert.ToBase64String(imgEncrypt);

                    //objbll.ImageName = UTF8Encoding.UTF8.GetBytes(encimgname).ToString();

                    //objbll.BookTypeName = Convert.ToString(dt.Rows[i]["BookTypeName"]);
                    _select.Add(objbll);

                }

            }

            return _select;
        }
        public ImagesModel LastImgName()
        {
            SqlConnection Connection = null;
            List<ImagesModel> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "SELECT LAST(ImageID) FROM Les_Images";


            try
            {
                Connection = DBConnection.GetDBConn();

                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText);
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
                _select = new List<ImagesModel>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ImagesModel objbll = new ImagesModel();
                    objbll.imageID = Convert.ToInt32(dt.Rows[i]["ImageID"]);
                    _select.Add(objbll);

                }

            }
            return (_select == null) ? null : _select[0];
            //return _select;
        }
        public void ImagesUpdate(int FileID, int bookidfk, int lessonidfk, int extensiontypeidfk, int chapteridfk, string NickName)
        {
            //object i = null;
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            try
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "USP_WEB_LES_IMAGES_UPDATE";
                Connection = DBConnection.GetDBConn();
                SqlParameter[] aParam = new SqlParameter[]
                {
                    new SqlParameter("@FileID" , FileID) ,
                new SqlParameter("@BookIDFK" , bookidfk) ,
                new SqlParameter("@LessonIDFK" , lessonidfk) ,
                new SqlParameter("@ChapterIDFK" , chapteridfk) ,
                new SqlParameter("@ExtensionTypeIDFK" ,extensiontypeidfk),
                new SqlParameter("@NickName" ,NickName)
               };


                SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText, aParam);
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
            // return Convert.ToInt32(i);
        }

        public void SoundVideoUpdateNickName(int FileID, string NickName)
        {

            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            try
            {

                SqlParameter[] aParam = new SqlParameter[]
               {
                    new SqlParameter("@FileID" , FileID) ,

                new SqlParameter("@NickName" ,NickName)
              };


                Command.CommandType = CommandType.Text;
                Command.CommandText = "UPDATE Les_MediaFiles SET NickName = @NickName where FileID = @FileID";
                Connection = DBConnection.GetDBConn();
                // SqlParameter[] aParam = new SqlParameter[]
                // {
                //     new SqlParameter("@FileID" , FileID) ,

                // new SqlParameter("@NickName" ,NickName)
                //};


                SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText, aParam);
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

        public bool CanDeleteImage(int LessonId = 0, string EncID = null)
        {
            bool result = true;
            DataTable dt = null;
            SqlConnection Connection = null;
            // System.Diagnostics.StackTrace CurrentStack = new System.Diagnostics.StackTrace();
            // Warning!!! Optional parameters not supported
            SqlCommand Command = new SqlCommand();
            try
            {
                Command.CommandType = CommandType.Text;
                Command.CommandText = "select * from CDS_LessonPlanDetail where LessonPlanIDFK = (Select lpm.LessonPlanID from cds_LessonPlanMaster lpm where LessonIDFK = "+LessonId+") and LessonPLanScript like '%"+EncID+"%'";
                Connection = DBConnection.GetDBConn();

                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText);
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
            if(dt != null && dt.Rows.Count > 0)
            {
                result = false;
            }
            return result;
        }

        public int DeleteImages(int id)
        {
            int i = 0;
            SqlConnection Connection = null;
            // System.Diagnostics.StackTrace CurrentStack = new System.Diagnostics.StackTrace();
            // Warning!!! Optional parameters not supported
            SqlCommand Command = new SqlCommand();
            try
            {
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "USP_CDS_DELETE_IMAGES";
                Connection = DBConnection.GetDBConn();
                SqlParameter[] rParam = new SqlParameter[] {
            new SqlParameter("@FileID" , id)
                };
                SqlHelper.ExecuteScalar(Connection, Command.CommandType, Command.CommandText, rParam);
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
          
            return i;
        }

        public List<ImagesModel> MediaExtionSize(int MediaTypeID)
        {
            SqlConnection Connection = null;
            List<ImagesModel> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "SELECT ExtensionName , MaxSizeinMB from LEAF_Main.dbo.Con_ExtensionTypes where MediaGroupType = @MediaGroupType";

            SqlParameter[] aParam = new SqlParameter[]
          {
               new SqlParameter("@MediaGroupType" , MediaTypeID) ,
          };

            try
            {
                Connection = DBConnection.GetDBConn();

                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, aParam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    _select = new List<ImagesModel>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ImagesModel objbll = new ImagesModel();
                        objbll.ExtensionName = Convert.ToString(dt.Rows[i]["ExtensionName"]);
                        objbll.MaxSizeinMB = Convert.ToInt32(dt.Rows[i]["MaxSizeinMB"]);

                        _select.Add(objbll);

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


            return _select;
        }
    }
}