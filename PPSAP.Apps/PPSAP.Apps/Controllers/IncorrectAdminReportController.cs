using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PPSAP.Apps.Proxy;
using PPSAP.Common;
using PPSAP.DTO;
using Newtonsoft.Json;

namespace PPSAP.Apps.Controllers
{
    [Authorize]
    public class IncorrectAdminReportController : Controller
    {
        // GET: IncorrectAdminReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminIncorrectQuestionDetails(int year, int? SubspecialtyId, DateTime? ExamStartDate = null, DateTime? ExamCompletedDate = null, int NoOfRecords = 10, int PageNo = 1)
        {
            try
            {
                IncorrectQuestionDetailsDTO incorrectReportDetails = new IncorrectQuestionDetailsDTO();
                incorrectReportDetails.SubspecialtyId = Convert.ToInt32(SubspecialtyId);
                incorrectReportDetails.ExamStartDate = ExamStartDate;
                incorrectReportDetails.ExamCompletedDate = ExamCompletedDate;
                incorrectReportDetails.NoOfRecords = NoOfRecords;
                incorrectReportDetails.PageNo = PageNo;
                incorrectReportDetails.Year = year;
                string examPostDataJson = JsonConvert.SerializeObject(incorrectReportDetails);
                string url = PPSAPGlobalConstants.SiteWebAPIUrl + "IncorrectAdminReport/AdminIncorrectQuestionDetails";
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                List<QuestionDetails> incorrectQuestiondetails = new List<QuestionDetails>();
                incorrectQuestiondetails = JsonConvert.DeserializeObject<List<QuestionDetails>>(result);
                ViewBag.incorrectQuestiondetailscount = incorrectQuestiondetails.Count;
                ViewBag.incorrectQuestiondetailsdata = incorrectQuestiondetails;
                ViewBag.FirstSerialNum = incorrectQuestiondetails[0].serialNumber;
                ViewBag.LastSerialNum = incorrectQuestiondetails[incorrectQuestiondetails.Count - 1].serialNumber;
                ViewBag.SubSpecialityId = SubspecialtyId;
                ViewBag.NoOfRecords = NoOfRecords;
                ViewBag.PageNo = PageNo;
                ViewBag.SubSpecialityNumber = incorrectQuestiondetails.Count > 0 ? incorrectQuestiondetails[0].Section : 0;
                ViewBag.SubSpeciality = incorrectQuestiondetails.Count > 0 ? incorrectQuestiondetails[0].SubSpeciality : null;
                ViewBag.RecordCount = incorrectQuestiondetails.Count > 0 ? incorrectQuestiondetails[0].QuestionCount : 0;
                ViewBag.year = year;
                ViewBag.ExamStartDate = ExamStartDate;
                ViewBag.ExamCompletedDate = ExamCompletedDate;

                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}