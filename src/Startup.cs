using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Serilog;
using DaxkoOrderAPI.Extensions;

namespace DaxkoOrderAPI
{
    public class Startup
    {
        private const string API_NAME = "DaxkoOrderAPI";
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public string ApplicationName { get; }
        public string ApplicationVersion { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
            ApplicationName = Assembly.GetExecutingAssembly().GetName().Name;
            ApplicationVersion = "v1";
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));  // for a single project
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());  // for a solution with many projects
            services.AddDbContexts(Configuration, Environment);
            services.AddApplicationServices();
            services.AddValidatorServices();
            services.AddServices(Configuration);
            services.AddSwaggerServices(ApplicationName, ApplicationVersion);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.RollingFile("logs/" + ApplicationName + "_log_{Date}.log")
                .WriteTo.Seq("http://bhm-svc01.adtrav.com:5341")
                .CreateLogger()
                .ForContext("ApplicationName", ApplicationName)
                .ForContext("Environment", env.EnvironmentName);

            app.UseGlobalExceptionHandling(loggerFactory);

            if (env.IsDevelopment() || env.IsClone())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.AddApplicationBuilderCollection(ApplicationName, ApplicationVersion);
        }
    }
}
