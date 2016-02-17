using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ReChat.Helpers;
using ReChat.Models;

namespace ReChat.Controllers
{
    public class AccountController : ApiController
    {
        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        public HttpResponseMessage POST(Enter msg)
        {
            if (msg == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            List<ResponseError> err = new List<ResponseError>();

            if (msg.Login == "" || msg.Password == "")
                err.Add(new ResponseError() { Key = "password", Text = "Пустые логины и пароли недопустимы!" });

            ReChat.User u = DBase.GetUser(msg.Login, Security.getMd5Hash(msg.Password));

            if (u == null)
                err.Add(new ResponseError() { Key = "login", Text = "Ошибка авторизации!" });

            if (err.Count>0)
                return Request.CreateResponse<List<ResponseError>>(HttpStatusCode.BadRequest, err);

            TokenMessage tm = new TokenMessage();
            tm.Token = u.Token;
            tm.Login = u.Login;

            User = u;

            ClientMessage m = new ClientMessage();
            m.DT = DateTime.Now;
            m.LastMessageID = -1;
            m.Text = "В чат вошел " + u.Login + "...";
            m.Token = u.Token;

            Chat.AddClientMessage(m, User);

            return Request.CreateResponse<TokenMessage>(HttpStatusCode.OK, tm);
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public HttpResponseMessage DELETE(Enter msg)
        {
            if (msg == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            List<ResponseError> err = new List<ResponseError>();

            if (msg.Login == "" || msg.Password == "")
                err.Add(new ResponseError() { Key = "empty", Text = "Пустые логины и пароли недопустимы!" }); 

            ReChat.User u = DBase.GetUser(msg.Login, Security.getMd5Hash(msg.Password));

            if (u == null)
                err.Add(new ResponseError() { Key = "Error", Text = "Ошибка авторизации!" }); 

            if (err.Count > 0)
                return Request.CreateResponse<List<ResponseError>>(HttpStatusCode.BadRequest, err);

            try
            {
                DBase.DeleteUser(u);
            }
            catch
            {
                err.Add(new ResponseError() { Key = "db", Text = "Ошибка работы с базой данных!" });
            }

            if (err.Count > 0)
                return Request.CreateResponse<List<ResponseError>>(HttpStatusCode.BadRequest, err);

            return Request.CreateResponse<String>(HttpStatusCode.OK, "Данные о пользователе '" + msg.Login + "' успешно удалены.");
        }
    }
}
