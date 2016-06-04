using System.Collections.Generic;
using ReChat.Models;

namespace ReChat.Models
{
   static class Chat
    {
        //Хранение сообщений
        private static List<Message> messages;

        //Количество последних загружаемых сообщений из чата при подключении
        public static int AmountLastMessages { get; set; } = 15;

        //После каждых SaveToDBCount сообщений, будет происходить сохранение их в БД
        public static int SaveToDBCount { get; set; } = 3;
        static Chat()
        {
            messages = new List<Message>();
        }


        public static List<ServerMessage> GetLastNewServerMessages(int idLastMessage, User user)
        {
            if (idLastMessage < -1 || idLastMessage>=messages.Count || AmountLastMessages<=0) return null;

            List<ServerMessage> SMessages = new List<ServerMessage>();

            lock (messages)
            {
                int count = messages.Count;
                int startID = 0;

                if (count - idLastMessage > AmountLastMessages)
                    startID = count - AmountLastMessages;
                else
                    startID = idLastMessage+1;
                
                for (int i = startID; i < count; i++)
                {
                    ServerMessage sm = new ServerMessage
                    {
                        DT = messages[i].DT,
                        Id = messages[i].Id,
                        Name = messages[i].User.Login,
                        Text = messages[i].Text + " ("+messages[i].Id.ToString()+")"
                    };

                    SMessages.Add(sm);
                }

            }

            return SMessages;
        }

        public static void AddClientMessage(ClientMessage msg, User user)
        {
            if (string.IsNullOrEmpty(msg.Token)) return;

            if (user == null) return;

            Message ms = new Message
            {
                DT = msg.DT,
                Text = msg.Text,
                User = user,
                Id = 0
            };

            lock (messages)
            {
                messages.Add(ms);
                messages[messages.Count - 1].Id = messages.Count - 1;
            }

            DBase.AddToLog(ms);
        }

    }
}
