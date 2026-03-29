using PPSAP.Apps.Proxy;
using PPSAP.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PPSAP.Apps.Controllers
{
    [Authorize]
    public class SearchByExamController : Controller
    {
        // GET: SearchByExam
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchByQuestions(int? ExamId, string SearchTerm, string Filter, int NoOfRecords = 10, int PageNo = 1)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                if (SearchTerm.Length > 2)
                {
                    AssesmentDetailVM assDetail = new AssesmentDetailVM();
                    assDetail.ExamId = Convert.ToInt32(ExamId);
                    assDetail.Filter = Convert.ToString(Filter);
                    assDetail.SearchTerm = Convert.ToString(SearchTerm);
                    assDetail.NoOfRecords = NoOfRecords;
                    assDetail.PageNo = PageNo;
                    assDetail.UserId = user._userId;
                    string examPostDataJson = JsonConvert.SerializeObject(assDetail);
                    var data = System.Text.Encoding.UTF8.GetBytes(examPostDataJson);
                    string url = PPSAPGlobalConstants.SiteWebAPIUrl + "ViewAssessment/SearchByQuestions";
                    string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");

                    List<QuestionDetails> searchByQuestions = new List<QuestionDetails>();
                    searchByQuestions = JsonConvert.DeserializeObject<List<QuestionDetails>>(result);

                    ViewBag.Loading = true;
                    if (searchByQuestions.Count > 0)
                    {
                        ViewBag.Loading = false;
                        ViewBag.FirstSerialNum = searchByQuestions[0].serialNumber;
                        ViewBag.LastSerialNum = searchByQuestions[searchByQuestions.Count - 1].serialNumber;
                    }

                    ViewBag.ExamId = ExamId;
                    ViewBag.SearchTerm = SearchTerm;
                    ViewBag.NoOfRecords = NoOfRecords;
                    ViewBag.PageNo = PageNo;
                    ViewBag.Filter = Filter;
                    ViewBag.viewCurrentRecordcount = searchByQuestions.Count;
                    ViewBag.viewAssessmentdetailscount = searchByQuestions.Count > 0 ? searchByQuestions[0].QuestionCount : 0;
                    ViewBag.viewAssessmentdetails = searchByQuestions;
                    return View();
                }
                else
                {
                    TempData["msg"] = "<script>alert('Please enter minimum 3 characters');</script>";
                    return View("Index");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}