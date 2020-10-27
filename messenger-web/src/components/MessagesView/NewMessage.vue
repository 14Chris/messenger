<template>
  <div class="full-page">
    <div class="field is-horizontal conv-header">
      <div class="field-label is-normal">
        <label>To : </label>
        <b-tag v-for="f in selected" :key="f.id"
               type="is-primary"
               closable
               aria-close-label="Close tag"
               @close="deleteFriendFromSelected(f.id)">
          {{f.first_name}} {{f.last_name}}
        </b-tag>
      </div>
      <div class="field-body">
        <div class="field">
          <p class="control">
            <b-autocomplete
                :data="friendsSearch"
                placeholder="Write person name"
                field="title"
                :loading="isFetching"
                @typing="getFriendsBySearch"
                @select="friendsSelected">

              <template slot-scope="props">
                <div class="media">
                  <div class="media-left">

                  </div>
                  <div class="media-content">
                    {{ props.option.first_name }} {{ props.option.last_name }}
                  </div>
                </div>
              </template>
            </b-autocomplete>
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
import MessageList from "@/components/MessagesView/MessageList";

const api = new ApiService();

export default {
  name: "NewMessage",
  components: {MessageList},
  data() {
    return {
      friendsSearch: [],
      selected: [],
      isFetching: false,
      message:"",
      conversation: null
    }
  },
  methods: {
      addNewConversation(){
        var model = {
          texte: this.message,
          friends: this.selected.map(f => f.id)
        }

        api.create("conversation", JSON.stringify(model))
        .then(response=> {
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
    friendsSelected(option){
        var selectedFriends = [...this.selected]
      selectedFriends.push(option)

      var users = selectedFriends.map(f=>f.id)

      api.create("conversation/exists/", JSON.stringify(users))
          .then(response=> {
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
          .finally(() => {
            this.selected.push(option)
          })
    },

      getFriendsBySearch(search){
        if (!search.length) {
          this.friendsSearch = []
          return
        }
        this.isFetching = true
      api.getData("friends/search/"+search)
          .then(response=> {
            console.log(response)
            if (response.ok == true) {
              response.json()
                  .then(data => {
                    if (data.ResponseType == 1) {
                      this.friendsSearch = data.Result.filter(arrayItem => !this.selected.map(x=>x.id).includes(arrayItem.id));
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
    deleteFriendFromSelected(friendId){
      this.selected = this.selected.filter(arrayItem => arrayItem.id !== friendId);

      api.create("conversation/exists/", JSON.stringify(this.selected.map(x=>x.id)))
          .then(response=> {
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

    }
  }
}
</script>

<style scoped>s
.div-to {
  display: flex;
}

/*.friends-div{*/
/*  height:10%;*/
/*}*/

/*.conv-div{*/
/*  height:80%;*/
/*}*/

/*.message-div{*/
/*  height:10%;*/
/*}*/
</style>