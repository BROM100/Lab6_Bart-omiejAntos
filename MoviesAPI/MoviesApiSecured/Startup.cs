﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using MoviesApi.Models;

namespace MoviesApi
{
    public class Startup
    {
        private const string APIKEYNAME = "ApiKey";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            
            services.AddDbContext<MoviesContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MoviesApiContext"));
                options.EnableSensitiveDataLogging();
            });

            //add swagger generator
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MoviesApi",
                    Version = "v1",
                    Description = "Moje API do obsługi zadań",
                    Contact = new OpenApiContact { Name = "BA", Email = "bartekantos@rmail.com" },
                    License = new OpenApiLicense { Name = "Github", Url = new System.Uri("https://github.com/BROM100") }
                });
                c.AddSecurityDefinition(APIKEYNAME, new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Name = APIKEYNAME,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Prosze podać zakupiony klucz do API."
                });

                var key = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = APIKEYNAME
                    },
                    In = ParameterLocation.Header
                };

                var requirement = new OpenApiSecurityRequirement
                    { { key, new List<string>() }};

                c.AddSecurityRequirement(requirement);

            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MoviesApi v1"));
            }

            app.UseHttpsRedirection();  //demo - outside Swagger ui
            app.UseRouting();            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
            });
        }
    }
}
