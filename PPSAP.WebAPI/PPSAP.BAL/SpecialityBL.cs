using System.Collections.Generic;
using PPSAP.Common;
using PPSAP.DAL;
using PPSAP.DTO;

namespace PPSAP.BAL
{
    public static class SpecialityBL
    {
        public static List<SubSpecialityDetailVM> GetSpecialityList(ExamDTO exam)
        {
            return SpecialityDAL.GetSpecialityList(exam.UserId);
        }
    }
}
