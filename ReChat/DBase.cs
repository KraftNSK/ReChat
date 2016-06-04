using System.Linq;
using ReChat.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ReChat
{
    public static class DBase
    {
        private static ChatContext eLog = new ChatContext();

        private static object _sync = new object();

        //Кэшируем пользователей
        private static ConcurrentDictionary<string, User> users = new ConcurrentDictionary<string, User>();

        private static List<Log> logs = new List<Log>();
        private static int logCount = 0;

        static DBase()
        {

        }

        /// <summary>
        /// Добавление сообщения чата в очередь на запись в БД
        /// </summary>
        /// <param name="msg"></param>
        public static void AddToLog(Message msg)
        {
            lock (_sync)
            {
                logs.Add(new Log
                {
                    User = msg.User,
                    Text = msg.Text,
                    DT = msg.DT
                });
                logCount++;
            }

            if (logCount%100 == 0)
                lock (_sync)
                {
                    eLog.Logs.AddRange(logs);
                    eLog.SaveChanges();
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
                    User k = entity.Users.First(u => u.Login == login && u.Password == password);
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
                    User k = entity.Users.First(u => u.Token == token);

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
