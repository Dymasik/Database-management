using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DatabaseManagement.Models
{
    public class RowDto {
        public IList<ColumnValueDto> values { get; set; }

        public RowDto()
        {
            values = new List<ColumnValueDto>();
        }
    }
    public class ColumnValueDto
    {
        public object value { get; set; }
        public ColumnDto column { get; set; }
    }

    public class ColumnDto
    {
        public string name { get; set; }
        public ColumnType type { get; set; }
        public bool isKey { get; set; }
    }

    public class TableDto {
        public string Name { get; set; }
        public TableStructure Structure { get; set; }
        public IList<RowDto> Rows { get; set; } = new List<RowDto>();
    }
}
