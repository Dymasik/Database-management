using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseManagement.Models
{
    public class Column
    {
        public string Name { get; private set; }
        public ColumnType Type { get; private set; }

        public Column(string name, ColumnType type) => (Name, Type) = (name, type);

        public void Validate() {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("The name of column should be filled in!");
        }
    }

    public enum ColumnType { 
        INT,
        REAL,
        CHAR,
        STRING,
        DATE,
        DATE_INTL
    }

    public static class ColumnTypeExtensions
    {
        public static ColumnType GetValue(string type)
        {
            switch (type)
            {
                case "int":
                    return ColumnType.INT;
                case "real":
                    return ColumnType.REAL;
                case "char":
                    return ColumnType.CHAR;
                case "string":
                    return ColumnType.STRING;
                case "date":
                    return ColumnType.DATE;
                case "date_intl":
                    return ColumnType.DATE_INTL;
                default:
                    throw new InvalidCastException($"The type {type} is not supported!");
            }
        }

        public static string GetStringValue(ColumnType type)
        {
            switch (type)
            {
                case ColumnType.INT:
                    return "int";
                case ColumnType.REAL:
                    return "real";
                case ColumnType.CHAR:
                    return "char";
                case ColumnType.STRING:
                    return "string";
                default:
                    throw new InvalidCastException($"The type {type} is not supported!");
            }
        }
    }
}
