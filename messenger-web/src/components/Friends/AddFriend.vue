<template>
  <div class="box">
    <form @submit.prevent="addFriend">

      <div class="field">
        <label class="label">Friend email</label>
        <div class="control">
          <input class="input" v-model="model.email" type="text" placeholder="Type your friend email...">
        </div>
      </div>

      <button class="button">Add</button>
    </form>
  </div>
</template>

<script>
import ApiService from "@/service/api";

const api = new ApiService();

export default {
  name: "AddFriend",
  data() {
    return {
      model: {
        email:""
      }
    }
  },
  methods: {
    addFriend() {
      console.log(this.model)
      api.create("friends/request", JSON.stringify(this.model.email))
          .then(response=> {
            if (response.ok == true) {
              response.json()
                  .then(() => {
                    this.$buefy.notification.open({
                      message: 'Your friend request has been sent. Please wait for your friend to accept it.',
                      type: 'is-success',
                      duration:5000,
                      closable:true
                    })
                  })
            }
          })
          .catch(err => {
            console.log(err)
          })
    }
  }
}
</script>

<style scoped>

</style>