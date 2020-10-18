using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseManagement.Models
{
    public class RowRequest
    {
        public RowDto Row { get; set; }
        public string TableName { get; set; }
    }
}
