using PPSAP.Common;
using PPSAP.BAL;
using PPSAP.DTO;
using System.Collections.Generic;
using System.Web.Http;

namespace PPSAP.WebAPI.Controllers
{
    public class UtilizationReportsController : ApiController
    {
        [Route("api/UtilizationReports/UtilizationReports")]
        [HttpPost]
        public List<UtilizationReportsVM> UtilizationReports(UtilizationReportsVM utilizationReports)
        {
            return UtilizationReportBL.UtilizationReports(utilizationReports);
        }

        [Route("api/UtilizationReports/AtAGlance")]
        [HttpPost]
        public UtilizationReportsVM AtAGlance(UtilizationReportsVM utilizationReports)
        {
            return UtilizationReportBL.AtAGlance(utilizationReports);
        }
    }
}