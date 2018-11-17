using System;

namespace Oracle_Client
{
    public class ColumnSchemaInfo
    {
        public string ColumnName { get; set; }
        public int ColumnOrdinal { get; set; }
        public int ColumnSize { get; set; }
        public int NumericPrecision { get; set; }
        public int NumericScale { get; set; }
        public bool IsUnique { get; set; }
        public bool IsKey { get; set; }
        public bool IsRowID { get; set; }
        public string BaseColumnName { get; set; }
        public string BaseSchemaName { get; set; }
        public string BaseTableName { get; set; }
        public Type DataType { get; set; }
        public int ProviderType { get; set; }
        public bool AllowDBNull { get; set; }
        public bool IsAliased { get; set; }
        public bool IsByteSemantic { get; set; }
        public bool IsExpression { get; set; }
        public bool IsHidden { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsLong { get; set; }

        public override string ToString()
        {
            return $"{nameof(ColumnName)}: {ColumnName}, {nameof(ColumnOrdinal)}: {ColumnOrdinal}, {nameof(ColumnSize)}: {ColumnSize}, {nameof(NumericPrecision)}: {NumericPrecision}, {nameof(NumericScale)}: {NumericScale}, {nameof(IsUnique)}: {IsUnique}, {nameof(IsKey)}: {IsKey}, {nameof(IsRowID)}: {IsRowID}, {nameof(BaseColumnName)}: {BaseColumnName}, {nameof(BaseSchemaName)}: {BaseSchemaName}, {nameof(BaseTableName)}: {BaseTableName}, {nameof(DataType)}: {DataType}, {nameof(ProviderType)}: {ProviderType}, {nameof(AllowDBNull)}: {AllowDBNull}, {nameof(IsAliased)}: {IsAliased}, {nameof(IsByteSemantic)}: {IsByteSemantic}, {nameof(IsExpression)}: {IsExpression}, {nameof(IsHidden)}: {IsHidden}, {nameof(IsReadOnly)}: {IsReadOnly}, {nameof(IsLong)}: {IsLong}";
        }
    }
}