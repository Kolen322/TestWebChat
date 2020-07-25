using Microsoft.Extensions.DependencyInjection;
using WebServer.BLL.Domain.Repositories;
using WebServer.DAL.Repositories;

namespace WebServer.DAL.Configuration
{
    public static class DalServicesRegistrator
    {
        public static void AddDal(this IServiceCollection services)
        {
            services.AddTransient<IMessageRepository, MessageRepository>();
        }
    }
}
