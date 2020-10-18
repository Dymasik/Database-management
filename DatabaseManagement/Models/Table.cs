using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseManagement.Models
{
    public class Table
    {
        public string Name { get; private set; }
        public TableStructure Structure { get; private set; }
        public IList<Row> Rows { get; private set; } = new List<Row>();


        public Table()
        {
            Structure = new TableStructure();
        }

        public Table(string name) => (Name, Structure) = (name, new TableStructure());

        public Table(TableStructure structure) => Structure = structure;

        public Table(string name, TableStructure structure) => (Name, Structure) = (name, structure);

        public void AddColumn(string name, ColumnType type) {
            var column = new Column(name, type);
            Structure.AddColumn(column);
        }

        public void RemoveColumn(string name) {
            Structure.RemoveColumn(name);
        }

        public TableDto ToDto() {
            var table = new TableDto();
            table.Name = Name;
            table.Structure = Structure;
            var rows = new List<RowDto>();
            foreach (var row in Rows)
            {
                rows.Add(row.ToDto());
            }
            table.Rows = rows;
            return table;
        }
    }
}
