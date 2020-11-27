<template>
  <div class="container">
    <h3 class="title is-3">Forgot password</h3>
    <form @submit.prevent="ForgotPassword">
      <!-- New password -->
      <div class="field" v-if="$v.email.required">
        <label class="label">Email</label>
        <div class="control">
          <input
            class="input"
            type="password"
            placeholder="New password"
            v-model="email"
          />
        </div>
      </div>

      <div class="field" v-else>
        <label class="label">Email</label>
        <div class="control">
          <input
            class="input is-danger"
            type="password"
            placeholder="New password"
            v-model="email"
          />
        </div>
        <p class="help is-danger">Email is required</p>
      </div>

      <button class="button is-primary">Send</button>
    </form>
  </div>
</template>

<script>
import { required, } from "vuelidate/lib/validators";
import ApiService from "@/service/api";

var api = new ApiService();
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
            response.json().then((data) => {
              if (data.ResponseType == 1) {
                this.$buefy.notification.open({
                  message:
                    "If an account is linked to this email address, you will receive a email with a link to reset your password",
                  type: "is-success",
                  duration: 5000,
                  closable: true,
                });

                this.$router.push("/login");
              }
            });
          }
        })
        .catch(() => {});
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