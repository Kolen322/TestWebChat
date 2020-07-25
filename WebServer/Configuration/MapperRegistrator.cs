using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace WebServer.Configuration
{
    public static class MapperRegistrator
    {
        public static void AddMapper(this IServiceCollection services)
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddApi());

            services.AddSingleton(mapper.CreateMapper());
        }

        public static IMapperConfigurationExpression AddApi(this IMapperConfigurationExpression expression)
        {
            return new ApiMapperConfigurator(expression).AddConfiguration();
        }
    }
}
