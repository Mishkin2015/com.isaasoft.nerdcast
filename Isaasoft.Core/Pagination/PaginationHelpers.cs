using System;

namespace Isaasoft.Core.Pagination
{
    public static class PaginationHelpers
    {
        public static string GetLogicalString(LogicalOperators logical)
        {
            switch (logical)
            {
                case LogicalOperators.And:
                    return "&&";
                case LogicalOperators.Or:
                    return "||";
                default:
                    throw new NotImplementedException();
            }
        }

        public static object ConvertValue<T>(ExpressionProperty expression)
        {
            var type = expression.TargetType;

            if (type == null)
            {
                type = typeof(T);
            }

            var value = ConvertValue(type, expression.PropertyName, expression.WithTheValue);

            return value;
        }

        public static object ConvertValue(Type type, string propertyName, object value)
        {
            var propertyType = type.GetProperty(propertyName).PropertyType;

            switch (propertyType.Name)
            {
                case "Boolean":
                    return Convert.ToBoolean(value);
                case "Byte":
                    return Convert.ToByte(value);
                case "Char":
                    return Convert.ToChar(value);
                case "DateTime":
                    return Convert.ToDateTime(value);
                case "Decimal":
                    return Convert.ToDecimal(value);
                case "Double":
                    return Convert.ToDouble(value);
                case "Int16":
                    return Convert.ToInt16(value);
                case "Int32":
                    return Convert.ToInt32(value);
                case "Int64":
                    return Convert.ToInt64(value);
                case "SByte":
                    return Convert.ToSByte(value);
                case "Single":
                    return Convert.ToSingle(value);
                case "String":
                    return Convert.ToString(value);
                case "UInt16":
                    return Convert.ToUInt16(value);
                case "UInt32":
                    return Convert.ToUInt32(value);
                case "UInt64":
                    return Convert.ToUInt64(value);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
