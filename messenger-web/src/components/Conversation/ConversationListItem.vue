<template>
  <div class="conv-list-item" @click="OpenConversation">
    <div class="current-conv-indicator"></div>
    <div class="conv-data">
      <Avatar class="conv-avatar" userId="1"></Avatar>
      <div>
        <h1>{{ conversationItem.name }}</h1>
        <div class="last-meesage-text">{{ conversationItem.last_message }}</div>
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