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
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            ServiceCallVM serviceCall = new ServiceCallVM
            {
                userId = user._userId,
            };
            //string serviceJson = JsonConvert.SerializeObject(serviceCall);
            //string urlUserRenewalMesssage = PPSAPGlobalConstants.SiteWebAPIUrl + "dashboard/GetRenewal";
            //string resultUserRenewalMesssage = HttpProxy.HttpPost(urlUserRenewalMesssage, serviceJson, "application/json; charset=utf-8", "POST");
            //
            //// UserDataDTO userdata = new UserDataDTO();
            //List<UserDataDTO> userdata = new List<UserDataDTO>();
            //
            //// userdata = JsonConvert.DeserializeObject<UserDataDTO>(resultUserRenewalMesssage);
            //userdata = JsonConvert.DeserializeObject<List<UserDataDTO>>(resultUserRenewalMesssage);

            ServiceCallVM serviceCallOptin = new ServiceCallVM
            {
                userId = user._userId,
            };
            string serviceJsonOptin = JsonConvert.SerializeObject(serviceCallOptin);
            string urlGetsatatus = PPSAPGlobalConstants.SiteWebAPIUrl + "dashboard/GetUserOptIn";
            string resultOptInStatus = HttpProxy.HttpPost(urlGetsatatus, serviceJsonOptin, "application/json; charset=utf-8", "POST");
            string optInStatus = JsonConvert.DeserializeObject<string>(resultOptInStatus);
            ViewBag.optInStatus = optInStatus == null ? "0" : optInStatus;

            SearchParameters searchParameters = new SearchParameters()
            {
                UserId = user._userId,
            };

            JsonResult dataresult = new JsonResult();
            string examPostDataJson = JsonConvert.SerializeObject(searchParameters);
            string url = PPSAPGlobalConstants.SiteWebAPIUrl + "NotificationMessageSetting/GetAllMessageSettingByUser";
            string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
            List<NotificationMessageSetting> notificationMessageSettings = new List<NotificationMessageSetting>();
            notificationMessageSettings = JsonConvert.DeserializeObject<List<NotificationMessageSetting>>(result);

            ViewBag.NotificationMessageSettings = notificationMessageSettings;

            if (Session["IsNotificationMessageUseInsert"] == null || !(bool)(Session["IsNotificationMessageUseInsert"]))
            {
                string insertMessagePostDataJson = JsonConvert.SerializeObject(searchParameters);
                string insertUrl = PPSAPGlobalConstants.SiteWebAPIUrl + "NotificationMessageSetting/InsertOrUpdateUserMessageSetting";
                string insertResult = HttpProxy.HttpPost(insertUrl, insertMessagePostDataJson, "application/json; charset=utf-8", "POST");
                Session["IsNotificationMessageUseInsert"] = JsonConvert.DeserializeObject<bool>(insertResult);
                ViewBag.ShowFirstTime = true;
            }
            else
            {
                ViewBag.ShowFirstTime = false;
            }

            return View();
        }

        public ActionResult AboutPage()
        {
            return View();
        }

        public ActionResult HelpPage()
        {
            return View();
        }

        public ActionResult MyGetmyExamHistoryList()
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            ServiceCallVM serviceCall = new ServiceCallVM
            {
                userId = user._userId,
            };
            string serviceJson = JsonConvert.SerializeObject(serviceCall);
            string urlGetExamStatusCount = PPSAPGlobalConstants.SiteWebAPIUrl + "dashboard/GetmyExamHistoryList";
            string resultExamHistoryCount = HttpProxy.HttpPost(urlGetExamStatusCount, serviceJson, "application/json; charset=utf-8", "POST");
            List<ExamHistoryVM> examHistory = new List<ExamHistoryVM>();
            if (resultExamHistoryCount != null)
            {
                examHistory = JsonConvert.DeserializeObject<List<ExamHistoryVM>>(resultExamHistoryCount);
            }

            examHistory.RemoveAll(x => x.ExamType == "Spaced Repetition");
            return Json(examHistory);
        }

        // This Method Will Call the Web Service for getting the Exam Status Count
        public ActionResult GetExamStatusCount()
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            ServiceCallVM serviceCall = new ServiceCallVM
            {
                userId = user._userId,
            };
            string serviceJson = JsonConvert.SerializeObject(serviceCall);
            string urlGetExamStatusCount = PPSAPGlobalConstants.SiteWebAPIUrl + "dashboard/GetExamStatusCount";
            string resultExamHistoryCount = HttpProxy.HttpPost(urlGetExamStatusCount, serviceJson, "application/json; charset=utf-8", "POST");
            ExamStatusCountVM examStatusCount = new ExamStatusCountVM();
            examStatusCount = JsonConvert.DeserializeObject<ExamStatusCountVM>(resultExamHistoryCount);
            return Json(examStatusCount);
        }

        // This Method Will Call the Web Service for getting the Exam Score
        public ActionResult GetExamScore()
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            ServiceCallVM serviceCall = new ServiceCallVM
            {
                userId = user._userId,
            };
            string serviceJson = JsonConvert.SerializeObject(serviceCall);
            string urlGetExamScore = PPSAPGlobalConstants.SiteWebAPIUrl + "dashboard/GetExamScore";
            string resultExamScore = HttpProxy.HttpPost(urlGetExamScore, serviceJson, "application/json; charset=utf-8", "POST");
            List<ExamScoreDetailVM> examStatusCount = new List<ExamScoreDetailVM>();
            examStatusCount = JsonConvert.DeserializeObject<List<ExamScoreDetailVM>>(resultExamScore);
            return Json(examStatusCount);
        }

        public JsonResult GetChartDetail(bool scoreCompWithPeers = false, int examType = -1, DateTime? fromDate = null, DateTime? toDate = null)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            ChartParameterVM chartParameters = new ChartParameterVM
            {
                UserId = user._userId,
                ScoreCompWithPeers = scoreCompWithPeers,
                ExamType = examType,
                FromDate = fromDate,
                ToDate = toDate,
            };

            string serviceJson = JsonConvert.SerializeObject(chartParameters);
            string urlGetChartDetail = PPSAPGlobalConstants.SiteWebAPIUrl + "dashboard/GetChartDetail";
            string resultChartDetail = HttpProxy.HttpPost(urlGetChartDetail, serviceJson, "application/json; charset=utf-8", "POST");
            List<DashboardChartVM> examStatusCount = new List<DashboardChartVM>();

            examStatusCount = JsonConvert.DeserializeObject<List<DashboardChartVM>>(resultChartDetail);
            return Json(examStatusCount, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUserStatusIsFirst()
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            ServiceCallVM serviceCall = new ServiceCallVM
            {
                userId = user._userId,
            };
            string serviceJson = JsonConvert.SerializeObject(serviceCall);
            string urlGetsatatus = PPSAPGlobalConstants.SiteWebAPIUrl + "dashboard/GetUserStatusIsFirst";
            string resultUserStatus = HttpProxy.HttpPost(urlGetsatatus, serviceJson, "application/json; charset=utf-8", "POST");
            int userStatusIsFirst = JsonConvert.DeserializeObject<int>(resultUserStatus);
            return Json(userStatusIsFirst);
        }

        public ActionResult UpdateUserStatusIsFirst()
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            ServiceCallVM serviceCall = new ServiceCallVM
            {
                userId = user._userId,
            };
            string serviceJson = JsonConvert.SerializeObject(serviceCall);
            string urlGetsatatus = PPSAPGlobalConstants.SiteWebAPIUrl + "dashboard/UpdateUserStatusIsFirst";
            HttpProxy.HttpPost(urlGetsatatus, serviceJson, "application/json; charset=utf-8", "POST");
            return null;
        }

        public ActionResult GetUserOptInStatus()
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
            optInStatus = optInStatus == null ? "0" : optInStatus;
            return Json(optInStatus);
        }

        public ActionResult UpdateUserOptInStatus(string optIn)
        {
            if (optIn == "Y")
            {
                TempData["OptMessage"] = "Thank you for sharing your data.";
            }
            else
            {
                if (Request.UrlReferrer.AbsolutePath.Contains("ExamHistory"))
                {
                    TempData["OptMessage"] = "Thank you. To change your answer, click the 'Share data' link below.";
                }
                else
                {
                    TempData["OptMessage"] = "Thank you. To change your answer, click the 'Share data' link below.";
                }
            }

            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            ServiceCallVM serviceCall = new ServiceCallVM
            {
                userId = user._userId,
                OptIn = optIn,
            };
            string serviceJson = JsonConvert.SerializeObject(serviceCall);
            string urlGetsatatus = PPSAPGlobalConstants.SiteWebAPIUrl + "dashboard/UpdateUserOptIn";
            string resultStatus = HttpProxy.HttpPost(urlGetsatatus, serviceJson, "application/json; charset=utf-8", "POST");
            int userOptInStatus = JsonConvert.DeserializeObject<int>(resultStatus);
            return Json(userOptInStatus);
        }

    }
}