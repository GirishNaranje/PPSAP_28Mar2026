using System.Collections.Generic;
using System.Web.Http;
using PPSAP.Common;
using PPSAP.BAL;
using PPSAP.DTO;

namespace PPSAP.WebAPI.Controllers
{
    public class OptInReportsController : ApiController
    {
        [Route("api/OptInReports/OptInReports")]
        [HttpPost]
        public List<OptInReports> OptInReports(OptInReports optInReports)
        {
            return OptInReportBL.OptInReports(optInReports);
        }

        [Route("api/OptInReports/GetOptIn")]
        [HttpPost]
        public List<OptInReports> GetOptIn(OptInReports optInReports)
        {
            return OptInReportBL.GetOptIn(optInReports);
        }
    }
}