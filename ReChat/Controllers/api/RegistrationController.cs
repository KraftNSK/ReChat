using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ReChat.Models;
using ReChat.Helpers;

namespace ReChat.Controllers
{
    public class RegistrationController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Registration(Registration msg)
        {
            if(msg == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            List<ResponseError> err = new List<ResponseError>();

            //Проверяем основные поля на заполненность, на случай, если такой проверки нет на клиенте
            if (msg.Login == null)
                err.Add(new ResponseError() { Key = "email", Text = "Не указан логин!" });

            if (msg.Email==null)
                err.Add(new ResponseError() { Key = "email", Text = "Не указан почтовый ящик!" });

            if (msg.Login == null)
                err.Add(new ResponseError() { Key = "pwd", Text = "Не указан пароль!" });

            //Дальше нет смысла проверять, если есть вышеуказанные ошибки
            if (err.Count > 0)
                return Request.CreateResponse<List<ResponseError>>(HttpStatusCode.BadRequest, err);

            /*                НЕ РАБОТАЕТ проверка на сответствие формату

            pattern = @"^[a-zа-яё][a-z_а-яё0-9]++$";
            if (msg.Login == "" || Regex.IsMatch(msg.Login, pattern, RegexOptions.IgnoreCase))
                ModelState.AddModelError("login", "Неверный формат логина!");

            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

            if (msg.Email != "" || !Regex.IsMatch(msg.Email, pattern, RegexOptions.IgnoreCase))
                ModelState.AddModelError("email", "Неверный формат адреса электронной почты!");

            */

            if (DBase.EmailUsed(msg.Email))
                err.Add(new ResponseError() { Key = "email", Text = "Почтовый ящик уже используется!" });

            if (DBase.UserExist(msg.Login))
                err.Add(new ResponseError() { Key = "loginreg", Text = "Логин занят!" });

            if (msg.Password.Length<5)
                err.Add(new ResponseError() { Key = "pwd", Text = "Пароль должен быть не короче 5 символов!" });

            if(err.Count > 0)
                return Request.CreateResponse<List<ResponseError>>(HttpStatusCode.BadRequest, err);

            ReChat.User user = new ReChat.User();
            user.Login = msg.Login;
            user.FirstName = msg.FirstName != null ? msg.FirstName : "";
            user.LastName = msg.LastName != null ? msg.LastName : "";
            user.Email = msg.Email;
            user.RegDate = DateTime.Now; 
            user.Cookies = Guid.NewGuid().ToString(); 
            user.IsBaned = false;
            user.Token = Security.GetToken();
            user.Password = Security.getMd5Hash(msg.Password);

            try
            {
                user.idRole = (new Entities()).Roles.First(r => r.RoleName == "user").Id;
                DBase.AddUser(user);
            }
            catch
            {
                err.Add(new ResponseError() { Key = "loginreg", Text = "Ошибка работы с базой данных!" });
            }

            if (err.Count > 0)
                return Request.CreateResponse<List<ResponseError>>(HttpStatusCode.BadRequest, err);

            return Request.CreateResponse<String>(HttpStatusCode.OK, "User is created!");
        }

    }
}
