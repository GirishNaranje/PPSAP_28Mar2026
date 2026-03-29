using System.Configuration;

namespace PPSAP.Common
{
    public static class PPSAPGlobalConstants
    {
        public static string SiteWebAPIUrl = ConfigurationManager.AppSettings["PPSAP_API_URL"];
    }
}
