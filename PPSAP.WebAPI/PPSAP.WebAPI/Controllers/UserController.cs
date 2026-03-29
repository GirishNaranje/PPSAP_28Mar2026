using System.Collections.Generic;
using System.Web.Http;
using PPSAP.Common;
using PPSAP.BAL;
using PPSAP.DTO;

namespace PPSAP.WebAPI.Controllers
{
    public class UserController : ApiController
    {
        [Route("api/User/GetOrAddUser")]
        [HttpPost]
        public List<UserDataDTO> GetOrAddUser(UserJsonVM userDTO)
        {
            return UserBL.GetOrAddUser(userDTO);
        }

        [Route("api/User/PPSAP_GetAllUsers")]
        [HttpPost]
        public List<UserDataDTO> PPSAP_GetAllUsers()
        {
            return UserBL.PPSAP_GetAllUsers();
        }
    }
}