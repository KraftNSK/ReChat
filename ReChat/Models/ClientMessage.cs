using System;

namespace ReChat.Models
{
    //Модель сообщения отправляемого клиентом на сервер
    public class ClientMessage
    {
        public string Token { get; set; }
        public string Text { get; set; }
        public DateTime DT { get; set; }
        public int LastMessageID { get; set; }
    }
}
