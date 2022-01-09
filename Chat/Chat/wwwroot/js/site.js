"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.getElementById("messengerButton").disabled = true;

document.getElementById("messengerButton").addEventListener("click", function (event) {
    var message = document.getElementById("messengerInput").value;
    var recieverId = document.getElementById("recieverId").value;

    connection.invoke("SendPrivateMessage", recieverId, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();

    document.getElementById("messengerInput").value = "";
});

connection.on("ReceiveMessage", function (senderName, message) {
    var p = document.createElement("p");
    p.textContent = `${message}`;
    p.classList.add("reciever");
    document.getElementsByClassName("messageContainer")[0].appendChild(p);
});


connection.start().then(function () {
    document.getElementById("messengerButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});


document.getElementById("messengerInput").addEventListener("focus", (e) => {
    e.preventDefault();
    var recieverId = document.getElementById("recieverId").value;

    connection.invoke("Typing", recieverId).catch(function (err) {
        return console.error(err.toString());
    });
});
document.getElementById("messengerInput").addEventListener("blur", (e) => {
    e.preventDefault();
    var recieverId = document.getElementById("recieverId").value;

    connection.invoke("HideTyping", recieverId).catch(function (err) {
        return console.error(err.toString());
    });
});

connection.on("ShowTyping", function () {
    for (var i = 0; i < document.getElementsByClassName("typing").length; i++) {
        document.getElementsByClassName("typing")[i].remove();
    }
    var p = document.createElement("p");
    p.classList.add("typing");
    p.textContent = `Typing...`;
    document.getElementsByClassName("messageContainer")[0].appendChild(p);
});

connection.on("HideTyping", function () {
    for (var i = 0; i < document.getElementsByClassName("typing").length; i++) {
        document.getElementsByClassName("typing")[i].remove();
    }
});

connection.on("Disconnected", function () {
    for (var i = 0; i < document.getElementsByClassName("typing").length; i++) {
        document.getElementsByClassName("typing")[i].remove();
    }
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});