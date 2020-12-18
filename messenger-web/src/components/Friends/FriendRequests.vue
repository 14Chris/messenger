<template>
  <div>
    <div v-if="requests != null">
      <div class="box" v-for="f in requests" :key="f.id" @click="GoToUserProfile(f.id)">
        <h1>{{ f.first_name }} {{ f.last_name }}</h1>
        <div class="request-actions">
          <button class="button is-success is-light" @click.prevent="AcceptFriendRequest(f.id)">
              <span class="icon">
                <i class="fas fa-check"></i>
              </span>
          </button>
          <button class="button button is-danger is-light" @click.prevent="DeleteFriendRequest(f.id)">
              <span class="icon">
                <i class="fas fa-times"></i>
              </span>
          </button>
        </div>

      </div>
    </div>
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
  name: "FriendRequests",
  data() {
    return {
      requests: null
    }
  },
  mounted() {
    this.GetFriendRequests()
  },
  methods: {
    GetFriendRequests() {
      api.getData("friends/request")
          .then(response => {
            if (response.ok == true) {
              response.json()
                  .then(data => {
                      this.requests = data.Result
                  })
            }
            else{
              this.requests = []
            }
          })
    },
    AcceptFriendRequest(id){
      api.create("friends/request/accept", JSON.stringify(id))
          .then(response => {
            console.log(response)
            if (response.ok == true) {
              this.getFriendRequests()
            }
            else {
              openNotification({
                message: 'Error while accepting the request',
                type: 'danger',
                duration: 5000
              })
            }
          })
    },
    DeleteFriendRequest(id){
      api.create("friends/request/delete", JSON.stringify(id))
          .then(response => {
            if (response.ok == true) {
              this.getFriendRequests()
            }
            else {
              openNotification({
                message: 'Error while deleting the request',
                type: 'danger',
                duration: 5000
              })
            }
          })
    },
     GoToUserProfile(userId){
      this.$router.push("profile/"+userId)
    }
  }
}
</script>

<style scoped>
.box{
  margin-top: 10px;
}

</style>