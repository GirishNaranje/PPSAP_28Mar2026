using System.Collections.Generic;
using PPSAP.Common;
using PPSAP.DAL;
using PPSAP.DTO;

namespace PPSAP.BAL
{
    public static class ExamHistoryBL
    {
        public static List<ExamHistoryDTO> ExamHistoryDetails(ExamHistoryDTO examhistory)
        {
            return ExamHistoryDAL.ExamHistoryDetails(examhistory);
        }

        public static void DeleteExamHistoryDetails(ExamHistoryDTO examhistory)
        {
            ExamHistoryDAL.DeleteExamHistoryDetails(examhistory.ExamId, examhistory.UserId);
        }

        public static PdfDetailsDataVM GetPdfDetails(ExamHistoryDTO examhistory)
        {
            return ExamHistoryDAL.GetPdfDetails(examhistory);
        }

        // Reset Exam
        public static int ResetExam(int userId)
        {
            return ExamHistoryDAL.ResetExam(userId);
        }
    }
}
