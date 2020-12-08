<template>
  <div class="conversations">
    <div class="conversation-list-header">
      <h3 class="title is-3">Conversations</h3>
    </div>
    <div class="conversation-list">
      <ConversationListItem v-for="c in conversations" :key="c.id" :conversation-item="c"></ConversationListItem>
    </div>
    <div class="conversation-list-bottom">
      <router-link class="button" id="new-conversation-button" to="/conv/new" tag="button">
                <span class="icon">
                  <img src="@/assets/icons/plus-white.svg"/>
                </span>
      </router-link>
    </div>

  </div>
</template>

<script>
import ApiService from "../../service/api";
import ConversationListItem from "@/components/Conversation/ConversationListItem";

import eventBus from "@/eventBus";

const api = new ApiService();
export default {
  name: "FriendsList",
  components: {
    ConversationListItem
  },
  data() {
    return {
      conversations: []
    }
  },
  created() {
    eventBus.$on("message-received", data => {
      this.MessageReceived(data)
    });
  },
  mounted() {
    this.GetUserConversations()
  },
  methods: {
    GetUserConversations() {
      api.getData("conversation/user")
          .then(response => {
            if (response.ok == true) {
              response.json()
                  .then(data => {
                    if (data.ResponseType == 1) {
                      this.conversations = data.Result
                    } else {
                      this.conversations = []
                    }
                  })
            }
          })
          .catch(() => {
            console.log()
          })
    },
    MessageReceived(data) {
      let object = JSON.parse(data)
      let finded = false
      this.conversations = this.conversations.map(function (item) {
        if (item.id == object.conversation.id) {
          finded = true
          return object.conversation
        } else {
          return item
        }
      });

      if (!finded) {
        this.conversations.unshift(object.conversation)
      }
    }

  }
}
</script>

<style scoped lang="scss">
.conversations {
  height: 100%;
  margin-left: 20px;
  display: flex;
  flex-direction: column;
}

.conversation-list-header {
  margin-bottom: 20px;
}

.conversation-list {
  flex: 1;
}

.conversation-list-bottom {
  height: 60px;
  position: relative;
}

#new-conversation-button {
  margin: 0;
  position: absolute;
  top: 50%;
  left: 50%;
  -ms-transform: translate(-50%, -50%);
  transform: translate(-50%, -50%);

  width: 60px;
  height: 60px;
  border-radius: 50%;

  background-color: #349CFC;
}
</style>