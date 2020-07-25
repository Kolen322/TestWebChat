using System;

namespace WebServer.BLL.Domain.Invariance
{
    public interface IMessage
    {
        public int Id { get; }
        public string Content { get; }
        public DateTime DateTime { get; }
        public int Number { get; }
    }
}
