<template>
  <div>
    <div v-if="requests != null">
      <div class="box" v-for="f in requests" :key="f.id">
        <h1>{{ f.first_name }} {{ f.last_name }}</h1>
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
    }
  }
}
</script>

<style scoped>

</style>