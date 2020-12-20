<template>
  <div>
    <h5 class="title is-5">{{ $t('title') }}</h5>
    <form @submit.prevent="ChangePassword">
      <!-- Old password -->
      <div class="field" v-if="!$v.model.oldPassword.$invalid">
        <label class="label">{{ $t('oldPasswordLabel') }}</label>
        <div class="control">
          <input class="input" type="password" :placeholder="$t('oldPasswordPlaceholder')" v-model="model.oldPassword">
        </div>
      </div>

      <div class="field" v-else>
        <label class="label">{{ $t('oldPasswordLabel') }}</label>
        <div class="control">
          <input class="input is-danger" type="password" :placeholder="$t('oldPasswordPlaceholder')"
                 v-model="model.oldPassword">
        </div>
        <p class="help is-danger">{{ $t('oldPasswordRequiredError') }}</p>
      </div>

      <div class="field" v-if="!$v.model.newPassword.$invalid">
        <label class="label">{{ $t('newPasswordLabel') }}</label>
        <div class="control">
          <input class="input" type="password" :placeholder="$t('newPasswordPlaceholder')" v-model="model.newPassword">
        </div>
      </div>

      <div class="field" v-else>
        <label class="label">{{ $t('newPasswordLabel') }}</label>
        <div class="control">
          <input class="input is-danger" type="password" :placeholder="$t('newPasswordPlaceholder')"
                 v-model="model.newPassword">
        </div>
        <p class="help is-danger input-errors">
          <span v-for="(error,index) in GetPasswordErrors()" :key="index">
            {{ error }}
          </span></p>
      </div>

      <div class="field" v-if="!$v.model.repeatPassword.$invalid">
        <label class="label">{{ $t('confirmNewPasswordLabel') }}</label>
        <div class="control">
          <input class="input" type="password" :placeholder="$t('confirmNewPasswordPlaceholder')"
                 v-model="model.repeatPassword">
        </div>
      </div>

      <div class="field" v-else>
        <label class="label">{{ $t('confirmNewPasswordLabel') }}</label>
        <div class="control">
          <input class="input is-danger" type="password" :placeholder="$t('confirmNewPasswordPlaceholder')"
                 v-model="model.repeatPassword">
        </div>
        <p class="help is-danger input-errors"><span v-for="(error,index) in GetConfirmPasswordErrors()" :key="index">
            {{ error }}
          </span></p>
      </div>

      <button class="button is-primary">{{ $t('changeButton') }}</button>
    </form>
  </div>
</template>

<script>
import {required, minLength, sameAs,} from "vuelidate/lib/validators";
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
      model: {
        oldPassword: "",
        newPassword: "",
        repeatPassword: ""
      }
    }
  },
  methods: {
    GetPasswordErrors() {
      var errors = [];
      if (!this.$v.model.newPassword.required) {
        errors.push(this.$t('newPasswordRequiredError'));
      } else {
        if (!this.$v.model.newPassword.minLength) {
          errors.push(
              this.$t('newPasswordLengthError')
          );
        }
        if (!this.$v.model.newPassword.oneNumber) {
          errors.push(this.$t('newPasswordNumberError'));
        }
        if (!this.$v.model.newPassword.oneUpperCase) {
          errors.push(this.$t('newPasswordUpperError'));
        }
        if (!this.$v.model.newPassword.oneLowerCase) {
          errors.push(this.$t('newPasswordLowerError'));
        }
        if (!this.$v.model.newPassword.specialCharacter) {
          errors.push(this.$t('newPasswordSpecialCharError'));
        }
      }
      return errors;
    },
    GetConfirmPasswordErrors() {
      var errors = [];
      if (!this.$v.model.repeatPassword.required) {
        errors.push(this.$t('confirmNewPasswordRequiredError'));
      } else if (!this.$v.model.repeatPassword.sameAsPassword) {
        errors.push(this.$t('confirmNewPasswordSameError'));
      }
      return errors;
    },
    ChangePassword() {
      if (!this.$v.$invalid) {
        api
            .update("users/password", JSON.stringify(this.model))
            .then((response) => {
              if (response.ok == true) {
                openNotification({
                  message: this.$t("requestSuccess"),
                  type: 'success',
                  duration: 5000
                })
                this.model.newPassword = ""
                this.model.oldPassword = ""
                this.model.repeatPassword = ""

              } else {
                response.json().then((data) => {
                  var errorMessage = ""

                  switch (data.Message) {
                    case "OLD_PASSWORD_DIFFERENT":
                      errorMessage = this.$t("oldPasswordDifferentError");
                      break;
                    case "SAME_NEW_PASSWORD":
                      errorMessage = this.$t("sameNewPasswordError");
                      break;
                    case "NEW_PASSWORD_TOO_WEAK":
                      errorMessage = this.$t("newPasswordTooWeak");
                      break;
                    default:
                      errorMessage = this.$t("requestError")
                  }

                  openNotification({
                    message: errorMessage,
                    type: 'danger',
                    duration: 5000
                  })

                })
              }
            })
            .catch(() => {
              openNotification({
                message: this.$t("requestError"),
                type: 'danger',
                duration: 5000
              })
            });
      }
    }
  },
  validations: {
    model: {
      oldPassword: {
        required
      },
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
        specialCharacter(password) {
          let regexSpeChar = RegExp("(?=.*[*\\.!@$%^&\\(\\){}\\[\\]:;<>,?\\/~_+\\-=|].*)")
          return regexSpeChar.test(password);
        }
      },
      repeatPassword: {
        required,
        sameAsPassword: sameAs("newPassword"),
      },
    },
  },
}

</script>

<style>

</style>

<i18n>
{
  "en": {
    "title": "Change password",
    "oldPasswordLabel": "Old password",
    "oldPasswordPlaceholder": "Old password",
    "newPasswordLabel": "New password",
    "newPasswordPlaceholder": "New password",
    "confirmNewPasswordLabel": "Confirm new password",
    "confirmNewPasswordPlaceholder": "Confirm new password",
    "oldPasswordRequiredError": "Old password is required",
    "newPasswordRequiredError": "New password is required",
    "confirmNewPasswordRequiredError": "New password confirmation is required",
    "newPasswordNumberError": "New password must have at least one number",
    "newPasswordUpperError": "New password must have at least one upper case character",
    "newPasswordLowerError": "New password must have at least one lower case character",
    "newPasswordSpecialCharError": "New password must have at least one special character among these : *.!@$%^&(){}[]:;<>,?/~_+-=|",
    "newPasswordLengthError": "Password must have at least 8 characters",
    "confirmNewPasswordSameError": "New password confirmation has to be the same as new password",
    "changeButton": "Change",
    "requestSuccess": "Password have been changed",
    "requestError": "Error while changing password",
    "oldPasswordDifferentError": "Old password is incorrect",
    "sameNewPasswordError": "New password has to be different from old password",
    "newPasswordTooWeak": "New password is too weak"
  },
  "fr": {
    "title": "Changement du mot de passe",
    "oldPasswordLabel": "Ancien mot de passe",
    "oldPasswordPlaceholder": "Ancien mot de passe",
    "newPasswordLabel": "Nouveau mot de passe",
    "newPasswordPlaceholder": "Nouveau mot de passe",
    "confirmNewPasswordLabel": "Confirmation du nouveau mot de passe",
    "confirmNewPasswordPlaceholder": "Confirmation du nouveau mot de passe",
    "oldPasswordRequiredError": "L'ancien mot de passe est requis",
    "newPasswordRequiredError": "Le nouveau mot de passe est requis",
    "confirmNewPasswordRequiredError": "La confirmation du nouveau mot de passe est requis",
    "newPasswordNumberError": "Le mot de passe doit avoir au moins un nombre",
    "newPasswordUpperError": "Le mot de passe doit avoir au moins une lettre majuscule",
    "newPasswordLowerError": "Le mot de passe doit avoir au moins une lettre minuscule",
    "newPasswordSpecialCharError": "Le mot de passe doit avoir au moins un caractère spécial parmi ceuxlà : *.!@$%^&(){}[]:;<>,?/~_+-=|",
    "newPasswordLengthError": "Le mot de passe doit avoir au moins 8 caractères",
    "confirmNewPasswordSameError": "Le mot de passe de confirmation est différent du mot de passe",
    "changeButton": "Changer",
    "requestSuccess": "Votre mot de passe a bien été changé",
    "requestError": "Erreur lors du changement de votre mot de passe",
    "oldPasswordDifferentError": "L'ancien mot de passe est incorrect",
    "sameNewPasswordError": "Le nouveau mot de passe doit être différent de l'ancien",
    "newPasswordTooWeak": "Le nouveau mot de passe est trop faible"
  }
}
</i18n>