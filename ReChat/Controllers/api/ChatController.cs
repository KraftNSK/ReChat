using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ReChat.Models;


namespace ReChat.Controllers
{
    public class ChatController : ApiController
    {

        //Используем для опроса сервера о новых сообщениях
        public HttpResponseMessage POST(RequestNewMsg message)
        {
            if (message.LastMessageID < -1)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);


            User = DBase.GetUserByToken(message.Token);

            if (User != null)
                return Request.CreateResponse<List<ServerMessage>>(HttpStatusCode.OK, Chat.GetLastNewServerMessages(message.LastMessageID, User));
            else
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        //Используем для приема нового сообщения от пользователя в чат
        public HttpResponseMessage Put(ClientMessage message)
        {


            if (message == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            if (message.Text == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            User = DBase.GetUserByToken(message.Token);
            if (User != null)
            {
                Chat.AddClientMessage(message, User);
                return Request.CreateResponse<List<ServerMessage>>(HttpStatusCode.OK, Chat.GetLastNewServerMessages(message.LastMessageID, User));
            }
            else
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }


    }
}
