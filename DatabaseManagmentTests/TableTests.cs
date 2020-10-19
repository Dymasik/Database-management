using DatabaseManagement.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseManagmentTests
{
    [TestFixture]
    class TableTests
    {
        private Table _table;

        [SetUp]
        public void Setup() {
            var structure = new TableStructure();
            var idColumn = new Column("id", ColumnType.INT);
            idColumn.IsKey = true;
            var nameColumn = new Column("name", ColumnType.STRING);
            structure.AddColumn(idColumn);
            structure.AddColumn(nameColumn);
            _table = new Table("test", structure);
            var row = new Row();
            row.Values.Add(new ColumnValue<int> {
                Column = idColumn,
                Value = 1
            });
            row.Values.Add(new ColumnValue<string>
            {
                Column = nameColumn,
                Value = "abc"
            });
            _table.Rows.Add(row);
            row = new Row();
            row.Values.Add(new ColumnValue<int>
            {
                Column = idColumn,
                Value = 2
            });
            row.Values.Add(new ColumnValue<string>
            {
                Column = nameColumn,
                Value = "test"
            });
            _table.Rows.Add(row);
        }

        [Test]
        public void GetFilteredRows_WhenForwardNotExistsColumn_ThrowNewException() {
            Assert.That(() => _table.GetFilteredRows("abc", "abc"), 
                Throws.Exception.TypeOf<NotImplementedException>());
        }

        [Test]
        public void GetFilteredRows_WhenForwardExistsColumn_ReturnsFilteredRows()
        {
            var beforeRowCount = _table.Rows.Count;

            var rows = _table.GetFilteredRows("id", "1");
            var afterRowCount = rows.Count();
            var rowNameValue = (rows.FirstOrDefault().Values.Where(v => v.Column.Name.Equals("name")).FirstOrDefault() as ColumnValue<string>).Value; 

            Assert.That(beforeRowCount, Is.EqualTo(2));
            Assert.That(afterRowCount, Is.EqualTo(1));
            Assert.That(rowNameValue, Is.EqualTo("abc"));
        }

        [Test]
        public void AddColumn_AddNotKeyColumn_VerifyNewNotKeyColumnInStructure()
        {
            _table.AddColumn("test1", ColumnType.REAL, false);

            var column = _table.Structure.Where(c => c.Name.Equals("test1")).FirstOrDefault();

            Assert.That(column, Is.Not.Null);
            Assert.That(column.IsKey, Is.False);
        }
    }
}
