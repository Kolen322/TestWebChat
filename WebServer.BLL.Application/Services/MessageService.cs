using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WebServer.BLL.Domain.Entities;
using WebServer.BLL.Domain.Invariance;
using WebServer.BLL.Domain.Repositories;
using WebServer.BLL.Domain.Services;

namespace WebServer.BLL.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly ILogger<MessageService> _logger;
        private readonly IMessageRepository _messageRepository;

        public MessageService(ILogger<MessageService> logger, IMessageRepository messageRepository)
        {
            _logger = logger;
            _messageRepository = messageRepository;
        }

        public IMessage AddMessage(Message message)
        {
            message.DateTime = DateTime.Now;
            return _messageRepository.AddMessage(message);
        }

        public IEnumerable<IMessage> GetMessagesBetweenDates(DateTime from, DateTime to)
        {
            return _messageRepository.GetMessagesBetweenDate(from, to);
        }
    }
}
