using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PPSAP.DTO;
using PPSAP.Common;
using PPSAP.Apps.Proxy;
using Newtonsoft.Json;
using System.Web.Security;
//using PPSAP.Apps.EmailService;
using System.Net.Mail;
using System.Net;
using System.Security;

namespace PPSAP.Apps.Controllers
{
    public class LoginController : Controller
    {
        /*private readonly EmailService _emailService;

        public LoginController()
        {
            // Initialize the EmailService with your SendGrid API key
            _emailService = new EmailService("your-sendgrid-api-key");
        }*/

        public ActionResult Index()
        {
            return View("SignIn");
        }

        public ActionResult SignIn(UserDTO user)
        {
            List<UserDTO> lstUser = new List<UserDTO>();
            string userJson = JsonConvert.SerializeObject(user);
            string urlUserData = PPSAPGlobalConstants.SiteWebAPIUrl + "login/ValidateUser";
            string resultUserData = HttpProxy.HttpPost(urlUserData, userJson, "application/json; charset=utf-8", "POST");
            lstUser = JsonConvert.DeserializeObject<List<UserDTO>>(resultUserData);

            if (lstUser.Count > 0)
            {
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                      1,
                      lstUser[0].UserEmail + "|" + lstUser[0].FirstName + ' ' + lstUser[0].LastName + "|" + lstUser[0].UserId,
                      DateTime.Now,
                      DateTime.Now.Add(FormsAuthentication.Timeout),
                      false,
                      lstUser[0].Role);

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);

                System.Web.HttpContext.Current.Session["UserID"] = lstUser[0].UserId;
                UserIdentity user1 = new UserIdentity(authTicket.Name);
                string bSCCUserName = user1._userName;
                System.Web.HttpContext.Current.Session["UserNameFullName"] = bSCCUserName;

                // Set ViewBag to pass user role to _Layout.cshtml
                ViewBag.UserRole = lstUser[0].Role;

                // Set the value of ViewBag.UserRole in a cookie
                var userRoleCookie = new HttpCookie("UserRole", lstUser[0].Role);
                Response.Cookies.Add(userRoleCookie);

                string a = System.Web.HttpContext.Current.User.Identity.Name;
                bool a1 = System.Web.HttpContext.Current.User.IsInRole("U");
                bool a2 = System.Web.HttpContext.Current.User.IsInRole("A");

                if (lstUser[0].Role == "A")
                {
                    TempData["Msg"] = "You are successfully logged in as Admin";
                    return RedirectToAction("Index", "AllQuestions");
                }
                else if (lstUser[0].Role == "U")
                {
                    TempData["Msg"] = "You are successfully logged in as User";
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                TempData["Msg"] = "Login Failed. Email or Password mismatch";
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult SignUp_CreateUser(UserDTO newuser)
        {
            int createUserResult = 0;
            string newuserJson = JsonConvert.SerializeObject(newuser);
            string urlUserData = PPSAPGlobalConstants.SiteWebAPIUrl + "login/SignUp_CreateUser";
            string resultUserData = HttpProxy.HttpPost(urlUserData, newuserJson, "application/json; charset=utf-8", "POST");

            // Assuming the resultUserData contains an integer representing the status of user creation
            if (int.TryParse(resultUserData, out createUserResult))
            {
                if (createUserResult > 0)
                {
                    TempData["Msg"] = "User Created Successfully";
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    TempData["Msg"] = "Email exists, please login or click on forget password";
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                // Handle error case when resultUserData couldn't be parsed as an integer
                TempData["Msg"] = "An error occurred while processing your request.";
                return View();
            }
        }

        public ActionResult Forgotpassword()
        {
            return View();
        }

        public ActionResult PasswordRecover(UserDTO existuser)
        {
            List<UserDTO> lstUser = new List<UserDTO>();
            string userJson = JsonConvert.SerializeObject(existuser);
            string urlUserData = PPSAPGlobalConstants.SiteWebAPIUrl + "login/PasswordRecover";
            string resultUserData = HttpProxy.HttpPost(urlUserData, userJson, "application/json; charset=utf-8", "POST");
            lstUser = JsonConvert.DeserializeObject<List<UserDTO>>(resultUserData);
            //SendEmail(lstUser);
            if (lstUser.Count > 0)
            {
                foreach(var user in lstUser)
                {
                    if(user.Role == "U")
                    {
                        user.Role = "User";
                    }
                    else
                    {
                        user.Role = "Admin";
                    }
                }

                SendEmail(lstUser);
                TempData["Msg"] = "User Details sent to your email address.";
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["Msg"] = "Email not found. You can create an account";
                return RedirectToAction("SignUp", "Login");
            }
        }

        public ActionResult SignOut()
        {
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Clear();
            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
            return RedirectToAction("Index", "Home");
        }

        //For Timeout check.
        [HttpPost]
        public JsonResult KeepSessionAlive()
        {
            return new JsonResult
            {
                Data = "Beat Generated"
            };
        }

        public ActionResult SendEmail(List<UserDTO> userdetails)
        {
            string fromMail = "member.services2024@gmail.com";
            string fromPassword = "mqqanpxjleudlhbg";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "PPSAP - Password Recover";
            message.To.Add(new MailAddress(userdetails[0].UserEmail));

            // Construct the HTML body dynamically using user details
            string htmlBody = "<html><body>";
            htmlBody += "<h3><b>User Details</b></h3><br/>";
            foreach (var user in userdetails)
            {
                    /*if (user.Role == "U")
                    {
                        user.Role = "User";
                    }
                    else
                    {
                        user.Role = "Admin";
                    }*/

                htmlBody += "<p>UserID: " + user.UserId + "</p>";
                htmlBody += "<p>First Name: " + user.FirstName + "</p>";
                htmlBody += "<p>Last Name: " + user.LastName + "</p>";
                htmlBody += "<p>Email: " + user.UserEmail + "</p>";
                htmlBody += "<p>Password: " + user.Password + "</p>";
                htmlBody += "<p>Role: " + user.Role + "</p>";
            }

            htmlBody += "</body></html>";

            message.Body = htmlBody;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);

            return RedirectToAction("Index", "Login");
        }
    }
}