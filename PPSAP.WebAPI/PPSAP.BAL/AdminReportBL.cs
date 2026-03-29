using System.Collections.Generic;
using PPSAP.Common;
using PPSAP.DAL;
using PPSAP.DTO;

namespace PPSAP.BAL
{
    public static class AdminReportBL
    {
        public static List<AdminReportVM> AdminReportDetails(AdminReportVM adminreportDetails)
        {
            return AdminReportDAL.AdminReportDetails(adminreportDetails);
        }

        public static List<AdminReportVM> GetPYGYear(AdminReportVM pgyDetails)
        {
            return AdminReportDAL.GetPYGYear(pgyDetails);
        }

        public static List<AdminReportVM> AdminReportDetailsExport(AdminReportVM adminreportDetails)
        {
            return AdminReportDAL.AdminReportDetailsExport(adminreportDetails);
        }
    }
}
