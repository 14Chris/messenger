<template>
<div id="app">
    <router-view/>
  </div>
</template>

<script>

export default {
  name:'App',
  async mounted() {
    await this.$store.dispatch('getUserSession')
  },
    computed : {
    },
    methods: {
      logout: function () {
        this.$store.dispatch('logout')
        .then(() => {
          this.$router.push('/login')
        })
      }
    },
  }
</script>


<style lang="scss">
  @import "./buefy-config.scss";
</style>

<style>

html, body{
  height:100% !important;
}

#app {
  /* font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50; */
  /* margin-top: 60px; */
  height:100% !important;
}
</style>
