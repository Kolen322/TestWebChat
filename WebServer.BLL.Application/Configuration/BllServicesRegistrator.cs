using Microsoft.Extensions.DependencyInjection;
using WebServer.BLL.Application.Services;
using WebServer.BLL.Domain.Services;

namespace WebServer.BLL.Application.Configuration
{
    public static class BllServicesRegistrator
    {
        public static void AddBll(this IServiceCollection services)
        {
            services.AddTransient<IMessageService, MessageService>();
        }
    }
}
