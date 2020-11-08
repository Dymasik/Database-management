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
    public class TableController : ControllerBase
    {

        public TableController(IUrlHelperFactory urlHelperFactory,
                           IActionContextAccessor actionContextAccessor)
        {
            UrlHelperFactory = urlHelperFactory;
            ActionContextAccessor = actionContextAccessor;
        }

        public IUrlHelperFactory UrlHelperFactory { get; }
        public IActionContextAccessor ActionContextAccessor { get; }

        [HttpGet(Name = nameof(Get))]
        public IEnumerable<TableDto> Get()
        {
            var database = Database.GetInstance();
            database.LoadTables();
            var tables = database.Select(t => t.ToDto()).ToArray();
            for (int i = 0; i < tables.Length; i++)
            {
                tables[i].Links = tables[i].CreateLinks(UrlHelperFactory, ActionContextAccessor);
            }
            return tables;
        }

        [HttpGet("{name}", Name = nameof(GetTable))]
        public TableDto GetTable(string name)
        {
            var database = Database.GetInstance();
            var table = database.Where(t => t.Name.Equals(name)).FirstOrDefault()?.ToDto();
            table.Links = table.CreateLinks(UrlHelperFactory, ActionContextAccessor);
            return table;
        }
    }
}
