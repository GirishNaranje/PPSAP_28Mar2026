using System.Collections.Generic;
using System.Web.Mvc;
using PPSAP.Apps.Proxy;
using PPSAP.Common;
using Newtonsoft.Json;

namespace PPSAP.Apps.Controllers
{
    [Authorize]
    public class CMECreditController : Controller
    {
        // GET: CMECredit
        public ActionResult CMEIndexPage()
        {
            return View();
        }

        public ActionResult GetCMECredit()
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            UserIdVM userVM = new UserIdVM
            {
                UserId = user._userId,
            };
            string cmePostDataJson = JsonConvert.SerializeObject(userVM);
            string url = PPSAPGlobalConstants.SiteWebAPIUrl + "CMECredit/GetCreditDetails";
            string result = HttpProxy.HttpPost(url, cmePostDataJson, "application/json; charset=utf-8", "POST");
            List<CMECreditVM> cmeReport = new List<CMECreditVM>();
            cmeReport = JsonConvert.DeserializeObject<List<CMECreditVM>>(result);
            return Json(cmeReport);
        }
    }
}