<template>
  <div id="conv-messages-list" ref="convMessagesList" >
    <div id="messages-list" ref="messagesList">
      <MessageItem
          class="message-item-element"
          v-for="(m, index) of SortedMessagesByDate"
          :key="m.id"
          :message="m"
          :lastUserMessage="LastUserMessage(m.sender_id, index)"
      ></MessageItem>
    </div>
  </div>
</template>

<script>
import MessageItem from "@/components/Messages/MessageItem";
import ApiService from "@/service/api";
import eventBus from "../../eventBus";

const api = new ApiService();
export default {
  name: "MessageList",
  components: {MessageItem},
  props: {
    convMessages: {
      required: true
    },
    convId: {
      type: Number,
      required: true
    }
  },
  created() {
    eventBus.$on("message-received", (data) => {
      this.MessageReceived(data);
    });

    this.$nextTick(function () {
      document.getElementById("conv-messages-list").addEventListener('scroll', this.HandleScroll);
    })
  },
  destroyed() {
    document.getElementById("conv-messages-list").removeEventListener('scroll', this.HandleScroll);
  },
  mounted() {
    this.messages = this.convMessages
    this.$nextTick(function () {
      //Scroll list of message to the latest (bottom)
      let divMessages = this.$refs.convMessagesList
      divMessages.scrollTop = divMessages.scrollHeight - divMessages.clientHeight
    })
  },
  computed: {
    SortedMessagesByDate() {
      var messageTempArray = this.messages
      if (messageTempArray != null) {
        messageTempArray.sort(this.SortMessageByDate);
        return messageTempArray
      } else {
        return []
      }
    },
  },
  data() {
    return {
      loading: false,
      messages: null
    }
  },
  methods: {
    SortMessageByDate(dateA, dateB) {
      return new Date(dateA.date) - new Date(dateB.date);
    },
    LastUserMessage(messageSenderId, index) {
      if (index == this.SortedMessagesByDate.length - 1) {
        return true
      } else {
        return this.SortedMessagesByDate[index + 1].sender_id != messageSenderId
      }
    },
    LoadMoreMessages() {
      let lastMessageId = this.messages[0].id
      api.getData("conversation/" + this.convId + "/messages/" + lastMessageId + "/more")
          .then((response) => {
            if (response.ok == true) {
              response.json().then((data) => {
                  let divMessages = this.$refs.convMessagesList

                  //Get the previous height of message list
                  var previousHeight = document.getElementById("messages-list").offsetHeight

                  //Add all the loaded messages in top of messages array
                  data.Result.forEach(element=>{
                    this.messages.unshift(element)
                  })

                  //When DOM is updated
                  this.$nextTick(() => {
                    //Get the new height of message list
                    let newHeight = document.getElementById("messages-list").offsetHeight

                    //Set the scroll position to previous one
                    divMessages.scrollTop = newHeight - previousHeight
                  });

              });
            }
          })
          .finally(() => {
            this.loading = false;
          });
    },
    HandleScroll() {
      let divMessages = this.$refs.convMessagesList
      if(divMessages.scrollTop == 0){
        this.LoadMoreMessages()
      }
    },
    MessageReceived(data) {
      var object = data;

      if (object.message.conversation_id == this.convId) {
        this.messages.push(object.message);
      }
    },

  }
}
</script>

<style scoped>
#messages-list {
  display: flex;
  flex-direction: column;
}

#conv-messages-list{
  height: 100%;
  overflow-y: auto;
}


</style>