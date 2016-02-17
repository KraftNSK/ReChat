ChatUrl = "/api/chat";
AccountUrl = "/api/Account"
RegUrl = "/api/Registration"
ChatForm = document.getElementById('ChatForm');
Msgs = ko.observableArray([]);
LastMessageID = -1;
tokenKey = "tokenInfo";
isAuth = false;
MyLogin = LoginString = '';
arrErr = []; //массив ошибок с id
Wait = false;

frmR = document.getElementById("frmReg");
frmL = document.getElementById("frmLogin");
frmChat = document.getElementById("frmChat");


ko.applyBindings(new ChatViewModel());

//Модель для хранения входящих ошибок
function ResponseError(data) {
    this.key = data.key;
    this.text = data.text;
}

//Модель серверного сообщения
function MsgServer(data) {
    this.msg = data.text;
    this.name = data.name;
    this.dt = data.dt;
    this.id = data.id;
}


/////////////////  Вспомогательные  \\\\\\\\\\\\\\\\\

//Подсветка ошибочных полей
function SetInputValidError(inputID, errorMsg, inputClass, errorMsgClass) {
    $('#' + inputID).removeClass();
    $('#' + inputID).addClass(inputClass);

    $('#' + inputID).next('span').text(errorMsg);
    $('#' + inputID).next('span').removeClass();
    $('#' + inputID).next('span').addClass(errorMsgClass)
}

//Удаление подсветки с ошибочных полей
function DelInputValidError(inputID, errorMsg, inputClass, errorMsgClass) {
    $('#' + inputID).removeClass(inputClass);

    $('#' + inputID).next('span').text('');
    $('#' + inputID).next('span').removeClass(errorMsgClass);
}

function AddElements(newData) {
    var i;
    var obj;
    for (i = 0; i < newData.length; i++) {
        obj = {
            id: newData[i].id,
            name: newData[i].name,
            msg: newData[i].msg,
            dt: newData[i].dt
        }

        Msgs.push(obj)
    }
}

//Форматирование даты для вывода в окно чата
function FormatDate(date) {
    var d = new Date(date);
    h = d.getHours().toString();
    m = d.getMinutes().toString();
    s = d.getSeconds().toString();

    if (h.length < 2) h = "0" + h;
    if (m.length < 2) m = "0" + m;
    if (s.length < 2) s = "0" + s;

    return h + ":" + m + ":" + s;
}

//Запрос новых сообщений с сервера
function GetMessages() {
    var d = {
        LastMessageID: LastMessageID,
        token: sessionStorage.getItem(tokenKey)
    };
    if (Wait) return;
    Wait = true;
    $.ajax({
        url: ChatUrl,
        type: 'POST',
        dataType: "json",
        data: d,
        success: function (data, textStatus) {
            var messages = $.map(data, function (msg) {
                var m = new MsgServer(msg);
                m.dt = FormatDate(m.dt);
                LastMessageID = msg.id;
                return m;
            });
            AddElements(messages);
            if (messages.length > 0)
                ChatForm.scrollTop = 99999;
            Wait = false;
        },
        error: function (jqxhr, status, errorMsg) {
            Wait = false;
        }
    });
}

//Просить данные каждые 250мс
function startTimer() {
    if (isAuth) {
        GetMessages();
        setTimeout(startTimer, 250);
    } else {
        setTimeout(startTimer, 250);
    }
}

//Вью-модель чата
function ChatViewModel() {
    var self = this;

    this.addMessageControl = {
        myMessage: ko.observable(),
        myDT: function () {
            return new Date().toJSON()
        },

        addMessage: function (context) {
            
            if ($("#myMessage").val() == '') return;
            value = {
                Token: sessionStorage.getItem(tokenKey),
                text: context.myMessage(),
                dt: context.myDT(),
                LastMessageID: LastMessageID,
            };

            if (Wait) return;
            Wait = true;
            $.ajax({
                url: ChatUrl,
                type: 'PUT',
                dataType: "json",
                data: value,
                success: function (data, textStatus) {
                    var messages = $.map(data, function (msg) {
                        var m = new MsgServer(msg);
                        m.dt = FormatDate(m.dt);
                        LastMessageID = msg.id;
                        return m;
                    });
                    AddElements(messages);
                    ChatForm.scrollTop = 99999;
                    Wait = false;
                    $("#myMessage").val('');
                },
                error: function (jqxhr, status, errorMsg) {
                Wait = false;
            }

            });

        }
    }
}


/////////////////  Обработка событий  \\\\\\\\\\\\\\\\\

//Обработка нажатия Enter инпута чата
$("#myMessage").keyup(function (event) {
    if (event.keyCode == 13) {
        $("#btnSend").click();
    }
});

//Возврат фокуса инпуту чата
$("#btnSend").click(function (event) {
    $("#myMessage").focus();
});

//Нажата конопка отправки регистрационных данных
$("#btnRegSend").on('click',  function () {
    for (i = 0; i < arrErr.length; i++) {
        DelInputValidError(arrErr[i].key, '  ' + arrErr[i].text, 'input-validation-error', 'field-validation-error');
    }
    arrErr = [];
    if ($('#pwd').val() != $('#pwd2').val()) {
        arrErr.push(new ResponseError({ 'key': 'pwd', 'text': 'Пароли не совпадают!' }));
    }

    if ($('#pwd').val().length < 5) {
        arrErr.push(new ResponseError({ 'key': 'pwd', 'text': 'Пароль должен быть не менее 5 символов!' }));
    }

    if ($('#email').val() == '') {
        arrErr.push(new ResponseError({ 'key': 'email', 'text': 'Укажите почту!' }));
    }

    if ($('#loginreg').val() == '') {
        arrErr.push(new ResponseError({ 'key': 'loginreg', 'text': 'Укажите логин!' }));
    }
    if (arrErr.length > 0) {
        for (i = 0; i < arrErr.length; i++) 
            SetInputValidError(arrErr[i].key, '  ' + arrErr[i].text, 'input-validation-error', 'field-validation-error');
        return;
    }


    var d = {
        login: $('#loginreg').val(),
        password: $('#pwd').val(),
        firstname: $('#firstname').val(),
        lastname: $('#lastname').val(),
        email: $('#email').val()
    };
    if (Wait) return;
    Wait = true;
    $.ajax({
        url: RegUrl,
        type: 'POST',
        dataType: "json",
        data: d,
        success: function (data, textStatus) {
            frmR.style.display = "none";
            frmL.style.display = "block";
            frmChat.style.display = "none";
            Wait = false;

            $('#loginreg').val('');
            $('#pwd').val('');
            $('#firstname').val('');
            $('#lastname').val('');
            $('#email').val('');
            $('#pwd2').val('')

        },
        error: function (jqxhr, status, errorMsg) {
            arrErr = $.map(jqxhr.responseJSON, function (msg) {
                var m = new ResponseError(msg);
                return m;
            });
            for (i = 0; i < arrErr.length; i++) {
                SetInputValidError(arrErr[i].key, '  ' + arrErr[i].text, 'input-validation-error', 'field-validation-error');
            }
            Wait = false;
        }
    });
});

//Обработка нажатия Enter инпута логина
$("#login").keyup(function (event) {
    if (event.keyCode == 13) {
        $("#btnLogin").click();
    }
});

//Обработка нажатия Enter инпута пароля при авторизации
$("#password").keyup(function (event) {
    if (event.keyCode == 13) {
        $("#btnLogin").click();
    }
});

//Нажата конопка логина
$("#btnLogin").on('click', function () {
    for (i = 0; i < arrErr.length; i++) {
        DelInputValidError(arrErr[i].key, '  ' + arrErr[i].text, 'input-validation-error', 'field-validation-error');
    }
    arrErr = [];
    if ($('#login').val() == '') {
        arrErr.push(new ResponseError({ 'key': 'login', 'text': 'Укажите логин!' }));
    }

    if ($('#password').val().length < 5) {
        arrErr.push(new ResponseError({ 'key': 'password', 'text': 'Укажите пароль!' }));
    }

    if (arrErr.length > 0) {
        for (i = 0; i < arrErr.length; i++)
            SetInputValidError(arrErr[i].key, '  ' + arrErr[i].text, 'input-validation-error', 'field-validation-error');
        return;
    }

    var d = {
        login: $('#login').val(),
        password: $('#password').val(),
        cookie: " "
    };
    if (Wait) return;
    Wait = true;
    $.ajax({
        url: AccountUrl,
        type: 'POST',
        dataType: "json",
        data: d,
        success: function (data, textStatus) {
            cookie = data.cookie;
            sessionStorage.setItem(tokenKey, data.token);
            LoginString = "Вы работаете под " + data.login + "    ";
            MyLogin = data.login;
            isAuth = true;
            frmR.style.display = "none";
            frmL.style.display = "none";
            frmChat.style.display = "block";
            $('#nm').text(LoginString);
            Wait = false;
            GetMessages();
        },
        error: function (jqxhr, status, errorMsg) {
            arrErr = $.map(jqxhr.responseJSON, function (msg) {
                var m = new ResponseError(msg);
                return m;
            });
            for (i = 0; i < arrErr.length; i++) {
                SetInputValidError(arrErr[i].key, '  ' + arrErr[i].text, 'input-validation-error', 'field-validation-error');
            }
            Wait = false;
        }

        
    });
});

//Нажата конопка регистрации
$("#btnReg").on('click', function () {
    frmR.style.display = "block";
    frmL.style.display = "none";
    frmChat.style.display = "none";
});

//Нажата кнопка отмены регистрации
$("#btnRegCancel").on('click', function () {
    frmR.style.display = "none";
    frmL.style.display = "block";
    frmChat.style.display = "none";
});

//Нажата конопка выхода
$("#btnExit").on('click', function () {
    value = {
        Token: sessionStorage.getItem(tokenKey),
        text: MyLogin+' вышел из чата...',
        dt: function () {
            return new Date().toJSON()
        },
        LastMessageID: LastMessageID,
    };

    $.ajax({
        url: ChatUrl,
        type: 'PUT',
        dataType : "json",
        data: value})

    frmR.style.display = "none";
    frmL.style.display = "block";
    frmChat.style.display = "none";
    isAuth = false;
    sessionStorage.clear();
    MyLogin = '';
    LastMessageID = -1;
});

//Для авторизации по куки
if (!isAuth) {
    //Отображаем блок ввода логина и пароля
    //Отображаем ссылку на страницу регистрации
    frmR.style.display = "none";
    frmL.style.display = "block";
    frmChat.style.display = "none";
} else {
    frmR.style.display = "none";
    frmL.style.display = "none";
    frmChat.style.display = "block";
    //Отображаем элементы чата
}

//таймер опроса сервера на предмет новых сообщений
startTimer();

