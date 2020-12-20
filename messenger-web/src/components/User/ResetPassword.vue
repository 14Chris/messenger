<template>
  <div class="container">
    <h3 class="title is-3">{{$t('title')}}</h3>
    <div v-if="tokenOk == null">{{$t('loadingText')}}</div>
    <div v-else-if="tokenOk == true">
      <form @submit.prevent="ResetPassword">
        <!-- New password -->
        <div class="field" v-if="!$v.model.newPassword.$invalid">
          <label class="label">{{ $t('passwordLabel') }}</label>
          <div class="control">
            <input
              class="input"
              type="password"
              :placeholder="$t('passwordPlaceholder')"
              v-model="model.newPassword"
            />
          </div>
        </div>

        <div class="field" v-else>
          <label class="label">{{ $t('passwordLabel') }}</label>
          <div class="control">
            <input
              class="input is-danger"
              type="password"
              :placeholder="$t('passwordPlaceholder')"
              v-model="model.newPassword"
            />
          </div>
          <p class="help is-danger input-errors">
            <span v-for="(error,index) in GetPasswordErrors()" :key="index">
            {{ error }}
          </span></p>
        </div>

        <!-- New password confirmation-->
        <div class="field" v-if="!$v.model.repeatNewPassword.$invalid">
          <label class="label">{{ $t('confirmPasswordLabel') }}</label>
          <div class="control">
            <input
              class="input"
              type="password"
              :placeholder="$t('confirmPasswordPlaceholder')"
              v-model="model.repeatNewPassword"
            />
          </div>
        </div>

        <div class="field" v-else>
          <label class="label">{{ $t('confirmPasswordLabel') }}</label>
          <div class="control">
            <input
              class="input is-danger"
              type="password"
              :placeholder="$t('confirmPasswordPlaceholder')"
              v-model="model.repeatNewPassword"
            />
          </div>
          <p class="help is-danger input-errors">
            <span v-for="(error,index) in GetConfirmPasswordErrors()" :key="index">
            {{ error }}
          </span>
          </p>
        </div>

        <button class="button is-primary">{{$t('resetPasswordButton')}}</button>
      </form>
    </div>
    <div v-else>{{this.$t('tokenValidationErrorTest')}}</div>
  </div>
</template>

<script>
import { required, minLength, sameAs } from "vuelidate/lib/validators";
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
      tokenOk: null,
      model: {
        newPassword: "",
        repeatNewPassword: "",
        token: "",
      },
    };
  },
  mounted() {
    console.log(this.$route)
    this.model.token = this.$route.params.token;
    this.ValidateToken(this.model.token);
  },
  methods: {
    ValidateToken(token) {
      api
        .getData("users/reset_password/" + token)
        .then((response) => {
          if (response.ok == true) {
            this.tokenOk = true;
          }
          else {
            this.tokenOk = false;
            openNotification({
              message: this.$t('tokenValidationError'),
              type: "danger",
              duration: 5000,
            });
            this.$router.push("/login");
          }
        })
        .catch(() => {
          this.tokenOk = false;
          openNotification({
            message: this.$t('tokenValidationRequestError'),
            type: "danger",
            duration: 5000,
          });
          this.$router.push("/login");
        });
    },
    ResetPassword() {
      var resetPasswordModel = {
        newPassword: this.model.newPassword,
        token: this.model.token,
      };

      api
        .create("users/reset_password", JSON.stringify(resetPasswordModel))
        .then((response) => {
          console.log(response);
          if (response.ok == true) {
              openNotification({
                message:
                    this.$t('requestSuccess'),
                type: "success",
                duration: 5000,
              });
              this.$router.push("/login");
          } else {
            response.json().then((data) => {
              var errorMessage = ""
              switch (data.Message) {
                case "SAME_NEW_PASSWORD":
                  errorMessage = this.$t('sameNewPasswordError')
                  break;
                case "NEW_PASSWORD_TOO_WEAK":
                  errorMessage = this.$t('passwordTooWeakError')
                  break;
                default:
                  errorMessage = this.$t('requestError')
              }

              openNotification({
                message: errorMessage,
                type: "danger",
                duration: 5000,
              });
            })
          }
        })
        .catch(() => {
          openNotification({
            message: this.$t('requestError'),
            type: "danger",
            duration: 5000,
          });
        });
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
      newPassword: {
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
      repeatNewPassword: {
        required,
        sameAsPassword: sameAs("newPassword"),
      },
    },
  },
};
</script>

<style>
</style>

<i18n>
{
  "en": {
    "title": "Reset password",
    "passwordLabel": "Password",
    "passwordPlaceholder": "Password",
    "confirmPasswordLabel": "Confirm Password",
    "confirmPasswordPlaceholder": "Confirm Password",
    "sameNewPasswordError": "New password has to be different from older one",
    "passwordTooWeakError": "New password is too weak",
    "requestError": "Error while changing your password",
    "requestSuccess": "Your password have been changed. You can now log in.",
    "tokenValidationError": "The token provided is invalid",
    "tokenValidationRequestError": "Error while validating the token provided",
    "tokenValidationErrorTest": "Error during token validation",
    "resetPasswordButton": "Change",
    "loadingText": "Loading..."
  },
  "fr": {
    "title": "Réinialisation du mot de passe",
    "passwordLabel": "Mot de passe",
    "passwordPlaceholder": "Mot de passe",
    "confirmPasswordLabel": "Confirmez le mot de passe",
    "confirmPasswordPlaceholder": "Confirmez le mot de passe",
    "requestError": "Error pendant le changement de votre mot de passe",
    "requestSuccess": "Votre mot de passe a été changé. Vous pouvez maintenant vous connecter",
    "tokenValidationError": "Le token fourni est invalide",
    "tokenValidationRequestError": "Erreur pendant la validation du token fourni",
    "tokenValidationErrorTest": "Erreur pendant la validation du token fourni",
    "resetPasswordButton": "Changer",
    "loadingText": "Chargement..."
  }
}
</i18n>