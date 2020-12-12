<template>
  <div class="container">
    <form class="login" @submit.prevent="login">
      <h1>Sign in</h1>


      <div class="field">
        <label class="label">Email</label>
        <div class="control">
          <input class="input" v-model="model.email"
                 type="email"
                 placeholder="Email">
        </div>
      </div>

      <div class="field">
        <label class="label">Password</label>
        <div class="control">
          <input class="input" v-model="model.password"
                 type="password"
                 placeholder="Password">
        </div>
      </div>
      <button type="submit">Login</button>
    </form>
    <router-link to="/forgot_password"
      ><a>Forgot your password ?</a></router-link
    >

    <p>No account ? Create one :</p>
    <router-link to="/register">
      <button type="submit">Register</button>
    </router-link>
  </div>
</template>

<script>
import { required } from "vuelidate/lib/validators";
import ApiService from "../service/api";
import Notification from "@/shared_components/Notification/Notification";

var api = new ApiService();

import Vue from 'vue'

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
  name: "Login",
  components: {},
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
          if (resp.ResponseType == 1) {
            const token = resp.Result.token;
            const user = resp.Result.user;
            this.$store.dispatch("login", { token, user }).then(() => {
              this.$router.push("/");
            });
          } else {
            var errorMessage = "";

            switch (resp.Message) {
              case "BAD_CREDENTIALS":
                errorMessage = "Invalid credentials";
                break;
              case "NOT_ACTIVATED":
                errorMessage =
                  "Your account has not been activated yet. Please check your emails and clicked on the link to activate it.";
                break;
              default:
                errorMessage = "Error while signing in to the app";
            }

            openNotification({
              message: errorMessage,
              type: 'danger',
              duration: 5000
            })
           }
        })
        .catch(() => {
          openNotification({
            message:  "Error while signing in to the app",
            type: 'danger',
            duration: 5000
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