<template>
  <div class="box">
    <form @submit.prevent="addFriend">
      <div class="field">
        <label class="label">Friend email</label>
        <div class="control">
          <input class="input" v-model="model.email" type="text" placeholder="Type your friend email...">
        </div>
      </div>

      <button class="button is-primary">Add</button>
    </form>
  </div>
</template>

<script>
import ApiService from "@/service/api";
import Vue from "vue";
import Notification from "@/shared_components/Notification/Notification";

const api = new ApiService();
const NotificationComponent = Vue.extend(Notification)

const openNotification = (propsData = {
  title: '',
  message: '',
  type: '',
  direction: '',
  duration: 4500,
  container: '.notifications'
}) => {
  return new NotificationComponent({
    el: document.createElement('div'),
    propsData
  })
}

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
                    openNotification({
                      message: 'Your friend request has been sent. Please wait for your friend to accept it.',
                      type: 'success',
                      duration: 5000
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