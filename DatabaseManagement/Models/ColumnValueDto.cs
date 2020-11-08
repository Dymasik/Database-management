using DatabaseManagement.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DatabaseManagement.Models
{
    public class RowDto {
        public IList<ColumnValueDto> values { get; set; } = new List<ColumnValueDto>();
        public IList<LinkDto> Links { get; set; } = new List<LinkDto>();

        public void AddLink(IUrlHelperFactory urlHelperFactory,
                           IActionContextAccessor actionContextAccessor,
                           string tableName, string columnName, string value)
        {
            var helper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            var keyColumnValue = values.Where(v => v.column.isKey).FirstOrDefault()?.value;
            Links.Add(new LinkDto(
                helper.Link(nameof(RowController.GetFilteredRows), new { tableName = tableName, columnName = columnName, value = value }),
                "self",
                "GET"
            ));
            Links.Add(new LinkDto(
                helper.Link(nameof(RowController.Delete), new { tableName = tableName, key = keyColumnValue} ),
                "delete_row",
                "DELETE"
            ));
            Links.Add(new LinkDto(
                helper.Link(nameof(RowController.Save), new { }),
                "save_row",
                "POST"
            ));
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

    public class TableDto
    {
        public string Name { get; set; }
        public TableStructure Structure { get; set; }
        public IList<RowDto> Rows { get; set; } = new List<RowDto>();
        public IList<LinkDto> Links { get; set; }

        public TableDto()
        {
            Links = new List<LinkDto>();
        }

        public IList<LinkDto> CreateLinks(IUrlHelperFactory urlHelperFactory,
                   IActionContextAccessor actionContextAccessor)
        {
            var list = new List<LinkDto>();
            var helper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            list.Add(new LinkDto(
                helper.Link(nameof(TableController.Get), new { }),
                "get_all_tables",
                "GET"
            ));
            list.Add(new LinkDto(
                helper.Link(nameof(TableController.GetTable), new { name = Name }),
                "self",
                "GET"
            ));
            return list;
        }
    }

    public class LinkDto {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
        public LinkDto(string href, string rel, string method)
        {
            this.Href = href;
            this.Rel = rel;
            this.Method = method;
        }
    }
}
