using System.Linq;
using ReChat.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ReChat
{
    public static class DBase
    {
        private static object _sync = new object();

        private static ChatContext eLog;

        //Кэшируем пользователей
        private static ConcurrentDictionary<string, User> users = new ConcurrentDictionary<string, User>();

        private static List<Log> logs = new List<Log>();
        private static int logCount = 0;
        private static readonly int MaxCache = 50;

        static DBase()
        {

        }

        /// <summary>
        /// Добавление сообщения чата в очередь на запись в БД
        /// </summary>
        /// <param name="msg"></param>
        public static async Task AddToLogAsync(Message msg)
        {
            lock (_sync)
            {
                logs.Add(new Log
                {
                    Text = msg.Text,
                    DT = msg.DT,
                    UserId = msg.User.Id
                    
                });
                logCount++;

                if (logCount == MaxCache)
                {
                    eLog = new ChatContext();
                    eLog.Logs.AddRange(logs);
                    logs.Clear();
                }
            }

            if (logCount == MaxCache)
            {
                logCount = 0;
                eLog.SaveChangesAsync();
            }



        }

        /// <summary>
        /// Запись сообщений в базу
        /// </summary>
        /// <returns>true - если запись прошла успешно</returns>
        public static bool UserExist(string login)
        {
            using (var entity = new ChatContext())
            {
                try
                {
                    User k = entity.Users.First(u => u.Login == login);
                    return k != null;
                }
                catch
                {
                    return false;
                };
            }
        }

        public static bool EmailUsed(string email)
        {
            using (var entity = new ChatContext())
            {
                try
                {
                    User k = entity.Users.First(u => u.Email == email);
                    return k != null;
                }
                catch
                {
                    return false;
                };
            }
        }
        
        public static User GetUser(string login, string password)
        {
            using (var entity = new ChatContext())
            {
                try
                {
                    User k = entity.Users.Include(c => c.Role).First(u => u.Login == login && u.Password == password);
                    return k;
                }
                catch
                {
                    return null;
                };
            }
        }

        public static User GetUserByCookeis(string coockies)
        {
            using (var entity = new ChatContext())
            {
                try
                {
                    User k = entity.Users.First(u => u.Cookies == coockies);
                    return k;
                }
                catch
                {
                    return null;
                };
            }
        }

        public static User GetUserByToken(string token)
        {
            if (users.ContainsKey(token))
                return users[token];

            using (var entity = new ChatContext())
            {
                try
                {
                    User k = entity.Users.Include(c => c.Role).First(u => u.Token == token);

                    users.TryAdd(token, k);

                    return k;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static bool AddUser(User user)
        {
            using (var entity = new ChatContext())
            {
                try
                {
                    entity.Users.Add(user);
                    entity.SaveChanges();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public static bool UpdateUser(User user)
        {

            using (var entity = new ChatContext())
            {
                User k = entity.Users.First(u => u.Id == user.Id);
                k.Email = user.Email;
                k.Role = user.Role;
                k.IsBaned = user.IsBaned;
                k.LastName = user.LastName;
                k.Password = user.Password;
                k.Token = user.Token;
                try
                {
                    entity.SaveChanges();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public static bool DeleteUser(User user)
        {

            using (var entity = new ChatContext())
            {
                User k = entity.Users.First(u => u.Id == user.Id);
                if (k != null)
                {
                    try
                    {
                        entity.Users.Remove(k);
                        entity.SaveChanges();
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
    }


}
