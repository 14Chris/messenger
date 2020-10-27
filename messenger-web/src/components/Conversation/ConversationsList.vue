<template>
  <div class="conversations">
    <div class="conversation-list-header">
      <h3 class="title is-3">Conversations</h3>
    </div>
    <div class="conversation-list">
      <ConversationListItem v-for="c in conversations" :key="c.id" :conversation-item="c"></ConversationListItem>
    </div>
    <div class="conversation-list-bottom">
      <router-link id="new-conversation-button" to="/conv/new">
      <button class="button navbar-icon">
              <span class="icon">
                <i class="fas fa-plus"></i>
              </span>
      </button>
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
  components:{
    ConversationListItem
  },
  data(){
    return {
      conversations:[]
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
  methods:{
    GetUserConversations(){
      api.getData("conversation/user")
          .then(response=> {
            console.log(response)
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
          .catch(err => {
            console.log(err)
          })
    },
    MessageReceived(data){
      var object = JSON.parse(data)
      let finded = false
      this.conversations = this.conversations.map(function(item) {
        if(item.id == object.conversation.id){
          finded = true
         return object.conversation
        }
        else{
          return item
        }
      });

      if(!finded){
        this.conversations.unshift(object.conversation)
      }
    }

}
}
</script>

<style scoped lang="scss">
.conversations{
  height:100%;
  margin-left: 20px;
}

  .conversation-list-header{
    display: flex;
    flex-direction: row;
    margin-left: 10px;
    margin-bottom: 20px;
  }
  .conversation-list{
    height:90%;
  }
  .conversation-list-bottom{

  }



  #new-conversation-button{
    margin: 0 auto !important;

    button{
      width: 60px;
      height: 60px;

      background-color: #349CFC;
      i{
        color: white;
      }
    }

  }
</style>