var CHAT = {};
var OPTIONS = {};

function Login(data) {
    if (data.success) {
        $("#login_modal").modal('hide');
        $("#userName").html(data.name);
        // Имя текущего пользователя
        OPTIONS.MyName = data.name;
        // Ключ комнаты чата
        OPTIONS.RoomKey = 'MAIN';
        // Прокси-объект чата
        CHAT = $.connection.chat;

        //// Обработчик получения сообщения
        //CHAT.OnSend = OnSend;
        //// Обработчик присоединения к комнате
        //CHAT.OnJoinRoom = OnJoinRoom;

    } else {
        $("#login").addClass("error");
        $("login-error").html(data.errors["login"]).show();
    }

    $('#users').delegate('li', 'click', SelectUser);

    //$("#msg").keydown(function (e) {
    //    if (e.keyCode == 13 && !e.shiftKey) {
    //        Send();
    //        return false;
    //    }
    //    return true;
    //});

    $.connection.hub.start(function () {
        CHAT.joinRoom(OPTIONS.RoomKey, OPTIONS.MyName);
    });
}

function SetName() {
    $("#login_modal").modal();
    $("#login").focus();
}

function SelectUser() {
    alert("user clicked");
}