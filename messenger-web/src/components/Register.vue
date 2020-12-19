<template>
  <div class="container">
    <div class="card centered-card">
      <div class="card-content">
    <h2 class="title is-2">{{ $t('title') }}</h2>
    <form @submit.prevent="register">
      <!-- First name -->
      <div class="field" v-if="!$v.model.first_name.$invalid">
        <label class="label">First Name</label>
        <div class="control">
          <input class="input" type="text" placeholder="First Name" v-model="model.first_name">
        </div>
      </div>

      <div class="field" v-else>
        <label class="label">First Name</label>
        <div class="control">
          <input class="input is-danger" type="text" placeholder="First Name" v-model="model.first_name">
        </div>
        <p class="help is-danger">First name is required</p>
      </div>

      <!-- Last name -->
      <div class="field" v-if="!$v.model.last_name.$invalid">
        <label class="label">Last Name</label>
        <div class="control">
          <input class="input" type="text" placeholder="Last Name" v-model="model.last_name">
        </div>
      </div>

      <div class="field" v-else>
        <label class="label">Last Name</label>
        <div class="control">
          <input class="input is-danger" type="text" placeholder="Last Name" v-model="model.last_name">
        </div>
        <p class="help is-danger">Last name is required</p>
      </div>

      <!-- Email -->
      <div class="field" v-if="!$v.model.email.$invalid">
        <label class="label">Email</label>
        <div class="control">
          <input class="input" type="text" placeholder="Email" v-model="model.email">
        </div>
      </div>

      <div class="field" v-else>
        <label class="label">Email</label>
        <div class="control">
          <input class="input is-danger" type="text" placeholder="Email" v-model="model.email">
        </div>
        <p class="help is-danger input-errors">
          <span v-for="(error,index) in GetEmailErrors()" :key="index">
          {{ error }}
      </span>
        </p>
      </div>

      <!-- Password -->
      <div class="field" v-if="!$v.model.password.$invalid">
        <label class="label">Password</label>
        <div class="control">
          <input class="input" type="password" placeholder="Password" v-model="model.password">
        </div>
      </div>

      <div class="field" v-else>
        <label class="label">Password</label>
        <div class="control">
          <input class="input is-danger" type="password" placeholder="Password" v-model="model.password">
        </div>
        <p class="help is-danger input-errors">
          <span v-for="(error,index) in GetPasswordErrors()" :key="index">
          {{ error }}
      </span>
        </p>
      </div>

      <!-- Password confirmation -->
      <div class="field" v-if="!$v.model.password_confirmation.$invalid">
        <label class="label">Confirm Password</label>
        <div class="control">
          <input class="input" type="password" placeholder="Confirm Password" v-model="model.password_confirmation">
        </div>
      </div>

      <div class="field" v-else>
        <label class="label">Confirm Password</label>
        <div class="control">
          <input class="input is-danger" type="password" placeholder="Confirm Password"
                 v-model="model.password_confirmation">
        </div>
        <p class="help is-danger input-errors">
          <span v-for="(error,index) in GetConfirmPasswordErrors()" :key="index">
            {{ error }}
          </span>
        </p>
      </div>

      <div>
        <button class="button is-primary" type="submit">Register</button>
      </div>
      <p>Already have an account ? Log in :</p>
      <router-link to="/login">
        <button class="button is-primary is-light" type="button">Sign in</button>
      </router-link>

    </form>
  </div>
  </div>
  </div>
</template>

<script>
import {required, minLength, sameAs, email} from "vuelidate/lib/validators";
import ApiService from "../service/api";

import Notification from "@/shared_components/Notification/Notification";

const api = new ApiService();

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
  name: "Register",
  data() {
    return {
      model: {
        first_name: "",
        last_name: "",
        email: "",
        password: "",
        password_confirmation: "",
      },
    };
  },
  methods: {
    register: function () {
      let data = {
        first_name: this.model.first_name,
        last_name: this.model.last_name,
        email: this.model.email,
        password: this.model.password,
      };

      this.$v.$touch();

      if (!this.$v.$invalid) {
        api
            .create("users", JSON.stringify(data))
            .then((response) => response.json()).then(data => {
          if (data.ResponseType == 1) {
            openNotification({
              message: "Your account has been created. Please check your emails to validate your before sign in.",
              type: 'success',
              duration: 5000
            })
            this.$router.push("/login");
          } else {
            openNotification({
              message: "An error happened while creating your account",
              type: 'danger',
              duration: 5000
            })
          }
        })
            .catch(() => {
              openNotification({
                message: "An error happened while creating your account",
                type: 'danger',
                duration: 5000
              })
            });
      }
    },
    GetEmailErrors() {
      var errors = [];
      if (!this.$v.model.email.required) {
        errors.push("Email is required");
      } else {
        if (!this.$v.model.email.email) {
          errors.push("Please enter valid email address");
        }
        if (!this.$v.model.email.isUnique) {
          errors.push("Email is already taken");
        }
      }
      return errors;
    },
    GetPasswordErrors() {
      var errors = [];
      if (!this.$v.model.password.required) {
        errors.push("Password is required");
      } else {
        if (!this.$v.model.password.minLength) {
          errors.push(
              "Password must have at least " +
              this.$v.model.password.$params.minLength.min +
              " letters."
          );
        }
        if (!this.$v.model.password.oneNumber) {
          errors.push("Password must have at least one number");
        }
        if (!this.$v.model.password.oneUpperCase) {
          errors.push("Password must have at least one upper case character");
        }
        if (!this.$v.model.password.oneLowerCase) {
          errors.push("Password must have at least one lower case character");
        }
        if (!this.$v.model.password.specialCharacter) {
          errors.push("Password must have at least one special character among these : *.!@$%^&(){}[]:;<>,?/~_+-=|");
        }
      }
      return errors;
    },
    GetConfirmPasswordErrors() {
      var errors = [];
      if (!this.$v.model.password_confirmation.required) {
        errors.push("Confirmation password is required");
      } else if (!this.$v.model.password_confirmation.sameAsPassword) {
        errors.push("Confirmation password has to be the same as password");
      }
      return errors;
    },
  },
  validations: {
    model: {
      first_name: {
        required,
      },
      last_name: {
        required,
      },
      email: {
        required,
        email,
        async isUnique(value) {
          if (value == null || value == "") return true;

          if (this.chkUsernameAvailabilityTimer) {
            clearTimeout(this.chkUsernameAvailabilityTimer);
            this.chkUsernameAvailabilityTimer = null;
          }
          return new Promise((resolve) => {
            this.chkUsernameAvailabilityTimer = setTimeout(() => {
              api.getData("users/email_exists/" + value).then((response) => {
                if (response.ok) {
                  resolve(true);
                } else {
                  resolve(false);
                }
              })
            }, 500);
          });
        },
      },
      password: {
        required,
        minLength: minLength(8),
        oneNumber(password) {
          return /(?=.*\d.*)/.test(password);
        },
        oneUpperCase(password) {
          return /(?=.*[A-Z].*)/.test(password);
        },
        oneLowerCase(password) {
          return /(?=.*[a-z].*)/.test(password);
        },
        specialCharacter(password){
          let regexSpeChar = RegExp("(?=.*[*\\.!@$%^&\\(\\){}\\[\\]:;<>,?\\/~_+\\-=|].*)")
          return regexSpeChar.test(password);
        }
      },
      password_confirmation: {
        required,
        sameAsPassword: sameAs("password"),
      },
    },
  },
};
</script>

<style>
.error {
  color: red;
}

.input-errors{
  display: flex !important;
  flex-direction: column;
}
</style>

<i18n>
{
  "en": {
    "title": "Register",

  },
  "fr": {
    "title": "S'enregistrer",
  }
}
</i18n>