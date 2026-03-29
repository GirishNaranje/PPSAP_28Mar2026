using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PPSAP.Apps.Proxy;
using Newtonsoft.Json;
using PPSAP.Common;
using PPSAP.DTO;

namespace PPSAP.Apps.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //return RedirectToAction("Index","Login");
            return View("HomePage");
        }

        public ActionResult About()
        {
            /*ViewBag.Message = "Your application description page.";*/
            return View();
        }

        public ActionResult Contact()
        {
            /*ViewBag.Message = "Your contact page.";*/
            return View();
        }

        public ActionResult TermsofServices()
        {
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult GetAllUsers()
        {
            ViewBag.Message = "This Data coming from Database";

            //string serviceJson = JsonConvert.SerializeObject(serviceCall);
            string urlUserRenewalMesssage = PPSAPGlobalConstants.SiteWebAPIUrl + "User/PPSAP_GetAllUsers";
            string resultUserRenewalMesssage = HttpProxy.HttpPost(urlUserRenewalMesssage, "application/json; charset=utf-8", "POST");

            List<UserDataDTO> userdata = new List<UserDataDTO>();
            userdata = JsonConvert.DeserializeObject<List<UserDataDTO>>(resultUserRenewalMesssage);

            return View(userdata);
        }
    }
}