﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Post.API.Controllers
{
    /// <summary>
    /// Test Controller
    /// </summary>
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Test Get OP
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //模拟操作
            Thread.Sleep(3000);
            return new string[]
            {
                Environment.GetEnvironmentVariable("POD_IP"),
                Environment.MachineName,
                HttpContext.TraceIdentifier,
                HttpContext.Request.Host.Host,
                HttpContext.Connection.LocalIpAddress.ToString(),
                HttpContext.Connection.LocalPort.ToString(),
                HttpContext.Connection.RemoteIpAddress.ToString()
            };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
