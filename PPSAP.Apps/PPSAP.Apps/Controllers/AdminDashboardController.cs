using System;
using System.Web.Mvc;
using PPSAP.Apps.Proxy;
using PPSAP.Common;
using Newtonsoft.Json;

namespace PPSAP.Apps.Controllers
{
    [Authorize]
    public class AdminDashboardController : Controller
    {
        // GET: AdminDashboard
        public ActionResult Index()
        {
            return RedirectToAction("Index", "AllQuestions");
        }

        public ActionResult GetUserListIndex()
        {
            return View();
        }

        //public ActionResult ImportQASData()
        //{
        //    return View();
        //}

        //public ActionResult SubmitQASData()
        //{
        //    ServiceCallVM serviceCall = new ServiceCallVM
        //    {
        //        Status = 7,
        //    };
        //    string serviceJson = JsonConvert.SerializeObject(serviceCall);
        //    string urlSubmitData = PPSAPGlobalConstants.SiteWebAPIUrl + "admindashboard/SubmitQASData";
        //    string resultSubmitData = HttpProxy.HttpPost(urlSubmitData, serviceJson, "application/json; charset=utf-8", "POST");
        //    return View();
        //}

        //public ActionResult UpdateQASData()
        //{
        //    ServiceCallVM serviceCall = new ServiceCallVM
        //    {
        //        Status = 7,
        //        Modifiedafter = DateTime.Now.AddDays(-1),
        //    };
        //    string serviceJson = JsonConvert.SerializeObject(serviceCall);
        //    string urlUpdateData = PPSAPGlobalConstants.SiteWebAPIUrl + "admindashboard/UpdateQASData";
        //    string resultUpdateData = HttpProxy.HttpPost(urlUpdateData, serviceJson, "application/json; charset=utf-8", "POST");
        //    return View();
        //}

        //public ActionResult RetiredQASData()
        //{
        //    ServiceCallVM serviceCall = new ServiceCallVM
        //    {
        //        Status = 8,
        //        Modifiedafter = DateTime.Now.AddDays(-1),
        //    };
        //    string serviceJson = JsonConvert.SerializeObject(serviceCall);
        //    string urlRetiredData = PPSAPGlobalConstants.SiteWebAPIUrl + "admindashboard/RetiredQASData";
        //    string resultRetiredData = HttpProxy.HttpPost(urlRetiredData, serviceJson, "application/json; charset=utf-8", "POST");
        //    return View();
        //}

        public ActionResult GetQuestionCountFromQAS()
        {
            ServiceCallVM serviceCall = new ServiceCallVM
            {
                Status = 7,
            };
            string serviceJson = JsonConvert.SerializeObject(serviceCall);
            string urlGetCountdData = PPSAPGlobalConstants.SiteWebAPIUrl + "admindashboard/GetQuestionCount";
            string resultQuestionCount = HttpProxy.HttpPost(urlGetCountdData, serviceJson, "application/json; charset=utf-8", "POST");
            int questionCount = JsonConvert.DeserializeObject<int>(resultQuestionCount);
            return Json(questionCount, JsonRequestBehavior.AllowGet);
        }
    }
}