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
                console.log(user);
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
@import "./buefy-config.scss";
</style>

<style>
html,
body {
  height: 100% !important;
}

#app {
  /* font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50; */
  /* margin-top: 60px; */
  height: 100% !important;
}
</style>
