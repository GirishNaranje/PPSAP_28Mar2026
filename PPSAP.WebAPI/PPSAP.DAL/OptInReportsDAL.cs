using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using PPSAP.Common;
using PPSAP.DTO;
using PPSAP.SQLHelper;
using PPSAP.SQLHelper.DataAccessProvider;

namespace PPSAP.DAL
{
    public class OptInReportsDAL
    {
        public static List<OptInReports> OptInReports(OptInReports optInReports)
        {
            string ordering = string.Empty;
            if (optInReports.OrderBy != null)
            {
                ordering = " order by " + optInReports.OrderBy + " " + optInReports.Seq;
            }

            List<OptInReports> reportList = new List<OptInReports>();
            SqlParameter[] objSqlParameter =
            {
             new SqlParameter("@UserYear ", optInReports.Year),
             new SqlParameter("@Order", ordering),
        };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "PPSAP_GetOptInReports", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    OptInReports reportListBO = new OptInReports();

                    reportListBO.UserId = Convert.ToInt32(objSqlDataReader["userid"]);
                    reportListBO.UserName = Convert.ToString(objSqlDataReader["UserName"]);

                    object customerIdObj = objSqlDataReader["userid"];
                    reportListBO.MasterCustomerID = customerIdObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["userid"]);
                    reportListBO.OptIn = Convert.ToString(objSqlDataReader["OptIn"]);
                    reportList.Add(reportListBO);
                }

                objSqlDataReader.Close();
            }

            return reportList;
        }

        public static List<OptInReports> GetOptIn(OptInReports optInReports)
        {
            List<OptInReports> reportList = new List<OptInReports>();
            SqlParameter[] objSqlParameter =
            {
              new SqlParameter("@UserYear ", optInReports.Year),
        };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "PPSAP_GetOptIn", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    OptInReports reportListBO = new OptInReports();
                    reportListBO.OptIn = Convert.ToString(objSqlDataReader["OptIn"]);
                    reportList.Add(reportListBO);
                }

                objSqlDataReader.Close();
            }

            return reportList;
        }
    }
}
