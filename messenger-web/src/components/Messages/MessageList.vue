<template>
  <div id="messages-list">
    <MessageItem v-for="m of SortedMessagesByDate" :key="m.id" :message="m"></MessageItem>
  </div>
</template>

<script>
import MessageItem from "@/components/Messages/MessageItem";
export default {
  name: "MessageList",
  components: {MessageItem},
  props:["messages"],
  mounted(){
    var element = document.getElementById("messages-list");
    element.scrollTop = element.scrollHeight
  },
  computed:{
    SortedMessagesByDate(){
      var messageTempArray = this.messages
      messageTempArray.sort(this.SortMessageByDate);

      return messageTempArray
    }
  },
  methods:{
    SortMessageByDate(dateA, dateB) {
      return new Date(dateA.date) - new Date(dateB.date);
    }
  }
}
</script>

<style scoped>
  #messages-list{
    display: flex;
    flex-direction: column;
    /*overflow-y: hidden;*/
  }
</style>