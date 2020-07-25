using System;
using WebServer.BLL.Domain.Invariance;

namespace WebServer.BLL.Domain.Entities
{
    public class Message : IMessage
    {
        public int Id { get; set; }
        public string Content { get; set ; }
        public DateTime DateTime { get; set; }
        public int Number { get; set; }

    }
}
