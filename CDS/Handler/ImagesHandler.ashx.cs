using System;
using System.IO;
using System.Text;
using System.Web;

namespace WEB_APP.Handler
{
    /// <summary>
    /// Summary description for ImagesHandler
    /// </summary>
    public class ImagesHandler : IHttpHandler
    {
        string path1 = string.Empty;

        public bool IsReusable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            var mainpath = "";
            if (context.Request.QueryString["isUWR"] != null)
            {
                mainpath = System.Configuration.ConfigurationSettings.AppSettings["OLDMediaPath"];
            }
            else
            {
                mainpath = System.Configuration.ConfigurationSettings.AppSettings["MediaPath"];
            }
            if (context.Request.QueryString["LessonFilePath"] != null)
            {
                try
                {
                    var imagepath = context.Request.QueryString["LessonFilePath"].ToString();
                    var imageMainPath = mainpath + imagepath;
                    byte[] buffers = null;
                    buffers = File.ReadAllBytes(imageMainPath);
                    context.Response.BinaryWrite(buffers);
                    context.Response.Flush();

                }
                catch (Exception ex)
                {
                    context.Response.Clear();
                    byte[] buffer = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/images/no_photo.jpg"));
                    context.Response.ContentType = "image/png";
                    context.Response.BinaryWrite(buffer);
                    context.Response.Flush();
                }


            }
            else if (context.Request.QueryString["FeedBackFilePath"] != null)
            {
                try
                {
                    var imagepath = context.Request.QueryString["FeedBackFilePath"].ToString();
                    var imageMainPath = mainpath + imagepath;
                    byte[] buffers = null;
                    buffers = File.ReadAllBytes(imageMainPath);
                    context.Response.BinaryWrite(buffers);
                    context.Response.Flush();

                }
                catch (Exception ex)
                {
                    context.Response.Clear();
                    byte[] buffer = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/images/no_photo.jpg"));
                    //byte[] buffer = File.ReadAllBytes(mainpath + @"picture\Teacher\no_photo.jpg");
                    context.Response.ContentType = "image/png";
                    context.Response.BinaryWrite(buffer);
                    context.Response.Flush();
                }


            }
            else
            {
                var imgid = context.Request.QueryString["key"].ToString();
                string from = context.Request.QueryString["from"].ToString();

                byte[] ToImgDesc = Convert.FromBase64String(imgid);
                var ImgDesc = UTF8Encoding.UTF8.GetString(ToImgDesc);
                byte[] buffer = null;
                if (from == "true")
                {
                    try
                    {
                        buffer = File.ReadAllBytes(ImgDesc);

                    }
                    catch (Exception ex)
                    {
                    }

                }
                else
                {
                    if (ImgDesc != null && mainpath != null)
                    {
                        try
                        {
                            buffer = File.ReadAllBytes(mainpath + ImgDesc);
                        }
                        catch (Exception ex)
                        {
                        }

                    }

                }
                try
                {
                    context.Response.BinaryWrite(buffer);
                    context.Response.Flush();

                }
                catch (Exception ex)
                {
                }
            }
        }

    }

}