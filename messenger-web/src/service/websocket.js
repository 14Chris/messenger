import eventBus from "../eventBus.js";

export default function CreateWebSocket(token){
    var socket = new WebSocket("wss://" + process.env.VUE_APP_API_URL + "/chat",
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

    socket.onclose = function (event) {
        console.log("on close", event)
    };

    socket.onerror = function (event) {
        console.log("on error", event)
    };

    return socket;
}