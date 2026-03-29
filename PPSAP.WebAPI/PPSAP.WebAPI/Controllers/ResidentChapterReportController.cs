using System.Collections.Generic;
using System.Web.Http;
using PPSAP.Common;
using PPSAP.BAL;
using PPSAP.DTO;

namespace PPSAP.WebAPI.Controllers
{
    public class ResidentChapterReportController : ApiController
    {
        [Route("api/ResidentChapterReport/ResidentChapterReportDetails")]
        [HttpPost]
        public List<ResidentChapterReportDetailsDTO> ResidentChapterReportDetails(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            return ResidentChapterReportDetailsBL.ResidentChapterReportDetails(reportChapterDetails);
        }

        [Route("api/ResidentChapterReport/ResidentChartDetail")]
        [HttpPost]
        public List<ResidentChapterReportDetailsDTO> ResidentChartDetail(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            return ResidentChapterReportDetailsBL.ResidentChartDetail(reportChapterDetails);
        }
    }
}