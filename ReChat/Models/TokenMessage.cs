
namespace ReChat.Models
{
    /// <summary>
    /// Модель передаваемая клиенту в случае успешной аутентификации
    /// </summary>
    public class TokenMessage
    {
        public string Login { get; set; }
        public string Token { get; set; }
        public string Cookie { get; set; }
    }
}
