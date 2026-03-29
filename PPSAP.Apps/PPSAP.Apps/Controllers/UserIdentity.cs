using System;

namespace PPSAP.Apps.Controllers
{
    public class UserIdentity
    {
        public string _userName;
        public int _userId;
        public string _userEmail;

        public UserIdentity(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                _userEmail = null;
                _userName = null;
                _userId = -1;
            }
            else
            {
                string[] words = username.Split('|');
                _userEmail = words[0];
                _userName = words[1];
                _userId = Convert.ToInt32(words[2]);
            }
        }
    }
}