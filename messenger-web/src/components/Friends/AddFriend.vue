<template>
  <div class="box">
    <h3 class="title is-3">{{$t('title')}}</h3>
    <form @submit.prevent="addFriend">
      <div class="field">
        <label class="label">{{$t('friendEmailLabel')}}</label>
        <div class="control">
          <input class="input" v-model="model.email" type="text" :placeholder="$t('friendEmailPlaceholder')">
        </div>
      </div>

      <button class="button is-primary">{{$t('addButton')}}</button>
    </form>
  </div>
</template>

<script>
import ApiService from "@/service/api";

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
  name: "AddFriend",
  data() {
    return {
      model: {
        email:""
      }
    }
  },
  methods: {
    addFriend() {
      console.log(this.model)
      api.create("friends/request", JSON.stringify(this.model.email))
          .then(response=> {
            if (response.ok == true) {
              response.json()
                  .then(() => {
                    openNotification({
                      message: this.$t('requestSuccess'),
                      type: 'success',
                      duration: 5000
                    })
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
    }
  }
}
</script>

<style scoped>

</style>

<i18n>
{
  "en": {
    "title": "Add friend",
    "friendEmailLabel": "Friend email",
    "friendEmailPlaceholder": "Type your friend email...",
    "addButton": "Add",
    "requestSuccess": "Your friend request has been sent. Please wait for your friend to accept it.",
    "requestError": "Error while sending your friend a request"
  },
  "fr": {
    "title": "Ajouter un ami",
    "friendEmailLabel": "Email de votre ami",
    "friendEmailPlaceholder": "Taper l'email de votre ami...",
    "addButton": "Ajouter",
    "requestError": "Une erreur est survenue lors de l'envoi de la requête à votre ami"
  }
}
</i18n>