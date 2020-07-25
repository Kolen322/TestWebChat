using AutoMapper;
using WebServer.BLL.Domain.Entities;
using WebServer.Models;

namespace WebServer.Configuration
{
    public class ApiMapperConfigurator
    {
        private readonly IMapperConfigurationExpression _expression;

        public IMapperConfigurationExpression AddConfiguration() => _expression;

        public ApiMapperConfigurator(IMapperConfigurationExpression expression)
        {
            MappingApiInputMessage(expression);
            MappingApiOutputMessage(expression);
            _expression = expression;
        }

        private void MappingApiInputMessage(IMapperConfigurationExpression expression)
        {
            expression.CreateMap<ApiInputMessage, Message>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content))
                .ForMember(
                    dest => dest.Number,
                    opt => opt.MapFrom(src => src.Number))
                .ForMember(
                    dest => dest.DateTime,
                    opt => opt.Ignore());
        }

        private void MappingApiOutputMessage(IMapperConfigurationExpression expression)
        {
            expression.CreateMap<Message, ApiOutputMessage>().ReverseMap();
        }
    }
}
