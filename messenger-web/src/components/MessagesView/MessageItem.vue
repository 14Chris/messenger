<template>
  <div>
  <div class="message" v-bind:class="[$store.state.user.id != message.sender_id ? 'other' : 'sender']">
    {{message.text}}
<!--    {{message.date}}-->
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
  .message{
    min-width: 100px;
    max-width: 500px;
    padding: 20px;
    border-radius: 30px;

    margin-bottom: 2px;
    font-family: Helvetica, Arial, sans-serif;
  }

  .other{
    background-color: lightgrey;
    float: left;
    border-bottom-left-radius: 0px;
  }

  .sender{
    background-color: #349CFC;
    float: right;
    border-bottom-right-radius: 0px;
    color:white;
  }

</style>