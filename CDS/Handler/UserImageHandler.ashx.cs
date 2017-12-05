using CDS.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CDS.Handler
{
    /// <summary>
    /// Summary description for UserImageHandler
    /// </summary>
    public class UserImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var id = "";
                if (context.Request.QueryString["vuser"] != null)
                    id = EncyptionDcryption.GetDecryptedText(context.Request.QueryString["key"].ToString());
                else
                    id = context.Request.QueryString["key"].ToString();
                context.Response.Clear();
                byte[] buffer = File.ReadAllBytes(System.Configuration.ConfigurationSettings.AppSettings["MediaPath"] + @"User\" + id + ".PNG");
                context.Response.ContentType = "image/png";
                context.Response.BinaryWrite(buffer);
                context.Response.Flush();
            }
            catch (Exception ex)
            {

                context.Response.Clear();
                byte[] buffer = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/images/profile-img.png"));
                context.Response.ContentType = "image/png";
                context.Response.BinaryWrite(buffer);
                context.Response.Flush();
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}