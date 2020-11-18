<template>
  <div>
    <img v-if="imageExists" :src="userPictureUrl" alt="Avatar" class="avatar" />
    <div v-else class="avatar avatar-empty"></div>
  </div>
</template>

<script>
import ApiService from "@/service/api";
const api = new ApiService();
export default {
  data() {
    return {
      imageExists: false,
    };
  },
  mounted() {
    this.CheckUrlValidity();
  },
  props: ["userId"],
  computed: {
    userPictureUrl() {
      return api.apiUrl + "/users/" + this.userId + "/picture";
    },
  },
  methods: {
    CheckUrlValidity() {
      api.headData("users/" + this.userId + "/picture").then((response) => {
        console.log(response);
        this.imageExists = response.ok;
      });
    },
  },
};
</script>

<style>
.avatar {
  vertical-align: middle;
  width: 100px;
  height: 100px;
  border-radius: 50%;
}

.avatar-empty {
  background-color: red;
}
</style>