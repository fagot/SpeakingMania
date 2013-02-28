var HUB = {};
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
        HUB = $.connection.userHub;
        HUB.OnJoinRoom = OnJoinRoom;
        HUB.OnUpdateUsers = OnUpdateUsers;


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
        HUB.server.joinRoom(OPTIONS.RoomKey, OPTIONS.MyName);
    });
}

function SetName() {
    $("#login_modal").modal();
    $("#login").focus();
}
function OnJoinRoom(key) {
    OPTIONS.MyKey = key;
}

function OnUpdateUsers(data) {
    var usersList = $("#users ul"),
            newList = "";
    for (var i = 0; i < data.length; i++) {
        var user = data[i];
        newList += '<li data-key="' + user.Key + '"><i class="icon-user"></i>' + user.Name + '</li>';
    }
    usersList.html(newList);
}
function SelectUser() {
    alert("user clicked");
}