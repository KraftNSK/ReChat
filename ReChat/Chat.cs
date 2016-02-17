using System.Collections.Generic;
using System.Security.Principal;

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


        public static List<ServerMessage> GetLastNewServerMessages(int idLastMessage, IPrincipal user)
        {
            if (idLastMessage < -1 || idLastMessage>=messages.Count-1 || AmountLastMessages<=0) return null;
            

            int count = messages.Count;
            int startID = 0;

            if (count - idLastMessage > AmountLastMessages)
                startID = count - AmountLastMessages;
            else
                startID = idLastMessage + 1;


            List<ServerMessage> SMessages = new List<ServerMessage>();

            for (int i = startID; i < count; i++)
            {
                ServerMessage sm = new ServerMessage();
                sm.DT = messages[i].DT;
                sm.Id = messages[i].Id;
                sm.Name = messages[i].User.Login;
                sm.Text = messages[i].Text;

                SMessages.Add(sm);
            }

            return SMessages;
        }

        public static void AddClientMessage(ClientMessage msg, IPrincipal user)
        {
            if (msg.Token == null || msg.Token == "") return;

            if (user != null)
            {
                Message ms = new Message();
                ms.DT = msg.DT;
                ms.Text = msg.Text;
                ms.User = (User)user;
                ms.Id = 0;
                messages.Add(ms);
                messages[messages.Count - 1].Id = messages.Count - 1;

                DBase.AddToLog(messages[messages.Count - 1]);
                if(messages.Count % SaveToDBCount == 0)
                    DBase.SaveLogsToDB();
            }
        }

    }
}
