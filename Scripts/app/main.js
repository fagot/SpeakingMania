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
        HUB.client.OnJoinRoom = OnJoinRoom;
        HUB.client.OnUpdateUsers = OnUpdateUsers;
        HUB.client.OnUpdateRooms = OnUpdateRooms;


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

function Room(data) {
    HUB.client.OnCreateRoom = OnCreateRoom;
}



function SetName() {
    $("#login_modal").modal();
    $("#login").focus();
}
function OnJoinRoom(key) {
    //alert("join");
    OPTIONS.MyKey = key;
    $("#userId").val(key);

}

function OnUpdateUsers(data) {
    
    var usersList = $("#users"),
            newList = "";
    for (var i = 0; i < data.length; i++) { 
        var user = data[i];
        newList += '<li data-key="' + user.UserIdentity + '"><i class="icon-user" style="color:#40FF00"></i>' + user.UserName + '</li>';
        
    }
   //alert(usersList);
    usersList.html(newList);
}

function OnCreateRoom() {
    alert("room create");
}
function OnUpdateRooms(data) {
}

function SelectUser() {
    alert("user clicked");
}