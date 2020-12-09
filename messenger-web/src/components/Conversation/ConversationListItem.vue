<template>
  <div class="conv-list-item" @click="OpenConversation">
    <div v-if="convId == conversationItem.id" class="current-conv-indicator"></div>
    <div class="conv-data">
      <Avatar class="conv-avatar" :userId="conversationItem.friends_ids[0].id"></Avatar>
      <div>
        <h1>{{ conversationItem.name }}</h1>
        <div class="last-meesage-text">
          <span v-if="conversationItem.last_message_sender.id == $store.state.user.id">You : </span>
          <!--  If group conversation -->
          <span v-else-if="conversationItem.friends_ids > 1">{{ conversationItem.last_message_sender.first_name}} {{ conversationItem.last_message_sender.last_name }} </span>

          <span>{{ conversationItem.last_message }}</span></div>
        <div class="last-message-date">{{ MessageDate }}</div>
      </div>
    </div>
  </div>
</template>

<script>
import Avatar from "@/components/User/Avatar/Avatar";
import moment from "moment";
export default {
  name: "FriendListItem",
  props: ["conversationItem"],
  components: {
    Avatar,
  },
  data(){
    return {
      convId: -1
    }
  },
  mounted(){
    this.UpdateConvId()
  },
  watch: {
    $route() {
        this.UpdateConvId()
    }
  },
  computed: {
    MessageDate() {
      var date = this.conversationItem.last_message_date;
      return moment(date).calendar();
    },
  },
  methods: {
    OpenConversation() {
      this.$router.push("/conv/" + this.conversationItem.id);
    },
    UpdateConvId(){
      if(this.$route.name=="conversation"){
        this.convId = this.$route.params.id
      } 
    }
  },
};
</script>

<style scoped>
.conv-list-item {
  margin-bottom: 10px;
  cursor: pointer;
  overflow-y: hidden;
  display: flex;
  flex-direction: row;
}

.current-conv-indicator {
  margin-right: 15px;
  background-color: #349cfc;
  width: 3px;
}

.last-meesage-text {
  font-size: 0.8em;
}

.last-message-date {
  font-size: 0.8em;
  color: rgb(179, 179, 179);
}

.conv-data {
  display: flex;
}

.conv-avatar {
  height:fit-content;
  margin: auto 0;
  margin-right:15px !important;
}
</style>