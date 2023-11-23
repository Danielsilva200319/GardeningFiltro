using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.UnitOfWork;
using AspNetCoreRateLimit;
using Domain.Interfaces;

namespace Api.Extension
{
    public static class ApplicationServiceExtensions
    {
        public static void ConfigureCore(this IServiceCollection services) =>
    services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
    });
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        public static void ConfigureRatelimiting(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddInMemoryRateLimiting();
            services.Configure<IpRateLimitOptions>(Options =>
            {
                Options.EnableEndpointRateLimiting = true;
                Options.StackBlockedRequests = false;
                Options.HttpStatusCode = 429;
                Options.RealIpHeader = "X-Real-Ip";
                Options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule{
                        Endpoint="*",
                        Period="10s",
                        Limit=10
                    }
                };
            });
        }
    }
}