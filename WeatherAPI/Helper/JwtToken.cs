using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAPI.Helper
{
    /// <summary>
    /// Helper class to generate Jwt Token
    /// </summary>
    public static class JwtToken
    {
        private const string SECRET_KEY = "XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234"; // TODO : To add this secret in enviornment varaible 
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

        public static string GenerateJwtToken()
        {
            //security key length should be > 256b
            //so you need to make sure that your private key has a proper length

            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256);


            //Create a Token

            var header = new JwtHeader(credentials);

            //Token will be good for 1 mins
            DateTime Expiry = DateTime.UtcNow.AddMinutes(1);
            int ts = (int)(Expiry - new DateTime(1970, 1, 1)).TotalSeconds;


            //Some Paayload that containts infomration about the customer

            var payload = new JwtPayload
            {
                {"sub","NordeaJWTToken" },
                {"Name","Hemant" },
                {"email","hemat.1666@gmail.com" },
                {"exp",ts },
                { "iss","http://localhost:5001"}, // Issuer - The party generating the JWT (Like FG/AAD)
                { "aud","http://localhost:5001"} // Audience - The address of the resource
            };

            var secToken = new JwtSecurityToken(header, payload);

            var handler = new JwtSecurityTokenHandler();

            var tokenString = handler.WriteToken(secToken); //securityToken

            Log.Information($"Jwt Token Generated:{tokenString}");

            return tokenString;




        }

    }
}
