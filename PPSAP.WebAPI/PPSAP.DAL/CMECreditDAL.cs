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
    public static class CMECreditDAL
    {
        public static List<CMECreditVM> GetCreditDetails(UserIdVM user)
        {
            List<CMECreditVM> creditList = new List<CMECreditVM>();

            SqlParameter[] objSqlParameter =
            {
                                            new SqlParameter("@UserId", user.UserId),
                                         };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "PPSAP_GetCMEDetails", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    CMECreditVM creditData = new CMECreditVM();
                    object subspecialtyidObj = objSqlDataReader["subspecialtyid"];
                    creditData.SubSpecialityId = subspecialtyidObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["subspecialtyid"]);
                    object bCSCSectionNumberObj = objSqlDataReader["BCSCSectionNumber"];
                    creditData.BCSCSectionNumber = bCSCSectionNumberObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["BCSCSectionNumber"]);
                    object subspecialty_nameObj = objSqlDataReader["subspecialty_name"];
                    creditData.SubSpecialityName = subspecialty_nameObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["subspecialty_name"]);
                    object attemptedCountObj = objSqlDataReader["AttemptedCount"];
                    creditData.AttemptedCount = attemptedCountObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["AttemptedCount"]);
                    object cMECreditPathObj = objSqlDataReader["CMECreditPath"];
                    creditData.CMECreditPath = cMECreditPathObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["CMECreditPath"]);
                    creditList.Add(creditData);
                }

                objSqlDataReader.Close();
            }

            return creditList;
        }
    }
}
