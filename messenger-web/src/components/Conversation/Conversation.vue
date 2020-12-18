<template>
  <div v-if="loading" class="full-page">Loading...</div>
  <div v-else class="full-page">
    <div v-if="conversation" class="columns conv-columns">
      <div class="conversation column is-two-thirds">
        <div class="conv-header">
          <h4 class="title is-4 conversation-name">{{ conversation.name }}</h4>
        </div>
        <div class="conv-messages">
            <MessageList :key="conversation.id" :convId="conversation.id" :convMessages="conversation.messages"></MessageList>
        </div>
        <hr class="part-separator"/>
        <div class="conv-send-message">
          <SendMessageBar :message-submit="SendNewMessage"></SendMessageBar>
        </div>
      </div>
      <div class="column is-narrow column-no-top-marging">
        <hr class="conv-part-separator"/>
      </div>
      <div class="column">
        <ConversationDetail :key="conversation.id" :conversationId="conversation.id"></ConversationDetail>
      </div>
    </div>
    <div v-else>
      <p>A problem occured when retrieving conversation</p>
    </div>
  </div>
</template>

<script>
import ApiService from "@/service/api";
import MessageList from "@/components/Messages/MessageList";
import ConversationDetail from "@/components/Conversation/ConversationDetail";

// import eventBus from "../../eventBus.js";
import SendMessageBar from "@/components/Messages/SendMessageBar";

const api = new ApiService();
export default {
  name: "Conversation",
  components: {MessageList, ConversationDetail, SendMessageBar},
  data() {
    return {
      conversation: null,
      loading: false,
    };
  },
  watch:{
    $route (){
      this.GetConversationById(this.$route.params.id)
    }
  },
  mounted() {
    this.GetConversationById(this.$route.params.id)
  },
  methods: {
    GetConversationById(id) {
      this.loading = true;
      api
          .getData("conversation/" + id)
          .then((response) => {
            if (response.ok == true) {
              response.json().then((data) => {
                  this.conversation = data.Result;
              });
            }
            else {
              this.conversation = null;
            }
          })
          .finally(() => {
            this.loading = false;
          });
    },
    SendNewMessage(message) {
        var model = {
          type: "send_message",
          data: {
            conversation_id: this.conversation.id,
            text: message.text,
          },
        };

        this.$store.state.chatWebsocket.send(JSON.stringify(model));
    },
  },
};
</script>

<style>
.conversation {
  height: 100%;
  display: flex !important;
  flex-direction: column;
}

.conv-header {
  height: 50px;
}

.conv-messages {
  flex: 1;
  overflow-y: auto;
}

.conv-send-message {
  height: 40px;
  margin-top: 10px;
}

.conv-columns {
  height: 100% !important;
  margin: 0 !important;
}

.conv-part-separator {
  margin: 0 0.5em;
  background-color: #f2f2f2;
  width: 2px;
  height: 100%;
}

.send-message-input {
  margin-right: 15px;
  flex: 1;
  border-radius: 7px;
  background-color: #f2f2f2;
  border: 0;
  height: 40px;
  text-indent: 15px;
}

.send-message-input:focus {
  outline: none;
}

</style>