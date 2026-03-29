using System.Collections.Generic;
using System.Web.Http;
using PPSAP.BAL;
using PPSAP.DTO;

namespace PPSAP.WebAPI.Controllers
{
    public class AdminChapterReportController : ApiController
    {
        [Route("api/AdminChapterReport/AdminChapterReportDetails")]
        [HttpPost]
        public List<ResidentChapterReportDetailsDTO> AdminChapterReportDetails(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            return AdminChapterReportDetailsBL.AdminChapterReportDetails(reportChapterDetails);
        }

        [Route("api/AdminChapterReport/AdminChartDetail")]
        [HttpPost]
        public List<ResidentChapterReportDetailsDTO> AdminChartReportDetail(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            return AdminChapterReportDetailsBL.AdminChartDetail(reportChapterDetails);
        }
    }
}