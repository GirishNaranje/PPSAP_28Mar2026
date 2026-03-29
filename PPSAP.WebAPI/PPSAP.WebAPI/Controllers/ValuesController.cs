using System.Collections.Generic;
using System.Web.Http;
using PPSAP.WebAPI.ExceptionFilter;

namespace PPSAP.WebAPI.Controllers
{
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [CustomExceptionFilter]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
