<template>
  <nav class="navbar" role="navigation" aria-label="main navigation">
    <div class="navbar-brand">
      <a class="navbar-item" href="/">
        <img src='@/assets/chat_logo.png' alt="Chat app logo">
      </a>

      <a role="button" class="navbar-burger burger" aria-label="menu" aria-expanded="false"
         data-target="navbarBasicExample">
        <span aria-hidden="true"></span>
        <span aria-hidden="true"></span>
        <span aria-hidden="true"></span>
      </a>
    </div>

    <div id="navbarBasicExample" class="navbar-menu">
      <div id="navbar-search" class="navbar-item">
          <Search></Search>
      </div>
      <div class="navbar-item">
        <router-link id="new-conversation-button" to="/friends">
          <button class="button navbar-icon">
                <span class="icon">
                  <i class="fas fa-users"></i>
                </span>
          </button>
        </router-link>
      </div>
      <div class="navbar-item">
        <button class="button navbar-icon">
              <span class="icon">
                <i class="fas fa-cog"></i>
              </span>
        </button>
      </div>
      <div class="navbar-item">
        <b-dropdown aria-role="list" position="is-bottom-left"
                    append-to-body>
          <button class="button navbar-icon" slot="trigger">
              <span class="icon">
                <i class="fas fa-user"></i>
              </span>
          </button>

          <b-dropdown-item aria-role="listitem">{{$store.state.user.first_name}} {{$store.state.user.last_name}}</b-dropdown-item>
          <b-dropdown-item aria-role="listitem" @click="logout">Logout</b-dropdown-item>
        </b-dropdown>
      </div>
    </div>
  </nav>
</template>

<script>
import Search from './Search'

export default {
  components: {
    Search
  },
  methods:{
    logout: function () {
      this.$store.dispatch('logout')
          .then(() => {
            this.$router.push('/login')
          })
    }
  }

}
</script>

<style>

#navbarBasicExample{
  display: flex;
}

#navbar-buttons{
  right:10px;
  position: absolute;
}

#navbar-search {
  flex-grow: 1;
  max-width: 600px;
  min-width: 200px;
  margin: 0 auto;
}

.navbar-icon{
  border-radius: 50% !important;
}

</style>