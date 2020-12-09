<template>
  <div class="container">
    
    <div v-if="isLoading">
      <p>Loading...</p>
    </div>
    <div v-else>
      <div v-if="userProfile != null">
        <h3 class="title is-3">{{ userProfile.first_name }} {{ userProfile.last_name }}</h3>
        <Avatar class="profile-avatar" :userId="userProfile.id"></Avatar>
        <div class="box">
          <h4 class="title is-4">Informations</h4>
          <div class="field">
              <label class="label">Email</label>
  
                <p class="control">
                  <input
                    class="input is-static"
                    type="email"
                    :value="userProfile.email"
                    readonly
                  />
                </p>
              </div>
        
        </div>
      </div>
      <div v-else>
        <p>An error occured while retrieving user profile</p>
      </div>
    </div>
  </div>
</template>

<script>
import ApiService from "@/service/api";
import Avatar from "@/components/User/Avatar/Avatar";
const api = new ApiService();
export default {
  name: "Profile",
  components: {
    Avatar,
  },
  data() {
    return {
      userProfile: null,
      isLoading: false,
    };
  },
  mounted() {
    this.GetUserProfileById(this.$route.params.id);
  },
  methods: {
    GetUserProfileById(id) {
      this.isLoading = true;
      api
        .getData("users/" + id + "/profile")
        .then((response) => {
          console.log(response);
          if (response.ok == true) {
            response.json().then((data) => {
              if (data.ResponseType == 1) {
                this.userProfile = data.Result;
              } else {
                this.userProfile = null;
              }
            });
          }
        })
        .catch((err) => {
          console.log(err);
        })
        .finally(() => {
          this.isLoading = false;
        });
    },
  },
};
</script>

<style scoped>

.box {
  margin-top: 25px;
}

.profile-avatar{
  height: 200px;
  width: 200px;
  margin: 0 auto;
}
</style>