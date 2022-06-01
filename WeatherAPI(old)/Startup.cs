using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WeatherAPI.Handlers;

namespace WeatherAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IWeatherServiceHandler, WeatherServiceHandler>();
            services.AddSingleton<IDBHandler, DBHandler>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
            .AddJwtBearer("JwtBearer", jwtOptions =>
             {
                 jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                 {
                    //The Signingkey is defined in the token controller class
                    IssuerSignKey = SIGNING_KEY,
                     ValidationIssuer = true,
                     VadlidationAudience = true,
                     ValidIssuer = "http://localhost:5000",
                     ValidaAudiene = "http://localhost:5000",
                     ValidateLifeTime = true
                 };

             });

            //swagger configurations
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Nordea Weather API", Version = "v1", Description = "---API Discription ---", Contact = new Microsoft.OpenApi.Models.OpenApiContact { Name = "Hemant Krishna", Email = "hemant.1666@gmail.com" } });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

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
    }
}
