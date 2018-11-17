using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Newtonsoft.Json;

namespace Oracle_Client
{
    internal static class DataReaderExts
    {
        public static IEnumerable<(object columnName, string key, object value)> GetSchemaInfoRecords(this IDataReader reader)
        {
            var tableSchema = reader.GetSchemaTable();

            if (tableSchema == null)
            {
                throw new ArgumentException("no schema table available from DataReader", nameof(reader));
            }

            return from columnMetadata in tableSchema.Rows.OfType<DataRow>()
                let columnName = columnMetadata["ColumnName"]
                from columnAttribute in tableSchema.Columns.OfType<DataColumn>()
                let key = columnAttribute.ColumnName
                let rawValue = columnMetadata[columnAttribute.ColumnName]
                let value = GetValue(rawValue, key)
                select (columnName, key, value);


            object GetValue(object rawValue, string key)
            {
                if (!(rawValue is DBNull)) return rawValue;

                return key.StartsWith("Is") ? (object)false : 0;
            }
        }

        public static IEnumerable<ColumnSchemaInfo> GetSchemaInfo(this DbDataReader reader)
        {
            var schemaInfoRecords = reader.GetSchemaInfoRecords();

            var schemaInfoMaps = from columnAttribute in schemaInfoRecords
                group columnAttribute by columnAttribute.columnName
                into columnAttributes
                select columnAttributes.Aggregate(new Dictionary<string, object>(), (results, record) =>
                {
                    results.Add(record.key, record.value);
                    return results;
                });

            return from columnAttributes in schemaInfoMaps
                let serializedColAttrs = JsonConvert.SerializeObject(columnAttributes)
                let column = JsonConvert.DeserializeObject<ColumnSchemaInfo>(serializedColAttrs)
                select column;
        }
    }
}