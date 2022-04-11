using FotoGeoLocationWebApplication.Controllers;
using FotoGeoLocationWebApplication.Data;
using FotoGeoLocationWebApplication.GpsData;
using FotoGeoLocationWebApplication.Login;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FotoGeoLocationWebApplication
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*services.AddCors(options => options.AddPolicy("wszystkoDozwolone", builder =>
              {
                 builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                 // builder.AllowAnyMethod().WithOrigins("http://192.168.55.8:4200").AllowAnyHeader().Build();//.WithOrigins("http://192.168.55.8:4200/").AllowAnyHeader().Build();
              }));*/
            services.AddLogging();

            services.AddDbContext<DataContext>(options =>
             options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            services.AddCors(options => {
                options.AddPolicy("AllowMyOrigin",
                builder => builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod());
            });

            //app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowAnyCredentials());
            services.AddScoped<IAuthorization, Authorization>();
            services.AddScoped<IRegister, Register>();
            services.AddScoped<IHashProvider, HashProvider>();
            services.AddScoped<IGpsDataExtractor, GpsDataExtractor>();
            
            services.AddControllers();
            services.AddHttpContextAccessor();
           // services.AddSingleton<IGpsDataExtractor, GpsDataExtractor>();
            // services.AddSingleton<UploadPicturesController>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FotoGeoLocationWebApplication", Version = "v1" });
            });

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(_configuration.GetConnectionString("DefaultConnection"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FotoGeoLocationWebApplication v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            //app.UseCors("wszystkoDozwolone");
            app.UseCors("AllowMyOrigin");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
