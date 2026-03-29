using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PPSAP.Apps.Proxy;
using PPSAP.Common;
using PPSAP.DTO;
using Newtonsoft.Json;
using Serilog;
using System.Web.Script.Serialization;

namespace PPSAP.Apps.Controllers
{
    public class UserController : Controller
    {
        public List<UserDataDTO> GetUserData(string cookievalue, string path, ActionExecutingContext filterContext)
        {
            var log = new LoggerConfiguration().WriteTo.RollingFile(path, shared: true, retainedFileCountLimit: 7).CreateLogger();

            log.Information("Start: Controller:UserController, Method:GetUserData " + Convert.ToString(DateTime.Now));
            UserJsonVM userJsondata = new UserJsonVM();
            List<UserDataDTO> lstUser = new List<UserDataDTO>();
            userJsondata = GetUserDataSSO(cookievalue, path);

            if (userJsondata != null)
            {
                try
                {
                    string userJson = JsonConvert.SerializeObject(userJsondata);
                    string urlUserData = PPSAPGlobalConstants.SiteWebAPIUrl + "User/GetOrAddUser";
                    string resultUserData = HttpProxy.HttpPost(urlUserData, userJson, "application/json; charset=utf-8", "POST");
                    lstUser = JsonConvert.DeserializeObject<List<UserDataDTO>>(resultUserData);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }

                if (lstUser.Count > 0)
                {
                    if (System.Web.HttpContext.Current.User.Identity.Name == string.Empty)
                    {
                        // FormsAuthentication.SetAuthCookie(lstUser[0].Email + "|" + lstUser[0].FirstName + ' ' + lstUser[0].LastName + "|" + lstUser[0].UserId, true);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                          1, // version
                          lstUser[0].Email + "|" + lstUser[0].FirstName + ' ' + lstUser[0].LastName + "|" + lstUser[0].UserId,           // user name
                          DateTime.Now, // created
                          DateTime.Now.Add(FormsAuthentication.Timeout),   // expires // DateTime.Now.AddMinutes(1),  //
                          false, // rememberMe?
                          lstUser[0].Role); // can be used to store roles

                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                        HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                        //HttpCookie authCookie = new HttpCookie("UserCookiesSession", encryptedTicket);
                        System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
                    }

                    // var s = System.Web.HttpContext.Current.User.Identity.Name;
                    System.Web.HttpContext.Current.Session["UserID"] = (lstUser[0].UserId);
                    UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                    string bSCCUserName = user._userName;
                    System.Web.HttpContext.Current.Session["UserNameFullName"] = bSCCUserName;
                    TempData["Msg"] = "You are successfully logged in  ";
                }

                log.Information("End: Controller:UserController, Method:GetUserData " + Convert.ToString(DateTime.Now));
                return lstUser;
            }
            else
            {
                log.Information("Start: Controller:UserController, Method:GetUserData " + Convert.ToString(DateTime.Now));
                return null;
            }
        }

        // SSO Code Implmentation
        public ActionResult RedirectToDashaord()
        {
            bool userRoleCheck = System.Web.HttpContext.Current.User.IsInRole("A");
            if (userRoleCheck)
            {
                return RedirectToAction("Index", "AdminDashboard");
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }

        public UserJsonVM GetUserDataSSO(string cookievalue, string path)
        {
            string ssoApiUrl = ConfigurationManager.AppSettings["SSO_API_URL"];
            var log = new LoggerConfiguration().WriteTo.RollingFile(path, shared: true, retainedFileCountLimit: 7).CreateLogger();
            UserJsonVM userData = new UserJsonVM();
            int cookieExpiry = Convert.ToInt32(ConfigurationManager.AppSettings["CookieExpiry"]);
            try
            {
                //Test User AAO12@aao.org
                /*userData.MasterCustomerId = "000001010532";
                userData.FirstName = "Education";
                userData.LastName = "Division";
                userData.PrimaryEmail = "AAO12@aao.org";

                userData.CommunicationsEmail = "AAO12@aao.org";
                userData.ResidencyStart = null;
                userData.ResidencyEnd = null;
                userData.ResidencyProgramName = "San Antonio Uniformed Svcs Hlth Ed Consortium: Ophthalmology";//null;
                userData.ResidencyProgramId = null;
                userData.ProgramDirectorProgramName = null;
                userData.ProgramDirectorProgramId = null;
                userData.MasterCustomerId = "000001010532";
                userData.RenewalLink = "";
                userData.RenewalMessage = "Your subscription expires on Sunday, February 28, 2021.";*/

                //Test User AAO05@aao.org
                userData.MasterCustomerId = "000001854037";
                userData.FirstName = "Education";
                userData.LastName = "Division Test 5";
                userData.PrimaryEmail = "AAO05@aao.org";

                userData.CommunicationsEmail = "AAO05@aao.org";
                userData.ResidencyStart = null;
                userData.ResidencyEnd = null;
                userData.ResidencyProgramName = null;
                userData.ResidencyProgramId = null;
                userData.ProgramDirectorProgramName = null;
                userData.ProgramDirectorProgramId = null;
                userData.MasterCustomerId = "000001854037";
                userData.RenewalLink = "";
                userData.RenewalMessage = "Your subscription expires on Thursday, August 31, 2023.";

                //Test User AAO01@aao.org
                /*userData.MasterCustomerId = "000001010531";
                userData.FirstName = "Education";
                userData.LastName = "Division";
                userData.PrimaryEmail = "AAO01@aao.org";

                userData.CommunicationsEmail = "AAO01@aao.org";
                userData.ResidencyStart = null;
                userData.ResidencyEnd = null;
                userData.ResidencyProgramName = null;
                userData.ResidencyProgramId = null;
                userData.ProgramDirectorProgramName = null;
                userData.ProgramDirectorProgramId = null;
                userData.MasterCustomerId = "000001010531";
                userData.RenewalLink = "";
                userData.RenewalMessage = "Your subscription expires on Thursday, August 31, 2023.";*/

                //Test User AAO3P@aao.org  -- for HG-459
                /*userData.MasterCustomerId = "000001010529";
                userData.FirstName = "Education";
                userData.LastName = "Division";
                userData.PrimaryEmail = "AAO3P@aao.org";

                userData.CommunicationsEmail = "AAO3P@aao.org";
                userData.ResidencyStart = null;
                userData.ResidencyEnd = null;
                userData.ResidencyProgramName = "Educational Institution";//null;
                userData.ResidencyProgramId = null;
                userData.ProgramDirectorProgramName = null;
                userData.ProgramDirectorProgramId = null;
                userData.MasterCustomerId = "000001010529";
                userData.RenewalLink = "";
                userData.RenewalMessage = "";*/

                //User erickmrg@gmail.com - SR error
                /*userData.MasterCustomerId = "000001729519";
                userData.FirstName = "Erick";
                userData.LastName = "Rivera-Grana";
                userData.PrimaryEmail = "erickmrg@gmail.com";

                userData.CommunicationsEmail = "erickmrg@gmail.com";
                userData.ResidencyStart = null;
                userData.ResidencyEnd = null;
                userData.ResidencyProgramName = "San Antonio Uniformed Svcs Hlth Ed Consortium: Ophthalmology";//null;
                userData.ResidencyProgramId = null;
                userData.ProgramDirectorProgramName = null;
                userData.ProgramDirectorProgramId = null;
                userData.RenewalLink = "";
                userData.RenewalMessage = null;*/

                //User lucy.ma.28@gmail.com - resume exam error
                /*userData.MasterCustomerId = "000001746449";
                userData.FirstName = "Lucy";
                userData.LastName = "Ma";
                userData.PrimaryEmail = "lucy.ma.28@gmail.com";

                userData.CommunicationsEmail = "lucy.ma.28@gmail.com";
                userData.ResidencyStart = null;
                userData.ResidencyEnd = null;
                userData.ResidencyProgramName = null;
                userData.ResidencyProgramId = null;
                userData.ProgramDirectorProgramName = null;
                userData.ProgramDirectorProgramId = null;
                userData.RenewalLink = "";
                userData.RenewalMessage = null;*/


                //Test User aao01@aao.org
                /*userData.MasterCustomerId = "000001010531";
                userData.FirstName = "Education";
                userData.LastName = "Division";
                userData.PrimaryEmail = "aao01@aao.org";

                userData.CommunicationsEmail = "aao01@aao.org";
                userData.ResidencyStart = null;
                userData.ResidencyEnd = null;
                userData.ResidencyProgramName = null; //"San Antonio Uniformed Svcs Hlth Ed Consortium: Ophthalmology";//null;
                userData.ResidencyProgramId = null;
                userData.ProgramDirectorProgramName = null;
                userData.ProgramDirectorProgramId = null;
                userData.MasterCustomerId = "000001010531";
                userData.RenewalLink = "";
                userData.RenewalMessage = null;*/
                /*
                //Test User
                *//*userData.MasterCustomerId = "000001010532";
                userData.FirstName = "Education";
                userData.LastName = "Division";
                userData.PrimaryEmail = "AAO12@aao.org";

                userData.CommunicationsEmail = "AAO12@aao.org";
                userData.ResidencyStart = null;
                userData.ResidencyEnd = null;
                userData.ResidencyProgramName = "San Antonio Uniformed Svcs Hlth Ed Consortium: Ophthalmology";//null;
                userData.ResidencyProgramId = null;
                userData.ProgramDirectorProgramName = null;
                userData.ProgramDirectorProgramId = null;
                userData.MasterCustomerId = "000001010532";
                userData.RenewalLink = "";
                userData.RenewalMessage = "Your subscription expires on Sunday, February 28, 2021.";*//*

                //Sailaja Bondalapati User
                *//*userData.MasterCustomerId = "000001557440";
                userData.FirstName = "Sailaja";
                userData.LastName = "Bondalapati";
                userData.PrimaryEmail = "bondalapati29@gmail.com";

                userData.CommunicationsEmail = "bondalapati29@gmail.com";
                userData.ResidencyStart = null;
                userData.ResidencyEnd = null;
                userData.ResidencyProgramName = null;
                userData.ResidencyProgramId = null;
                userData.ProgramDirectorProgramName = null;
                userData.ProgramDirectorProgramId = null;
                userData.MasterCustomerId = "000001557440";
                userData.RenewalLink = null;
                userData.RenewalMessage = null;*//*

                //Manisha User
                *//*userData.MasterCustomerId = "000001747595";
                userData.FirstName = "Manisha";
                userData.LastName = "Miller";
                userData.PrimaryEmail = "manishamillermd@gmail.com";

                userData.CommunicationsEmail = "manishamillermd@gmail.com";
                userData.ResidencyStart = null;
                userData.ResidencyEnd = null;
                userData.ResidencyProgramName = "San Antonio Uniformed Svcs Hlth Ed Consortium: Ophthalmology";//null;
                userData.ResidencyProgramId = null;
                userData.ProgramDirectorProgramName = null;
                userData.ProgramDirectorProgramId = null;
                userData.MasterCustomerId = "000001747595";
                userData.RenewalLink = "";
                userData.RenewalMessage = "Your subscription expires on Sunday, February 28, 2021.";*//*

                //Frederick User
                *//*userData.MasterCustomerId = "000001563718";
                userData.FirstName = "Frederick";
                userData.LastName = "Blodi";
                userData.PrimaryEmail = "frederick.blodi@gmail.com";

                userData.CommunicationsEmail = "frederick.blodi@gmail.com";
                userData.ResidencyStart = null;
                userData.ResidencyEnd = null;
                userData.ResidencyProgramName = "San Antonio Uniformed Svcs Hlth Ed Consortium: Ophthalmology";//null;
                userData.ResidencyProgramId = null;
                userData.ProgramDirectorProgramName = null;
                userData.ProgramDirectorProgramId = null;
                userData.MasterCustomerId = "000001563718";
                userData.RenewalLink = "";
                userData.RenewalMessage = "Your subscription expires on Sunday, February 28, 2021.";*//*
                */
                HttpCookie cookie = new HttpCookie("BCSC_AAO_MasterCustomerId", userData.MasterCustomerId);
                cookie.Expires = DateTime.Now.AddDays(cookieExpiry);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                HttpCookie redirectCookie = new HttpCookie("BCSC_AAO_MasterCustomerId_store_redirect", "0");

                redirectCookie.Expires = DateTime.Now.AddDays(cookieExpiry);
                System.Web.HttpContext.Current.Response.Cookies.Add(redirectCookie);
                return userData;

/*
                Uri myUri = new Uri(ssoApiUrl);
                WebRequest myWebRequest = HttpWebRequest.Create(ssoApiUrl);
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)myWebRequest;
                NetworkCredential myNetworkCredential = new NetworkCredential("aaoMaster", cookievalue);
                CredentialCache myCredentialCache = new CredentialCache();
                myCredentialCache.Add(myUri, "Basic", myNetworkCredential);
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Credentials = myCredentialCache;
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                Stream responseStream = myHttpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);
                string pageContent = myStreamReader.ReadToEnd();
                responseStream.Close();
                myHttpWebResponse.Close();
                log.Information("Response Text by  WebRequest :" + pageContent);
                log.Information("Response Text by  WebRequest :" + myHttpWebResponse.StatusCode);
                if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    var userJsondata = UserJson.FromJson(pageContent);
                    userData.MasterCustomerId = userJsondata.MasterCustomerId;
                    userData.FirstName = userJsondata.FirstName;
                    userData.LastName = userJsondata.LastName;
                    if (userJsondata.PrimaryEmail == string.Empty)
                    {
                        userData.PrimaryEmail = userJsondata.CommunicationsEmail;
                    }
                    else
                    {
                        userData.PrimaryEmail = userJsondata.PrimaryEmail;
                    }

                    userData.PrimaryEmail = userJsondata.PrimaryEmail;
                    userData.CommunicationsEmail = userJsondata.CommunicationsEmail;
                    userData.ResidencyStart = userJsondata.ResidencyStart;
                    userData.ResidencyEnd = userJsondata.ResidencyEnd;
                    userData.ResidencyProgramName = userJsondata.ResidencyProgramName;
                    userData.ResidencyProgramId = userJsondata.ResidencyProgramId;
                    userData.ProgramDirectorProgramName = userJsondata.ProgramDirectorProgramName;
                    userData.ProgramDirectorProgramId = userJsondata.ProgramDirectorProgramId;
                    userData.MasterCustomerId = userJsondata.MasterCustomerId;
                    userData.RenewalLink = userJsondata.RenewalLink;
                    userData.RenewalMessage = userJsondata.RenewalMessage;
                    //System.Web.HttpContext.Current.Session["BCSC_AAO_MasterCustomerId"] = userJsondata.MasterCustomerId;
                    //System.Web.HttpContext.Current.Session["BCSC_AAO_MasterCustomerId_store_redirect"] = 0;

                    HttpCookie cookie = new HttpCookie("BCSC_AAO_MasterCustomerId", userJsondata.MasterCustomerId);
                    cookie.Expires = DateTime.Now.AddDays(cookieExpiry);
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                    HttpCookie redirectCookie = new HttpCookie("BCSC_AAO_MasterCustomerId_store_redirect", "0");

                    redirectCookie.Expires = DateTime.Now.AddDays(cookieExpiry);
                    System.Web.HttpContext.Current.Response.Cookies.Add(redirectCookie);
                    return userData;
                }
                else
                {
                    log.Information("403 Error Occured");
                    //System.Web.HttpContext.Current.Session["BCSC_AAO_MasterCustomerId_store_redirect"] = 1;
                    HttpCookie redirectCookie = new HttpCookie("BCSC_AAO_MasterCustomerId_store_redirect", "1");
                    redirectCookie.Expires = DateTime.Now.AddDays(cookieExpiry);
                    System.Web.HttpContext.Current.Response.Cookies.Add(redirectCookie);
                    return userData = null;
                }
*/
            }
            catch (WebException e)
            {
                Console.WriteLine("\r\nWebException Raised. The following error occured : {0}", e.Status);
                log.Information("403 Error Occured + Before " + e.Message);
                //System.Web.HttpContext.Current.Session["BCSC_AAO_MasterCustomerId_store_redirect"] = 1;
                HttpCookie redirectCookie = new HttpCookie("BCSC_AAO_MasterCustomerId_store_redirect", "1");
                redirectCookie.Expires = DateTime.Now.AddDays(cookieExpiry);
                System.Web.HttpContext.Current.Response.Cookies.Add(redirectCookie);
                return userData = null;
            }
            catch (Exception e)
            {
                Console.WriteLine("\nThe following Exception was raised : {0}", e.Message);
                log.Information("403 Error Occured Final" + e.Message);
                //System.Web.HttpContext.Current.Session["BCSC_AAO_MasterCustomerId_store_redirect"] = 1;
                HttpCookie redirectCookie = new HttpCookie("BCSC_AAO_MasterCustomerId_store_redirect", "1");
                redirectCookie.Expires = DateTime.Now.AddDays(cookieExpiry);
                System.Web.HttpContext.Current.Response.Cookies.Add(redirectCookie);
                return userData = null;
            }
        }
    }
}