using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using PPSAP.BAL;
using PPSAP.Common;
using PPSAP.DTO;

namespace PPSAP.WebAPI.Controllers
{
    public class AdminDashboardController : ApiController
    {
        [Route("api/admindashboard/GetUserListView")]
        [HttpPost]
        [HttpGet]
        public List<UserDTO> GetUserList()
        {
            return UserBL.GetUserList().ToList();
        }
    }
}