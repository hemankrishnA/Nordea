using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Helper;

namespace WeatherAPI.Controllers
{
    /// <summary>
    /// API to generate JWT Token to authenicate Nordea Weather API
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class JWTController : Controller
    {
        /// <summary>
        /// To generate JWT Token
        /// </summary>
        [HttpGet]
        public IActionResult JWT()
        {
            Log.Information($"Excecution started for JWT Token Generation : {HttpContext.Request.Path}");
            
            try { return new ObjectResult(JwtToken.GenerateJwtToken()); }
            
            catch(Exception ex)
            {
                Log.Error($"Exception occured : {ex.Message}");
                return BadRequest(ex.Message);
            }
           
        }
    }
}
