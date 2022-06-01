using LiteDB;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Models;

namespace WeatherAPI.Handlers
{
    /// <summary>
    /// Helper class to information from Database
    /// </summary>
    public class DBHandler : IDBHandler
    {
        private  readonly IConfiguration _config;

        public DBHandler(IConfiguration configuration)
        {
            _config = configuration;
        }


        /// <summary>
        /// Method to get city information based on city id
        /// </summary>        
        public City GetCityByID(int id)
        {
            String Dbpath = Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()) + _config["AppSettings:DbPath"];

            using (var db = new LiteDatabase(Dbpath))
            {
                var col = db.GetCollection<City>("cities");
                var result = col.FindById(id);
                return result;
            }
        }

        /// <summary>
        /// Method to get all city information 
        /// </summary>         
        public  IEnumerable<City> GetAll()
        {
            String Dbpath = Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())+_config["AppSettings:DbPath"];

            using (var db = new LiteDatabase(Dbpath))
            {
                var col = db.GetCollection<City>("cities");
                var result = col.FindAll().ToList();
                return result;
            }
        }

    }
}
