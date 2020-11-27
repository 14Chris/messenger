<template>
  <div class="container">
    <form class="login" @submit.prevent="login">
      <h1>Sign in</h1>
      <b-field label="Email">
            <b-input v-model="model.email" type="email" placeholder="Email"></b-input>
        </b-field>
        <b-field label="Password">
          <b-input v-model="model.password" type="password" placeholder="Password" password-reveal></b-input>
          </b-field>
      <b-button native-type="submit">Login</b-button>
     
    </form>
     <router-link to="/forgot_password"><a>Forgot your password ?</a></router-link>
    
    <p>No account ?  Create one :</p>
       <b-button tag="router-link"
                to="/register"
                type="is-info">
                Register
            </b-button>
  </div>
</template>a

<script>
import { required } from "vuelidate/lib/validators";
import ApiService from "../service/api";

var api = new ApiService();
export default {
  name: "Login",
  data() {
    return {
      model: {
        email: "",
        password: "",
      },
    };
  },
  methods: {
    login: function () {
      var userLogin = {
        email: this.model.email,
        password: this.model.password,
      };

      api
        .create("login", JSON.stringify(userLogin))
        .then((response) => response.json())
            .then((resp) => {
              if(resp.ResponseType == 1){
              const token = resp.Result.token;
              const user = resp.Result.user;
              this.$store.dispatch("login", { token, user }).then(() => {
                this.$router.push("/");
              });
            
          } else if (resp.HttpStatus == 401) {
             this.$buefy.notification.open({
                    message: 'Invalid credentials',
                    type: 'is-danger',
                    duration:5000,
                    closable:true
                })
          }
        })
        .catch(() => {
          this.$buefy.notification.open({
                    message: 'Error while signing in to the app',
                    type: 'is-danger',
                    duration:5000,
                    closable:true
                })
        });
    },
  },
  validations: {
    model: {
      email: {
        required,
      },
    },
    password: {
      required,
    },
  },
};
</script>

<style>
</style>