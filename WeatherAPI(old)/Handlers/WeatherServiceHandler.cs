using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeatherAPI.Models;

namespace WeatherAPI.Handlers
{
    /// <summary>
    /// Helper class to information from Weather Service
    /// </summary>
    public class WeatherServiceHandler : IWeatherServiceHandler
    {
        private readonly IConfiguration _config;

        public readonly IDBHandler _dbhandler;


        public WeatherServiceHandler(IConfiguration configuration , IDBHandler dbhandler)
        {
            _config = configuration;
            _dbhandler = dbhandler;
        }


        /// <summary>
        /// Method to get weather information from weather service for all cities
        /// </summary>
        public async Task<List<WeatherInfoResponse>> GetAllWeatherInfo()
        {
            //list to store all the final weather info list
            List<WeatherInfoResponse> finallist = new List<WeatherInfoResponse>();

            try
            {
                //Get configurations from appsettings
                var Weatherapi_BaseURL = _config["AppSettings:Weatherapi_BaseURL"];
                var Weatherapi_AppId = _config["AppSettings:Weatherapi_AppID"];

                //Get city information from database
                var citydata = _dbhandler.GetAll();

                foreach (var item in citydata)
                {
                    var url = $"{Weatherapi_BaseURL}?id={item.Id}&appid={Weatherapi_AppId}&units=metric";
                    var request = new RestRequest(url, Method.Get);
                    var response = await new RestClient().ExecuteAsync(request);
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        var returnType = response.Content.ToString();
                        WeatherInfo Obj = JsonConvert.DeserializeObject<WeatherInfo>(response.Content.ToString());
                        if (Obj != null)
                        {
                            WeatherInfoResponse info = new WeatherInfoResponse();
                            info.ID = item.Id;
                            info.Name = item.Name;
                            info.CountryCode = item.CountryCode;
                            info.Description = item.Description;
                            info.latitude = Obj.list[0].coord.lat;
                            info.logitude = Obj.list[0].coord.lon;
                            info.temprature = Obj.list[0].main.temp;

                            finallist.Add(info);

                        }
                        else
                        {
                            throw new Exception($"Weather Service did not return OK when calling {url}. StatusCode returned was {response.StatusCode} and content {response.Content.ToString()}");
                        }
                    }
                    

                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return finallist;
        }


        /// <summary>
        /// Method to get weather information from weather service for single city ID
        /// </summary>
        public async Task<WeatherInfoResponse> GetWeatherInfoByID(int ID)
        {
            try
            {
                //Get configurations from appsettings
                var Weatherapi_BaseURL = _config["AppSettings:Weatherapi_BaseURL"];
                var Weatherapi_AppId = _config["AppSettings:Weatherapi_AppID"];
               
                //Get city information from database
                var citydata = _dbhandler.GetCityByID(ID);


                var url = $"{Weatherapi_BaseURL}?id={ID}&appid={Weatherapi_AppId}&units=metric";
                var request = new RestRequest(url, Method.Get);
                var response = await new RestClient().ExecuteAsync(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var returnType = response.Content.ToString();
                        WeatherInfo Obj = JsonConvert.DeserializeObject<WeatherInfo>(response.Content.ToString());
                        if (Obj != null)
                        {
                            WeatherInfoResponse info = new WeatherInfoResponse();
                            info.ID = citydata.Id;
                            info.Name = citydata.Name;
                            info.CountryCode = citydata.CountryCode;
                            info.Description = citydata.Description;
                            info.latitude = Obj.list[0].coord.lat;
                            info.logitude = Obj.list[0].coord.lon;
                            info.temprature = Obj.list[0].main.temp;


                            return info;
                        }


                        return null;
                    default:
                        throw new Exception($"Weather Service did not return OK when calling {url}. StatusCode returned was {response.StatusCode} and content {response.Content.ToString()}");
                }

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}

