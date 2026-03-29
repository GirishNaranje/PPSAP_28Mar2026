using System.Collections.Generic;
using PPSAP.Common;
using PPSAP.DAL;
using PPSAP.DTO;

namespace PPSAP.BAL
{
    public static class IncorrectQuestionDetailsBL
    {
        public static List<QuestionDetails> IncorrectQuestionDetails(ReportsVM incorrectReportDetails)
        {
            List<QuestionDetails> reportQuestionDetailsList = new List<QuestionDetails>();
            List<IncorrectQuestionDetailsDTO> questionList = new List<IncorrectQuestionDetailsDTO>();
            questionList = IncorrectQuestionDetailsDAL.IncorrectQuestionDetails(incorrectReportDetails.UserId, incorrectReportDetails.SubspecialtyId, incorrectReportDetails.ExamStartDate, incorrectReportDetails.ExamCompletedDate, incorrectReportDetails.NoOfRecords, incorrectReportDetails.PageNo, incorrectReportDetails.Year);
            foreach (var item in questionList)
            {
                item.UserId = incorrectReportDetails.UserId;
                QuestionDetails reportQuestionDetails = new QuestionDetails();

                if (item.ExamType == 3)
                {
                    reportQuestionDetails = AssessmentBL.GetExamQuestionForSR(item.ExamId, item.QuestionId, item.UserId);
                }
                else
                {
                    reportQuestionDetails = AssessmentBL.GetExamQuestion(item.ExamId, item.QuestionId, item.UserId);
                }

                reportQuestionDetails.QuestionCount = item.QuestionIdCount;
                reportQuestionDetails.serialNumber = item.Rownumber;
                reportQuestionDetails.SubSpeciality = item.Subspecialty;
                reportQuestionDetails.Section = item.Section;
                reportQuestionDetailsList.Add(reportQuestionDetails);
            }

            return reportQuestionDetailsList;
        }
    }
}
