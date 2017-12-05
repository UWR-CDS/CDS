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

namespace CDS.Models
{
    public class FAQHandler
    {
        public string strErrMsg = "";
        CommonLogic objlogic = new CommonLogic();
        SqlCommand Command = new SqlCommand();
        public List<mdl_FAQ> GetQuestionAnswers()
        {
            SqlConnection Connection = null;
            List<mdl_FAQ> _select = null;
            DataTable dt = null;

            Command.CommandType = CommandType.StoredProcedure;

            Command.CommandText = "USP_CDS_GETFAQs";
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
                _select = new List<mdl_FAQ>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    mdl_FAQ _faq = new mdl_FAQ();
                    _faq.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    _faq.Questions = Convert.ToString(dt.Rows[i]["Questions"]);
                    _faq.Answers = Convert.ToString(dt.Rows[i]["Answers"]);
                    _faq.QuestionType = Convert.ToInt32(dt.Rows[i]["QuestionType"]);
                    _select.Add(_faq);

                }

            }

            return _select;
        }
    }
}