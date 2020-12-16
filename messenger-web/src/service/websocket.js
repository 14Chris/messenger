import eventBus from "../eventBus.js";

export default function CreateWebSocket(token){
    var socket = new WebSocket("wss://localhost:5001/chat",
        ["access_token", token]
    );

    socket.onopen = function () {
        // console.log("on open", event)
    };

    socket.onmessage = function (event) {
        // console.log("receive data from server : ", event.data);
        let objectData = JSON.parse(event.data)

        switch(objectData.type){
            case "MessageAdded":eventBus.$emit('message-received', objectData.data);
            break;

            case "ConversationCreated":eventBus.$emit('conversation-created', objectData.data);
            break;

        }
    }

    socket.onclose = function () {
        // console.log("on close", event)
    };

    socket.onerror = function () {
        // console.log("on error", event)
    };

    return socket;
}