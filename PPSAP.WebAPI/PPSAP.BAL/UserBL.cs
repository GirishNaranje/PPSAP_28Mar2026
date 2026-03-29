using System.Collections.Generic;
using PPSAP.Common;
using PPSAP.DAL;
using PPSAP.DTO;
using System;

namespace PPSAP.BAL
{
    public class UserBL
    {
        public static List<UserDTO> GetUserDetails(int userId)
        {
            return UserDAL.GetByUserID(userId);
        }

        public static List<UserDTO> ValidateUser(string userName, string password)
        {
            return UserDAL.ValidateUser(userName, password);
        }

        public static string GetUserByRole(string userName)
        {
            return UserDAL.GetUserByRole(userName);
        }

        public static int CreateUser(UserDTO objUser)
        {
            return UserDAL.CreateUser(objUser);
        }

        public static List<UserDTO> GetUserList()
        {
            return UserDAL.GetUsers();
        }

        public static int UpdateUser(UserDTO objUser)
        {
            return UserDAL.UpdateUsers(objUser);
        }

        public static List<UserDataDTO> GetOrAddUser(UserJsonVM objUser)
        {
            return UserDAL.GetOrAddUser(objUser);
        }

        public static List<UserDataDTO> GetRenewal(ServiceCallVM userService)
        {
            return UserDAL.GetByUserData(Convert.ToInt32(userService.userId));
        }

        public static List<UserDataDTO> PPSAP_GetAllUsers()
        {
            return UserDAL.PPSAP_GetAllUsers();
        }

        public static int SignUp_CreateUser(UserDTO newuser)
        {
            if(newuser.Role == "Admin")
            {
                newuser.Role = "A";
                newuser.isLoggedFirst = false;
                newuser.IsActive = true;
            }
            else
            {
                newuser.Role = "U";
                newuser.isLoggedFirst = false;
                newuser.IsActive = true;
            }
            return UserDAL.SignUp_CreateUser(newuser);
        }

        public static List<UserDTO> PasswordRecover(UserDTO existuser)
        {
            return UserDAL.PasswordRecover(existuser);
        }
    }
}
