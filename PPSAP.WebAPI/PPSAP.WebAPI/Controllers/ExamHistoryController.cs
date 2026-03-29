using System.Collections.Generic;
using System.Web.Http;
using PPSAP.Common;
using PPSAP.BAL;
using PPSAP.DTO;

namespace PPSAP.WebAPI.Controllers
{
    public class ExamHistoryController : ApiController
    {
        [Route("api/ExamHistory/ExamHistoryDetails")]
        [HttpPost]
        public List<ExamHistoryDTO> ExamHistoryDetails(ExamHistoryDTO examhistory)
        {
            return ExamHistoryBL.ExamHistoryDetails(examhistory);
        }

        [Route("api/ExamHistory/DeleteExamHistoryDetails")]
        [HttpPost]
        public void DeleteExamHistoryDetails(ExamHistoryDTO examhistory)
        {
            ExamHistoryBL.DeleteExamHistoryDetails(examhistory);
        }

        [Route("api/ExamHistory/GetPdfDetails")]
        [HttpPost]
        public PdfDetailsDataVM GetPdfDetails(ExamHistoryDTO examhistory)
        {
            return ExamHistoryBL.GetPdfDetails(examhistory);
        }

        [Route("api/ExamHistory/ResetExam")]
        [HttpGet]
        [HttpPost]
        public int ResetExam(ExamCountOnExamTypeVM examCount)
        {
            return ExamHistoryBL.ResetExam(examCount.UserId);
        }
    }
}