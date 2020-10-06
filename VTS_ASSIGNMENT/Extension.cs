using System.Data.SqlClient;

namespace VTS_ASSIGNMENT
{
        public static class DbReaderExtension
        {
            public static string GetSafeString(this SqlDataReader reader, int colIndex)
            {
                return !reader.IsDBNull(colIndex) ? reader.GetString(colIndex) : string.Empty;
            }

            public static bool GetSafeBool(this SqlDataReader reader, int colIndex)
            {
                return !reader.IsDBNull(colIndex) && reader.GetBoolean(colIndex);
            }

            public static int GetSafeInt(this SqlDataReader reader, int colIndex)
            {
                return !reader.IsDBNull(colIndex) ? reader.GetInt32(colIndex) : -1;
            }

            public static double GetSafeDouble(this SqlDataReader reader, int colIndex)
            {
                return !reader.IsDBNull(colIndex) ? reader.GetDouble(colIndex) : -1;
            }
        }
}