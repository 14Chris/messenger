<template>
  <div class="columns">
    <div class="column column-illustration">
      <img class="illustration" src="../assets/illustrations/user_chat_illustration.svg">
    </div>
    <div class="column">
      <div class="div-form">
    <h3 class="title is-3">{{ $t('title') }}</h3>
    <form @submit.prevent="register">
      <!-- First name -->
      <div class="field" v-if="!$v.model.first_name.$invalid">
        <label class="label">{{$t("firstNameLabel")}}</label>
        <div class="control">
          <input class="input" type="text" :placeholder="$t('firstNamePlaceholder')" v-model="model.first_name">
        </div>
      </div>

      <div class="field" v-else>
        <label class="label">{{$t("firstNameLabel")}}</label>
        <div class="control">
          <input class="input is-danger" type="text" :placeholder="$t('firstNamePlaceholder')"  v-model="model.first_name">
        </div>
        <p class="help is-danger">{{ $t('firstNameRequiredError') }}</p>
      </div>

      <!-- Last name -->
      <div class="field" v-if="!$v.model.last_name.$invalid">
        <label class="label">{{$t("lastNameLabel")}}</label>
        <div class="control">
          <input class="input" type="text" :placeholder="$t('lastNamePlaceholder')"  v-model="model.last_name">
        </div>
      </div>

      <div class="field" v-else>
        <label class="label">{{$t("lastNameLabel")}}</label>
        <div class="control">
          <input class="input is-danger" type="text" :placeholder="$t('lastNamePlaceholder')"  v-model="model.last_name">
        </div>
        <p class="help is-danger">{{ $t('lastNameRequiredError') }}</p>
      </div>

      <!-- Email -->
      <div class="field" v-if="!$v.model.email.$invalid">
        <label class="label">{{$t("emailLabel")}}</label>
        <div class="control">
          <input class="input" type="text" :placeholder="$t('emailPlaceholder')"  v-model="model.email">
        </div>
      </div>

      <div class="field" v-else>
        <label class="label">{{$t("emailLabel")}}</label>
        <div class="control">
          <input class="input is-danger" type="text" :placeholder="$t('emailPlaceholder')"  v-model="model.email">
        </div>
        <p class="help is-danger input-errors">
          <span v-for="(error,index) in GetEmailErrors()" :key="index">
          {{ error }}
      </span>
        </p>
      </div>

      <!-- Password -->
      <div class="field" v-if="!$v.model.password.$invalid">
        <label class="label">{{$t("passwordLabel")}}</label>
        <div class="control">
          <input class="input" type="password" :placeholder="$t('passwordPlaceholder')"  v-model="model.password">
        </div>
      </div>

      <div class="field" v-else>
        <label class="label">{{$t("passwordLabel")}}</label>
        <div class="control">
          <input class="input is-danger" type="password" :placeholder="$t('passwordPlaceholder')" v-model="model.password">
        </div>
        <p class="help is-danger input-errors">
          <span v-for="(error,index) in GetPasswordErrors()" :key="index">
          {{ error }}
      </span>
        </p>
      </div>

      <!-- Password confirmation -->
      <div class="field" v-if="!$v.model.password_confirmation.$invalid">
        <label class="label">{{$t("confirmPasswordLabel")}}</label>
        <div class="control">
          <input class="input" type="password" :placeholder="$t('confirmPasswordPlaceholder')" v-model="model.password_confirmation">
        </div>
      </div>

      <div class="field" v-else>
        <label class="label">{{$t("confirmPasswordLabel")}}</label>
        <div class="control">
          <input class="input is-danger" type="password" :placeholder="$t('confirmPasswordPlaceholder')"
                 v-model="model.password_confirmation">
        </div>
        <p class="help is-danger input-errors">
          <span v-for="(error,index) in GetConfirmPasswordErrors()" :key="index">
            {{ error }}
          </span>
        </p>
      </div>

      <div>
        <button class="button is-primary" type="submit">{{$t('registerButton')}}</button>
      </div>
      <p>{{$t('loginLabel')}}</p>
      <router-link to="/login">
        <button class="button is-primary is-outlined" type="button">{{$t('loginButton')}}</button>
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
              message: this.$t('accountCreationSuccess'),
              type: 'success',
              duration: 5000
            })
            this.$router.push("/login");
          } else {
            openNotification({
              message: this.$t('accountCreationError'),
              type: 'danger',
              duration: 5000
            })
          }
        })
            .catch(() => {
              openNotification({
                message: this.$t('accountCreationError'),
                type: 'danger',
                duration: 5000
              })
            });
      }
    },
    GetEmailErrors() {
      var errors = [];
      if (!this.$v.model.email.required) {
        errors.push(this.$t('emailRequiredError'));
      } else {
        if (!this.$v.model.email.email) {
          errors.push(this.$t('emailValidityError'));
        }
        if (!this.$v.model.email.isUnique) {
          errors.push(this.$t('emailUniqueError'));
        }
      }
      return errors;
    },
    GetPasswordErrors() {
      var errors = [];
      if (!this.$v.model.password.required) {
        errors.push(this.$t('passwordRequiredError'));
      } else {
        if (!this.$v.model.password.minLength) {
          errors.push(
              this.$t('passwordLengthError')
          );
        }
        if (!this.$v.model.password.oneNumber) {
          errors.push(this.$t('passwordNumberError'));
        }
        if (!this.$v.model.password.oneUpperCase) {
          errors.push(this.$t('passwordUpperError'));
        }
        if (!this.$v.model.password.oneLowerCase) {
          errors.push(this.$t('passwordLowerError'));
        }
        if (!this.$v.model.password.specialCharacter) {
          errors.push(this.$t('passwordSpecialCharError'));
        }
      }
      return errors;
    },
    GetConfirmPasswordErrors() {
      var errors = [];
      if (!this.$v.model.password_confirmation.required) {
        errors.push(this.$t('confirmPasswordRequiredError'));
      } else if (!this.$v.model.password_confirmation.sameAsPassword) {
        errors.push(this.$t('confirmPasswordSameError'));
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

.column{
  position: relative;
}

.column-illustration{
  background-color: whitesmoke;
}

.illustration{
  /* Center vertically and horizontally*/
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);

  height:50%;
}

.div-form{
  margin:20px;
}
</style>

<i18n>
{
  "en": {
    "title": "Register",
    "firstNameLabel": "First Name",
    "firstNamePlaceholder": "First Name",
    "lastNameLabel": "Last Name",
    "lastNamePlaceholder": "Last Name",
    "emailLabel": "Email",
    "emailPlaceholder": "Email",
    "passwordLabel": "Password",
    "passwordPlaceholder": "Password",
    "confirmPasswordLabel": "Confirm Password",
    "confirmPasswordPlaceholder": "Confirm Password",
    "registerButton": "Register",
    "loginButton": "Login",
    "loginLabel": "Already have an account ? Login :",
    "accountCreationSuccess": "Your account has been created. An email just been sent to you with the instructions for activating your account",
    "accountCreationError": "An error happened while creating your account"

  },
  "fr": {
    "title": "S'enregistrer",
    "firstNameLabel": "Prénom",
    "firstNamePlaceholder": "Prénom",
    "lastNameLabel": "Nom",
    "lastNamePlaceholder": "Nom",
    "emailLabel": "Email",
    "emailPlaceholder": "Email",
    "passwordLabel": "Mot de passe",
    "passwordPlaceholder": "Mot de passe",
    "confirmPasswordLabel": "Confirmez le mot de passe",
    "confirmPasswordPlaceholder": "Confirmez le mot de passe",
    "registerButton": "S'enregistrer",
    "loginButton": "Connexion",
    "loginLabel": "Vous avez déjà un compte ? Connectez-vous :",
    "accountCreationSuccess": "Votre compte a été créé. Un email vient de vous être envoyer contenant les instructions pour activer votre compte",
    "accountCreationError": "Un erreur est apparue pendant la création de votre compte"
  }
}
</i18n>