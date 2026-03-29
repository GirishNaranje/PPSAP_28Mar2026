using System.Collections.Generic;
using System.Web.Http;
using PPSAP.Common;
using PPSAP.BAL;
using PPSAP.DTO;

namespace PPSAP.WebAPI.Controllers
{
    public class IncorrectAdminReportController : ApiController
    {
        [Route("api/IncorrectAdminReport/AdminIncorrectQuestionDetails")]
        [HttpPost]
        public List<QuestionDetails> AdminIncorrectQuestionDetails(AdminReportVM incorrectReportDetails)
        {
            return AdminIncorrectQuestionDetailsBL.AdminIncorrectQuestionDetails(incorrectReportDetails);
        }
    }
}