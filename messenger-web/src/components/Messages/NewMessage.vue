<template>
  <div class="full-page">
    <div class="field is-horizontal conv-header">
      <div class="field-label is-normal">
        <label>To : </label>
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
      <MessageList v-if="conversation != null" :messages="conversation.messages"></MessageList>
    </div>
    <div class="conv-send-message">
      <div class="field is-horizontal">
        <div class="field-label is-normal">
          <label></label>
        </div>
        <div class="field-body">
          <input class="input" v-model="message" type="text" placeholder="Enter your message">
          <button id="new-conversation-button" class="button navbar-icon" @click="addNewConversation">
              <span class="icon">
                <i class="fas fa-reply"></i>
              </span>
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ApiService from "../../service/api";
import MessageList from "@/components/Messages/MessageList";
import autocomplete from "@/components/Messages/FriendAutocomplete";
import eventBus from "@/eventBus";

const api = new ApiService();

export default {
  name: "NewMessage",
  components: {MessageList, autocomplete},
  data() {
    return {
      friendsSearch: [],
      selected: [],
      isFetching: false,
      message: "",
      conversation: null
    }
  },
  mounted(){
    eventBus.$on("friendselected", (data) => {
      this.friendsSelected(data)
    });
  },
  methods: {
    addNewConversation() {
      var model = {
        texte: this.message,
        friends: this.selected.map(f => f.id)
      }

      api.create("conversation", JSON.stringify(model))
          .then(response => {
            if (response.ok == true) {
              response.json()
                  .then(data => {
                    if (data.ResponseType == 1) {
                      this.$router.push("/");
                    }
                  })
            }
          })
          .catch(err => {
            console.log(err)
          })
    },
    friendsSelected(option) {
      this.friendsSearch = []
      var selectedFriends = [...this.selected]
      selectedFriends.push(option)
      var users = selectedFriends.map(f => f.id)
      api.create("conversation/exists/", JSON.stringify(users))
          .then(response => {
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
                    if (data.ResponseType == 1) {
                      this.friendsSearch = data.Result.filter(arrayItem => !this.selected.map(x => x.id).includes(arrayItem.id));
                    } else {
                      this.friendsSearch = []
                    }
                  })
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