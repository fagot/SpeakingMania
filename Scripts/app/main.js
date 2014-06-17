var options = {};
var isRoomOwner = false;
function Login(data) {
    var hub = {};
    var options = {};
    if (data.success) {
        HideModals();
        $("#userName").html(data.name);
        options.roomId = "MAIN";
        // Прокси-объект чата

        hub = $.connection.userHub;
        hub.client.JoinRoom = OnJoinRoom;
        hub.client.UpdateUsers = OnUpdateUsers;
        hub.client.UpdateRooms = OnUpdateRooms;
        hub.client.SetUsername = OnSetUsername;
        $.connection.hub.start(function () {
            //hub.server.login(options.MyName);
            //hub.server.joinRoom(options.roomId);
            //hub.server.updateRooms();
            //SetNameDialog();
        });

    } else {
        $("#login").addClass("error");
        $("login-error").html(data.errors["login"]).show();
    }

    $('#users').delegate('li', 'click', SelectUser);
    $('#rooms').delegate('tr', 'click', SelectRoom);



}

function Register() {
    alert("registred");
    //location.reload();
}
function LoggedIn() {
    alert("logged");
    Login({ success: true });
}
function CreateRoom(data) {
    var hub = {};
    var options = {};
    if (data.success) {
        hub = $.connection.userHub;
        hub.client.OnUpdateRooms = OnUpdateRooms;
        options.roomName = data.roomName;
        options.roomId = data.roomId;
        options.userId = data.userId;
        isRoomOwner = data.isRoomOwner;
        $("#new_room_modal").modal('hide');
        hub.server.joinRoom(options.roomId);
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
function OnSetUsername() {
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
    $("#signin_modal").modal('show')
    $("#signup_modal").modal('hide')
    $("#login_modal").modal('hide')
}
function SignUp() {
    $("#signin_modal").modal('hide');
    $("#signup_modal").modal('show');
    $("#login_modal").modal('hide');
}

function LogIn() {
    $("#signin_modal").modal('hide');
    $("#signup_modal").modal('hide');
    $("#login_modal").modal('show');
   // alert("SAS");
}

function HideModals() {
    $("#signin_modal").modal('hide');
    $("#signup_modal").modal('hide');
    $("#login_modal").modal('hide');
}

/* Login dialod navigation end*/

function OnJoinRoom(userId, roomId) {
   // alert(roomId);
    //$.post("Home/SaveUserData", { userId: key }, function (data) {
    //    //do whatever with the response

    //});
    //if (roomKey != 'MAIN') {
    //    $("#roomsBlock").addClass('hidden');
    //    $("#privateBlock").removeClass('hidden');
    //} else {
    //    $("#roomsBlock").removeClass('hidden');
    //    $("#privateBlock").addClass('hidden');
    //}
    //$.post(
    //"/Home/SaveUserData",
    //{
    //    UserId: userId
    //},
    //alert("succes")
    //);
    alert("A");
    $("input[name='userId']").each(function (k, v) {
        $(v).val(userId);
    });
    $("#join_room_modal").modal('hide');

}

function OnUpdateUsers(data) {
 //alert("DFS");
    var usersList = $("#users"),
            newList = "";
    for (var i = 0; i < data.length; i++) {
        var user = data[i];
        newList += '<li data-key="' + user.UserIdentity + '"><i class="icon-user" style="color:#40FF00"></i>' + user.UserName + '</li>';

    }
   
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
    var userId = $(this).attr("data-key");
    alert(userId);
}
function SelectRoom() {
    $("#lbl_roomName").text($(this).children("td").eq(1).html());
    $("#roomKey").val($(this).attr("data-key"));
    JoinRoomDialog();
}

function LeaveRoom() {
}