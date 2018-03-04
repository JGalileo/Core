using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public CoreObject SearchCoreObject()
        {
            return new CoreObject
            {
                UniqueId = "Core",
                DisplayName = "Object",
            };
        }

        [HttpPost]
        public dynamic ErrorData([FromBody]string[] values)
        {
            throw new NotImplementedException();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public int Post([FromBody]string value)
        {
            return 0;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
