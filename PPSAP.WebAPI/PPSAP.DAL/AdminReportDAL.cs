using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PPSAP.Common;
using PPSAP.SQLHelper;
using PPSAP.SQLHelper.DataAccessProvider;

namespace PPSAP.DAL
{
    public static class AdminReportDAL
    {
        public static List<AdminReportVM> AdminReportDetails(AdminReportVM adminReportDetails)
        {
            List<AdminReportVM> reportList = new List<AdminReportVM>();
            try
            {
                SqlConnection con = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
                string procedure = "PPSAP_GetAvgAdminSectionReport";

                SqlCommand cmd = new SqlCommand(procedure, con);
                SqlDataReader r;

                // Configure command and add parameters.
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StartDate", Convert.ToDateTime(adminReportDetails.ExamStartDate).ToString("yyyy-MM-dd")));
                cmd.Parameters.Add(new SqlParameter("@EndDate", Convert.ToDateTime(adminReportDetails.ExamCompletedDate).ToString("yyyy-MM-dd")));
                cmd.Parameters.Add(new SqlParameter("@UserYear ", adminReportDetails.Year));
                cmd.Parameters.Add(new SqlParameter("@OrderColumn", adminReportDetails.OrderBy));
                cmd.Parameters.Add(new SqlParameter("@OrderSequence", adminReportDetails.Seq));
                cmd.CommandTimeout = 400;

                // Execute the command.
                con.Open();
                r = cmd.ExecuteReader();
                while (r.Read())
                {
                    AdminReportVM reportListBO = new AdminReportVM();
                    reportListBO.SubspecialtyId = Convert.ToInt32(r["SubspecialtyId"]);
                    reportListBO.SubspecialtyName = Convert.ToString(r["Subspecialty_Name"]);
                    reportListBO.Correct = Convert.ToInt32(r["correct"]);
                    reportListBO.InCorrect = Convert.ToInt32(r["InCorrect"]);
                    reportListBO.Score = Convert.ToInt32(r["Score"]);
                    reportListBO.UserId = Convert.ToInt32(r["UserId"]);
                    reportListBO.BCSCSectionNumber = Convert.ToInt32(r["RowNumber"]);
                    reportList.Add(reportListBO);
                }

                con.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Console.Write(ex.StackTrace);
                return null;
            }

            return reportList;
        }

        public static List<AdminReportVM> AdminReportDetailsExport(AdminReportVM adminReportDetails)
        {
            List<AdminReportVM> reportListExport = new List<AdminReportVM>();
            SqlParameter[] objSqlParameter =
            {
                                             new SqlParameter("@StartDate", adminReportDetails.ExamStartDate),
                                             new SqlParameter("@EndDate", adminReportDetails.ExamCompletedDate),
                                             new SqlParameter("@UserYear ", adminReportDetails.Year),
                                         };

            SqlConnection con = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            string procedure = "PPSAP_GetAvgAdminSectionReportExport";

            SqlCommand cmd = new SqlCommand(procedure, con);
            SqlDataReader r;

            // Configure command and add parameters.
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@StartDate", adminReportDetails.ExamStartDate));
            cmd.Parameters.Add(new SqlParameter("@EndDate", adminReportDetails.ExamCompletedDate));
            cmd.Parameters.Add(new SqlParameter("@UserYear ", adminReportDetails.Year));
            cmd.CommandTimeout = 400;

            // Execute the command.
            con.Open();
            r = cmd.ExecuteReader();
            while (r.Read())
            {
                AdminReportVM reportListBOExport = new AdminReportVM();
                reportListBOExport.SubspecialtyId = Convert.ToInt32(r["SubspecialtyId"]);
                reportListBOExport.SubspecialtyName = Convert.ToString(r["Subspecialty_Name"]);
                reportListBOExport.Correct = Convert.ToInt32(r["correct"]);
                reportListBOExport.InCorrect = Convert.ToInt32(r["InCorrect"]);
                reportListBOExport.Score = Convert.ToInt32(r["Score"]);
                reportListBOExport.UserId = Convert.ToInt32(r["UserId"]);
                reportListBOExport.BCSCSectionNumber = Convert.ToInt32(r["RowNumber"]);
                reportListExport.Add(reportListBOExport);
            }

            con.Close();
            return reportListExport;
        }

        public static List<AdminReportVM> GetPYGYear(AdminReportVM pgyDetails)
        {
            List<AdminReportVM> pgyList = new List<AdminReportVM>();
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            string sqlQueryChoice = string.Empty;
            sqlQueryChoice = "Select distinct isnull(convert(varchar,year(null)),'Non-Resident') as PGYear from PPSAPuser where Role ='U'";
            connection.Open();
            SqlCommand cmd = new SqlCommand(sqlQueryChoice, connection);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    AdminReportVM pgyListBO = new AdminReportVM();
                    pgyListBO.PGYYear = Convert.ToString(reader["PGYear"]);
                    pgyList.Add(pgyListBO);
                }

                reader.Close();
            }

            return pgyList;
        }
    }
}
