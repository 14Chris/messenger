<template>
  <div>
    <div v-if="friends != null">
      <button class="button is-primary is-light" @click="OpenAddFriendModal">Add new friend</button>
      <div class="box" v-for="f in friends" :key="f.id" @click="GoToUserProfile(f.id)">
        <h1>{{ f.first_name }} {{ f.last_name }}</h1>
        <button class="button button is-danger is-light" @click="DeleteFriend(f.id)">
              <span class="icon">
                <i class="fas fa-times"></i>
              </span>
        </button>
      </div>
    </div>

    <div class="modal" id="add-friend-modal">
      <div class="modal-background"></div>
      <div class="modal-content">
        <AddFriend></AddFriend>
      </div>
      <button class="modal-close is-large" aria-label="close" @click="CloseAddFriendModal"></button>
    </div>
  </div>
</template>

<script>
import ApiService from "@/service/api";
import AddFriend from "@/components/Friends/AddFriend";

const api = new ApiService();

export default {
  name: "FriendsList",
  components: {AddFriend},
  data() {
    return {
      friends: null,
    }
  },
  mounted() {
    this.GetFriends()
  },
  methods: {
    GetFriends() {
      api.getData("friends")
          .then(response => {
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
    },
    OpenAddFriendModal() {
      var element = document.getElementById("add-friend-modal");
      element.classList.add("is-active");
    },
    CloseAddFriendModal() {
      var element = document.getElementById("add-friend-modal");
      element.classList.remove("is-active");
    },
    DeleteFriend(id) {
      api.delete("friends", JSON.stringify(id))
          .then(response => {
            console.log(response)
            if (response.ok == true) {
              response.json()
                  .then(data => {
                    if (data.ResponseType == 1) {
                      this.getFriends()
                    } else {
                      this.$buefy.notification.open({
                        message: 'Error while deleting the friend',
                        type: 'is-danger',
                        duration: 5000,
                        closable: true
                      })
                    }
                  })
            }
          })
          .catch(err => {
            console.log(err)
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
    cursor: pointer;
  }
</style>