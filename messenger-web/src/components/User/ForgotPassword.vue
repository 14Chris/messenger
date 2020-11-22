<template>
  <div>
      <div>Forgot password</div>
      <form @submit.prevent="ForgotPassword">
        <label>Email :</label>
        <input v-model="email" type="email" required>
        <button>Validate</button>
      </form>
  </div>
</template>

<script>
import ApiService from "@/service/api";

var api = new ApiService();
export default {
  data(){
    return {
        email:""
    }
  },
  methods:{
    ForgotPassword(){
api.create("users/forgot_password", JSON.stringify(this.email))
  .then((response) => {
            console.log(response);
            if (response.ok == true) {
              response.json().then((data) => {
                if (data.ResponseType == 1) {
                  this.$buefy.notification.open({
                    message: "If an account is linked to this email address, you will receive a email with a link to reset your password",
                    type: "is-success",
                    duration: 5000,
                    closable: true,
                  });
                } 
              });
            } 
          })
          .catch(() => {
          
          });
    }
  }
}
</script>

<style>

</style>