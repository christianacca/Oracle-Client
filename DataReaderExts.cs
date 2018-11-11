using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Oracle_Client
{
    internal static class DataReaderExts
    {
        public static IEnumerable<(object columnName, string key, object value)> GetSchemaInfo(this IDataReader reader)
        {
            var tableSchema = reader.GetSchemaTable();

            var schemaInfo = from columnMetadata in tableSchema.Rows.OfType<DataRow>()
                let columnName = columnMetadata["ColumnName"]
                from columnAttribute in tableSchema.Columns.OfType<DataColumn>()
                let key = columnAttribute.ColumnName
                let value = columnMetadata[columnAttribute.ColumnName]
                select (columnName, key, value);
            return schemaInfo;
        }
    }
}