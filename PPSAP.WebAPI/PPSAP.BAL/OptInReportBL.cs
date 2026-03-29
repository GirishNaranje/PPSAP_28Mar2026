using System.Collections.Generic;
using PPSAP.Common;
using PPSAP.DAL;
using PPSAP.DTO;

namespace PPSAP.BAL
{
    public class OptInReportBL
    {
        public static List<OptInReports> OptInReports(OptInReports optInReports)
        {
            return OptInReportsDAL.OptInReports(optInReports);
        }

        public static List<OptInReports> GetOptIn(OptInReports optInReports)
        {
            return OptInReportsDAL.GetOptIn(optInReports);
        }
    }
}
