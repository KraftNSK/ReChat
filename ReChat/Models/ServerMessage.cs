using System;

namespace ReChat.Models
{
    /// <summary>
    /// Модель одного сообщения чата для отправки клиенту
    /// </summary>
    public class ServerMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime DT { get; set; }
    }
}
