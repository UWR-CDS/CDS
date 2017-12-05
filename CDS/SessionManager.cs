using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS
{    
    public class SessionManager
    {
        // private constructor
        private SessionManager() { }

        // **** add your session properties here, e.g like this:
        public string EncUserID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int PrivigilesID { get; set; }
        public int isMaster { get; set; }
        public int EntityID { get; set; }
        public string EntityName { get; set; }
        public string DefaultPage { get; set; }
        public int UserID { get; set; }

        // Gets the current session.
        public static SessionManager Current
        {
            get
            {
                SessionManager session =(SessionManager)HttpContext.Current.Session["__SessionManager__"];

                if (session == null){
                    session = new SessionManager();
                    HttpContext.Current.Session["__SessionManager__"] = session;
                }
                return session;
            }
        }


       
    }
}