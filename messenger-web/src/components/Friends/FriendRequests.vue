<template>
  <div>
    <div v-if="requests != null">
      <div class="box" v-for="f in requests" :key="f.id">
        <h1>{{ f.first_name }} {{ f.last_name }}</h1>
        <div class="request-actions">
          <button class="button is-success is-light" @click="acceptFriendRequest(f.id)">
              <span class="icon">
                <i class="fas fa-check"></i>
              </span>
          </button>
          <button class="button button is-danger is-light" @click="deleteFriendRequest(f.id)">
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

const api = new ApiService();

export default {
  name: "FriendRequests",
  data() {
    return {
      requests: null
    }
  },
  mounted() {
    this.getFriendRequests()
  },
  methods: {
    getFriendRequests() {
      api.getData("friends/request")
          .then(response => {
            console.log(response)
            if (response.ok == true) {
              response.json()
                  .then(data => {
                    if (data.ResponseType == 1) {
                      this.requests = data.Result
                    } else {
                      this.requests = []
                    }
                  })
            }
          })
          .catch(err => {
            console.log(err)
          })
    },
    acceptFriendRequest(id){
      api.create("friends/request/accept", JSON.stringify(id))
          .then(response => {
            console.log(response)
            if (response.ok == true) {
              response.json()
                  .then(data => {
                    if (data.ResponseType == 1) {
                      this.getFriendRequests()
                    } else {
                      this.$buefy.notification.open({
                        message: 'Error while accepting the request',
                        type: 'is-danger',
                        duration:5000,
                        closable:true
                      })
                    }
                  })
            }
          })
          .catch(err => {
            console.log(err)
          })
    },
    deleteFriendRequest(id){
      api.create("friends/request/delete", JSON.stringify(id))
          .then(response => {
            console.log(response)
            if (response.ok == true) {
              response.json()
                  .then(data => {
                    if (data.ResponseType == 1) {
                      this.getFriendRequests()
                    }
                    else {
                      this.$buefy.notification.open({
                        message: 'Error while deleting the request',
                        type: 'is-danger',
                        duration:5000,
                        closable:true
                      })
                    }
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
.box{
  margin-top: 10px;
}

.request-actions {
  /*button{*/
  /*  margin-right: 5px;*/
  /*}*/
}
</style>