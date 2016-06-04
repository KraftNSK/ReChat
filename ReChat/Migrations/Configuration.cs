namespace ReChat.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ReChat.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ReChat.Models.ChatContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ReChat.Models.ChatContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Roles.AddOrUpdate(
                r => r.RoleName,
                new Role { Id = 1, RoleName = "admins" },
                new Role { Id = 2, RoleName = "users" });
            context.SaveChanges();

            context.Users.AddOrUpdate(
                u => u.Email,
                new User { Email = "admin@admin.ru", FirstName = "Admin", Id = 1, Role = context.Roles.FirstOrDefault(r=>r.RoleName =="admins"), IsBaned = false, LastName = "adminovich", Login = "admin", Password = "1", RegDate = DateTime.Now, Token = "safsdfsdfs" },
                new User { Email = "u@ya.ru", FirstName = "user", Id = 2, Role = context.Roles.FirstOrDefault(r => r.RoleName == "users"), IsBaned = false, LastName = "uu", Login = "user", Password = "1", RegDate = DateTime.Now, Token = "aswwewewewew" });

        }
    }
}
