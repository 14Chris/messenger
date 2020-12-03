<template>
<div class="message-item">
  <div class="message-container" v-bind:class="[$store.state.user.id != message.sender_id ? 'position-left' : 'position-right']">
    <div class="message-text" v-bind:class="[$store.state.user.id != message.sender_id ? 'other' : 'sender']">
      {{message.text}}
    </div>
    <div class="message-date">
        {{message.date}}
    </div>
  </div>
  </div>
</template>

<script>
export default {
  name: "MessageItem",
  props:["message"]
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
  }

  .position-right{
    float: right;
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

</style>