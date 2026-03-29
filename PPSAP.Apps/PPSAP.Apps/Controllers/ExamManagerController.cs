using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using PPSAP.Apps.Proxy;
using PPSAP.Common;
using PPSAP.DTO;
using Newtonsoft.Json;
using System.Configuration;
using Serilog;

namespace PPSAP.Apps.Controllers
{
    [Authorize]
    public class ExamManagerController : Controller
    {
        private string _sAttrLogPath = ConfigurationManager.AppSettings.Get("LogfilePath");
        private Serilog.Core.Logger log;

        // GET: ExamManager
        public ExamManagerController()
        {
            log = new LoggerConfiguration().WriteTo.RollingFile(this._sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();
        }

        public ActionResult Index()
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userid:{2}", "ExamManagerController", "Index", user._userId);
                var enumExamTypeData = from ExamManagerEnum.ExamType e in Enum.GetValues(typeof(ExamManagerEnum.ExamType))
                                       select new
                                       {
                                           ID = (int)e,
                                           Name = e.ToString(),
                                       };
                var enumExamModeData = from ExamManagerEnum.ExamMode e in Enum.GetValues(typeof(ExamManagerEnum.ExamMode))
                                       select new
                                       {
                                           ID = (int)e,
                                           Name = e.ToString(),
                                       };
                var enumExamSelectionType = from ExamManagerEnum.ExamQuestionSelectionType e in Enum.GetValues(typeof(ExamManagerEnum.ExamQuestionSelectionType))
                                            select new
                                            {
                                                ID = (int)e,
                                                Name = e.ToString(),
                                            };

                // Exam Type List
                List<SelectListItem> examType = new List<SelectListItem>();
                foreach (var enumExamSelectionTypeView in enumExamTypeData)
                {
                    var res = (ExamManagerEnum.ExamType)Enum.Parse(typeof(ExamManagerEnum.ExamType), enumExamSelectionTypeView.Name.ToString());
                    string value = GetEnumDescription(res);
                    examType.Add(new SelectListItem() { Text = value, Value = enumExamSelectionTypeView.ID.ToString(), Selected = false });
                }
                examType.RemoveAll(x => x.Value == "3");
                ExamDTO exam = new ExamDTO();
                exam.UserId = user._userId;

                // Get Question Type Count List
                string urlGetQuestionTypeCount = PPSAPGlobalConstants.SiteWebAPIUrl + "ExamManager/GetQuestionTypeCount";
                string questuionTypeDataJson = JsonConvert.SerializeObject(exam);
                string resultGetQuestionTypeCount = HttpProxy.HttpPost(urlGetQuestionTypeCount, questuionTypeDataJson, "application/json; charset=utf-8", "POST");
                List<QuestionTypeCountDTO> questionTypeListCount = new List<QuestionTypeCountDTO>();
                questionTypeListCount = JsonConvert.DeserializeObject<List<QuestionTypeCountDTO>>(resultGetQuestionTypeCount);
                string[] questionCount = new string[4];
                foreach (var item in questionTypeListCount)
                {
                    questionCount[0] = item.TotalCount.ToString();
                    questionCount[1] = item.ExamSkipQuestionCount.ToString();
                    questionCount[2] = item.IncorrectAnswerCount.ToString();
                    questionCount[3] = item.MarkQuestionCount.ToString();
                }

                List<SelectListItem> questionType = new List<SelectListItem>();
                foreach (var enumExamSelectionTypeView in enumExamSelectionType)
                {
                    var res = (ExamManagerEnum.ExamQuestionSelectionType)Enum.Parse(typeof(ExamManagerEnum.ExamQuestionSelectionType), enumExamSelectionTypeView.Name.ToString());
                    string value = GetEnumDescription(res);
                    questionType.Add(new SelectListItem() { Text = value, Value = enumExamSelectionTypeView.ID.ToString(), Selected = false });
                }

                // Get SubSpeciality
                string url = PPSAPGlobalConstants.SiteWebAPIUrl + "ExamManager/GetSpecialityList";
                string result = HttpProxy.HttpPost(url, questuionTypeDataJson, "application/json; charset=utf-8", "POST");
                List<SubSpecialityDetailVM> specialityType = new List<SubSpecialityDetailVM>();
                specialityType = JsonConvert.DeserializeObject<List<SubSpecialityDetailVM>>(result);
                List<SelectListItem> categoryType = new List<SelectListItem>();
                foreach (var item in specialityType)
                {
                    categoryType.Add(new SelectListItem()
                    {
                        Text = item.SpecialityName,
                        Value = item.SpecialityId.ToString() + ":" + item.TotalCount.ToString() + ":" + item.MarkQuestionCount.ToString() + ":" + item.IncorrectAsnwerCount.ToString() + ":" + item.ExamSkipQuestionCount.ToString(),
                        Selected = false,
                    });
                }

                ViewBag.ExamType = examType;
                ViewBag.EnumExamModeList = new SelectList(enumExamModeData, "ID", "Name");
                ViewBag.QuestionTypeListCount = questionCount;
                ViewBag.QuestionTypelist = questionType;
                ViewBag.CategoryTypelist = categoryType;
                log.Information("end Controller :{0} method :{1} userid:{2}", "ExamManagerController", "Index", user._userId);
                return View();
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + " Controller :{0} method :{1}", "ExamManagerController", "Index");
                throw;
            }
        }

        public ActionResult CreateExam(ExamDTO exam)
        {
            try
            {
                if (!string.IsNullOrEmpty(exam.TypeofCategoryListstring))
                {
                    exam.TypeofCategoryList = exam.TypeofCategoryListstring.Split(',').ToList();
                }

                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId:{2} ExamType:{3}", "ExamManagerController", "CreateExam", user._userId, exam.ExamType);
                log.Information(
                    "ExamName:{0}, ExamId:{1}, ExamAttemptId:{2}, ExamAnswerToShow:{3}, ExamCreateDate:{4}, ExamMode:{5}, NoofQuestions:{6}, TypeofQuestion:{7}, TypeofCategory:{8}, ExamType:{9}, TypeofCategoryListstring:{10}, userId:{11}",
                    exam.ExamName, exam.ExamId, exam.ExamAttemptId, exam.ExamAnswerToShow, exam.ExamCreateDate, exam.ExamMode, exam.NoofQuestions, exam.TypeofQuestion, exam.TypeofCategory, exam.ExamType, exam.TypeofCategoryListstring, user._userId);
                exam.UserId = user._userId;
                log.Information("UserId :{0}", user._userId);
                string url = PPSAPGlobalConstants.SiteWebAPIUrl + "ExamManager/CreateExam";
                if (exam.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition))
                {
                    //exam.TypeofCategoryList = new List<string>() { "12", "13", "9", "2", "6", "8", "7", "10", "1", "3", "4", "5", "11" };
                    exam.TypeofQuestionList = new List<string>() { "0" };
                    exam.ExamAnswerToShow = true;
                }

                log.Information("ExamType :{0}", exam.ExamType);
                string examPostDataJson = JsonConvert.SerializeObject(exam);
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                ResponseStatusVM examResponse = new ResponseStatusVM();
                examResponse = JsonConvert.DeserializeObject<ResponseStatusVM>(result);

                // Sending Controls to Assessment Controller
                if (examResponse.ErrorCode == 1)
                {
                    if (examResponse.SystemGeneratedQuestionCount < examResponse.UserEnteredQuestionCount)
                    {
                        log.Information("ErrorCode: {0}", examResponse.ErrorCode + " Controller:ExamManagerController" + " method:CreateExam" + " UserId:" + user._userId);
                        TempData["MsgError"] = examResponse.Errormessage + ", with " + examResponse.SystemGeneratedQuestionCount + " questions , based on the availability of questions for your selected options.";
                    }

                    log.Information("Redirect to Controller: Controller:{0}, Method:{1}, ExamId:{2}", "Assessment", "Index", examResponse.ExamId);
                    return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Assessment", action = "Index", ExamId = examResponse.ExamId }));
                }
                else
                {
                    TempData["MsgError"] = examResponse.Errormessage;
                    log.Information("end Controller :{0} method :{1} userId:{2} ExamType:{3} ExamName:{4}, ExamId:{5}", "ExamManagerController", "CreateExam", user._userId, exam.ExamType, exam.ExamName, exam.ExamId);
                    log.Information("Redirect to Controller :{0} method :{1}", "Dashboard", "Index");
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + " Controller:ExamManagerController" + " method:CreateExam" + "UserId:" + user._userId + " ExamId:" + exam.ExamId + " ExamName:" + exam.ExamName);
                throw;
            }
        }

        public static string GetEnumDescription(Enum value)
        {
            try
            {
                FieldInfo fieldinfo = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
                else
                {
                    return value.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: "+ ex.StackTrace);
                throw;
            }
        }

        public ActionResult CheckExamNameAvailable(string ExamName)
        {
            try
            {
                log.Information("start Controller :{0} method :{1} ExamName:{2}", "ExamManagerController", "CheckExamNameAvailable", ExamName);
                log.Information("ExamName :{0}", ExamName);
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                ExamNameVM examName = new ExamNameVM
                {
                    ExamName = ExamName,
                    UserId = user._userId,
                };
                log.Information("ExamName :{0} UserId :{1}", examName.ExamName, examName.UserId);
                string checkAvailableExamJson = JsonConvert.SerializeObject(examName);

                // Calling ViewProgress Service
                string urlCheckExamName = PPSAPGlobalConstants.SiteWebAPIUrl + "ExamManager/CheckExamNameAvailable";
                string checkExamNameResult = HttpProxy.HttpPost(urlCheckExamName, checkAvailableExamJson, "application/json; charset=utf-8", "POST");
                string checkExamName = JsonConvert.DeserializeObject<string>(checkExamNameResult);
                log.Information("end Controller :{0} method :{1} ExamName:{2}", "ExamManagerController", "CheckExamNameAvailable", ExamName);
                return Json(checkExamName, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + "ExamName:" + ExamName);
                throw;
            }
        }

        public ActionResult GetExamCountOnExamType()
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId:{2}", "ExamManagerController", "GetExamCountOnExamType", user._userId);
                ExamCountOnExamTypeVM examCount = new ExamCountOnExamTypeVM
                {
                    UserId = user._userId,
                };

                // Get ExamId By UserId
                string countOnExamTypeDataJson = JsonConvert.SerializeObject(examCount);
                string urlGetCountOnExamType = PPSAPGlobalConstants.SiteWebAPIUrl + "ExamManager/GetExamCountOnExamType";
                string resultGetExamIdBYUserId = HttpProxy.HttpPost(urlGetCountOnExamType, countOnExamTypeDataJson, "application/json; charset=utf-8", "POST");
                ExamCountOnExamTypeVM count = JsonConvert.DeserializeObject<ExamCountOnExamTypeVM>(resultGetExamIdBYUserId);
                ViewBag.QuestionTypeCount = count;
                log.Information("QuestionTypeCount :{0}", count);
                log.Information("end Controller :{0} method :{1} userId:{2}", "ExamManagerController", "GetExamCountOnExamType", user._userId);
                return Json(count, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId);
                throw;
            }
        }

        public ActionResult GetQuestionTypeCountBySection(string sectionValue)
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} sectionValue{2} userId:{3} ", "ExamManagerController", "GetQuestionTypeCountBySection", sectionValue, user._userId);

                QuestionCountOnSection sectoinValue = new QuestionCountOnSection
                {
                    UserId = user._userId,
                    SectionId = sectionValue,
                };

                // Get ExamId By UserId
                string sectoinValueDataJson = JsonConvert.SerializeObject(sectoinValue);
                string urlGetCountOnsectoinValue = PPSAPGlobalConstants.SiteWebAPIUrl + "ExamManager/GetQuestionTypeCountBySection";
                string resultGetExamIdBYUserId = HttpProxy.HttpPost(urlGetCountOnsectoinValue, sectoinValueDataJson, "application/json; charset=utf-8", "POST");
                QuestionCountOnSection sectionWiseCount = JsonConvert.DeserializeObject<QuestionCountOnSection>(resultGetExamIdBYUserId);
                ViewBag.QuestionTypeCount = sectionWiseCount;
                log.Information("QuestionTypeCount :{0}", sectionWiseCount);
                log.Information("end Controller :{0} method :{1} sectionValue{2} userId:{3} ", "ExamManagerController", "GetQuestionTypeCountBySection", sectionValue, user._userId);
                return Json(sectionWiseCount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + "sectionValue:" + sectionValue);
                throw;
            }
        }

        // This Method Will Call the Web Service for getting Spaced Repetition Data
        public ActionResult SpacedRepetition(ExamDTO srExamData)
        {
            try
            {
                log.Information("start Controller:{0} method:{1} userId:{2}", "ExamManagerController", "SpacedRepetition", srExamData.UserId);
                ExamDTO spacedRepititionExam = new ExamDTO
                {
                    NoofQuestions = -1,
                    ExamCreateDate = DateTime.Now,
                    ExamType = 3,
                    ExamAnswerToShow = true,
                };
                List<string> listofCategory = srExamData.TypeofCategoryList;
                spacedRepititionExam.TypeofCategoryListstring = string.Join(",", listofCategory);

                log.Information("end Controller :{0} method :{1} ExamType:{2} userId:{3}", "ExamManagerController", "SpacedRepetition", spacedRepititionExam.ExamType, srExamData.UserId);
                log.Information("Redirect to Controller :{0} method :{1}", "ExamManagerController", "CreateExam");
                return RedirectToAction("CreateExam", spacedRepititionExam);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + " Controller:ExamManagerController" + " method:SpacedRepetition" + "UserId:" + user._userId);
                throw;
            }
        }
    }
}