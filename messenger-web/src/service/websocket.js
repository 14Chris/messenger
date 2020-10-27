import eventBus from "../eventBus.js";

export default function CreateWebSocket(token){
    var socket = new WebSocket("wss://localhost:5001/chat",
        ["access_token", token]
    );

    socket.onopen = function (event) {
        console.log("on open", event)
    };

    socket.onmessage = function (event) {
        console.log("receive data from server : ", event.data);
        eventBus.$emit('message-received', event.data)
    }

    socket.onclose = function (event) {
        console.log("on close", event)
    };

    socket.onerror = function (event) {
        console.log("on error", event)
    };

    return socket;
}