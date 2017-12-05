using CDS.Logic;
using CDS.Manager;
using CDS.Models;
using LEAF_Logic;
using Microsoft.Office.Interop.Word;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.Widget;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CDS.Controllers
{
    public class HTMLTOWORDController : Controller
    {
        [HttpGet]
        public ActionResult Index(string LessonPlanID = null, int BookTypeID = 0, int BookID = 0, int IsBookPage = 0)
        {

            if (LessonPlanID != null)
            {
                try
                {
                    double span = CommonMng.getrandomnumber();
                    if (LessonPlanID.StartsWith("'") && LessonPlanID.EndsWith("'"))
                    {
                        LessonPlanID = LessonPlanID.Replace("'", "");
                    }
                    bool isnumber = CommonMng.IsNumeric(LessonPlanID);
                    int ID = 0;
                    if (isnumber)
                    {
                        ID = Convert.ToInt32(LessonPlanID);
                    }
                    else
                    {
                        ID = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(LessonPlanID));
                        ViewBag.url = ID;
                    }
                    string filename = "Lesson" + span + ".doc";
                    string getscriptpath = CommonMng.baseUrl + "/HTMLTOWORD/GetWORD?LessonPlanId=" + ID + "&&BookTypeID=" + BookTypeID + "&&BookID=" + BookID + "&&IsBookPage=" + IsBookPage;
                    string htmlString = GetHtmlFromAspx(getscriptpath);
                    string originalstring = htmlString;
                    originalstring = originalstring.Replace("<link href='" + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/Content", "<link href='" + CommonMng.baseUrl + "/Content");
                    originalstring = originalstring.Replace("src='" + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/'), "src='" + CommonMng.baseUrl);
                    originalstring = originalstring.Replace("..\\Handler\\ImagesHandler.ashx", CommonMng.baseUrl + "\\Handler\\ImagesHandler.ashx");
                    int count = 0;
                    string substr = "src=\"data:image";
                    while (originalstring.Contains(substr))
                    {
                        string replaceStr = originalstring.Substring(originalstring.IndexOf(substr) + 5, (originalstring.Substring(originalstring.IndexOf(substr) + 5)).IndexOf("\""));
                        string scrString = replaceStr.Substring(replaceStr.IndexOf("base64,") + 7);
                        if (CommonMng.getimegbytes == null)
                            CommonMng.getimegbytes = new List<getimegbytes>();
                        CommonMng.getimegbytes.Add(new getimegbytes { id = count, getImegbytes = Convert.FromBase64String(scrString) });
                        originalstring = originalstring.Replace(replaceStr, "../Handler/ImageConvertorHandler.ashx?key=" + count);
                        count++;
                    }
                    originalstring = originalstring.Replace("src=\"..", "src=" + CommonMng.baseUrl);
                    bool exists = System.IO.Directory.Exists(Server.MapPath(@"~/TempFile"));
                    if (!exists)
                        System.IO.Directory.CreateDirectory(Server.MapPath(@"~/TempFile"));

                    string rtfFile = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/TempFile"), filename);

                    if (originalstring != null)
                    {
                        System.IO.File.WriteAllText(rtfFile, originalstring);

                    }

                    if (rtfFile != null && rtfFile != "")
                    {
                        ConvertWordtopdf(rtfFile);
                        KillProcess();
                        string p = CommonMng.baseUrl + "/TempFile/" + filename.Replace(".doc", ".pdf");
                        return Json(p, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {

                    return Json(null, JsonRequestBehavior.AllowGet);
                }




            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }


        }
        [HttpGet]
        public ActionResult GetWORD(int LessonPlanId = 0, int BookTypeID = 0, int BookID = 0, int IsBookPage = 0)
        {
            if (LessonPlanId != 0)
            {
                DataSet ds = null;
                CommonMng.getimegbytes = new List<getimegbytes>();
                LessonInfo ltp = new LessonInfo();
                ds = new PdfManager().LessonToPdfData(LessonPlanId);
                if (ds!=null)
                {
                    ltp = new LessonInfoManager().GetPDfLessonHeader(ds.Tables[0]);
                    //if (BookTypeID == 1)
                    //{
                    if (ds != null && ds.Tables.Count >= 3 && ds.Tables[2].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            int count = 0;
                            string script = ds.Tables[2].Rows[i]["Script"].ToString();
                            script = script.Replace("@Baseurl", CommonMng.baseUrl);
                            script = script.Replace("%5C", "/");
                            script = script.Replace("&amp;&amp;", "&&");
                            script = script.Replace("<img style=\"height:32px; width:32px;\" onclick=\"showToast('", "<img height=32 width=32 onclick=\"showToast('");
                            script = script.Replace("<img style=\"height: 32px; width: 32px;\" onclick=\"showToast('", "<img height=32 width=32 onclick=\"showToast('");
                            script = script.Replace("<img", "<img class=\"img-responsive\"");
                            script = script.Replace("class=\" \"", " ");
                            string substr = "src=\"data:image";
                            while (script.Contains(substr))
                            {
                                string replaceStr = script.Substring(script.IndexOf(substr) + 5, (script.Substring(script.IndexOf(substr) + 5)).IndexOf("\""));
                                string scrString = replaceStr.Substring(replaceStr.IndexOf("base64,") + 7);
                                if (CommonMng.getimegbytes == null)
                                    CommonMng.getimegbytes = new List<getimegbytes>();
                                CommonMng.getimegbytes.Add(new getimegbytes { id = count, getImegbytes = Convert.FromBase64String(scrString) });
                                script = script.Replace(replaceStr, "../Handler/ImageConvertorHandler.ashx?key=" + count);
                                count++;
                            }
                            script = script.Replace("src=\"..", "src=" + CommonMng.baseUrl);
                            ds.Tables[2].Rows[i]["Script"] = script;
                        }
                    }
                    ltp.sections = new LessonInfoManager().GetLessonSections(ds.Tables[1]);
                    ltp.subsections = new LessonInfoManager().GetLessonSubSections(ds.Tables[2]);
                    if (ltp.LanguageID == 1)
                    {
                        return View("EnglishLessonPDF", ltp);
                    }
                    else
                    {
                        return View("UrduLessonPDF", ltp);
                    }

                    //    }
                    //    //else
                    //    //{
                    //    //    if (IsBookPage == 1)
                    //    //    {
                    //    //        BookPagePrint obj = new BookPagePrint();
                    //    //        obj = new BookComposingHandler().GetBookPageDetail(ID);
                    //    //        return View("GetBookPagePrint", obj);
                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        ltp.TotalPagesIamges = new List<string>();
                    //    //        List<BookViewData> pges = new List<BookViewData>();
                    //    //        if (ID == 0)
                    //    //        {
                    //    //            pges = new BookComposingHandler().GetTotalImagesBook(BookID, BookTypeID);
                    //    //        }
                    //    //        else
                    //    //        {
                    //    //            pges = new BookComposingHandler().GetTotalImagesBook(ltp.BookID, BookTypeID);
                    //    //        }

                    //    //        foreach (var item in pges)
                    //    //        {
                    //    //            string img = baseUrl + item.Src;
                    //    //            img = img.Replace("\\", "/");
                    //    //            img = img.Replace("..", "/");
                    //    //            ltp.TotalPagesIamges.Add(img);
                    //    //        }
                    //    //        return View("GetBookPrint", ltp);
                    //    //    }

                    //    }
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
        }
        public static string GetHtmlFromAspx(string url)
        {
            string contents = "";

            if (url.Length > 6)
            {
                //open 'http://' file
                if ((url[0] == 'h' || url[0] == 'H') && (url[1] == 't' || url[1] == 'T') &&
                    (url[2] == 't' || url[2] == 'T') && (url[3] == 'p' || url[3] == 'P') &&
                    url[4] == ':' && url[5] == '/' && url[6] == '/')
                {

                    Stream StreamHttp = null;
                    WebResponse resp = null;
                    HttpWebRequest webrequest = null;
                    try
                    {
                        webrequest = (HttpWebRequest)WebRequest.Create(url);
                        resp = webrequest.GetResponse();
                        StreamHttp = resp.GetResponseStream();
                        StreamReader sr = new StreamReader(StreamHttp);
                        contents = sr.ReadToEnd();
                        return contents;
                    }
                    catch
                    {
                    }
                }
                else
                {
                    try
                    {
                        StreamReader sr = new StreamReader(url);
                        contents = sr.ReadToEnd();
                        sr.Close();

                    }
                    catch
                    {
                    }

                }
            }
            return contents;
        }
        public void KillProcess()
        {
            try
            {
                Process[] procs1 = Process.GetProcessesByName("AcroRd32");

                foreach (Process proc1 in procs1)
                {
                    proc1.Kill();
                }
            }
            catch (Exception ex)
            {

            }

        }
        private string RemoveImageResponciveClass(string strMain)
        {
            string result = "";
            string[] strArray = strMain.Split(new string[] { "<img" }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length > 0)
            {
                for (int i = 0; i < strArray.Length; i++)
                {
                    string da = getBetween(strArray[i], "width=\"", "\"");
                    if (da != null && da != "")
                    {
                        int value = Convert.ToInt32(da);
                        if (value > 660)
                        {
                            int Start, End;
                            Start = strArray[i].IndexOf("width=\"", 0) + "\"".Length;
                            End = strArray[i].IndexOf("\"", Start);
                            strArray[i] = strArray[i].Remove(Start, End).Insert(Start, "width=\"660" + "\"");
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(strArray[i]) && strArray[i].IndexOf("img-responsive") == -1)
                        result += strArray[i].Replace("<img", "<img class=\"img-responsive\"");

                    else
                        result += strArray[i];
                }
            }
            else
                result = strMain;
            return result;
        }
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
        public void ConvertWordtopdf(string Lessonpdfpath)
        {

            if (Lessonpdfpath != "" && Lessonpdfpath != null)
            {
                try
                {

                    FileInfo abc = new FileInfo(Lessonpdfpath);
                    if (abc.Exists == true)
                    {
                        string FileExtension = Path.GetExtension(Lessonpdfpath);
                        string outfile = Lessonpdfpath.Replace(".doc", ".pdf");
                        if (FileExtension == ".doc" || FileExtension == ".docx")
                        {

                            Application app = new Application();
                            Microsoft.Office.Interop.Word.Document doc = null;
                            doc = app.Documents.Open(Lessonpdfpath, Type.Missing, true);


                            doc.ExportAsFixedFormat(outfile, WdExportFormat.wdExportFormatPDF, false);

                            doc.Close(false, Type.Missing, Type.Missing);
                            app.Quit(false, false, false);
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);

                            abc.Delete();
                            Lessonpdfpath = outfile;
                            AddPageNumber(outfile);
                        }

                    }
                }
                catch (Exception ex)
                {


                }

            }
        }
        protected void AddPageNumber(string path)
        {
            if (path != "" && path != null)
            {
                try
                {
                    PdfDocument doc = new PdfDocument();

                    doc.LoadFromFile(path);

                    //set the margin

                    PdfUnitConvertor unitCvtr = new PdfUnitConvertor();

                    PdfMargins margin = new PdfMargins();

                    margin.Top = unitCvtr.ConvertUnits(2.54f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);

                    margin.Bottom = margin.Top;

                    margin.Left = unitCvtr.ConvertUnits(10.17f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);

                    margin.Right = margin.Left;

                    //draw page number

                    DrawPageNumber(doc.Pages, margin, 1, doc.Pages.Count);

                    //save the file

                    doc.SaveToFile(path, FileFormat.PDF);

                    // Lessonpdfpath = path;
                    path = path;

                }
                catch (Exception ex)
                {
                }
            }

        }
        private static void DrawPageNumber(PdfPageCollection section, PdfMargins margin, int startNumber, int pageCount)
        {

            foreach (PdfPageBase page in section)

            {

                page.Canvas.SetTransparency(0.5f);

                PdfBrush brush = PdfBrushes.Black;

                PdfPen pen = new PdfPen(brush, 0.75f);

                PdfTrueTypeFont font = new PdfTrueTypeFont(new System.Drawing.Font("Arial", 12f, System.Drawing.FontStyle.Bold), true);

                PdfStringFormat format = new PdfStringFormat(PdfTextAlignment.Right);

                format.MeasureTrailingSpaces = true;

                float space = font.Height * 0.75f;

                float x = margin.Left;

                float width = page.Canvas.ClientSize.Width - margin.Left - margin.Right;

                float y = page.Canvas.ClientSize.Height - margin.Bottom + space;

                y = y + 1;

                String numberLabel = String.Format("{0} of {1}", startNumber++, pageCount);

                page.Canvas.DrawString(numberLabel, font, brush, x + width, y, format);

                page.Canvas.SetTransparency(1);

            }

        }
    }
}