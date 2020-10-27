<template>
  <div class="full-page">
    <Navbar></Navbar>
    <hr class="part-separator"/>
    <div class="columns columns-home">
      <div class="column column-home is-one-fifth">
        <ConversationsList></ConversationsList>
      </div>
      <div class="column column-home container">
        <router-view></router-view>
      </div>
    </div>
  </div>
</template>

<script>
import Navbar from "./Navbar/Navbar"
import ConversationsList from "@/components/ConversationView/ConversationsList";

export default {
  components: {
    Navbar,
    ConversationsList
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

.navbar{
  height:60px;
}

.full-page{
  height:100%;
  display: flex;
  flex-direction: column;
}

.columns-home{
  flex:1;
  overflow-y: hidden;
}

.columns:last-child{
  margin-bottom: 0 !important;
}

.part-separator{
  margin: 0.5em 0;
}
</style>