
namespace ReChat.Models
{
    /// <summary>
    /// Модель для аутентификации и получения токена.
    /// Отправляется клиентом.
    /// </summary>
   public class Enter
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Cookie { get; set; }
    }
}
