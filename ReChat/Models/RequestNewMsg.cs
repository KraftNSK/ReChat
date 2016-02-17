
namespace ReChat.Models
{
    /// <summary>
    /// Модель используется клиентом при периодическом опросе сервера на предмет новых сообщений
    /// </summary>
    public class RequestNewMsg
    {
        public int LastMessageID { get; set; }
        public string Token { get; set; }
    }
}
