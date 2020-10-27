<template>
  <div v-if="isLoading" class="full-page">
    {{ $route.params.id }}
  </div>
  <div v-else class="full-page">
    <div v-if="conversation" class="conversation">
      <div class="conv-header">
      <h4 class="title is-4">{{conversation.name}}</h4>
      </div>
      <div class="conv-messages">
        <MessageList :messages="conversation.messages"></MessageList>
      </div>
      <div class="conv-send-message">
        <div class="field is-horizontal">
          <div class="field-label is-normal">
            <label></label>
          </div>
          <div class="field-body">
            <input class="input" v-model="message" type="text" placeholder="Enter your message">
            <button id="send-message-button" class="button navbar-icon" @click="SendNewMessage">
              <span class="icon">
                <i class="fas fa-reply"></i>
              </span>
            </button>
          </div>
        </div>
        </div>

    </div>
    <div v-else>
      <p>A problem occured when retrieving conversation</p>
    </div>
  </div>
</template>

<script>
import ApiService from "@/service/api";
import MessageList from "@/components/MessagesView/MessageList";

import eventBus from "../../eventBus.js";

const api = new ApiService();
export default {
  name: "Conversation",
  components: {MessageList},
  data() {
    return {
      conversation: null,
      isLoading: false,
      message:""
    }
  },
  created() {
    eventBus.$on("message-received", data => {
      this.MessageReceived(data)
    });
  },
  mounted() {
    this.GetConversationById(this.$route.params.id)
  },
  methods: {
    GetConversationById(id) {
      console.log(id)
      this.isLoading = true
      api.getData("conversation/" + id)
          .then(response => {
            console.log(response)
            if (response.ok == true) {
              response.json()
                  .then(data => {
                    if (data.ResponseType == 1) {
                      this.conversation = data.Result
                    } else {
                      this.conversation = null
                    }
                  })
            }
          })
          .catch(err => {
            console.log(err)
          })
          .finally(()=>{
            this.isLoading = false
          })
    },
    SendNewMessage(){
      var model = {
        type:"send_message",
        data: {
          conversation_id: this.conversation.id,

          text: this.message
        }
      }
      this.$store.state.chatWebsocket.send(JSON.stringify(model))
    },
    MessageReceived(data){
      var object = JSON.parse(data)

      if(object.message.conversation_id == this.conversation.id){
        console.log('new message received', object.message)
        this.conversation.messages.push(object.message)
      }
    }
  }
}
</script>

<style>
.conversation{
  height:100%;
  display:flex;
  flex-direction: column;
}

.conv-header{
  height:50px;
}

.conv-messages{
  flex:1;
  overflow-y: scroll;
}

.conv-send-message{
  height:50px;
}

#send-message-button{
  margin-left: 10px;
}
</style>