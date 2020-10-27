<template>
  <div class="box">
    <form @submit.prevent="addFriend">
      <div class="control has-icons-left has-icons-right">
        <input class="input" type="email" placeholder="Email">
        <span class="icon is-small is-left">
    <i class="fas fa-envelope"></i>
  </span>
      </div>
    </form>
  </div>
</template>

<script>
// import ApiService from "@/service/api";
//
// const api = new ApiService();

export default {
  name: "AddFriend",
  data() {
    return {
      model: {}
    }
  },
  methods: {
    addFriend() {
      console.log(this.model)
    }
  }
}
</script>

<style scoped>

</style>