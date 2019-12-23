using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DaxkoOrderAPI.Controllers
{
    /// <summary>
    /// Health Controller
    /// </summary>
    [Route("v1/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Gets the current DateTime
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(DateTime.Now.ToString());
        }
    }
}