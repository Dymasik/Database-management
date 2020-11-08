using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace DatabaseManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RowController : ControllerBase
    {
        private readonly IUrlHelper _urlHelper;

        public IUrlHelperFactory UrlHelperFactory { get; }
        public IActionContextAccessor ActionContextAccessor { get; }

        public RowController(IUrlHelperFactory urlHelperFactory,
                           IActionContextAccessor actionContextAccessor)
        {
            UrlHelperFactory = urlHelperFactory;
            ActionContextAccessor = actionContextAccessor;
        }

        [HttpGet]
        [Route("filter", Name = nameof(GetFilteredRows))]
        public IEnumerable<RowDto> GetFilteredRows(string tableName, string columnName, string value) {
            var database = Database.GetInstance();
            var table = database.Where(t => t.Name.Equals(tableName)).FirstOrDefault();
            if (table != null) {
                var rows = table.GetFilteredRows(columnName, value).Select(r => r.ToDto()).ToArray();
                foreach (var row in rows)
                {
                    row.AddLink(UrlHelperFactory, ActionContextAccessor, tableName, columnName, value);
                }
                return rows;
            }
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("delete", Name = nameof(Delete))]
        public void Delete(string tableName, string key) {
            var database = Database.GetInstance();
            var table = database.Where(t => t.Name.Equals(tableName)).FirstOrDefault();
            if (table != null) {
                var keyColumn = table.GetKeyColumn();
                if (keyColumn != null) {
                    var index = table.Rows.Where(r =>
                    {
                        return r.Values.Where(v =>
                        {
                            var isSelectedRow = false;
                            switch (v.Column.Type)
                            {
                                case ColumnType.INT:
                                    isSelectedRow = (v as ColumnValue<int>).Value == Convert.ToInt32(key);
                                    break;
                                case ColumnType.REAL:
                                    isSelectedRow = (v as ColumnValue<double>).Value == Convert.ToDouble(key);
                                    break;
                                case ColumnType.CHAR:
                                    isSelectedRow = (v as ColumnValue<char>).Value == Convert.ToChar(key);
                                    break;
                                case ColumnType.STRING:
                                    isSelectedRow = (v as ColumnValue<string>).Value == key;
                                    break;
                                default:
                                    break;
                            }
                            return v.Column.IsKey && isSelectedRow;

                        }).Any();
                    }).Select((r, i) => i).First();
                    table.Rows.RemoveAt(index);
                    database.Save();
                }
            }
        }

        [HttpPost]
        [Route("save", Name = nameof(Save))]
        public void Save(RowRequest request) {
            var database = Database.GetInstance();
            var table = database.Where(t => t.Name.Equals(request.TableName)).FirstOrDefault();
            if (table != null) {
                var selectedRow = new Row();
                var idColumnName = "id";
                var idColumnValue = request.Row.values.Where(value => value.column.name.Equals(idColumnName)).FirstOrDefault()?.value.ToString();
                var isSelectedRow = false;
                if (idColumnValue != null) {
                    foreach (var row in table.Rows) {
                        foreach (var columnValue in row.Values) {
                            if (columnValue.Column.Name.Equals(idColumnName)) {
                                
                                switch (columnValue.Column.Type)
                                {
                                    case ColumnType.INT:
                                        isSelectedRow = (columnValue as ColumnValue<int>).Value == Convert.ToInt32(idColumnValue);
                                        selectedRow = row;
                                        break;
                                    case ColumnType.REAL:
                                        isSelectedRow = (columnValue as ColumnValue<double>).Value == Convert.ToDouble(idColumnValue);
                                        selectedRow = row;
                                        break;
                                    case ColumnType.CHAR:
                                        isSelectedRow = (columnValue as ColumnValue<char>).Value == Convert.ToChar(idColumnValue);
                                        selectedRow = row;
                                        break;
                                    case ColumnType.STRING:
                                        isSelectedRow = (columnValue as ColumnValue<string>).Value == idColumnValue;
                                        selectedRow = row;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
                if (!isSelectedRow) {
                    selectedRow = new Row();
                }
                foreach (var rowItem in request.Row.values) {
                    var textValue = rowItem.value.ToString();
                    if (isSelectedRow)
                    {
                        foreach (var columnValue in selectedRow.Values)
                        {
                            if (columnValue.Column.Name.Equals(rowItem.column.name))
                            {
                                switch (columnValue.Column.Type)
                                {
                                    case ColumnType.INT:
                                        (columnValue as ColumnValue<int>).Value = Convert.ToInt32(textValue);
                                        break;
                                    case ColumnType.REAL:
                                        (columnValue as ColumnValue<double>).Value = Convert.ToDouble(textValue);
                                        break;
                                    case ColumnType.CHAR:
                                        (columnValue as ColumnValue<char>).Value = Convert.ToChar(textValue);
                                        break;
                                    case ColumnType.STRING:
                                        (columnValue as ColumnValue<string>).Value = textValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    else {
                        var column = table.GetColumnByName(rowItem.column.name);
                        switch (rowItem.column.type)
                        {
                            case ColumnType.INT:
                                selectedRow.Values.Add(new ColumnValue<int> { 
                                    Value = Convert.ToInt32(textValue),
                                    Column = column
                                });
                                break;
                            case ColumnType.REAL:
                                selectedRow.Values.Add(new ColumnValue<double>
                                {
                                    Value = Convert.ToDouble(textValue),
                                    Column = column
                                });
                                break;
                            case ColumnType.CHAR:
                                selectedRow.Values.Add(new ColumnValue<char>
                                {
                                    Value = Convert.ToChar(textValue),
                                    Column = column
                                });
                                break;
                            case ColumnType.STRING:
                                selectedRow.Values.Add(new ColumnValue<string>
                                {
                                    Value = textValue,
                                    Column = column
                                });
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (!isSelectedRow) {
                    table.Rows.Add(selectedRow);
                }
            }
            database.Save();
        }
    }
}
