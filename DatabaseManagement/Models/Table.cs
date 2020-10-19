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

        public void AddColumn(string name, ColumnType type, bool isKey = false) {
            var column = new Column(name, type);
            column.IsKey = isKey;
            Structure.AddColumn(column);
        }

        public void RemoveColumn(string name) {
            Structure.RemoveColumn(name);
        }

        public Column GetColumnByName(string name) {
            return Structure.Where(c => c.Name.Equals(name)).FirstOrDefault();
        }

        public Column GetKeyColumn() {
            return Structure.Where(c => c.IsKey).FirstOrDefault();
        }

        public IEnumerable<Row> GetFilteredRows(string columnName, string value) {
            var column = GetColumnByName(columnName);
            if (column == null)
                throw new NotImplementedException();
            return Rows.Where(r => r.Values.Where(v => {
                if (v.Column == column) {
                    var isEqualValue = false;
                    switch (v.Column.Type)
                    {
                        case ColumnType.INT:
                            isEqualValue = (v as ColumnValue<int>).Value == Convert.ToInt32(value);
                            break;
                        case ColumnType.REAL:
                            isEqualValue = (v as ColumnValue<double>).Value == Convert.ToDouble(value);
                            break;
                        case ColumnType.CHAR:
                            isEqualValue = (v as ColumnValue<char>).Value == Convert.ToChar(value);
                            break;
                        case ColumnType.STRING:
                            isEqualValue = (v as ColumnValue<string>).Value == value;
                            break;
                        default:
                            break;
                    }
                    return isEqualValue;
                }
                return false;
            }).Any());
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
