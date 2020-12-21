<template>
  <div class="full-page">
    <Navbar></Navbar>
    <hr class="part-separator"/>
    <div class="home">
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
import Navbar from "./Navbar/Navbar"

export default {
  components: {
    Navbar,
  },
  computed: {
    connectedUser: function () {
      return this.$store.getters.connectedUser
    },
    isLoggedIn: function () {
      return this.$store.getters.isLoggedIn
    }
  },
  methods: {
  },
}
</script>
<style>

.full-page{
  height:100%;
  display: flex;
  flex-direction: column;
}

.part-separator{

  margin: 0;
  background-color: #f2f2f2;
}

.home{
  flex:1;
  overflow-y: auto;
}

.column-no-top-marging{
  padding-top:0px !important;
  padding-bottom:0px !important;

}
</style>