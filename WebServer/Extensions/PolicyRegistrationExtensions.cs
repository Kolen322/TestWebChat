using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using WebServer.Configuration;

namespace WebServer.Extensions
{
    public static class PolicyRegistrationExtensions
    {
        public static void AddRetryPolicy(this IServiceCollection services, PolicyOptions options)
        {
            ISyncPolicy waitAndRetryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(options.NumberOfRetry, (counter, time) =>
                {
                    return TimeSpan.FromSeconds(5 + counter / 3);
                });

            services.AddSingleton(waitAndRetryPolicy);
        }
    }
}
