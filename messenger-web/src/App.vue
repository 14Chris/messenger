<template>
  <div id="app">
    <router-view />
  </div>
</template>

<script>
import ApiService from './service/api'
var api = new ApiService

export default {
  name: "App",
  mounted() {
    // await this.$store.dispatch('getUserSession') 
      api
        .getData("users/session")
        .then((response) => {
          if (response.ok) {
            response.json().then((resp) => {
              if (resp.ResponseType == 1) {
                const user = resp.Result;
                this.$store.dispatch("setUserSession", user);
              } else {
                this.$store.dispatch("logout").then(() => {
                  this.$router.push("/login");
                });
              }
            });
          }
        })
  },
  methods: {
    logout: function () {
      this.$store.dispatch("logout").then(() => {
        this.$router.push("/login");
      });
    },
  },
};
</script>


<style lang="scss">

</style>

<style>
html,
body {
  height: 100% !important;
}

#app {
  height: 100% !important;
}

button:focus {
  outline: none;
}

.icon{
  height: 20px;
  width: 20px;
}

.icon-button {
  margin-right: 15px;
  padding: 0;
  border: none;
  background: none;
  cursor: pointer;
}

</style>
