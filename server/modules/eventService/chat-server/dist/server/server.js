"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const express = require("express");
const app = express();
app.set("port", process.env.PORT || 3000);
let http = require("http").Server(app);
// set up socket.io and bind it to our
// http server.
let io = require("socket.io")(http);
// routing
app.get("/", (req, res) => {
    res.send("Roundcomb chat server");
});
app.post("/", (req, res) => {
    res.send("Roundcomb chat server");
});
// list of usernames which are currently connected to the chat
var usernames = {};
// rooms which are currently available in chat
var rooms = ['room1', 'room2', 'room3'];
// whenever a user connects on port 3000 via
// a websocket, log that a user has connected
io.on("connection", function (socket) {
    console.log("a user connected");
    // whenever we receive a 'message' we log it out
    socket.on("message", function (message) {
        console.log(message);
    });
});
const server = http.listen(3000, function () {
    console.log("listening on *:3000");
});
//# sourceMappingURL=server.js.map