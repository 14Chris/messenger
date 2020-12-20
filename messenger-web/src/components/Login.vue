<template>
  <div class="columns full-page">
    <div class="column column-illustration">
      <img class="illustration" src="../assets/illustrations/form_illustration.svg">
    </div>
    <div class="column">
      <div class="div-form">
        <h3 class="title is-3">{{$t('title')}}</h3>
        <form class="login" @submit.prevent="login">
          <div class="field">
            <label class="label">{{$t('emailLabel')}}</label>
            <div class="control">
              <input class="input" v-model="model.email"
                     type="email"
                     :placeholder="$t('emailPlaceholder')">
            </div>
          </div>

          <div class="field">
            <label class="label">{{$t('passwordLabel')}}</label>
            <div class="control">
              <input class="input" v-model="model.password"
                     type="password"
                     :placeholder="$t('passwordPlaceholder')">
            </div>
          </div>
          <div>
            <button class="button is-primary" type="submit">{{$t('loginButton')}}</button>
            <router-link to="/forgot_password" class="forgot-password-link">{{$t('forgotPasswordLabel')}}</router-link>
          </div>

        </form>
        <p>{{$t('registerLabel')}}</p>
        <router-link to="/register">
          <button class="button is-primary is-outlined" type="submit">{{$t('registerButton')}}</button>
        </router-link>
      </div>
      </div>
      </div>
</template>

<script>
import {required} from "vuelidate/lib/validators";
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
              this.$store.dispatch("login", {token, user}).then(() => {
                this.$router.push("/");
              });
            } else {
              var errorMessage = "";

              switch (resp.Message) {
                case "BAD_CREDENTIALS":
                  errorMessage = this.$t('invalidCredentialsError');
                  break;
                case "NOT_ACTIVATED":
                  errorMessage =
                       this.$t('unactiveAccountError');
                  break;
                default:
                  errorMessage = this.$t('loginError');
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
              message: this.$t('loginError'),
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
.container {
  position: relative !important;
}


.forgot-password-link{
  float: right;

}
</style>

<i18n>
{
  "en": {
    "title": "Login",
    "emailLabel": "Email",
    "emailPlaceholder": "Email",
    "passwordLabel": "Password",
    "passwordPlaceholder": "Password",
    "forgotPasswordLabel": "Forgot your password ?",
    "registerLabel": "No account ? Create one :",
    "registerButton": "Register",
    "loginButton": "Login",
    "invalidCredentialsError": "Invalid credentials",
    "unactiveAccountError": "Your account has not been activated yet. Please check your emails and clicked on the link to activate it.",
    "loginError": "Error while signing in to the app"
  },
  "fr": {
    "title": "Connexion",
    "emailLabel": "Email",
    "emailPlaceholder": "Email",
    "passwordLabel": "Mot de passe",
    "passwordPlaceholder": "Mot de passe",
    "forgotPasswordLabel": "Mot de passe oublié ?",
    "registerLabel": "Pas encore de compte ? Créez en un :",
    "registerButton": "S'enregistrer",
    "loginButton": "Connexion",
    "invalidCredentialsError": "Identifiants invalides",
    "unactiveAccountError": "Votre compte n'est pas encore activé. Veuillez vérifiez vos emails et clicker sur le lien fourni pour l'activer.",
    "loginError": "Erreur lors de la connexion à l'application"
  }
}
</i18n>