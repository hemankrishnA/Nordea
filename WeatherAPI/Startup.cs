using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeatherAPI.Handlers;

namespace WeatherAPI
{
    public class Startup
    {
        private const string SECRET_KEY = "XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234XXX1234"; // TODO : To add this secret in enviornment varaible 
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));


        public Startup(IConfiguration configuration)
        {
            Log.Information("Configuring Application Configuration....");
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {

                Log.Information("Configuring Services....");
                services.AddControllers();
                services.AddSingleton<IWeatherServiceHandler, WeatherServiceHandler>();
                services.AddSingleton<IDBHandler, DBHandler>();

                //JWT Authentication
                services.AddAuthentication(options =>
               {
                   options.DefaultAuthenticateScheme = "JwtBearer";
                   options.DefaultChallengeScheme = "JwtBearer";
               })
                    .AddJwtBearer("JwtBearer", jwtOptions =>
                     {
                         jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                         {
                         //The signingkey is defined in the token controller class
                         IssuerSigningKey = SIGNING_KEY,
                             ValidateIssuer = true,
                             ValidateAudience = true,
                             ValidIssuer = "http://localhost:5001", // TODO : Add this url in appsettings under JWT section
                         ValidAudience = "http://localhost:5001", // TODO : Add this url in appsettings under JWT section 
                         ValidateLifetime = true
                         };
                     });

                //swagger configurations
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Nordea Weather API", Version = "v1", Description = "---API Discription ---", Contact = new Microsoft.OpenApi.Models.OpenApiContact { Name = "Hemant Krishna", Email = "hemant.1666@gmail.com" } });
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                    });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
        });
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                });

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Configuration error in configure services method...");
                throw;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseHttpsRedirection();

                app.UseRouting();

                app.UseAuthentication();//Enable JWT Authentication

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("../swagger/v1/swagger.json", "Nordea Weather API");
                });
            }
            catch (Exception ex)
            {
                Log.Fatal("Configuration error in Configure method ...");
                throw;
            }
        }
    }
}
