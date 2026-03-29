using System.Collections.Generic;
using System.Web.Http;
using PPSAP.Common;
using PPSAP.BAL;

namespace PPSAP.WebAPI.Controllers
{
    public class AggregatePerformanceController : ApiController
    {
        [Route("api/AggregatePerformance/ReportDetails")]
        [HttpPost]
        public List<ReportsDetailsVM> ReportDetails(ReportsDetailsVM reportDetails)
        {
            return ReportDetailsBL.ReportDetails(reportDetails);
        }
    }
}