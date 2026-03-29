using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;
using System.Web.Http;
using PPSAP.Common;
using PPSAP.BAL;
using PPSAP.DTO;

namespace PPSAP.WebAPI.Controllers
{
    public class UserViewController : ApiController
    {

        [Route("api/UserView/GetQuestionById")]
        [HttpPost]
        public QuestionDetails GetQuestionById(UpdateSkipAnswered question)
        {
            return UserViewBL.GetQuestionById(question.questionId, question.userId);
        }
    }
}