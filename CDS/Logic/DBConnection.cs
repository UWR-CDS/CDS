using System.Data.SqlClient;

namespace CDS.Logic
{
    public static class DBConnection
    {
        private static string sCDSConnString = System.Configuration.ConfigurationSettings.AppSettings["CDS"];
        private static string sLogDBConnString = System.Configuration.ConfigurationSettings.AppSettings["LOG"];

        public static SqlConnection GetDBConn()
        {

            SqlConnection dbconnection = new SqlConnection(sCDSConnString);
            try
            {
                dbconnection.Open();
                return dbconnection;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new ExceptionTranslater(ex);
            }
        }

        public static SqlConnection GetLogDBConn()
        {

            SqlConnection dbconnection = new SqlConnection(sLogDBConnString);
            try
            {
                dbconnection.Open();
                return dbconnection;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new ExceptionTranslater(ex);
            }
        }

    }
}