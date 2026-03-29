using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PPSAP.Common;
using PPSAP.DTO;
using PPSAP.SQLHelper;
using PPSAP.SQLHelper.DataAccessProvider;

namespace PPSAP.DAL
{
    public static class ReportDetailsDAL
    {
        public static List<ReportsDetailsVM> ReportDetails(ReportsDetailsVM reportDetails)
        {
            List<ReportsDetailsVM> reportList = new List<ReportsDetailsVM>();
            SqlParameter[] objSqlParameter =
            {
                                            new SqlParameter("@UserId", reportDetails.UserId),
                                            new SqlParameter("@StartDate", reportDetails.ExamStartDate),
                                            new SqlParameter("@EndDate", reportDetails.ExamCompletedDate),
                                         };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "PPSAP_GetAvgResidentSectionReport", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    ReportsDetailsVM reportListBO = new ReportsDetailsVM();
                    object subspecialtyIdObj = objSqlDataReader["SubspecialtyId"];
                    reportListBO.SubspecialtyId = subspecialtyIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["SubspecialtyId"]);
                    object subspecialtyNameObj = objSqlDataReader["Subspecialty_Name"];
                    reportListBO.SubspecialtyName = subspecialtyNameObj is DBNull ? null : Convert.ToString(objSqlDataReader["Subspecialty_Name"]);
                    object correctObj = objSqlDataReader["correct"];
                    reportListBO.Correct = correctObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["correct"]);
                    object inCorrectObj = objSqlDataReader["InCorrect"];
                    reportListBO.InCorrect = inCorrectObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["InCorrect"]);
                    object scoreObj = objSqlDataReader["Score"];
                    reportListBO.Score = scoreObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Score"]);
                    object bCSCSectionNumberObj = objSqlDataReader["BCSCSectionNumber"];
                    reportListBO.BCSCSectionNumber = bCSCSectionNumberObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["BCSCSectionNumber"]);
                    reportList.Add(reportListBO);
                }

                objSqlDataReader.Close();
            }

            return reportList;
        }
    }
}
