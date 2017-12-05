using CDS.Logic;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace LEAF_Logic
{
    public static class CommonMng
    {
        public static string baseUrl = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
        public static List<getimegbytes> getimegbytes = null;
        public static int ServerTimeDiff = 0;
        public static DateTime CurrentTime()
        {
            // return DateTime.Now;
            //get
            //{
            return DateTime.Now.AddSeconds(ServerTimeDiff);
            //}
        }
        public static object setDateDBNull(DateTime Value)
        {
            return Value.ToString() == "1/1/0001 12:00:00 AM" ? DBNull.Value : (object)Value;
        }
        public static int CalcDurationInSeconds(DateTime StartDate, DateTime EndDate)
        {
            if (StartDate.ToString() == "1/1/0001 12:00:00 AM" || EndDate.ToString() == "1/1/0001 12:00:00 AM")
                return 0;
            TimeSpan Seconds = EndDate - StartDate;
            return Convert.ToInt32(Seconds.TotalSeconds);
        }
        public static int CalcDurationInSeconds(DateTime StartDate)
        {
            if (StartDate.ToString() == "1/1/0001 12:00:00 AM")
                return 0;
            TimeSpan Seconds = CommonMng.CurrentTime() - StartDate;
            return Convert.ToInt32(Seconds.TotalSeconds);
        }
        public static int CalcDurationInMilliSeconds(DateTime StartDate)
        {
            if (StartDate.ToString() == "1/1/0001 12:00:00 AM")
                return 0;
            TimeSpan MilliSeconds = CommonMng.CurrentTime() - StartDate;
            return Convert.ToInt32(MilliSeconds.TotalMilliseconds);
        }
        public static int CalcDurationInMilliSeconds(DateTime StartDate, DateTime EndDate)
        {
            if (StartDate.ToString() == "1/1/0001 12:00:00 AM" || EndDate.ToString() == "1/1/0001 12:00:00 AM")
                return 0;
            TimeSpan MilliSeconds = EndDate - StartDate;
            return Convert.ToInt32(MilliSeconds.TotalMilliseconds);
        }
        public static string getDuration(DateTime StartDate, DateTime EndDate)
        {
            if (StartDate.ToString() == "1/1/0001 12:00:00 AM" || EndDate.ToString() == "1/1/0001 12:00:00 AM")
                return "00:00:00";
            TimeSpan Seconds = EndDate - StartDate;
            return getDuration(Convert.ToInt32(Seconds.TotalSeconds));
        }
        public static string getDuration(DateTime StartDate)
        {
            if (StartDate.ToString() == "1/1/0001 12:00:00 AM")
                return "00:00:00";
            return getDuration(Convert.ToInt32((CommonMng.CurrentTime() - StartDate).TotalSeconds));
        }
        public static string getDuration(int Seconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(Seconds);
            return string.Format("{0:D2}:{1:D2}:{2:D2}",
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds
                                    );
            // return Duration;
        }
        private static DateTime InitaialDate = Convert.ToDateTime("2016-08-14");
        public static double getrandomnumber()
        {
            TimeSpan span = DateTime.Now - InitaialDate;
            return  span.TotalMilliseconds;
        }
        public static string getSystemIP()
        {
            string sIP = "";
            System.Net.IPAddress[] a = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList;
            for (int i = 0; i < a.Length; i++)
                sIP += a[i].ToString() + ",";

            // string myHost = System.Net.Dns.GetHostName();
            // sIP = System.Net.Dns.GetHostByName(myHost).AddressList[0].ToString();

            return sIP;
        }
        public static string getFilePath(string sPath)
        {
            try
            {
                string[] sArray = sPath.Split(':');
                return "FILE : " + sArray[1].Substring(sArray[1].LastIndexOf("\\") + 1) + ":" + sArray[sArray.Length - 1];
            }
            catch
            {
                return string.Empty;
            }
        }
        public static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
        public static string Count_KeyVocabulary(string Script, string Vocabulary, char SplitChar)
        {
            string result = "";
            try
            {
                int? Count = 0;
                if (!string.IsNullOrWhiteSpace(Vocabulary) && Vocabulary.Length > 0 && !string.IsNullOrWhiteSpace(Script))
                {
                    string[] strArray = Vocabulary.Split(SplitChar).Select(a => a.Trim()).Distinct().ToArray();
                    if (strArray.Length > 0)
                    {
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            string vocabulary = strArray[i].ToString().Replace(',', '|').Replace(':', '|').Replace(' ', '|').Replace('-', '|').Replace('،', '|');
                            Count = TextTool.CountStringOccurrences(Script, vocabulary);
                            if (i == strArray.Length - 1)
                            {
                                if (Count == 0)
                                {
                                    result += " " + vocabulary;
                                }
                                else
                                {
                                    result += " " + vocabulary + " <b>[" + Count + "]</b>";
                                }
                            }
                            else
                            {
                                if (Count == 0)
                                {
                                    result += " " + vocabulary + "<b> ,</b>";
                                }
                                else
                                {
                                    result += " " + vocabulary + " <b>[" + Count + "] ,</b>";
                                }
                            }
                        }
                    }
                }
                else
                    result = Vocabulary;
            }
            catch (Exception ex)
            {
                result = Vocabulary;
            }
            return result;
        }

    }
}
public class getimegbytes {
    public int id { get; set; }
    public byte[] getImegbytes { get; set; }
}
public static class TextTool
{
    public static int CountStringOccurrences(string text, string pattern)
    {
        int count = 0;
        if (pattern.Contains("?"))
        {
            pattern = pattern.Replace("?", "");
        }
        count = Regex.Matches(text, @"\b" + pattern + @"\b", RegexOptions.Singleline | RegexOptions.IgnoreCase).Count;
        return count;
    }
}