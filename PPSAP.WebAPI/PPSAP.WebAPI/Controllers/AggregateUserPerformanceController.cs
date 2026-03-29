using PPSAP.Common;
using PPSAP.BAL;
using System.Collections.Generic;
using System.Web.Http;

namespace PPSAP.WebAPI.Controllers
{
    public class AggregateUserPerformanceController : ApiController
    {
        [Route("api/AggregateUserPerformance/AdminReportDetails")]
        [HttpPost]
        public List<AdminReportVM> AdminReportDetails(AdminReportVM adminreportDetails)
        {
            return AdminReportBL.AdminReportDetails(adminreportDetails);
        }

        [Route("api/AggregateUserPerformance/GetPYGYear")]
        [HttpPost]
        public List<AdminReportVM> GetPYGYear(AdminReportVM pgyDetails)
        {
            return AdminReportBL.GetPYGYear(pgyDetails);
        }

        [Route("api/AggregateUserPerformance/AdminReportDetailsExport")]
        [HttpPost]
        public List<AdminReportVM> AdminReportDetailsExport(AdminReportVM adminreportDetails)
        {
            return AdminReportBL.AdminReportDetailsExport(adminreportDetails);
        }
    }
}