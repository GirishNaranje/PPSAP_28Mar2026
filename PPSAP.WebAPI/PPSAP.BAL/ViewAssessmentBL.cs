using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPSAP.Common;
using PPSAP.DAL;
using PPSAP.DTO;

namespace PPSAP.BAL
{
    public static class ViewAssessmentBL
    {
        public static List<QuestionDetails> ViewAssessmentDetails(ExamQuestionDTO examque)
        {
            List<QuestionDetails> examQuestionDetailsList = new List<QuestionDetails>();
            List<int> questionList = new List<int>();
            questionList = ViewAssessmentDAL.ViewAssessmentDetails(Convert.ToInt32(examque.ExamId));
            foreach (int item in questionList)
            {
                QuestionDetails examQuestionDetails = new QuestionDetails();
                examQuestionDetails = AssessmentBL.GetExamQuestion((Convert.ToInt32(examque.ExamId)), item, Convert.ToInt32(examque.UserId));
                examQuestionDetailsList.Add(examQuestionDetails);
            }

            return examQuestionDetailsList;
        }

        public static List<QuestionDetails> FilterByQuestions(AssesmentDetailVM assDetail)
        {
            List<QuestionDetails> examQuestionDetailsList = new List<QuestionDetails>();
            List<QuestionIdWithCountVM> questionList = new List<QuestionIdWithCountVM>();
            questionList = ViewAssessmentDAL.FilterByQuestions(Convert.ToInt32(assDetail.ExamId), Convert.ToString(assDetail.Filter), assDetail.NoOfRecords, assDetail.PageNo);
            foreach (QuestionIdWithCountVM item in questionList)
            {
                QuestionDetails examQuestionDetails = new QuestionDetails();
                examQuestionDetails = AssessmentBL.GetExamQuestion((Convert.ToInt32(assDetail.ExamId)), item.QuestionId, Convert.ToInt32(assDetail.UserId));
                examQuestionDetails.QuestionCount = item.QuestionIdCount;
                examQuestionDetails.serialNumber = item.QuestionNo;
                examQuestionDetailsList.Add(examQuestionDetails);
            }

            return examQuestionDetailsList;
        }

        public static List<QuestionDetails> ViewDetails(ExamQuestionDTO examque)
        {
            List<QuestionDetails> examQuestionDetailsList = new List<QuestionDetails>();
            List<int> questionList = new List<int>();
            questionList = ViewAssessmentDAL.ViewDetails(Convert.ToInt32(examque.ExamId));
            foreach (int item in questionList)
            {
                QuestionDetails examQuestionDetails = new QuestionDetails();
                examQuestionDetails = AssessmentBL.GetExamQuestion((Convert.ToInt32(examque.ExamId)), item, Convert.ToInt32(examque.UserId));
                examQuestionDetailsList.Add(examQuestionDetails);
            }

            return examQuestionDetailsList;
        }

        public static List<QuestionDetails> SearchByQuestions(AssesmentDetailVM assDetail)
        {
            List<QuestionDetails> examQuestionDetailsList = new List<QuestionDetails>();
            List<QuestionIdWithExamId> questionList = new List<QuestionIdWithExamId>();
            questionList = ViewAssessmentDAL.SearchByQuestions(Convert.ToString(assDetail.UserId), Convert.ToString(assDetail.SearchTerm), Convert.ToString(assDetail.Filter), assDetail.NoOfRecords, assDetail.PageNo);

            foreach (QuestionIdWithExamId item in questionList)
            {
                QuestionDetails examQuestionDetails = new QuestionDetails(); //GetExamQuestion_Search GetExamQuestion_Search1
                examQuestionDetails = AssessmentBL.GetExamQuestion_Search((Convert.ToInt32(item.ExamId)), item.QuestionId, Convert.ToInt32(assDetail.UserId));
                examQuestionDetails.QuestionCount = item.QuestionIdCount;
                examQuestionDetails.serialNumber = item.QuestionNo;
                examQuestionDetailsList.Add(examQuestionDetails);
            }

            return examQuestionDetailsList;

            /*Parallel.ForEach(questionList, (item) =>
            {
                QuestionDetails examQuestionDetails = new QuestionDetails();
                examQuestionDetails = AssessmentBL.GetExamQuestion_Search((Convert.ToInt32(item.ExamId)), item.QuestionId, Convert.ToInt32(assDetail.UserId));
                examQuestionDetails.QuestionCount = item.QuestionIdCount;
                examQuestionDetails.serialNumber = item.QuestionNo;
                examQuestionDetailsList.Add(examQuestionDetails);
            });

            return examQuestionDetailsList.OrderBy(x => x.serialNumber).ToList();*/
        }
    }
}
