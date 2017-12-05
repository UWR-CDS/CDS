using CDS.Logic;
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
    public class PdfManager
    {
        public DataSet LessonToPdfData(int LessonPlanId)
        {
            SqlConnection Connection = null;
            DataSet ds = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "USP_WEB_CDS_GET_LESSON_DATA_FOR_PDF";

            SqlParameter[] aParam = new SqlParameter[] {
                 new SqlParameter("@LessonPlanID", LessonPlanId)
            };
            try
            {
                Connection = DBConnection.GetDBConn();

                ds = SqlHelper.ExecuteDataset(Connection, Command.CommandType, Command.CommandText, aParam);
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
            return ds;
        }

    }
}