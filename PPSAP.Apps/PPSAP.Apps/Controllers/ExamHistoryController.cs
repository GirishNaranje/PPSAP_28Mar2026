using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using PPSAP.Apps.Proxy;
using PPSAP.Common;
using PPSAP.DTO;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.pipeline.css;
using Newtonsoft.Json;
using Serilog;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;

namespace PPSAP.Apps.Controllers
{
    using iTextSharp.text;
    using HtmlAgilityPack;

    [Authorize]
    public class ExamHistoryController : Controller
    {
        private string _sAttrLogPath = ConfigurationManager.AppSettings.Get("LogfilePath");
        // GET: Assesment
        private Serilog.Core.Logger log;

        public ExamHistoryController()
        {
            log = new LoggerConfiguration().WriteTo.RollingFile(this._sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();
        }

        // POST: ExamHistory
        public ActionResult Index()
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            ServiceCallVM serviceCall = new ServiceCallVM
            {
                userId = user._userId,
            };
            string serviceJson = JsonConvert.SerializeObject(serviceCall);
            string urlGetsatatus = PPSAPGlobalConstants.SiteWebAPIUrl + "dashboard/GetUserOptIn";
            string resultOptInStatus = HttpProxy.HttpPost(urlGetsatatus, serviceJson, "application/json; charset=utf-8", "POST");
            string optInStatus = JsonConvert.DeserializeObject<string>(resultOptInStatus);
            ViewBag.optInStatus = optInStatus == null ? "0" : optInStatus;
            return View();
        }

        public ActionResult InfoMessage()
        {
            TempData["Msg"] = "Exam successfully submitted ";
            return RedirectToAction("Index", "ExamHistory");
        }

        public ActionResult ExamHistoryDetails(int? Status)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            JsonResult dataresult = new JsonResult();
            try
            {
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                ExamHistoryDTO examhistory = new ExamHistoryDTO();
                examhistory.UserId = user._userId;
                examhistory.ExamStatus = Convert.ToInt32(Status);
                string examPostDataJson = JsonConvert.SerializeObject(examhistory);
                string url = PPSAPGlobalConstants.SiteWebAPIUrl + "ExamHistory/ExamHistoryDetails";
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                List<ExamHistoryDTO> historydetails = new List<ExamHistoryDTO>();
                historydetails = JsonConvert.DeserializeObject<List<ExamHistoryDTO>>(result);
                historydetails.RemoveAll(x => x.ExamType == "Spaced Repetition");
                int totalRecords = historydetails.Count;
                int recFilter = historydetails.Count;
                historydetails = historydetails.Skip(startRec).Take(pageSize).ToList();
                dataresult = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = historydetails }, JsonRequestBehavior.AllowGet);
                return dataresult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public ActionResult DeleteExamHistoryDetails(int? ExamId)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                ExamHistoryDTO examhistory = new ExamHistoryDTO();
                examhistory.UserId = user._userId;
                examhistory.ExamId = Convert.ToInt32(ExamId);
                string url = PPSAPGlobalConstants.SiteWebAPIUrl + "ExamHistory/DeleteExamHistoryDetails";
                string examPostDataJson = JsonConvert.SerializeObject(examhistory);
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                return RedirectToAction("Index", "ExamHistory");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static string StripHTML(string input)
        {
            if (input != null)
            {
                return Regex.Replace(input, "<.*?>", string.Empty);
            }
            else
            {
                return null;
            }
        }

        public ActionResult ShareResult(int examId, string emaild, string dateTime)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                string htmlFile = string.Empty;
                ExamQuestionDTO examque = new ExamQuestionDTO();
                examque.UserId = user._userId;
                examque.ExamId = Convert.ToInt32(examId);
                string examPostDataJson = JsonConvert.SerializeObject(examque);

                string pdfDataUrl = PPSAPGlobalConstants.SiteWebAPIUrl + "ExamHistory/GetPdfDetails";
                string pdfDataResult = HttpProxy.HttpPost(pdfDataUrl, examPostDataJson, "application/json; charset=utf-8", "POST");
                PdfDetailsDataVM pdfDetails = new PdfDetailsDataVM();
                pdfDetails = JsonConvert.DeserializeObject<PdfDetailsDataVM>(pdfDataResult);
                htmlFile += "<html><head><style>body {background-color: #fff; font-size: 14px; font-family: Arial;} p {margin: 0;}</style></head><body>";
                htmlFile += "<p><b>Exam Summary</b></p>";
                if (pdfDetails.CompletionDate != null)
                {
                    htmlFile += "<p>This Exam, completed on " + pdfDetails.CompletionDate;
                }

                if (pdfDetails.UserName != null)
                {
                    htmlFile += " by " + pdfDetails.UserName;
                }

                if (pdfDetails.ResidencyEndDate != null)
                {
                    htmlFile += " with residency end date " + pdfDetails.ResidencyEndDate;
                }

                htmlFile += " contained " + pdfDetails.TotalQuestion + " questions";
                if (pdfDetails.SectionLIst.Count > 0)
                {
                    htmlFile += " from the following sections:</p><br/>";
                    foreach (SectionVM section in pdfDetails.SectionLIst)
                    {
                        htmlFile += "<p>" + section.SectionName + "</p>";
                    }
                }
                else
                {
                    htmlFile += ".</p>";
                }

                htmlFile += " <hr /><p>This was a " + pdfDetails.TimedExam + "</p>";
                htmlFile += "<p>This was a " + pdfDetails.ExamMode + "</p><hr />";
                htmlFile += "<p>Overall score: " + pdfDetails.OverallScore + "%</p>";
                htmlFile += "<p>Correct answers: " + pdfDetails.CorrectAnswers + "</p>";
                htmlFile += "<p>Incorrect answers: " + pdfDetails.IncorrectAnswers + "</p>";
                htmlFile += "<p>Unanswered: " + pdfDetails.Unanswered + "</p></body></html>";

                // No need to replace "\r\n" or use Server.HtmlEncode(htmlFile)

                bool flag = CreatePdfFromHtmlFile(htmlFile, examId, emaild, dateTime);

                if (flag)
                {
                    return Json("1");
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public bool CreatePdfFromHtmlFile(string emailBody, int examId, string emailId, string dateTime)
        {
            try
            {
                // SMTP Configuration
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 587; // or any other port your SMTP server uses
                string smtpUsername = "member.services2024@gmail.com";
                string smtpPassword = "mqqanpxjleudlhbg";

                // Sender and Recipient
                string senderEmail = "member.services2024@gmail.com";
                string recipientEmail = emailId;

                // Email Subject
                string subject = "Subject of the Email";

                // Email Body
                string body = emailBody;

                // Create the email message
                MailMessage mail = new MailMessage(senderEmail, recipientEmail, subject, body);
                mail.IsBodyHtml = true; // Set to true if email body contains HTML

                // SMTP client
                SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true; // Set to true if your SMTP server requires SSL

                // Send email
                smtpClient.Send(mail);

                return true; // Email sent successfully
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error sending email: " + ex.Message);
                return false; // Failed to send email
            }
        }

        /*public bool CreatePdfFromHtmlFile(string strhtmlData, int examId, string emailid, string dateTime)
        {
            string storeUrl = ConfigurationManager.AppSettings["STORE_URL"].ToString();
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                var filePath = @"~\ShareResultEmailTemplete\ShareResultEmailTemplete.html";
                DirectoryInfo directory = new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath(filePath));
                string username = user._userName;
                string userEmail = user._userEmail;
                List<string> cssFiles = new List<string>();
                string strhtml = Server.HtmlDecode(strhtmlData);
                string path = ConfigurationManager.AppSettings["ShareResultPDFPath"].ToString();
                var myUniqueFileName = string.Format(@"{0}.pdf", username + '_' + dateTime);

                // var myUniqueFileName = string.Format(@"{0}.pdf", Guid.NewGuid());
                string strFileName = path + myUniqueFileName;

                if (!(Directory.Exists(path)))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    DeletePDFFile(strFileName);
                }

                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(strFileName, FileMode.Create));
                ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
                cssFiles.ForEach(i => cssResolver.AddCssFile(System.Web.HttpContext.Current.Server.MapPath(i), true));
                using (var readHtml = new StringReader(strhtml))
                {
                    document.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, readHtml);
                    document.Close();
                    var sendFeedback = new AssessmentController();
                    var doc = new HtmlDocument();
                    doc.Load(directory.ToString());
                    string shareResultBody = doc.Text;
                    shareResultBody = shareResultBody.Replace("UserName", username);
                    shareResultBody = shareResultBody.Replace("Useremail", userEmail);
                    int result = sendFeedback.SendFeedback(shareResultBody, examId, strFileName, username, emailid);
                    DeletePDFFile(strFileName);
                    if (result == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }*/

        public static void DeletePDFFile(string filename)
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();

            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }
        }

        public ActionResult ResetExam()
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                log.Information("Start Controller:{0}, Method:{1}, userId:{2}", "ExamHistoryController", "ResetExam", user._userId);
                ExamCountOnExamTypeVM examReset = new ExamCountOnExamTypeVM
                {
                    UserId = user._userId,
                };

                // Get ExamId By UserId
                string examResetDataJson = JsonConvert.SerializeObject(examReset);
                string urlExamReset = PPSAPGlobalConstants.SiteWebAPIUrl + "ExamHistory/ResetExam";
                string resultExamReset = HttpProxy.HttpPost(urlExamReset, examResetDataJson, "application/json; charset=utf-8", "POST");
                int result = JsonConvert.DeserializeObject<int>(resultExamReset);
                ViewBag.ResetExam = result;
                log.Information("End Controller:{0}, Method:{1}, userId:{2}", "ExamHistoryController", "ResetExam", user._userId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " Controller:{0}, Method:{1}, UserId:{2}", "ExamHistoryController", "ResetExam", user._userId);
                Console.Write(ex.Message);
                return null;
            }
        }
    }
}