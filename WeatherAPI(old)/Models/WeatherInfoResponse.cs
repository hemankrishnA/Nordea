using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAPI.Models
{
    public class WeatherInfoResponse
    {
        public int ID { get; set; }

        public String Name { get; set; }

        public String CountryCode { get; set; }

        public String Description { get; set; }

        public float logitude { get; set; }

        public float latitude { get; set; }

        public float temprature { get; set; }
    }
}
