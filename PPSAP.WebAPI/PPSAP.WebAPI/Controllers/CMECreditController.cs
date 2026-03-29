using System.Collections.Generic;
using System.Web.Http;
using PPSAP.Common;
using PPSAP.BAL;
using PPSAP.DTO;

namespace PPSAP.WebAPI.Controllers
{
    public class CMECreditController : ApiController
    {
        [Route("api/CMECredit/GetCreditDetails")]
        [HttpPost]
        public List<CMECreditVM> GetCreditDetails(UserIdVM user)
        {
            return CMECreditBL.GetCreditDetails(user);
        }
    }
}