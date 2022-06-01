using LiteDB;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Models;

namespace WeatherAPI.Handlers
{
    /// <summary>
    /// Handler class to information from Database
    /// </summary>
    public class DBHandler : IDBHandler
    {
        private  readonly IConfiguration _config;

        /// <summary>
        /// Constructor for DB Helper
        /// </summary>
        public DBHandler(IConfiguration configuration)
        {
            _config = configuration;
        }


        /// <summary>
        /// Method to get city information based on city id
        /// </summary>        
        public City GetCityByID(int id)
        {
            Log.Information($"Database Info : Execution started for method GetCityByID{id}");
            try 
            {
                String Dbpath = Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()) + _config["AppSettings:DbPath"];

                using (var db = new LiteDatabase(Dbpath))
                {
                    var col = db.GetCollection<City>("cities");
                    var result = col.FindById(id);
                    if(result == null)
                    {
                        Log.Information($"Database Info : No record found  in database !");
                        return result;
                    }

                    Log.Information($"Database Info : Execution completed for method GetCityByID{id}");
                    return result;
                }
             }
            catch(Exception ex)
            {
                Log.Error($"Database Exception occured : {ex.Message}");
                throw new Exception(ex.Message);
            }
            
        }

        /// <summary>
        /// Method to get all city information 
        /// </summary>         
        public  IEnumerable<City> GetAll()
        {
            Log.Information($"Database Info : Execution started for method GetAll()");

            try
            {
                String Dbpath = Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()) + _config["AppSettings:DbPath"];

                using (var db = new LiteDatabase(Dbpath))
                {
                    var col = db.GetCollection<City>("cities");
                    var result = col.FindAll().ToList();
                    if (result.Count() == 0)
                    {
                        Log.Information($"Database Info : No record found  in database !");
                        return result;
                    }

                    Log.Information($"Database Info : Execution completed for method GetAll()");
                    return result;
                }
            }
            catch(Exception ex)
            {
                Log.Error($"Database Exception occured : {ex.Message}");
                throw new Exception(ex.Message);
            }
            
        }

    }
}
