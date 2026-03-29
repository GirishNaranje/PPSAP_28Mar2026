using System.Collections.Generic;
using System.Web.Http;
using PPSAP.Common;
using PPSAP.BAL;
using PPSAP.DTO;

namespace PPSAP.WebAPI.Controllers
{
    public class LoginController : ApiController
    {
        [Route("api/login/ValidateUser")]
        [HttpPost]
        public List<UserDTO> ValidateUser(UserDTO user)
        {
            return UserBL.ValidateUser(user.UserEmail, user.Password);
        }

        [Route("api/login/UpdateUser")]
        [HttpPost]
        public int UpdateUsers(UserDTO user)
        {
            return UserBL.UpdateUser(user);
        }

        [Route("api/login/GetUserByRole")]
        [HttpPost]
        public string GetUserByRole(UserVM userName)
        {
            return UserBL.GetUserByRole(userName.UserName);
        }

        [Route("api/login/SignUp_CreateUser")]
        [HttpPost]
        public int SignUp_CreateUser(UserDTO newuser)
        {
            return UserBL.SignUp_CreateUser(newuser);
        }

        [Route("api/login/PasswordRecover")]
        [HttpPost]
        public List<UserDTO> PasswordRecover(UserDTO existuser)
        {
            return UserBL.PasswordRecover(existuser);
        }
    }
}