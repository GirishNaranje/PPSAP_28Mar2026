using System.Collections.Generic;
using PPSAP.DAL;
using PPSAP.DTO;

namespace PPSAP.BAL
{
    public class AdminChapterReportDetailsBL
    {
        public static List<ResidentChapterReportDetailsDTO> AdminChapterReportDetails(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            return AdminChapterReportDetailsDAL.AdminChapterReportDetails(reportChapterDetails);
        }

        public static List<ResidentChapterReportDetailsDTO> AdminChartDetail(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            return AdminChapterReportDetailsDAL.AdminChapterReportDetails(reportChapterDetails);
        }
    }
}
