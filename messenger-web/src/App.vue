<template>
  <div id="app">
    <router-view />
  </div>
</template>

<script>

export default {
  name: "App",
  async mounted() {
    await this.$store.dispatch('getUserSession')
    this.$store.dispatch("CreateWebSocket")

    //Set the navigator default language for i18n config
    this.$i18n.locale = navigator.language.split(/-|_/)[0]

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
