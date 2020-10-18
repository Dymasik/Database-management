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
    public class TableController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<TableDto> Get()
        {
            var database = Database.GetInstance();
            database.LoadTables();
            return database.Select(t => t.ToDto());
        }

        [HttpGet("{name}")]
        public TableDto GetTable(string name)
        {
            var database = Database.GetInstance();
            return database.Where(t => t.Name.Equals(name)).FirstOrDefault()?.ToDto();
        }
    }
}
