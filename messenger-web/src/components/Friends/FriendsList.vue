<template>
  <div>
    <div v-if="friends != null">
        <div class="box" v-for="f in friends" :key="f.id">
          <h1>{{ f.first_name }} {{ f.last_name }}</h1>
        </div>
    </div>
<!--    <div v-else>-->
<!--      <p>Error loading data...</p>-->
<!--    </div>-->
  </div>
</template>

<script>
import ApiService from "@/service/api";

const api = new ApiService();

export default {
  name: "FriendsList",
  data() {
    return {
      friends: null
    }
  },
  mounted() {
    this.getFriends()
  },
  methods:{
    getFriends(){
      api.getData("friends")
          .then(response=> {
            console.log(response)
            if (response.ok == true) {
              response.json()
                  .then(data => {
                    if (data.ResponseType == 1) {
                      this.friends = data.Result
                    } else {
                      this.friends = []
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