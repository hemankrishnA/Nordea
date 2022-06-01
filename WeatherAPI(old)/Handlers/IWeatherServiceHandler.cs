using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Models;

namespace WeatherAPI.Handlers
{
    public interface IWeatherServiceHandler
    {
        public  Task<WeatherInfoResponse> GetWeatherInfoByID(int ID);

        public  Task<List<WeatherInfoResponse>> GetAllWeatherInfo();
        

    }
}
