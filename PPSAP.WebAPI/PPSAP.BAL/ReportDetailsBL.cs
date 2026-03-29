using System.Collections.Generic;
using PPSAP.Common;
using PPSAP.DAL;
using PPSAP.DTO;

namespace PPSAP.BAL
{
    public static class ReportDetailsBL
    {
        public static List<ReportsDetailsVM> ReportDetails(ReportsDetailsVM reportDetails)
        {
            return ReportDetailsDAL.ReportDetails(reportDetails);
        }
    }
}
