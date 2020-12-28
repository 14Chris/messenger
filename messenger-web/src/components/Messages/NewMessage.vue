<template>
  <div class="full-page">
    <div class="field is-horizontal conv-header">
      <div class="field-label is-normal">
        <label>{{$t('toLabel')}}</label>
        <span class="tag is-primary is-medium" v-for="f in selected" :key="f.id">
          {{ f.first_name }} {{ f.last_name }}
        <button class="delete is-small" @click="deleteFriendFromSelected(f.id)"></button>
      </span>
      </div>
      <div class="field-body">
        <div class="field">
          <p class="control">
            <autocomplete :typing="getFriendsBySearch" :search-result="friendsSearch"
                          :select="friendsSelected"></autocomplete>
          </p>
        </div>
      </div>
    </div>
    <div class="conv-messages">
      <MessageList v-if="conversation != null" :convId="conversation.id" :convMessages="conversation.messages"></MessageList>
    </div>
    <div class="conv-send-message">
      <SendMessageBar :message-submit="SendMessage"></SendMessageBar>
    </div>
  </div>
</template>

<script>
import ApiService from "../../service/api";
import MessageList from "@/components/Messages/MessageList";

import autocomplete from "@/components/Messages/FriendAutocomplete";
import eventBus from "@/eventBus";
import SendMessageBar from "@/components/Messages/SendMessageBar";


const api = new ApiService();

export default {
  name: "NewMessage",
  components: {SendMessageBar, MessageList, autocomplete},
  data() {
    return {
      friendsSearch: [],
      selected: [],
      isFetching: false,
      conversation: null
    }
  },
  mounted(){
    eventBus.$on("friendselected", (data) => {
      this.friendsSelected(data)
    });
  },
  methods: {
    //Send message
    SendMessage(message){
      if(this.conversation != null){
        this.SendNewMessage(message)
      }
      else{
        this.AddNewConversation(message)
      }
    },
    //Create conversation if not exists
    AddNewConversation(message) {
      var model = {
        texte: message.text,

        friends: this.selected.map(f => f.id)
      }

      api.create("conversation", JSON.stringify(model))
          .then(response => {
            if (response.ok == true) {
              this.$router.push("/");
            }
          })
          .catch(err => {
            console.log(err)
          })
    },
    //Send new message if conversation not exists
    SendNewMessage(message) {
      var model = {
        type: "send_message",
        data: {
          conversationId: this.conversation.id,
          text: message.text,
          gifId: message.gifId,
          stickerId: message.stickerId,
          gifUrl: message.gifUrl,
          stickerUrl: message.stickerUrl,
        },
      };

      this.$store.state.chatWebsocket.send(JSON.stringify(model));

      this.$router.push({ name: 'conversation', params: { id: this.conversation.id } })
    },
    friendsSelected(option) {
      this.friendsSearch = []
      var selectedFriends = [...this.selected]
      selectedFriends.push(option)
      var users = selectedFriends.map(f => f.id)
      api.create("conversation/exists/", JSON.stringify(users))
          .then(response => {
            if (response.ok == true) {
              this.conversation = null
            }
            else {
              if(response.status == 409){
                response.json()
                    .then(data => {
                      this.conversation = data.Result
                    })
              }
            }
          })
          .catch(err => {
            console.log(err)
          })
          .finally(() => {
            this.selected.push(option)
          })
    },

    getFriendsBySearch(search) {
      if (!search.length) {
        this.friendsSearch = []
        return
      }
      this.isFetching = true
      api.getData("friends/search/" + search)
          .then(response => {
            if (response.ok == true) {
              response.json()
                  .then(data => {
                      this.friendsSearch = data.Result.filter(arrayItem => !this.selected.map(x => x.id).includes(arrayItem.id));
                  })
            }
            else {
              this.friendsSearch = []
            }

          })
          .catch(err => {
            console.log(err)
          })
          .finally(() => {
            this.isFetching = false
          })
    },
    deleteFriendFromSelected(friendId) {
      this.selected = this.selected.filter(arrayItem => arrayItem.id !== friendId);

      api.create("conversation/exists/", JSON.stringify(this.selected.map(x => x.id)))
          .then(response => {
            if (response.ok == true) {
              this.conversation = null
            }
            else {
              if(response.status == 409){
                response.json()
                    .then(data => {
                      this.conversation = data.Result
                    })
              }
            }
          })
          .catch(err => {
            console.log(err)
          })
    }
  }
}
</script>

<style scoped>s
.div-to {
  display: flex;
}

.conv-header {
  margin-top: 10px !important;
}

.conv-send-message {
  margin-bottom: 10px !important;
}

</style>

<i18n>
{
  "en": {
    "toLabel": "To :"
  },
  "fr": {
    "toLabel": "À :"
  }
}
</i18n>