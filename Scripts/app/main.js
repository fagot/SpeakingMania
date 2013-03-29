var options = {};
var isRoomOwner = false;
function Login(data) {
    var hub = {};
    var options = {};
    if (data.success) {
        $("#login_modal").modal('hide');
        $("#userName").html(data.name);
        // Имя текущего пользователя
        options.MyName = data.name;
        // Ключ комнаты чата
        options.RoomKey = 'MAIN';
        // Прокси-объект чата
        hub = $.connection.userHub;
        hub.client.OnJoinRoom = OnJoinRoom;
        hub.client.OnUpdateUsers = OnUpdateUsers;
        hub.client.OnUpdateRooms = OnUpdateRooms;


    } else {
        $("#login").addClass("error");
        $("login-error").html(data.errors["login"]).show();
    }

    $('#users').delegate('li', 'click', SelectUser);
    $('#rooms').delegate('tr', 'click', SelectRoom);

    //$("#msg").keydown(function (e) {
    //    if (e.keyCode == 13 && !e.shiftKey) {
    //        Send();
    //        return false;
    //    }
    //    return true;
    //});
    //alert("LOGIN");
    $.connection.hub.start(function () {
        hub.server.login(options.MyName);
        hub.server.joinRoom(options.RoomKey);
        hub.server.updateRooms();
    });
}

function CreateRoom(data) {
    var hub = {};
    var options = {};
    if (data.success) {
        hub = $.connection.userHub;
        hub.client.OnUpdateRooms = OnUpdateRooms;
        options.roomName = data.roomName;
        options.roomKey = data.roomKey;
        options.userId = data.userId;
        isRoomOwner = data.isRoomOwner;
        $("#new_room_modal").modal('hide');
        hub.server.joinRoom(options.roomKey);
        hub.server.updateRooms();
        if (isRoomOwner) {
            //document.location = 'Home/Room';
        }
        
    }
    else {
        $("#roomForm").addClass("error");
        $("#room-error").html(data.errors["room"]).show();
    }
}


/* Dialogs */
function SetNameDialog() {
    $("#login_modal").modal();
    $("#login").focus();
}
function CreateRoomDialog() {
    $("#new_room_modal").modal();
    $("#roomName").focus();
}
function JoinRoomDialog() {
    $("#join_room_modal").modal();
}
/* Dialogs end*/

/* Login dialod navigation */
function SignIn() {
    $("#signInForm").show();
    $("#signUpForm").hide();
    $("#logIn").hide();
}
function SignUp() {
    $("#signInForm").hide();
    $("#signUpForm").show();
    $("#logIn").hide();
}

function LogIn() {
    $("#signInForm").hide();
    $("#signUpForm").hide();
    $("#logIn").show();
}
/* Login dialod navigation end*/

function OnJoinRoom(userId, roomKey) {
    //alert(roomKey);
    //$.post("Home/SaveUserData", { userId: key }, function (data) {
    //    //do whatever with the response

    //});
    if (roomKey != 'MAIN') {
        $("#roomsBlock").addClass('hidden');
        $("#privateBlock").removeClass('hidden');
    } else {
        $("#roomsBlock").removeClass('hidden');
        $("#privateBlock").addClass('hidden');
    }
    $("input[name='userId']").each(function (k, v) {
        $(v).val(userId);
    });
    $("#join_room_modal").modal('hide');

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
    
    var roomsList = $("#rooms"),
           newList = "";
    for (var i = 0; i < data.length; i++) {
        var room = data[i];
        newList += '<tr data-key="' + room.RoomIdentity + '" class="info"><td><i class="icon-comments icon-large" style="color:#FFBF00"></i></td><td>' + room.RoomName + '</td></tr>';

    }
    roomsList.html(newList);
    
}

function SelectUser() {
    alert("user clicked");
}
function SelectRoom() {
    $("#lbl_roomName").text($(this).children("td").eq(1).html());
    $("#roomKey").val($(this).attr("data-key"));
    JoinRoomDialog();
}

function LeaveRoom() {
}