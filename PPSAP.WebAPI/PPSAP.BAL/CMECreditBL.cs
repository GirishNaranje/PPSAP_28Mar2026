using System.Collections.Generic;
using PPSAP.Common;
using PPSAP.DAL;
using PPSAP.DTO;

namespace PPSAP.BAL
{
    public static class CMECreditBL
    {
        public static List<CMECreditVM> GetCreditDetails(UserIdVM user)
        {
            return CMECreditDAL.GetCreditDetails(user);
        }
    }
}
