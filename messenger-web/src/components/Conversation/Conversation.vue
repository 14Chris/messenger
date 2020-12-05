<template>
  <div v-if="isLoading" class="full-page">Loading...</div>
  <div v-else class="full-page">
    <div v-if="conversation" class="columns conv-columns">
      <div class="conversation column is-two-thirds">
        <div class="conv-header">
          <h4 class="title is-4 conversation-name">{{ conversation.name }}</h4>
        </div>
        <div class="conv-messages">
          <MessageList :messages="conversation.messages"></MessageList>
        </div>
        <div class="conv-send-message">
          <div class="field is-horizontal">
            <div class="field-body">
              <!-- Plus button -->
              <button class="button">
                <span class="icon">
                  <i class="fas fa-plus-circle"></i>
                </span>
              </button>

              <!-- GIF button -->
              <button class="button">
                <span class="icon">
                  <img src="@/assets/GIF-file-icon.png" />
                </span>
              </button>

              <!-- Sticker button -->
              <button class="button">
                <span class="icon">
                  <i class="far fa-sticky-note"></i>
                </span>
              </button>

              <!-- File button -->
              <button class="button">
                <span class="icon">
                  <i class="fas fa-paperclip"></i>
                </span>
              </button>

              <!-- Text input -->
              <input
                class="input"
                v-model="message"
                type="text"
                placeholder="Enter your message"
              />

              <!-- Smiley button -->
              <button class="button">
                <span class="icon">
                  <i class="fas fa-smile"></i>
                </span>
              </button>

              <!-- Send button -->
              <button
                id="send-message-button"
                class="button navbar-icon"
                @click="SendNewMessage"
              >
                <span class="icon">
                  <i class="fas fa-reply"></i>
                </span>
              </button>
            </div>
          </div>
        </div>
      </div>
      <div class="column is-narrow column-no-top-marging">
        <hr class="conv-part-separator" />
      </div>
      <div class="column">
        <ConversationDetail></ConversationDetail>
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

import eventBus from "../../eventBus.js";

const api = new ApiService();
export default {
  name: "Conversation",
  components: { MessageList, ConversationDetail },
  data() {
    return {
      conversation: null,
      isLoading: false,
      message: "",
    };
  },
  created() {
    eventBus.$on("message-received", (data) => {
      this.MessageReceived(data);
    });
  },
  mounted() {
    this.GetConversationById(this.$route.params.id);
  },
  methods: {
    GetConversationById(id) {
      console.log(id);
      this.isLoading = true;
      api
        .getData("conversation/" + id)
        .then((response) => {
          console.log(response);
          if (response.ok == true) {
            response.json().then((data) => {
              if (data.ResponseType == 1) {
                this.conversation = data.Result;
              } else {
                this.conversation = null;
              }
            });
          }
        })
        .catch((err) => {
          console.log(err);
        })
        .finally(() => {
          this.isLoading = false;
        });
    },
    SendNewMessage() {
      var model = {
        type: "send_message",
        data: {
          conversation_id: this.conversation.id,
          text: this.message,
        },
      };
      this.$store.state.chatWebsocket.send(JSON.stringify(model));
      this.message = "";
    },
    MessageReceived(data) {
      var object = JSON.parse(data);

      if (object.message.conversation_id == this.conversation.id) {
        console.log("new message received", object.message);
        this.conversation.messages.push(object.message);
      }
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
  height: 50px;
  margin-top: 10px;
}

#send-message-button {
  margin-left: 10px;
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
</style>