using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS
{
    public static class SessionValues
    {
        public static int UserID
        {
            get
            {
                if (null != HttpContext.Current.Session["LoginUserID"])
                    return Convert.ToInt32(HttpContext.Current.Session["LoginUserID"]);
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session["LoginUserID"] = value;
            }
        }
        public static string EncUserID
        {
            get
            {
                if (null != HttpContext.Current.Session["EncLoginUserID"])
                    return HttpContext.Current.Session["EncLoginUserID"] as string;
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["EncLoginUserID"] = value;
            }
        }
        public static string Email
        {
            get
            {
                if (null != HttpContext.Current.Session["LoginUserName"])
                    return HttpContext.Current.Session["LoginUserName"] as string;
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["LoginUserName"] = value;
            }
        }
        public static string UserName
        {
            get
            {
                if (null != HttpContext.Current.Session["UserName"])
                    return HttpContext.Current.Session["UserName"] as string;
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["UserName"] = value;
            }
        }
        public static int PrivigilesID
        {
            get
            {
                if (null != HttpContext.Current.Session["PrivigilesID"])
                    return Convert.ToInt32(HttpContext.Current.Session["PrivigilesID"]);
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session["PrivigilesID"] = value;
            }
        }
        public static int isMaster
        {
            get
            {
                if (null != HttpContext.Current.Session["isMaster"])
                    return Convert.ToInt32(HttpContext.Current.Session["isMaster"]);
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session["isMaster"] = value;
            }
        }
        public static int EntityID
        {
            get
            {
                if (null != HttpContext.Current.Session["EntityID"])
                    return Convert.ToInt32(HttpContext.Current.Session["EntityID"]);
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session["EntityID"] = value;
            }
        }
        public static string EntityName
        {
            get
            {
                if (null != HttpContext.Current.Session["EntityName"])
                    return HttpContext.Current.Session["EntityName"] as string;
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["EntityName"] = value;
            }
        }
        public static string DefaultPage
        {
            get
            {
                if (null != HttpContext.Current.Session["DefaultPage"])
                    return HttpContext.Current.Session["DefaultPage"] as string;
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["DefaultPage"] = value;
            }
        }

        public static void reset() {
            DefaultPage = null;
            Email = null;
            EncUserID = null;
            UserID = 0;
            UserName = null;
            PrivigilesID = 0;
            EntityName = null;
            EntityID = 0;
            isMaster = 0;
        }
    }
}