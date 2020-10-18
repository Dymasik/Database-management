using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System;

namespace DatabaseManagement.Models
{
    public class Database : IEnumerable<Table>
    {
        private readonly IList<Table> _tables;
        private static Database _instance;
        private static string _connectionString = "E:\\DatabaseManagement\\dbs\\db.json";

        private Database() {
            _tables = new List<Table>();
        }

        public void Save() {
            var result = new JObject();
            if (_tables.Any()) {
                var metadata = new JArray();
                var values = new JArray();
                foreach (var table in _tables)
                {
                    var tableValues = new JObject();
                    var tableMeta = new JObject();
                    tableMeta.Add("name", table.Name);
                    tableValues.Add("name", table.Name);
                    var columnsMeta = new JArray();
                    foreach (var column in table.Structure) {
                        var columnMeta = new JObject();
                        columnMeta.Add("name", column.Name);
                        columnMeta.Add("type", ColumnTypeExtensions.GetStringValue(column.Type));
                        columnsMeta.Add(columnMeta);
                    }
                    tableMeta.Add("columns", columnsMeta);

                    var rowsValues = new JArray();
                    foreach (var row in table.Rows)
                    {
                        var columnValues = new JArray();
                        foreach (var valueItem in row.Values)
                        {
                            var columnValue = new JObject();
                            columnValue.Add("name", valueItem.Column.Name);
                            switch (valueItem.Column.Type)
                            {
                                case ColumnType.INT:
                                    columnValue.Add("value", (valueItem as ColumnValue<int>).Value);
                                    break;
                                case ColumnType.REAL:
                                    columnValue.Add("value", (valueItem as ColumnValue<double>).Value);
                                    break;
                                case ColumnType.CHAR:
                                    columnValue.Add("value", (valueItem as ColumnValue<char>).Value);
                                    break;
                                case ColumnType.STRING:
                                    columnValue.Add("value", (valueItem as ColumnValue<string>).Value);
                                    break;
                                default:
                                    break;
                            }
                            columnValues.Add(columnValue);
                        }
                        var rowObj = new JObject();
                        rowObj.Add("columns", columnValues);
                        rowsValues.Add(rowObj);
                    }
                    tableValues.Add("rows", rowsValues);
                    values.Add(tableValues);
                    metadata.Add(tableMeta);
                }
                result.Add("metadata", metadata);
                result.Add("values", values);
            }
            File.WriteAllText(_connectionString, result.ToString());
        }

        public void LoadTables() {
            _tables.Clear();
            using (var streamReader = new StreamReader(_connectionString, Encoding.ASCII, true))
            {
                string json = streamReader.ReadToEnd();
                var database = JObject.Parse(json);
                var metadata = database.GetValue("metadata") as JArray;
                if (metadata != null)
                {
                    foreach (var tableObj in metadata)
                    {
                        if (tableObj is JObject tableMetadata)
                        {
                            var tableName = tableMetadata.Value<string>("name");
                            var table = new Table(tableName);
                            var columnsMeta = tableMetadata.Value<JArray>("columns");
                            foreach (var columnObj in columnsMeta)
                            {
                                if (columnObj is JObject columnMetadata)
                                {
                                    var columnName = columnMetadata.Value<string>("name");
                                    var columnType = columnMetadata.Value<string>("type");
                                    table.AddColumn(columnName, ColumnTypeExtensions.GetValue(columnType));
                                }
                            }
                            AddTable(table);
                        }
                    }
                }

                var values = database.GetValue("values") as JArray;
                if (values != null)
                {
                    foreach (var valueObj in values)
                    {
                        if (valueObj is JObject value)
                        {
                            var tableName = value.Value<string>("name");
                            var table = _tables.Where(t => t.Name.Equals(tableName)).FirstOrDefault();
                            var rows = value.Value<JArray>("rows");
                            foreach (var rowObj in rows)
                            {
                                var row = new Row();
                                if (rowObj is JObject rowJs)
                                {
                                    var columns = rowJs.Value<JArray>("columns");
                                    foreach (var columnObj in columns)
                                    {
                                        if (columnObj is JObject col) {
                                            IColumnValue columnValue;
                                            var name = col.Value<string>("name");
                                            var column = table.Structure.Where(column => column.Name.Equals(name)).FirstOrDefault();
                                            switch (column.Type)
                                            {
                                                case ColumnType.INT:
                                                    columnValue = new ColumnValue<int> { 
                                                        Column = column,
                                                        Value = col.Value<int>("value")
                                                    };
                                                    break;
                                                case ColumnType.REAL:
                                                    columnValue = new ColumnValue<double>
                                                    {
                                                        Column = column,
                                                        Value = col.Value<double>("value")
                                                    };
                                                    break;
                                                case ColumnType.CHAR:
                                                    columnValue = new ColumnValue<char>
                                                    {
                                                        Column = column,
                                                        Value = col.Value<char>("value")
                                                    };
                                                    break;
                                                case ColumnType.STRING:
                                                    columnValue = new ColumnValue<string>
                                                    {
                                                        Column = column,
                                                        Value = col.Value<string>("value")
                                                    };
                                                    break;
                                                default:
                                                    throw new NotImplementedException();
                                            }
                                            row.Values.Add(columnValue);
                                        }
                                    }
                                }
                                table.Rows.Add(row);
                            }
                        }
                    }
                }
            }
        }

        public void AddTable(Table table) {
            _tables.Add(table);
        }

        public IEnumerator<Table> GetEnumerator()
        {
            return _tables.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public static Database GetInstance() {
            if (_instance == null) {
                _instance = new Database();
            }
            return _instance;
        }

        public static void SetConnection(string path)
        {
            _connectionString = path;
        }
    }
}
