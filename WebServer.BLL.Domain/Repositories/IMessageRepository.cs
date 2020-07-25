using System;
using System.Collections.Generic;
using WebServer.BLL.Domain.Invariance;

namespace WebServer.BLL.Domain.Repositories
{
    public interface IMessageRepository
    {
        IEnumerable<IMessage> GetMessagesBetweenDate(DateTime from, DateTime to);
        IMessage AddMessage(IMessage message);

    }
}
