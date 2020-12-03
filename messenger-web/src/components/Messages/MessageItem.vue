<template>
<div class="message-item">
  <div class="message-container" v-bind:class="[$store.state.user.id != message.sender_id ? 'position-left' : 'position-right']">
    <div class="message-box">
    <div class="message-text" v-bind:class="[$store.state.user.id != message.sender_id ? 'other' : 'sender']">
          {{message.text}}
        </div>
        <div class="message-date">
            {{message.date}}
        </div>
    </div>
    <template v-if="lastUserMessage">
    <Avatar class="message-avatar" :userId="message.sender_id"></Avatar>
  </template>
    
  </div>
  
  </div>
</template>

<script>
import Avatar from "@/components/User/Avatar/Avatar"
export default {
  name: "MessageItem",
  props:["message", "lastUserMessage"],
  components:{
    Avatar
  }
}
</script>

<style scoped>

  .message-item{
    margin-bottom: 7px;
    width: 100%;
  }

  .message-container{
    width: fit-content;
    max-width: 80%;
    display: flex;
    flex-direction: row;
  }

  .message-text{
    padding: 20px;
    border-radius: 30px;
    margin-bottom: none;
    font-family: Helvetica, Arial, sans-serif;
    word-wrap: break-word;
  }

  
  .position-left{
    float: left;
    margin-left:50px;
  }

  .position-right{
    float: right;
    margin-right:50px;
  }

  .other{
    background-color: lightgrey;
    border-bottom-left-radius: 0px;
  }

  .sender{
    background-color: #349CFC;
    border-bottom-right-radius: 0px;
    color:white;
  }

  .message-date{
    font-size: 10px;
    color:lightgrey;
    width: fit-content;
    float: right;
  }

  .message-avatar{
    height: 50px;
    float: right;
  }

</style>