﻿using System.Linq;
using ReChat.Models;
using System.Collections.Concurrent;

namespace ReChat
{
    public static class DBase
    {
        private static Entities eLog;
        private static Log log;

        private static object _sync;

        //Кэшируем пользователей
        private static ConcurrentDictionary<string, User> users;

        static DBase()
        {
            users = new ConcurrentDictionary<string, User>();
            eLog = new Entities();
            _sync = new object();
        }
        /// <summary>
        /// Добавление сообщения чата в очередь на запись в БД
        /// </summary>
        /// <param name="msg"></param>
        public static void AddToLog(Message msg)
        {
            log = new Log();
            log.idUser = msg.User.Id;
            log.text = msg.Text;
            log.DT = msg.DT;

            lock (_sync)
            {
                eLog.Logs.Add(log);
            }
        }

        /// <summary>
        /// Запись сообщений в базу
        /// </summary>
        /// <returns>true - если запись прошла успешно</returns>
        public static bool SaveLogsToDB()
        {
            try
            {
                eLog.SaveChanges();
            }
            catch
            {
                return false;
            };

            return true;
        }

        public static bool UserExist(string login)
        {
            Entities e = new Entities();
            User k;

            try
            {
                k = e.Users.First(u => u.Login == login);
                if (k != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            };
        }

        public static bool EmailUsed(string email)
        {
            Entities e = new Entities();
            User k;

            try
            {
                k = e.Users.First(u => u.Email == email);
                if (k != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            };
        }


        public static User GetUser(string login, string password)
        {
            Entities e = new Entities();
            User k;


            try
            {
                k = e.Users.First(u => u.Login == login && u.Password == password);

                return k;
            }
            catch
            {
                return null;
            };
        }

        public static User GetUserByCookeis(string coockies)
        {
            
            Entities e = new Entities();
            User k;

            try
            {
                k = e.Users.First(u => u.Cookies == coockies);
                return k;
            }
            catch
            {
                return null;
            };
        }

        public static User GetUserByToken(string token)
        {

            Entities e = new Entities();
            User k;


            if (users.ContainsKey(token))
                return users[token];

            try
            {
                k = e.Users.First(u => u.Token == token);

                users.TryAdd(token, k);
          
                return k;
            }
            catch
            {
                return null;
            }
        }
        
        public static void AddUser(User user)
        {
            var entity = new Entities();
            
            entity.Users.Add(user);
            entity.SaveChanges();
        }

        public static void UpdateUser(User user)
        {

            using (var entity = new Entities())
            {
                User k = entity.Users.First(u => u.Id == user.Id);
                k.Email = user.Email;
                k.idRole = user.idRole;
                k.IsBaned = user.IsBaned;
                k.LastName = user.LastName;
                k.Password = user.Password;
                k.Token = user.Token;
                entity.SaveChanges();
            }
        }

        public static void DeleteUser(User user)
        {

            using (var entity = new Entities())
            {
                User k = entity.Users.First(u => u.Id == user.Id);
                entity.Users.Remove(k);
                entity.SaveChanges();
            }
        }
        
    }


}