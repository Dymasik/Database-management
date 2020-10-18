using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseManagement.Models
{
    public class TableStructure : IEnumerable<Column>
    {
        private IList<Column> _columns;

        public TableStructure()
        {
            _columns = new List<Column>();
        }

        public TableStructure(IList<Column> columns)
        {
            _columns = columns;
        }

        protected virtual void ValidateColumn(Column column)
        {
            column.Validate();
            foreach (var cl in _columns)
            {
                if (cl.Name.Equals(column.Name)) {
                    throw new ArgumentException("The column with same name already exists!");
                }
            }
        }

        public bool RemoveColumn(string name) {
            int index;
            for (index = 0; index < _columns.Count; index++)
            {
                if (_columns[index].Name.Equals(name)) {
                    break;
                }
            }
            try
            {
                _columns.RemoveAt(index);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return false;
            }
            return true;
        }

        public void AddColumn(Column column) {
            ValidateColumn(column);
            _columns.Add(column);
        }

        public IEnumerator<Column> GetEnumerator()
        {
            return _columns.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
