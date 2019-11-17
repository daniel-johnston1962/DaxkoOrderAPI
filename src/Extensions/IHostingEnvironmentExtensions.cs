using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DaxkoOrderAPI.Extensions 
{
    public static class IHostingEnvironmentExtensions
    {
        public static bool IsDevelopmentOrLocal(this IWebHostEnvironment env)
        {
            return env.IsDevelopment()
                   || env?.EnvironmentName?.ToLower() == "clone"
                   || env?.EnvironmentName?.ToLower() == "local";
        }

        public static bool IsClone(this IWebHostEnvironment env)
        {
            return env?.EnvironmentName?.ToLower() == "clone";
        }
    }
}
