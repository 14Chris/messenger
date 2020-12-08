<template>
  <div v-if="isLoading" class="full-page">Loading...</div>
  <div v-else class="full-page">
    <div v-if="conversation" class="columns conv-columns">
      <div class="conversation column is-two-thirds">
        <div class="conv-header">
          <h4 class="title is-4 conversation-name">{{ conversation.name }}</h4>
        </div>
        <div id="conv-messages-list" ref="convMessagesList" class="conv-messages">
          <MessageList :messages="conversation.messages"></MessageList>
        </div>
        <hr class="part-separator"/>
        <div class="conv-send-message">
          <div class="field is-horizontal">
            <div class="field-body">
              <!-- Plus button -->
              <button class="send-message-action">
                <span class="icon">
                  <img src="@/assets/icons/plus-grey.svg"/>
                </span>
              </button>

              <!-- GIF button -->
              <button class="send-message-action">
                <span class="icon">
                  <img src="@/assets/icons/gif-file-grey.svg"/>
                </span>
              </button>

              <!-- Sticker button -->
              <button class="send-message-action">
                <span class="icon">
                  <img src="@/assets/icons/sticky-note-grey.svg"/>
                </span>
              </button>

              <!-- File button -->
              <button class="send-message-action">
                <span class="icon">
                  <img src="@/assets/icons/paperclip-grey.svg"/>
                </span>
              </button>

              <!-- Text input -->
              <input
                  class="send-message-input"
                  v-model="message"
                  type="text"
                  placeholder="Enter your message"
                  v-on:keyup.enter="SendNewMessage"
              />

              <!-- Smiley button -->
              <button class="send-message-action">
                <span class="icon">
                  <img src="@/assets/icons/smile-grey.svg"/>
                </span>
              </button>

              <!-- Thumbs up button -->
              <button class="send-message-action">
                <span class="icon">
                  <img src="@/assets/icons/thumbs-up-grey.svg"/>
                </span>
              </button>
            </div>
          </div>
        </div>
      </div>
      <div class="column is-narrow column-no-top-marging">
        <hr class="conv-part-separator"/>
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
  components: {MessageList, ConversationDetail},
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
    this.GetConversationById(this.$route.params.id)
  },
  methods: {
    GetConversationById(id) {
      this.isLoading = true;
      api
          .getData("conversation/" + id)
          .then((response) => {
            if (response.ok == true) {
              response.json().then((data) => {
                if (data.ResponseType == 1) {
                  this.conversation = data.Result;
                  this.$nextTick(function () {
                    //Scroll list of message to the latest (bottom)
                    var divMessages =  this.$refs.convMessagesList
                    divMessages.scrollTop = divMessages.scrollHeight - divMessages.clientHeight
                  })
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
            // var element = document.getElementById("conv-messages-list");
            // console.log("scroll div messages",element)
            // element.scrollTop = element.scrollHeight


          });
    },
    SendNewMessage() {
      if (this.message.length > 0) {
        var model = {
          type: "send_message",
          data: {
            conversation_id: this.conversation.id,
            text: this.message,
          },
        };
        this.$store.state.chatWebsocket.send(JSON.stringify(model));
        this.message = "";
      }
    },
    MessageReceived(data) {
      var object = JSON.parse(data);

      if (object.message.conversation_id == this.conversation.id) {
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
  height: 56px;
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

.send-message-action {
  margin-right: 15px;
  padding: 0;
  border: none;
  background: none;
  cursor: pointer;
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