
namespace ReChat.Models
{
    /// <summary>
    /// Модель для регистрации нового пользователя. Отправляется клиентом
    /// </summary>
    public class Registration
    {
        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

    }
}

