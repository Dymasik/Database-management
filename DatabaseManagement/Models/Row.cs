using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseManagement.Models
{
    public class Row
    {
        public IList<IColumnValue> Values { get; private set; }

        public Row()
        {
            Values = new List<IColumnValue>();
        }

        public RowDto ToDto() {
            var row = new RowDto();
            foreach (var value in Values)
            {
                var columnDto = new ColumnDto { 
                    name = value.Column.Name,
                    type = value.Column.Type,
                    isKey = value.Column.IsKey
                };
                var columnValueDto = new ColumnValueDto { 
                    column = columnDto
                };
                switch (value.Column.Type)
                {
                    case ColumnType.INT:
                        columnValueDto.value = (value as ColumnValue<int>).Value;
                        break;
                    case ColumnType.REAL:
                        columnValueDto.value = (value as ColumnValue<double>).Value;
                        break;
                    case ColumnType.CHAR:
                        columnValueDto.value = (value as ColumnValue<char>).Value;
                        break;
                    case ColumnType.STRING:
                        columnValueDto.value = (value as ColumnValue<string>).Value;
                        break;
                    default:
                        break;
                }
                row.values.Add(columnValueDto);
            }
            return row;
        }
    }
}
