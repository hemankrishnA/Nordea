using System.Collections.Generic;
using WeatherAPI.Models;

namespace WeatherAPI.Handlers
{
    public interface IDBHandler
    {
        public City GetCityByID(int id);

        public IEnumerable<City> GetAll();


    }
}