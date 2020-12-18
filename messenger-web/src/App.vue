<template>
  <div id="app">
    <router-view />
  </div>
</template>

<script>
// import ApiService from './service/api'
// var api = new ApiService

export default {
  name: "App",
  async mounted() {
    await this.$store.dispatch('getUserSession')
    this.$store.dispatch("CreateWebSocket")

  },
};
</script>


<style>
@import "styles/mystyles.css";

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
