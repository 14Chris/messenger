<template>
  <div>
    <img v-if="newImage != null" :src="newImage" class="avatar" />
    <img
      v-else-if="imageExists"
      :src="userPictureUrl"
      alt="Avatar"
      class="avatar"
    />
    <div v-else class="avatar avatar-empty"></div>

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
          <span class="file-label"> Change picture </span>
        </span>
      </label>
    </div>

    <button
      class="button is-danger"
      v-if="newImage != null"
      @click="CancelImageChange"
    >
      Cancel
    </button>
    <button
      class="button is-primary"
      v-if="newImage != null"
      @click="ChangeImage"
    >
      Save
    </button>
    <button
      class="button is-danger"
      v-if="newImage == null && imageExists == true"
      @click="DeleteImage"
    >
      Delete
    </button>
  </div>
</template>

<script>
import ApiService from "@/service/api";
const api = new ApiService();
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
      api.headData("users/" + this.userId + "/picture").then((response) => {
        console.log(response);
        this.imageExists = response.ok;
      });
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
          }
        });
    },
    CancelImageChange() {
      this.newImage = null;
    },
    DeleteImage() {},
    FileUploaded(evt) {
      console.log(evt);
      console.log(this.newImage);
    },
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