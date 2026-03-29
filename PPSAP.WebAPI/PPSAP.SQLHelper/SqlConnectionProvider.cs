using System.Configuration;

namespace PPSAP.SQLHelper
{
    public enum DataAccessType
    {
        Read, // Read indicates Read operations
        Write, // Write indicates Insert/Delete/Update operations
    }

    public class SqlConnectionProvider
    {
        public static string GetConnectionString(DataAccessType enumDataAccessType)
        {
            string connectionString = string.Empty;
            switch (enumDataAccessType)
            {
                case DataAccessType.Read:
                    connectionString = ConfigurationManager.ConnectionStrings["PPSAPDBConnection"].ConnectionString;
                    break;
                case DataAccessType.Write:
                    connectionString = ConfigurationManager.ConnectionStrings["PPSAPDBConnection"].ConnectionString;
                    break;
            }

            return connectionString;
        }
    }
}
