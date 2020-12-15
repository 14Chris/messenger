<template>
  <div class="message-item">
    <div
      class="message-container"
      v-bind:class="[
        $store.state.user.id != message.sender_id
          ? 'position-left'
          : 'position-right',
      ]"
    >
      <template v-if="lastUserMessage && $store.state.user.id != message.sender_id">
        <Avatar class="message-avatar message-avatar-left" :userId="message.sender_id"></Avatar>
      </template>

      <div class="message-box" v-bind:class="[
        $store.state.user.id != message.sender_id
          ? 'message-box-left'
          : 'message-box-right',
      ]">
        <div
          class="message-text"
          v-bind:class="[
            $store.state.user.id != message.sender_id ? 'other' : 'sender',
          ]"
        >
          {{ message.text }}
        </div>
        <div class="message-date" v-if="lastUserMessage">
          {{ MessageDate }}
        </div>
      </div>
      <template v-if="lastUserMessage && $store.state.user.id == message.sender_id">
        <Avatar class="message-avatar message-avatar-right" :userId="message.sender_id"></Avatar>
      </template>
    </div>
  </div>
</template>

<script>
import Avatar from "@/components/User/Avatar/Avatar";
import moment from 'moment'

export default {
  name: "MessageItem",
  props: ["message", "lastUserMessage"],
  components: {
    Avatar,
  },
  mounted(){
    // console.log(this.message)
  },
  computed:{
    MessageDate(){
      var date =  this.message.date
      return moment(date).calendar()
    }
  }
};
</script>

<style scoped>
.message-item {
  margin-bottom: 7px;
  width: 100%;
}

.message-container {
  width: fit-content;
  max-width: 80%;
  display: flex;
  flex-direction: row;
  position: relative;
}

.message-text {
  padding: 20px;
  border-radius: 30px;
  margin-bottom: 0;
  font-family: Helvetica, Arial, sans-serif;
  word-wrap: anywhere !important;
}

.position-left {
  float: left;
}

.position-right {
  float: right;
}

.other {
  background-color: lightgrey;
  border-bottom-left-radius: 0px;
}

.sender {
  background-color: #349cfc;
  border-bottom-right-radius: 0px;
  color: white;
}

.message-date {
  font-size: 0.8em;
  color: lightgrey;
  width: fit-content;
  float: right;
}

.message-avatar-right {
  height: 50px;
  right: 5px;
}

.message-avatar-left {
  height: 50px;
  left: 5px;
}

.message-avatar{
  margin: auto 0;
  position:absolute;
  bottom: 0;
  width:40px !important;
  height:40px !important;
  
}

.message-box-left{
  padding-left : 70px;
}

.message-box-right{
  margin-right : 70px;
}
</style>