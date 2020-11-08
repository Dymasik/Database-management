using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseManagement.Models
{
    public class ColumnValue<T> : IColumnValue
    {
        public T Value { get; set; }
        public Column Column { get; set; }

        public string GetValue()
        {
            return Value.ToString();
        }
    }
}
