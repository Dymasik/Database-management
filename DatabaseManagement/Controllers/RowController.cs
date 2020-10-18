using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RowController : ControllerBase
    {
        [HttpPost]
        [Route("save")]
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
                foreach (var rowItem in request.Row.values) {
                    var textValue = rowItem.value.ToString();
                    if (isSelectedRow)
                    {
                        foreach (var columnValue in selectedRow.Values)
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
                    else {
                        var column = new Column(rowItem.column.name, rowItem.column.type);
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
