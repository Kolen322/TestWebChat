using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using WebServer.BLL.Domain.Entities;
using WebServer.BLL.Domain.Services;
using WebServer.Hub;
using WebServer.Models;

namespace WebServer.Controllers
{
    [Route("api/Messages")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private IHubContext<ChatHub> _hubContext;

        public MessageController(ILogger<MessageController> logger, IMessageService messageService, IMapper mapper, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            _messageService = messageService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [HttpGet("")]
        public IActionResult GetMessagesBeetwenDates([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var messages = _messageService.GetMessagesBetweenDates(from, to);
            var entity = messages.Select(m => _mapper.Map<ApiOutputMessage>(m)).ToList();
            return Ok(entity);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateMessage([FromBody]ApiInputMessage message)
        {
            var entity = _mapper.Map<Message>(message);
            var result = _messageService.AddMessage(entity);
            await SendToChat("Message", GenerateText(result.Content, result.DateTime, result.Number));
            return Ok(_mapper.Map<ApiOutputMessage>(result));
        }

        private async Task SendToChat(string type, string message)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync(type, message);
                _logger.LogInformation("Sending message to client chat is successful");
            }
            catch (Exception)
            {
                _logger.LogError("Error during sending message to client chat");
                throw new Exception("Error during sending message to client chat");
            }
        }


        private static string GenerateText(string content, DateTime dateTime, int number)
        {
            return $"{number}\n\n{dateTime}\n\n{content}";
        }
    }
}