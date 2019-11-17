using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DaxkoOrderAPI.Controllers
{
    [Route("v1/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(DateTime.Now.ToString());
        }
    }
}