using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Model
{
    public static class SQLiteHelper
    {

        public static int GetInt(this SQLiteDataReader reader, string ColumnName, int? fallbackValue = null)
        {
            var ordinal = reader.GetOrdinal(ColumnName);

            try
            {
                return reader.GetInt32(ordinal);
            }
            catch 
            {
                if (fallbackValue.HasValue) 
                    return fallbackValue.Value;
                else 
                    throw;
            }
        }

        public static string GetString(this SQLiteDataReader reader, string ColumnName)
        {
            var ordinal = reader.GetOrdinal(ColumnName);

            return reader.GetString(ordinal);

        }

        public static long? GetNullableLong(this SQLiteDataReader reader, string ColumnName, long? fallbackValue = null)
        {
            var ordinal = reader.GetOrdinal(ColumnName);

            try
            {
                return reader.GetInt64(ordinal);
            }
            catch
            {
                return fallbackValue;
            }
        }


        public static double? GetNullableDouble(this SQLiteDataReader reader, string ColumnName, double? fallbackValue = null)
        {
            var ordinal = reader.GetOrdinal(ColumnName);

            try
            {
                return reader.GetDouble(ordinal);
            }
            catch
            {
                return fallbackValue;
            }
        }

    }
}
