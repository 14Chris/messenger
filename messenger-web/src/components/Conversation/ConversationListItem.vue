<template>
  <div class="box conv-list-item" @click="OpenConversation">
    <!-- <Avatar :userId="conversationItem"></Avatar> -->
    <h1>{{ conversationItem.name }}</h1>
    <div>
      {{ conversationItem.last_message }}
      {{ conversationItem.last_message_date }}
    </div>
  </div>
</template>

<script>
// import Avatar from "@/components/User/Avatar/Avatar"
export default {
  name: "FriendListItem",
  props: ["conversationItem"],
  methods:{
    OpenConversation(){
      this.$router.push('/conv/'+this.conversationItem.id)
    }
  }
}
</script>

<style scoped>
.conv-list-item {
  margin-bottom: 10px;
  cursor: pointer;
  overflow-y: hidden;
}
</style>