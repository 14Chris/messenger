<template>
  <div>
      <h3 class="title is-3">Reset password</h3>
      <div v-if="tokenOk == null">
        Loading...
      </div>
      <div v-else-if="tokenOk == true">
        token : {{model.token}}
      </div>
      <div v-else>
        Error during token validation
      </div>
  </div>
</template>

<script>
import ApiService from "@/service/api";

var api = new ApiService();
export default {
data(){
    return {
        tokenOk:null,
        model:{
            newPassword:"",
            repeatNewPassword:"",
            token:""
        }
    }
  },
  mounted(){
      this.model.token = this.$route.params.token
      this.ValidateToken(this.model.token)
  },
  methods:{
    ValidateToken(token){
api.getData("users/reset_password/"+token)
  .then((response) => {
            console.log(response);
            if (response.ok == true) {
              response.json().then((data) => {
                if (data.ResponseType == 1) {
                    this.tokenOk = true 
                }
                else{
                    this.tokenOk = false
                    this.$buefy.notification.open({
                    message: "Invalid token",
                    type: "is-success",
                    duration: 5000,
                    closable: true,
                  });
                  this.$router.push("/login")
                } 
              });
            } 
          })
          .catch(() => {
              this.tokenOk = false
              this.$buefy.notification.open({
                    message: "Invalid token",
                    type: "is-success",
                    duration: 5000,
                    closable: true,
                  });
              this.$router.push("/login")
              
          });
    },
    ResetPassword(){
        var resetPasswordModel = {
            newPassword : this.model.newPassword,
            token :this.model.token
        }

        api.create("users/reset_password", JSON.stringify(resetPasswordModel))
  .then((response) => {
            console.log(response);
            if (response.ok == true) {
              response.json().then((data) => {
                if (data.ResponseType == 1) {
                  this.$buefy.notification.open({
                    message: "Your password have been changed. You can now log in.",
                    type: "is-success",
                    duration: 5000,
                    closable: true,
                  });
                  this.$router.push("/login")
                }
                else{
                    this.$buefy.notification.open({
                    message: "Error while resetting your password",
                    type: "is-danger",
                    duration: 5000,
                    closable: true,
                  });
                } 
              });
            } 
          })
          .catch(() => {
              this.$buefy.notification.open({
                    message: "Error while resetting your password",
                    type: "is-danger",
                    duration: 5000,
                    closable: true,
                  });
              
          });
    }
  }
}
</script>

<style>

</style>