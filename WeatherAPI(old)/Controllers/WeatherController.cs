using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Handlers;
using WeatherAPI.Models;

namespace WeatherAPI.Controllers
{
    /// <summary>
    /// APIs connected to Openweathermap service to get weather information from Nordea corporate office
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : Controller
    {
        private readonly IWeatherServiceHandler _weatherServiceHandler;

        /// <summary>
        /// Weather Constructor
        /// </summary>
        public WeatherController(IWeatherServiceHandler weatherServiceHandler)
        {
            _weatherServiceHandler = weatherServiceHandler;
        }

        /// <summary>
        /// To get the latest weather info for a city based on city id
        /// </summary>
        [HttpGet]
        [Route("GetWeatherInfo/{ID}")]
        public async Task<ActionResult<WeatherInfoResponse>> GetWeatherInfo(int ID)
        {
            try
            {
                var data = await _weatherServiceHandler.GetWeatherInfoByID(ID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To get the latest weather info for all cities
        /// </summary>
        [HttpGet]
        [Route("GetWeatherInfo")]
        public async Task<ActionResult<List<WeatherInfoResponse>>> GetWeatherInfo()
        {
            try
            {
                var data = await _weatherServiceHandler.GetAllWeatherInfo();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return  BadRequest(ex.Message); ;
            }
        }
    }
}
