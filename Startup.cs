using FotoGeoLocationWebApplication.Controllers;
using FotoGeoLocationWebApplication.Data;
using FotoGeoLocationWebApplication.GpsData;
using FotoGeoLocationWebApplication.Login;
using FotoGeoLocationWebApplication.Pictures;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Przed tokenem nale¿y dodaæ prefiks \"Bearer\""
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
            });
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
            services.AddScoped<IGpsDataExtractor, GpsDataExtractor>();//IPictureProvider
            services.AddScoped<IPictureProvider, PictureProvider>();//IPictureProvider

            services.AddControllers();
            services.AddHttpContextAccessor();
           // services.AddSingleton<IGpsDataExtractor, GpsDataExtractor>();
            // services.AddSingleton<UploadPicturesController>();
         /*   services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FotoGeoLocationWebApplication", Version = "v1" });
            });*/

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(_configuration.GetConnectionString("DefaultConnection"));
            });

           services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidIssuer = "http://localhost:45455",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("bardzotrudnehaslotokena"))
                };
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
