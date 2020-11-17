<template>
  <div>
      <h1>Edit profile</h1>
      <div v-if="isLoading">
      <h1>Loading...</h1>
    </div>
    <div v-else>
        <div v-if="userProfile != null">
          <AvatarEdit :userId="userProfile.id"></AvatarEdit>
          {{userProfile.first_name}}
          {{userProfile.last_name}}
          {{userProfile.email}}
      </div>
      <div v-else>
        <p>An error occured while retrieving user profile</p>
      </div>
    </div>
  </div>
</template>

<script>
import ApiService from "@/service/api";
import AvatarEdit from "@/components/User/Avatar/AvatarEdit"
const api = new ApiService();
export default {
  components:{
    AvatarEdit
  },
  data(){
      return {
        userProfile: null,
        isLoading:false
      }
    },
    mounted(){
      this.GetUserProfileById()
    },
    methods:{
      GetUserProfileById(){
        this.isLoading = true
        api.getData("users/profile")
          .then(response => {
            console.log(response)
            if (response.ok == true) {
              response.json()
                  .then(data => {
                    if (data.ResponseType == 1) {
                      this.userProfile = data.Result
                    } else {
                      this.userProfile = null
                    }
                  })
            }
          })
          .catch(err => {
            console.log(err)
          })
          .finally(()=>{
            this.isLoading = false
          })
      }
    }
}
</script>

<style>

</style>