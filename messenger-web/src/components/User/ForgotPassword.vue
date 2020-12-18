<template>
  <div class="container">
    <div class="card">
      <div class="card-content">
        <h2 class="title is-2">Forgot password</h2>
        <form @submit.prevent="ForgotPassword">
          <!-- New password -->
          <div class="field" v-if="$v.email.required">
            <label class="label">Email</label>
            <div class="control">
              <input
                class="input"
                type="text"
                placeholder="Email"
                v-model="email"
              />
            </div>
          </div>

          <div class="field" v-else>
            <label class="label">Email</label>
            <div class="control">
              <input
                class="input is-danger"
                type="text"
                placeholder="Email"
                v-model="email"
              />
            </div>
            <p class="help is-danger">Email is required</p>
          </div>

          <button class="button is-primary">Send</button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { required, } from "vuelidate/lib/validators";
import ApiService from "@/service/api";
import Vue from "vue";
import Notification from "@/shared_components/Notification/Notification";

var api = new ApiService();

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
  data() {
    return {
      email: "",
    };
  },
  methods: {
    ForgotPassword() {
      api
        .create("users/forgot_password", JSON.stringify(this.email))
        .then((response) => {
          console.log(response);
          if (response.ok == true) {
            openNotification({
              message: "If an account is linked to this email address, you will receive a email with a link to reset your password",
              type: 'success',
              duration: 5000
            })

            this.$router.push("/login");
          }
        })
        .catch(() => {
          openNotification({
            message: "Error while sending reset password request",
            type: 'danger',
            duration: 5000
          })
        });
    },
  },
  validations: {
    email: {
      required,
    },
  },
};
</script>

<style>
</style>