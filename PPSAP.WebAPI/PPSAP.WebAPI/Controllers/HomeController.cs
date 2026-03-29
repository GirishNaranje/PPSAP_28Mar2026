using System.Collections.Generic;
using System.Web.Mvc;
using PPSAP.BAL;
using PPSAP.Common;
using PPSAP.DTO;
using PPSAP.WebAPI.ExceptionFilter;

namespace PPSAP.WebAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //ViewBag.Title = "Home Page";
            return View();
        }

        //[CustomExceptionFilter]
        /*[Route("api/Home/PPSAP_GetAllUsers")]
        [HttpGet]
        public List<UserDataDTO> PPSAP_GetAllUsers()
        {
            return UserBL.PPSAP_GetAllUsers();
        }*/
    }
}
