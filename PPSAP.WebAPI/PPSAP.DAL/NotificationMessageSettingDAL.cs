using PPSAP.Common;
using PPSAP.DTO;
using PPSAP.SQLHelper;
using PPSAP.SQLHelper.DataAccessProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPSAP.DAL
{
    public class NotificationMessageSettingDAL
    {
        public static List<NotificationMessageSetting> GetAllNotificationMessageSetting(SearchParameters queDetails)
        {
            List<NotificationMessageSetting> performanceList = new List<NotificationMessageSetting>();
            SqlParameter[] objSqlParameter =
            {
                                              new SqlParameter("@OffsetValue", queDetails.OffsetValue),
                                              new SqlParameter("@PagingSize", queDetails.PagingSize),
                                              new SqlParameter("@Search", queDetails.Search),
                                         };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "PPSAP_GetAllNotificationMessageSetting", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    NotificationMessageSetting reportListBO = new NotificationMessageSetting();
                    object NotificationMessageSettingIdObj = objSqlDataReader["NotificationMessageSettingId"];
                    reportListBO.NotificationMessageSettingId = NotificationMessageSettingIdObj is DBNull ? 0 : Convert.ToInt32(NotificationMessageSettingIdObj);

                    object UserIdObj = objSqlDataReader["UserId"];
                    reportListBO.UserId = UserIdObj is DBNull ? (int?)null : Convert.ToInt32(UserIdObj);

                    object NotificationMessageObj = objSqlDataReader["NotificationMessage"];
                    reportListBO.NotificationMessage = NotificationMessageObj is DBNull ? null : Convert.ToString(NotificationMessageObj);

                    object NoOfTimeObj = objSqlDataReader["NoOfTime"];
                    reportListBO.NoOfTime = NoOfTimeObj is DBNull ? 0 : Convert.ToInt32(NoOfTimeObj);

                    object OnholdScreenTimeObj = objSqlDataReader["OnholdScreenTime"];
                    reportListBO.OnholdScreenTime = OnholdScreenTimeObj is DBNull ? 0 : Convert.ToInt32(OnholdScreenTimeObj);

                    object IsUnableObj = objSqlDataReader["IsUnable"];
                    reportListBO.IsUnable = IsUnableObj is DBNull ? false : Convert.ToBoolean(IsUnableObj);

                    object ISDoYouWishToSeeThisMessageAgainObj = objSqlDataReader["ISDoYouWishToSeeThisMessageAgain"];
                    reportListBO.ISDoYouWishToSeeThisMessageAgain = ISDoYouWishToSeeThisMessageAgainObj is DBNull ? false : Convert.ToBoolean(ISDoYouWishToSeeThisMessageAgainObj);

                    object TotalObj = objSqlDataReader["Total"];
                    reportListBO.Total = TotalObj is DBNull ? 0 : Convert.ToInt32(TotalObj);

                    performanceList.Add(reportListBO);
                }

                objSqlDataReader.Close();
            }

            return performanceList;
        }

        public static List<NotificationMessageSetting> GetAllMessageSettingByUser(SearchParameters queDetails)
        {
            List<NotificationMessageSetting> performanceList = new List<NotificationMessageSetting>();
            SqlParameter[] objSqlParameter =
            {
            new SqlParameter("@UserId", queDetails.UserId),
        };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "PPSAP_GetAllNotificationMessageSettingByUser", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    NotificationMessageSetting reportListBO = new NotificationMessageSetting();
                    object NotificationMessageSettingIdObj = objSqlDataReader["NotificationMessageSettingId"];
                    reportListBO.NotificationMessageSettingId = NotificationMessageSettingIdObj is DBNull ? 0 : Convert.ToInt32(NotificationMessageSettingIdObj);

                    object UserIdObj = objSqlDataReader["UserId"];
                    reportListBO.UserId = UserIdObj is DBNull ? (int?)null : Convert.ToInt32(UserIdObj);

                    object NotificationMessageObj = objSqlDataReader["NotificationMessage"];
                    reportListBO.NotificationMessage = NotificationMessageObj is DBNull ? null : Convert.ToString(NotificationMessageObj);

                    object NoOfTimeObj = objSqlDataReader["NoOfTime"];
                    reportListBO.NoOfTime = NoOfTimeObj is DBNull ? 0 : Convert.ToInt32(NoOfTimeObj);

                    object OnholdScreenTimeObj = objSqlDataReader["OnholdScreenTime"];
                    reportListBO.OnholdScreenTime = OnholdScreenTimeObj is DBNull ? 0 : Convert.ToInt32(OnholdScreenTimeObj);

                    object IsUnableObj = objSqlDataReader["IsUnable"];
                    reportListBO.IsUnable = IsUnableObj is DBNull ? false : Convert.ToBoolean(IsUnableObj);

                    object ISDoYouWishToSeeThisMessageAgainObj = objSqlDataReader["ISDoYouWishToSeeThisMessageAgain"];
                    reportListBO.ISDoYouWishToSeeThisMessageAgain = ISDoYouWishToSeeThisMessageAgainObj is DBNull ? false : Convert.ToBoolean(ISDoYouWishToSeeThisMessageAgainObj);


                    performanceList.Add(reportListBO);
                }

                objSqlDataReader.Close();
            }

            return performanceList;
        }


        public static NotificationMessageSetting GetNotificationMessageSetting(SearchParameters queDetails)
        {
            List<NotificationMessageSetting> performanceList = new List<NotificationMessageSetting>();
            SqlParameter[] objSqlParameter =
            {
            new SqlParameter("@id", queDetails.Id)
        };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "PPSAP_GetNotificationMessageSettingById", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    NotificationMessageSetting reportListBO = new NotificationMessageSetting();
                    object NotificationMessageSettingIdObj = objSqlDataReader["NotificationMessageSettingId"];
                    reportListBO.NotificationMessageSettingId = NotificationMessageSettingIdObj is DBNull ? 0 : Convert.ToInt32(NotificationMessageSettingIdObj);

                    object UserIdObj = objSqlDataReader["UserId"];
                    reportListBO.UserId = UserIdObj is DBNull ? (int?)null : Convert.ToInt32(UserIdObj);

                    object NotificationMessageObj = objSqlDataReader["NotificationMessage"];
                    reportListBO.NotificationMessage = NotificationMessageObj is DBNull ? null : Convert.ToString(NotificationMessageObj);

                    object NoOfTimeObj = objSqlDataReader["NoOfTime"];
                    reportListBO.NoOfTime = NoOfTimeObj is DBNull ? 0 : Convert.ToInt32(NoOfTimeObj);

                    object OnholdScreenTimeObj = objSqlDataReader["OnholdScreenTime"];
                    reportListBO.OnholdScreenTime = OnholdScreenTimeObj is DBNull ? 0 : Convert.ToInt32(OnholdScreenTimeObj);

                    object IsUnableObj = objSqlDataReader["IsUnable"];
                    reportListBO.IsUnable = IsUnableObj is DBNull ? false : Convert.ToBoolean(IsUnableObj);

                    object ISDoYouWishToSeeThisMessageAgainObj = objSqlDataReader["ISDoYouWishToSeeThisMessageAgain"];
                    reportListBO.ISDoYouWishToSeeThisMessageAgain = ISDoYouWishToSeeThisMessageAgainObj is DBNull ? false : Convert.ToBoolean(ISDoYouWishToSeeThisMessageAgainObj);

                    performanceList.Add(reportListBO);
                }

                objSqlDataReader.Close();
            }

            return performanceList.FirstOrDefault();
        }


        public static void SaveNotificationMessageSetting(NotificationMessageSetting notificationMessageSetting)
        {
            try
            {
                SqlParameter[] objSqlParameter =
                {
            new SqlParameter("@NotificationMessageSettingId", notificationMessageSetting.NotificationMessageSettingId),
            new SqlParameter("@NotificationMessage", notificationMessageSetting.NotificationMessage),
            new SqlParameter("@NoOfTime", Convert.ToInt32(notificationMessageSetting.NoOfTime)),
            new SqlParameter("@OnholdScreenTime", notificationMessageSetting.OnholdScreenTime),
            new SqlParameter("@IsUnable", notificationMessageSetting.IsUnable),
            new SqlParameter("@UserId", notificationMessageSetting.UserId),
            new SqlParameter("@ISDoYouWishToSeeThisMessageAgain", notificationMessageSetting.ISDoYouWishToSeeThisMessageAgain),
        };

                int result = SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "PPSAP_CreateOrUpdateNotificationMessageSetting", objSqlParameter);
            }
            catch (Exception e)
            {
                // to do
            }
        }

        public static bool DeleteMessageSetting(SearchParameters queDetails)
        {
            SqlParameter[] objSqlParameter =
            {
            new SqlParameter("@NotificationMessageSettingId", queDetails.Id),
        };

            int count = SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "PPSAP_DeleteNotificationMessageSetting", objSqlParameter);

            return count > 0 ? true : false;
        }

        public static bool InsertOrUpdateUserMessageSetting(SearchParameters queDetails)
        {
            SqlParameter[] objSqlParameter =
            {
            new SqlParameter("@UserId", queDetails.UserId),
        };

            int count = SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "PPSAP_InsertUserNotificationMessageSetting", objSqlParameter);

            return true;
        }
    }
}
