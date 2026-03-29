using System.Collections.Generic;
using PPSAP.Common;
using PPSAP.DAL;
using PPSAP.DTO;

namespace PPSAP.BAL
{
    public class UtilizationReportBL
    {
        public static List<UtilizationReportsVM> UtilizationReports(UtilizationReportsVM utilizationReports)
        {
            return UtilizationReportsDAL.UtilizationReports(utilizationReports);
        }

        public static UtilizationReportsVM AtAGlance(UtilizationReportsVM utilizationReports)
        {
            return UtilizationReportsDAL.AtAGlance(utilizationReports);
        }
    }
}
