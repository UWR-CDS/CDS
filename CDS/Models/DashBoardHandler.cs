using CDS.Logic;
using CDS.Models;
using LEAF_Logic;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
namespace CDS.Models
{
    public class DashBoardHandler
    {
        CommonLogic objlogic = new CommonLogic();
        public List<DashBoard> ScriptStatisticsData(int UserID) {

            SqlConnection Connection = null;
            List<DashBoard> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_DASHBOARD_TABS";
            SqlParameter[] aparam = new SqlParameter[] {
                new SqlParameter("@UserID",UserID)
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                _select = new List<DashBoard>();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText,aparam);
                if (dt.Rows.Count != 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DashBoard obj = new DashBoard();
                        obj.InProgressLesson = dt.Rows[i]["Inprocess"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Inprocess"]);
                        obj.TotalLesson = dt.Rows[i]["Total"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Total"]);
                        obj.CompletedLesson = dt.Rows[i]["Completed"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Completed"]);
                        obj.VerifiedLesson = dt.Rows[i]["Verified"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Verified"]);
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

        public List<DashBoardChart> Charts(int UserID,int TypeID) {

            SqlConnection Connection = null;
            List<DashBoardChart> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_DASHBOARD_GRAPH";
            SqlParameter[] aparam = new SqlParameter[] {
                new SqlParameter("@UserID",UserID),
                new SqlParameter("@TypeID",TypeID)
            }; 
            try
            {
                Connection = DBConnection.GetDBConn();
                _select = new List<DashBoardChart>();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText,aparam);
                if (dt.Rows.Count != 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DashBoardChart obj = new DashBoardChart();
                        obj.NewLesson = dt.Rows[i]["New"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["New"]);
                        obj.VerifyLesson = dt.Rows[i]["Verified"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Verified"]);
                        obj.CompleteLesson = dt.Rows[i]["Completed"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Completed"]);
                        obj.Date = dt.Rows[i]["Date"] == DBNull.Value ? "" : Convert.ToString(dt.Rows[i]["Date"]);
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

        public List<DashBoard> LessonTabData(int UserID)
        {
            SqlConnection Connection = null;
            List<DashBoard> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_CDS_DASHBOARD_TABS";
            SqlParameter[] aparam = new SqlParameter[] {
                new SqlParameter("@UserID",UserID)
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                _select = new List<DashBoard>();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText,aparam);
                if (dt.Rows.Count != 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DashBoard obj = new DashBoard();
                        obj.TotalLesson = dt.Rows[i]["Total"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Total"]);
                        obj.CompletedLesson = dt.Rows[i]["Completed"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Completed"]);
                        obj.InProgressLesson = dt.Rows[i]["Inprocess"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Inprocess"]);
                        obj.VerifiedLesson = dt.Rows[i]["Verified"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Verified"]);
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
    }
}