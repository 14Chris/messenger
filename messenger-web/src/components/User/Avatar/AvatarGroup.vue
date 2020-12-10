<template>
  <div>
    Avatar group
  </div>

</template>

<script>

export default {
  name: "AvatarGroup",
  props: ["friendsIds"],
  mounted(){
    console.log(this.friendsIds)
  }
}
</script>

<style scoped>

</style>