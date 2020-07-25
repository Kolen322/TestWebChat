using System;
using System.Collections.Generic;
using WebServer.BLL.Domain.Entities;
using WebServer.BLL.Domain.Invariance;

namespace WebServer.BLL.Domain.Services
{
    public interface IMessageService
    {
        IMessage AddMessage(Message message);
        IEnumerable<IMessage> GetMessagesBetweenDates(DateTime from, DateTime to);
    }
}
