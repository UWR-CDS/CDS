using CDS.Logic;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CDS.Manager
{
    public class ActivityLog
    {
        public string strErrMsg = "";
        /// <summary>
        /// Muhammad Azam   2016-06-16 09:26
        /// </summary>
        public void GenActivitylog(Int64 LoginTrackingID, int UserID, Int16 UserType, string Activity, string IP_Address)
        {
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_ACTIVITY_LOG";

            SqlParameter[] aParam = new SqlParameter[] {
                 new SqlParameter("@LoginTrackingID", LoginTrackingID),
                new SqlParameter("@UserID", UserID),
                new SqlParameter("@UserType", UserType),
                new SqlParameter("@Activity", Activity),
                new SqlParameter("@IP_Address", IP_Address)
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                SqlHelper.ExecuteNonQuery(Connection, Command.CommandType, Command.CommandText, aParam);

            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
        }

        //Mujahid 24-08-2016

        public DataTable CheckLoginTracking(int UserID)
        {
            string ip = HttpContext.Current.Request.UserHostAddress;
            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }
            int f = 0;
            DataTable dt = null;
            SqlConnection Connection = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "[USP_Login_Tracking_History]";

            SqlParameter[] aParam = new SqlParameter[]
            {
                new SqlParameter("@UserID", UserID),
                new SqlParameter("@IP", ip),
                 new SqlParameter("@ForceFullyLogout", f)
            };
            try
            {
                Connection = DBConnection.GetDBConn();
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, aParam);
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            return dt;
        }
        //Mujahid 24-08-2016
        public void UpdateLoginTracking(int ID)
        {
            SqlConnection Connection = null;
            Connection = DBConnection.GetDBConn();
            try
            {
                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "[USP_UPDATE_LOGIN_TRACKING]";
                Command.Connection = Connection;
                SqlParameter[] aParam = new SqlParameter[]
                {
                new SqlParameter("@LoginTrackID", ID),

                };
                SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, aParam);
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

        }
    }
}