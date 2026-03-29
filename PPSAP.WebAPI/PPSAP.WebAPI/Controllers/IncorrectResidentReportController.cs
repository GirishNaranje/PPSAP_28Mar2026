using System.Collections.Generic;
using System.Web.Http;
using PPSAP.Common;
using PPSAP.BAL;
using PPSAP.DTO;

namespace PPSAP.WebAPI.Controllers
{
    public class IncorrectResidentReportController : ApiController
    {
        [Route("api/IncorrectResidentReport/IncorrectQuestionDetails")]
        [HttpPost]
        public List<QuestionDetails> IncorrectQuestionDetails(ReportsVM incorrectReportDetails)
        {
            return IncorrectQuestionDetailsBL.IncorrectQuestionDetails(incorrectReportDetails);
        }
    }
}