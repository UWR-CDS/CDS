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
    public class Mngr_Message
    {
        CommonLogic objlogic = new CommonLogic();

        public string GetMessage()
        {
            string str = "";
            SqlConnection Connection = null;
            List<Message_Entites> _select = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_Message_Get";
            try
            {
                Connection = DBConnection.GetDBConn();

                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText);

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Message_Entites _Message = new Message_Entites();
                        str += Convert.ToString(dt.Rows[i]["Message"]) + "  " + "  " + "  " + "  " + "  " + "  ";
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
            return str;
        }
    }
}