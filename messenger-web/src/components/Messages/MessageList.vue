<template>
  <div id="messages-list">
      <MessageItem  
        v-for="(m, index) of SortedMessagesByDate" 
        :key="m.id" 
        :message="m" 
        :lastUserMessage="LastUserMessage(m.sender_id, index)"
      ></MessageItem>
  </div>
</template>

<script>
import MessageItem from "@/components/Messages/MessageItem";
export default {
  name: "MessageList",
  components: {MessageItem},
  props:["messages"],
  computed:{
    SortedMessagesByDate(){
      var messageTempArray = this.messages
      messageTempArray.sort(this.SortMessageByDate);

      return messageTempArray
    },
   
  },
  data(){
    return {
      
    }
  },
  mounted(){
    var element = document.getElementById("messages-list");
    element.scrollTop = element.scrollHeight
  },
  methods:{
    SortMessageByDate(dateA, dateB) {
      return new Date(dateA.date) - new Date(dateB.date);
    },
     LastUserMessage(messageSenderId, index){
      if(index == this.SortedMessagesByDate.length - 1){
        return true
      }
      else{
        return this.SortedMessagesByDate[index+1].sender_id != messageSenderId
      }
    }
  }
}
</script>

<style scoped>
  #messages-list{
    display: flex;
    flex-direction: column-reverse;
  }
  
</style>