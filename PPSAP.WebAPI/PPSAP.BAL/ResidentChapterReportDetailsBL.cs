using System.Collections.Generic;
using PPSAP.Common;
using PPSAP.DAL;
using PPSAP.DTO;

namespace PPSAP.BAL
{
    public static class ResidentChapterReportDetailsBL
    {
        public static List<ResidentChapterReportDetailsDTO> ResidentChapterReportDetails(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            return ResidentChapterReportDetailsDAL.ResidentChapterReportDetails(reportChapterDetails);
        }

        public static List<ResidentChapterReportDetailsDTO> ResidentChartDetail(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            return ResidentChapterReportDetailsDAL.ResidentChapterReportDetails(reportChapterDetails);
        }
    }
}
