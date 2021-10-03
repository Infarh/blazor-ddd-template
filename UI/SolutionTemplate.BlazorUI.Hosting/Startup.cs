using System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SolutionTemplate.DAL.Context;
using SolutionTemplate.DAL.Repositories;
using SolutionTemplate.DAL.Sqlite;
using SolutionTemplate.DAL.SqlServer;

namespace SolutionTemplate.BlazorUI.Hosting
{
    public record Startup(IConfiguration Configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var db_type = Configuration["Database"];

            switch (db_type)
            {
                default: throw new InvalidOperationException($"База данных {db_type} не поддерживается");

                case "InMemory":
                    services.AddDbContext<SolutionTemplateDB>(opt => opt.UseInMemoryDatabase("SolutionTemplateDb"))
                       .AddTransient<IDbInitializer>(
                            s => new SolutionTemplateDBInitializer(
                                s.GetRequiredService<SolutionTemplateDB>(), 
                                s.GetRequiredService<ILogger<SolutionTemplateDBInitializer>>()) 
                                { Ignore = true })
                       .AddSolutionTemplateRepositories();
                    break;

                case "SqlServer":
                    services.AddSolutionTemplateDbContextSqlServer(Configuration.GetConnectionString(db_type));
                    break;

                case "Sqlite":
                    services.AddSolutionTemplateDbContextSqlite(Configuration.GetConnectionString(db_type));
                    break;
            }

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                //opt.ApiVersionReader = new MediaTypeApiVersionReader("x-api-version");
                //opt.ApiVersionReader = new QueryStringApiVersionReader("x-api-version");
                opt.ApiVersionReader = new HeaderApiVersionReader("x-api-version");

            });

            services.AddSignalR();
            services.AddMediatR(typeof(Startup));
            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "SolutionTemplate.WEB.API", Version = "v1" }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SolutionTemplate.WEB.API v1"));
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
