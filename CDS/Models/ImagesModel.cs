using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace CDS.Models
{
    public class ImagesModel
    {
        /**********************************/
        /*BLL_LES_IMAGES (LEAF Name) Start*/

        public int imageID { get; set; }
        public int FileID { get; set; }

        //public string imageName { get; set; }
        public int BookIDFK { get; set; }
        public int ExtensionIDFK { get; set; }
        public int ChapterIDFK { get; set; }
        public int LessonIDFK { get; set; }
        public string ImageExt { get; set; }
        public string ImageName { get; set; }
        public int MediaGroupType { get; set; }
        public string EncName { get; set; }
        public string NickName { get; set; }
        public string ImageSrc { get; set; }

        public string ImageFolderPath { get; set; }

        /*********************************/
        /*BLL_LES_IMAGES (LEAF Name) END*/

        public string ImgName { get; set; }
        public string ImgSrc { get; set; }
        public List<ImagesModel> Search(string query, int filter)
        {
            List<ImagesModel> s = new List<ImagesModel>();

            var html = new HtmlDocument();
            //all
            string url = "";
            if (filter == 1)
            {
                url = string.Format(@"https://www.google.com.pk/search?q={0}&tbm=isch", query.Replace(" ", "+"));

                //url = string.Format(@"https://www.bing.com/images/search?q={0}&FORM=HDRSC2", query.Replace(" ", "+"));

            }
            else if (filter == 2)
            {
                url = string.Format(@"https://www.google.com.pk/search?q={0}&tbm=isch&tbs=sur:fc", query.Replace(" ", "+"));
            }
            else
            {
                url = string.Format(@"https://www.google.com.pk/search?tbm=isch&q={0}&tbs=sur:fmc", query.Replace(" ", "+"));
            }
            string data;
            using (var webClient = new System.Net.WebClient())
            {
                webClient.Proxy = null;
                webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_1) AppleWebKit/537.73.11 (KHTML, like Gecko) Version/7.0.1 Safari/537.73.11");
                data = webClient.DownloadString(url);
                webClient.Dispose();
            }
            //WebClient client = new WebClient();
            //client.Proxy = null;
            //client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_1) AppleWebKit/537.73.11 (KHTML, like Gecko) Version/7.0.1 Safari/537.73.11");
            //string data = client.DownloadString(url);

            html.LoadHtml(data); // load a string
            var root = html.DocumentNode;

            var a = root.Descendants("div").Where(div => div.GetAttributeValue("class", "") == "rg_meta");

            //HtmlNodeCollection linkNodes = root.SelectNodes("//div[@class='rg_meta']");
            HtmlNodeCollection linkNodes = root.SelectNodes("//div");

            //HtmlNodeCollection linkNodes = ;

            if (linkNodes != null)
            {
                foreach (HtmlNode img in linkNodes)
                {

                    ImagesModel model = new ImagesModel();
                    if (img.InnerHtml != "")
                    {
                        if ((img.InnerHtml.StartsWith("{") && img.InnerHtml.EndsWith("}")) || //For object
       (img.InnerHtml.StartsWith("[") && img.InnerHtml.EndsWith("]"))) //For array
                        {
                            JObject json = JObject.Parse(img.InnerHtml);
                            //if (json["sc"] == null)
                            //{
                            //    string name = (string)json["ou"];
                            //    string source = (string)json["tu"];
                            //    if (!name.Contains(".ashx"))
                            //    {
                            //        model.ImgName = (string)json["ou"];
                            //        model.ImgSrc = (string)json["tu"];
                            //        s.Add(model);
                            //    }
                            //}
                            string name = (string)json["ou"];
                            string source = (string)json["tu"];
                            if (!name.Contains(".ashx"))
                            {
                                model.ImgName = (string)json["ou"];
                                model.ImgSrc = (string)json["tu"];
                                s.Add(model);
                            }
                        }
                    }
                }
            }
            return s;

        }

        /**********************************/
        /*BLL_Con_Extension (LEAF Name) Start*/

        public int MyProperty { get; set; }
        public int ExtensionTypeID { get; set; }
        public string ExtensionName { get; set; }
        public int MaxSizeinMB { get; set; }

        /*********************************/
        /*BLL_Con_Extension (LEAF Name) END*/
    }

}