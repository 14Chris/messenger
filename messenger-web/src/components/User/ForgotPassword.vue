<template>
  <div class="container">
    <div class="card centered-card">
      <div class="card-content">
        <h3 class="title is-3">{{$t('title')}}</h3>
        <p>{{$t('instructionsTitle')}}</p>
        <form @submit.prevent="ForgotPassword">
          <!-- New password -->
          <div class="field" v-if="$v.email.required">
            <label class="label">{{$t('emailLabel')}}</label>
            <div class="control">
              <input
                class="input"
                type="text"
                :placeholder="$t('emailPlaceholder')"
                v-model="email"
              />
            </div>
          </div>

          <div class="field" v-else>
            <label class="label">{{$t('emailLabel')}}</label>
            <div class="control">
              <input
                class="input is-danger"
                type="text"
                :placeholder="$t('emailPlaceholder')"
                v-model="email"
              />
            </div>
            <p class="help is-danger">{{$t('emailRequiredError')}}</p>
          </div>

          <button class="button is-primary">{{$t('forgotPasswordButton')}}</button>
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
          if (response.ok == true) {
            openNotification({
              message: this.$t('requestSuccess'),
              type: 'success',
              duration: 5000
            })
            this.$router.push("/login");
          }
        })
        .catch(() => {
          openNotification({
            message: this.$t('requestError'),
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

<style scoped>

  html{
    background-color: whitesmoke;
  }

  .centered-card{
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    width: 50%;
  }
</style>

<i18n>
{
  "en": {
    "title": "Forgot password",
    "instructionsTitle": "Please enter your email address and we'll send you instructions on how to reset your password.",
    "emailLabel": "Email",
    "emailPlaceholder": "Email",
    "emailRequiredError": "Email is required",
    "forgotPasswordButton": "Send",
    "requestSuccess": "If an account is linked to this email address, you will receive a email with a link to reset your password",
    "requestError": "Error while sending forgot password request"
  },
  "fr": {
    "title": "Mot de passe oublié",
    "instructionsTitle": "Veuillez entrer votre adresse email et nous vous enverrons des instructions sur comment réinitialiser votre mot de passe.",
    "emailLabel": "Email",
    "emailPlaceholder": "Email",
    "emailRequiredError": "L'adresse email est requise",
    "forgotPasswordButton": "Envoyer",
    "requestSuccess": "Si un compte est lié à cette adresse email, vous recevrez un email contenant un lien pour réinitialiser votre mot de passe",
    "requestError": "Erreur lors de l'envoi de la requête de mot de passe oublié"
  }
}
</i18n>