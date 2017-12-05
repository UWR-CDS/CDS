using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WEB_APP.Handler
{
    /// <summary>
    /// Summary description for SoundsHandler
    /// </summary>
    public class SoundsHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var imgid = "";
            var mainpath = "";
            if (context.Request.QueryString["isUWR"] != null)
            {
                mainpath = System.Configuration.ConfigurationSettings.AppSettings["OLDMediaPath"];
            }
            else
            {
                mainpath = System.Configuration.ConfigurationSettings.AppSettings["MediaPath"];
            }
            if (context.Request.QueryString["VideoKey"] != null)
            {
                imgid = context.Request.QueryString["Videokey"].ToString();
                try
                {
                    byte[] buffer = File.ReadAllBytes(mainpath +@"FAQ\"+ imgid + ".Mp4");
                    //context.Response.ContentType = "image/png";
                    context.Response.BinaryWrite(buffer);
                    context.Response.Flush();
                }
                catch (Exception ex) {

                }
            }
            else {
                imgid = context.Request.QueryString["key"].ToString();
            byte[] ToImgDesc = Convert.FromBase64String(imgid);
            var ImgDesc = UTF8Encoding.UTF8.GetString(ToImgDesc);
            byte[] buffer = null;
            try
            {
                buffer = File.ReadAllBytes(mainpath + ImgDesc);
                context.Response.BinaryWrite(buffer);
                context.Response.Flush();

            }
            catch (Exception ex)
            {


            }

            }

            //context.Response.ContentType = "sound/*";

            // context.Response.Flush();

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