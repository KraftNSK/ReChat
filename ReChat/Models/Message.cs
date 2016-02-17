using System;

namespace ReChat.Models
{
    /// <summary>
    /// Хранит одно сообщение чата на серверной стороне
    /// </summary>
    public class Message
    {
        public int Id { get; set; }
        public DateTime DT { get; set; }
        public User User { get; set; }
        public string Text { get; set; }

    }
}
