<template>
  <div>
    <img v-if="newImage != null" :src="newImage" class="avatar" />
    <img
      v-else-if="imageExists"
      :src="userPictureUrl"
      alt="Avatar"
      class="avatar"
    />
    <img v-else src="@/assets/icons/default-user-avatar.svg" alt="DefaultAvatar" class="avatar" />

    <div class="file">
      <label class="file-label">
        <input
          class="file-input"
          type="file"
          name="resume"
          :v-model="newImage"
          @change="onFileChange"
        />
        <span class="file-cta">
          <span class="file-icon">
            <i class="fas fa-upload"></i>
          </span>
          <span class="file-label">{{$t('changePictureLabel')}}</span>
        </span>
      </label>
    </div>

    <button
      class="button is-danger"
      v-if="newImage != null"
      @click="CancelImageChange"
    >
      {{$t('cancelButton')}}
    </button>
    <button
      class="button is-primary"
      v-if="newImage != null"
      @click="ChangeImage"
    >
      {{$t('saveButton')}}
    </button>
    <button
      class="button is-danger"
      v-if="newImage == null && imageExists == true"
      @click="DeleteImage"
    >
      {{$t('deleteButton')}}
    </button>
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
  data() {
    return {
      imageExists: false,
      newImage: null,
    };
  },
  mounted() {
    this.CheckUrlValidity();
  },
  props: ["userId"],
  computed: {
    userPictureUrl() {
      return api.apiUrl +"/users/" + this.userId + "/picture";
    },
  },
  methods: {
    CheckUrlValidity() {
      api.headData("users/" + this.userId + "/picture")
          .then((response) => {
        this.imageExists = response.ok;
      })
    },
    ChangeImage() {
      var imageValue = this.newImage.split("base64,")[1];
      var imageModel = {
        picture: imageValue,
      };

      api
        .create("users/" + this.userId + "/picture", JSON.stringify(imageModel))
        .then((response) => {
          if (response.ok) {
            this.CheckUrlValidity();
            this.newImage = null;
            openNotification({
              message: this.$t('changePictureSuccess'),
              type: 'success',
              duration: 5000
            })
          }
        })
      .catch(()=>{
        openNotification({
          message: this.$t('changePictureError'),
          type: 'danger',
          duration: 5000
        })

      });
    },
    CancelImageChange() {
      this.newImage = null;
    },
    DeleteImage() {},
    onFileChange(e) {
      var files = e.target.files || e.dataTransfer.files;
      if (!files.length) return;
      this.createImage(files[0]);
    },
    createImage(file) {
      var reader = new FileReader();
      var vm = this;

      reader.onload = (e) => {
        vm.newImage = e.target.result;
      };
      reader.readAsDataURL(file);
    },
  },
};
</script>

<style scoped>
.avatar {
  vertical-align: middle;
  width: 200px;
  height: 200px;
  border-radius: 50%;
}

.avatar-empty {
  background-color: lightblue;
}
</style>

<i18n>
{
  "en": {
    "saveButton": "Save",
    "cancelButton": "Cancel",
    "deleteButton": "Delete",
    "changePictureLabel": "Change picture",
    "changePictureSuccess": "Your profile picture have been changed",
    "changePictureError": "An error occured while changing your profile picture"

  },
  "fr": {
    "saveButton": "Enregistrer",
    "cancelButton": "Annuler",
    "deleteButton": "Supprimer",
    "changePictureLabel": "Changer l'image",
    "changePictureSuccess": "Votre image de profil a été changé",
    "changePictureError": "Une erreur est survenue lors du changement de votre image de profil"
  }
}
</i18n>