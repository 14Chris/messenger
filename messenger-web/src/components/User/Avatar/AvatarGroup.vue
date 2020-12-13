<template>
  <div class="conv-friend-avatars">
    <Avatar class="conv-friend-avatar" :userId="friendsIds[0]"></Avatar>
    <Avatar class="conv-friend-avatar" :userId="friendsIds[1]"></Avatar>
    <Avatar class="conv-friend-avatar" v-if="friendsIds.length == 3" :userId="friendsIds[2]"></Avatar>
    <div class="avatar avatar-conv-count" v-else-if="friendsIds.length > 3"></div>
  </div>

</template>

<script>

import Avatar from "@/components/User/Avatar/Avatar";

export default {
  name: "AvatarGroup",
  props: ["friendsIds"],
  components:{
    Avatar
  },
}
</script>

<style scoped>
.conv-friend-avatars{
  display: flex;
  position:relative;
  width: fit-content;
}

.avatar {
  vertical-align: middle;
  width: 100%;
  height: 100%;
  border-radius: 50%;
}

.conv-friend-avatar{
  bottom: 0;
  width:70px !important;
  height:70px !important;
  margin: auto 5px auto 0;
  /*position: absolute;*/

}

</style>