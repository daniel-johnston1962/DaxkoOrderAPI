using AutoMapper;
using DaxkoOrderAPI.Data;
using DaxkoOrderAPI.Data.Orders;
using DaxkoOrderAPI.Features.Handlers;
using DaxkoOrderAPI.Repositories;
using DaxkoOrderAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Serilog;
using System;
using System.IO;
using DaxkoOrderAPI.Mapper;
using DaxkoOrderAPI.Features.Commands;
using DaxkoOrderAPI.Models.Validations;

namespace DaxkoOrderAPI.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddDbContexts(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddDbContext<DaxkoDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Daxko"))
                    .EnableSensitiveDataLogging(environment.IsDevelopment())
                    .UseLazyLoadingProxies());
        }

        public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            // where http client information to get to another API
            // example
            //services.AddHttpClient<IUserApiClient, UserApiClient>(client =>
            //{
            //    var serviceProvider = services.BuildServiceProvider();
            //    var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            //    var bearerToken = httpContextAccessor.HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token").Result;
            //    client.BaseAddress = new Uri(configuration["ApiStrings:SomeApiBaseUrl"]);
            //    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");
            //});
        }

        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddCors();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
                loggingBuilder.AddSerilog();
            });

            services.AddMvc(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                o.Filters.Add(new AuthorizeFilter(policy));
                o.EnableEndpointRouting = false;
            })
            .AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {

            // AddTransient
            services.AddTransient<IOrderItemHandler, OrderItemHandler>();
            services.AddTransient<ISaveOrderHandler, SaveOrderHandler>();
            services.AddTransient<IPastOrderHandler, PastOrderHandler>();
            services.AddTransient<IItemIDHandler, ItemIDHandler>();

            // AddSingleton
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // AddScoped
            services.AddScoped<IRepository<Item>, DaxkoRepository<Item>>();
            services.AddScoped<IRepository<OrderDetail>, DaxkoRepository<OrderDetail>>();
            services.AddScoped<IRepository<ShippedOrder>, DaxkoRepository<ShippedOrder>>();
            services.AddScoped<IHttpContextService, HttpContextService>();
            services.AddScoped(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            // AutoMapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new OrderItemDtoMapper());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddHttpContextAccessor();
        }

        //public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration, string apiName)
        //{
        //    services.AddAuthentication(o =>
        //    {
        //        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //    })
        //    .AddJwtBearer(o =>
        //    {
        //        o.Authority = configuration["Identity:Authority"];
        //        o.Audience = apiName;
        //        o.RequireHttpsMetadata = false;
        //        o.SaveToken = true;
        //    });
        //}

        public static void AddValidatorServices(this IServiceCollection services)
        {
            services.AddTransient<IValidator<OrderCommand>, OrderCommandValidator>();
        }

        public static void AddSwaggerServices(this IServiceCollection services, string ApplicationName, string ApplicationVersion)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ApplicationVersion, new OpenApiInfo
                {
                    Version = ApplicationVersion,
                    Title = ApplicationName,
                    Description = $"{ApplicationName} Documentation"
                });

                c.IncludeXmlComments(GetXmlCommentsPath(ApplicationName));
            });
        }
        private static string GetXmlCommentsPath(string ApplicationName)
        {
            var xmlFile = $"{ApplicationName}.xml";
            return Path.Combine(AppContext.BaseDirectory, xmlFile);
        }
    }
}
