using CDS.Logic;
using CDS.Models;
using LEAF_Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CDS.Manager
{
    public class Mngr_Con_Class
    {
        public List<Models.Con_Class> GradeDropDown(bool all = false)
        {
            SqlConnection Connection = null;
            List<Models.Con_Class> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CON_CLASS_SELECT_DROPDOWN";
            try
            {
                Connection = DBConnection.GetDBConn();

                dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText);
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
                _select = new List<Models.Con_Class>();

                if (all)
                {
                    Models.Con_Class objbll = new Models.Con_Class();
                    objbll.ClassID = 0;
                    objbll.ClassName = "All";

                    _select.Add(objbll);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Models.Con_Class objbll = new Models.Con_Class();
                    objbll.ClassID = Convert.ToInt32(dt.Rows[i]["ClassID"]);
                    objbll.ClassName = Convert.ToString(dt.Rows[i]["ClassName"]);

                    _select.Add(objbll);
                }

            }

            return _select;
        }

        public List<Models.Les_Subject> GradeSubjectDropDown(String ClassIDFK)
        {
            SqlConnection Connection = null;
            List<Models.Les_Subject> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_Subject_SELECT_DROPDOWN";
            SqlParameter[] prmz = {new SqlParameter ("@ClassIDFK", ClassIDFK),};
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, prmz);
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
                _select = new List<Models.Les_Subject>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Models.Les_Subject objbll = new Models.Les_Subject();
                    objbll.SubjectID = Convert.ToInt32(dt.Rows[i]["SubjectID"]);
                    objbll.SubjectName = Convert.ToString(dt.Rows[i]["SubjectName"]);

                    _select.Add(objbll);
                }

            }
            return _select;
        }

        public List<Models.Les_Subject> GetAllSubjects()
        {
            SqlConnection Connection = null;
            List<Models.Les_Subject> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "select ClassIDFK,(select ClassName from LEAF_Main.dbo.Con_Class where ClassID = ClassIDFK) ClassName,SubjectIDFK,";
            Command.CommandText += " (select SubjectName from LEAF_Main.dbo.Les_Subject where SubjectID = SubjectIDFK) SubjectName";
            Command.CommandText += " from LEAF_Main.dbo.Con_ClassSubject where isnull(ReadyForCDS,0)=1 order by SubjectIDFK,ClassIDFK";
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText);
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
                _select = new List<Models.Les_Subject>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Models.Les_Subject objbll = new Models.Les_Subject();
                    objbll.ClassID = Convert.ToInt32(dt.Rows[i]["ClassIDFK"]);
                    objbll.ClassName = Convert.ToString(dt.Rows[i]["ClassName"]);
                    objbll.SubjectID = Convert.ToInt32(dt.Rows[i]["SubjectIDFK"]);
                    objbll.SubjectName = Convert.ToString(dt.Rows[i]["SubjectName"]);

                    _select.Add(objbll);
                }

            }
            return _select;
        }

        public List<Models.Les_Subject> GetAllUserSubjects(int UserId)
        {
            SqlConnection Connection = null;
            List<Models.Les_Subject> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.Text;
            Command.CommandText = "select ClassID,(select ClassName from LEAF_Main.dbo.Con_Class where ClassID = C.ClassID) ClassName";
            Command.CommandText += ",SubjectID,(select SubjectName from LEAF_Main.dbo.Les_Subject where SubjectID = C.SubjectID) SubjectName";
            Command.CommandText += " from dbo.CDS_UserClassSubject C where UserID=" + UserId+ " order by SubjectID,ClassID";
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText);
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
                _select = new List<Models.Les_Subject>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Models.Les_Subject objbll = new Models.Les_Subject();
                    objbll.ClassID = Convert.ToInt32(dt.Rows[i]["ClassID"]);
                    objbll.ClassName = Convert.ToString(dt.Rows[i]["ClassName"]);
                    objbll.SubjectID = Convert.ToInt32(dt.Rows[i]["SubjectID"]);
                    objbll.SubjectName = Convert.ToString(dt.Rows[i]["SubjectName"]);
                    _select.Add(objbll);
                }

            }
            return _select;
        }
    }
}