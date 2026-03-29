using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using PPSAP.Common;
using Newtonsoft.Json;
using PPSAP.Apps.Proxy;

namespace PPSAP.Apps.Controllers
{
    [Authorize]
    public class OptInReportsController : Controller
    {
        // GET: OptInReports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OptInReports(int year)
        {
            JsonResult dataresult = new JsonResult();
            try
            {
                OptInReports reportDetails = new OptInReports()
                {
                    Year = Convert.ToInt32(year),
                };
                string examPostDataJson = JsonConvert.SerializeObject(reportDetails);
                string url = PPSAPGlobalConstants.SiteWebAPIUrl + "OptInReports/OptInReports";
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                List<OptInReports> report = new List<OptInReports>();
                report = JsonConvert.DeserializeObject<List<OptInReports>>(result);
                int totalRecords = report.Count;
                int recFilter = report.Count;
                return Json(report, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public ActionResult GetOptIn(int year)
        {
            JsonResult dataresult = new JsonResult();
            try
            {
                OptInReports reportDetails = new OptInReports()
                {
                   Year = Convert.ToInt32(year),
                };
                string examPostDataJson = JsonConvert.SerializeObject(reportDetails);
                string url = PPSAPGlobalConstants.SiteWebAPIUrl + "OptInReports/GetOptIn";
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                List<OptInReports> optInReports = new List<OptInReports>();
                optInReports = JsonConvert.DeserializeObject<List<OptInReports>>(result);
                OptInReports report = new OptInReports();
                report.TotalOptInAcceptCount = optInReports.Where(a => a.OptIn == "Y").Count();
                report.TotalOptInDeclineCount = optInReports.Where(a => a.OptIn == "N").Count();
                report.TotalUserCount = optInReports.Count;
                return Json(report, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public ActionResult ExportToExcel(int year)
        {
            OptInReports reportDetails = new OptInReports()
            {
                Year = Convert.ToInt32(year),
            };

            string examPostDataJson = JsonConvert.SerializeObject(reportDetails);
            string url = PPSAPGlobalConstants.SiteWebAPIUrl + "OptInReports/OptInReports";
            string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
            List<OptInReports> report = new List<OptInReports>();
            report = JsonConvert.DeserializeObject<List<OptInReports>>(result);
            var table = new System.Data.DataTable("report");
            table.Columns.Add("UserID", typeof(string));
            table.Columns.Add("User Name", typeof(string));
            table.Columns.Add("Status", typeof(string));
            foreach (var pro in report)
            {
                string UserId = pro.UserId.ToString();
                string userName = pro.UserName.ToString();
                /*string customerId = pro.MasterCustomerID.ToString();*/
                string status = pro.OptIn.ToString();

                table.Rows.Add(new object[] { UserId, userName, status });
            }

            var grid = new GridView();
            grid.DataSource = table;
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=OptInReports.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = string.Empty;
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return null;
        }
    }
}