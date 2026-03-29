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
    public class IncorrectQuestionDetailsDAL
    {
        public static List<IncorrectQuestionDetailsDTO> IncorrectQuestionDetails(int? userId, int? subspecialtyId, DateTime? examStartDate, DateTime? examCompletedDate, int? noOfRecords, int? pageNo, int year)
        {
            List<IncorrectQuestionDetailsDTO> incorrectReportList = new List<IncorrectQuestionDetailsDTO>();
            SqlParameter[] objSqlParameter =
                                            {
                                            new SqlParameter("@UserId", Convert.ToInt32(userId)),
                                            new SqlParameter("@SubspecialtyId", Convert.ToInt32(subspecialtyId)),
                                            new SqlParameter("@StartDate", examStartDate),
                                            new SqlParameter("@EndDate", examCompletedDate),
                                            new SqlParameter("@PageSize", noOfRecords),
                                            new SqlParameter("@PageIndex", pageNo),
                                            new SqlParameter("@UserYear", year),
                                         };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "PPSAP_GetInCorrectQuestionBytSection", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    IncorrectQuestionDetailsDTO inCorrectListBO = new IncorrectQuestionDetailsDTO();
                    object examidObj = objSqlDataReader["Examid"];
                    inCorrectListBO.ExamId = examidObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Examid"]);

                    object questionIdObj = objSqlDataReader["QuestionId"];
                    inCorrectListBO.QuestionId = questionIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["QuestionId"]);

                    object questionIdCountObj = objSqlDataReader["RecordCount"];
                    inCorrectListBO.QuestionIdCount = questionIdCountObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["RecordCount"]);

                    object subspecialtyObj = objSqlDataReader["subspecialty"];
                    inCorrectListBO.Subspecialty = questionIdCountObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["subspecialty"]);

                    object sectionObj = objSqlDataReader["section"];
                    inCorrectListBO.Section = sectionObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["section"]);

                    object rownumberObj = objSqlDataReader["rownumber"];
                    inCorrectListBO.Rownumber = rownumberObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["rownumber"]);

                    object examtypeObj = objSqlDataReader["examtype"];
                    inCorrectListBO.ExamType = examtypeObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["examtype"]);

                    incorrectReportList.Add(inCorrectListBO);
                }

                objSqlDataReader.Close();
            }

            return incorrectReportList;
        }
    }
}
