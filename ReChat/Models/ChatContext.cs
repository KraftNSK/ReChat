using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;

namespace ReChat.Models
{
    public class ChatContext: DbContext
    {
        public ChatContext(): base("Server=(localdb)\\mssqllocaldb;Database=ReChat;Trusted_Connection=True;MultipleActiveResultSets=true")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ChatContext>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ChatContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
