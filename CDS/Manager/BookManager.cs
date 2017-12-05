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
    public class BookManager
    {
        public List<BookType> GetBookTypeByBook(int BookId)
        {
            List<BookType> _books = null;
            SqlConnection Connection = null;
            DataTable dt = null;
            SqlCommand Command = new SqlCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = "LEAF_Main.dbo.USP_LES_GET_BOOKTYPE_BY_BOOK";
            try
            {
                Connection = DBConnection.GetDBConn();
                SqlParameter[] prams = new SqlParameter[] {
                    new SqlParameter("@BookIDFK" , BookId)
                                                        };
                dt = SqlHelper.ExecuteDataTable(Connection, Command.CommandType, Command.CommandText, prams);
                if (dt != null && dt.Rows.Count > 0)
                {
                    _books = new List<BookType>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        BookType obj = new BookType();
                        obj.BookTypeId= EncyptionDcryption.GetEncryptedText(Convert.ToInt32(dt.Rows[i]["BookTypeID"]).ToString());
                        obj.BookTypeName = Convert.ToString(dt.Rows[i]["BookTypeName"]);
                        _books.Add(obj);
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
            return _books;
        }
    }
}