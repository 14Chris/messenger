<template>
  <div class="container">
    <h3 class="title is-3">{{$t('title')}}</h3>
    <div v-if="isLoading">
      <h1>{{$t('loadingText')}}</h1>
    </div>
    <div v-else>
      <div v-if="userProfile != null">
        <AvatarEdit :userId="userProfile.id"></AvatarEdit>

        <!-- Informations -->
        <div class="box"> 
          <form @submit.prevent="UpdateUserInformations">
            <h4 class="title is-4">{{$t('informationsTitle')}}</h4>

            <div class="columns">
              <div v-if="!$v.userProfile.first_name.$invalid" class="field column">
                <label class="label">{{$t('firstNameLabel')}}</label>
                <div class="control">
                  <input
                    class="input"
                    type="text"
                    :placeholder="$t('firstNamePlaceholder')"
                    v-model="userProfile.first_name"
                    required
                  />
                </div>
              </div>

              <div v-else class="field column">
                <label class="label">{{$t('firstNameLabel')}}</label>
                <div class="control">
                  <input
                    class="input is-danger"
                     type="text"
                    :placeholder="$t('firstNamePlaceholder')"
                    v-model="userProfile.first_name"
                  />
                </div>
                <p class="help is-danger">{{$t('firstNameRequiredError')}}</p>
              </div>

              <div class="field column">
                <label class="label">{{$t('lastNameLabel')}}</label>
                <div class="control">
                  <input
                    class="input"
                    type="text"
                    :placeholder="$t('lastNamePlaceholder')"
                    v-model="userProfile.last_name"
                  />
                </div>
              </div>
            </div>

            <div class="field">
              <label class="label">{{$t('emailLabel')}}</label>
              <div class="control">
                <input
                  class="input"
                  type="text"
                  :placeholder="$t('emailPlaceholder')"
                  v-model="userProfile.email"
                  disabled
                  readonly="true"
                />
              </div>
            </div>

            <button class="button is-primary">{{$t('saveButton')}}</button>
          </form>
        </div>

        <!-- Security -->
         <div class="box">
            <h4 class="title is-4">{{$t('securityTitle')}}</h4>
            <ChangePassword></ChangePassword>
        </div>
      </div>
      <div v-else>
        <p>{{$t('getProfileError')}}</p>
      </div>
    </div>
  </div>
</template>

<script>
import { required } from "vuelidate/lib/validators";
import ApiService from "@/service/api";
import AvatarEdit from "@/components/User/Avatar/AvatarEdit";
import ChangePassword from "@/components/User/ChangePassword";
import Vue from "vue";
import Notification from "@/shared_components/Notification/Notification";

const api = new ApiService();

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
  components: {
    AvatarEdit,
    ChangePassword
  },
  data() {
    return {
      userProfile: null,
      isLoading: false,
    };
  },
  mounted() {
    this.GetUserProfileById();
  },
  methods: {
    GetUserProfileById() {
      this.isLoading = true;
      api
        .getData("users/profile")
        .then((response) => {
          if (response.ok == true) {
            response.json().then((data) => {
                this.userProfile = data.Result;
            });
          }
          else {
            this.userProfile = null;
          }
        })
        .finally(() => {
          this.isLoading = false;
        });
    },
    UpdateUserInformations() {
      if (!this.$v.$invalid) {
        api
          .update("users", JSON.stringify(this.userProfile))
          .then((response) => {
            console.log(response);
            if (response.ok == true) {
              response.json().then((data) => {
                  this.userProfile = data.Result;

                openNotification({
                  message: this.$t('requestSuccess'),
                  type: 'success',
                  duration: 5000
                })
              });
            }
            else {
              openNotification({
                message: this.$t('requestError'),
                type: 'danger',
                duration: 5000
              })
            }
          })
          .catch(() => {
            openNotification({
              message: this.$t('requestError'),
              type: 'danger',
              duration: 5000
            })
          })
          .finally(() => {
            this.isLoading = false;
          });
      }
    },
  },
  validations: {
    userProfile: {
      first_name: {
        required,
      },
    },
  },
};
</script>

<style scoped>
.box:first{
  margin-top: 25px;
}

.box{
   margin-bottom: 25px;
}
</style>

<i18n>
{
  "en": {
    "title": "Edit profile",
    "loadingText": "Loading...",
    "informationsTitle": "Informations",
    "securityTitle": "Security",
    "firstNameLabel": "First Name",
    "firstNamePlaceholder": "First Name",
    "lastNameLabel": "Last Name",
    "lastNamePlaceholder": "Last Name",
    "emailLabel": "Email",
    "emailPlaceholder": "Email",
    "getProfileError": "An error occured while retrieving your profile",
    "saveButton": "Save",
    "requestSuccess": "Your informations have been updated",
    "requestError": "Error while updating user informations"

  },
  "fr": {
    "title": "Modification du profil",
    "loadingText": "Chargement...",
    "informationsTitle": "Informations",
    "securityTitle": "Sécurité",
    "firstNameLabel": "Prénom",
    "firstNamePlaceholder": "Prénom",
    "lastNameLabel": "Nom",
    "lastNamePlaceholder": "Nom",
    "emailLabel": "Email",
    "emailPlaceholder": "Email",
    "getProfileError": "Une erreur est apparue pendant la récupération de votre profil",
    "saveButton": "Enregister"
  }
}
</i18n>