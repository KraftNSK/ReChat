using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReChat.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public DateTime RegDate { get; set; }
        public string Login { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Cookies { get; set; }
        public string Password { get; set; }
        public bool IsBaned { get; set; }
        public Role Role { get; set; }
    }
}
