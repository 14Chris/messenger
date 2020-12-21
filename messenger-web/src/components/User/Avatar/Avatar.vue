<template>
  <div>
    <img v-if="imageExists" :src="userPictureUrl" alt="Avatar" class="avatar" />
    <img v-else src="@/assets/icons/default-user-avatar.svg" alt="DefaultAvatar" class="avatar" />
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
        this.imageExists = response.ok;
      });
    },
  },
};
</script>

<style scoped>
.avatar {
  vertical-align: middle;
  width: 100%;
  height: 100%;
  border-radius: 50%;
}

.avatar-empty {
  background-color: lightblue;
}
</style>

