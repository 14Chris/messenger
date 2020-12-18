<template>
  <nav>
    <a class="navbar-item" href="/">
      <img src="@/assets/chat_logo.png" alt="Chat app logo"/>
    </a>

    <!--  Search bar  -->
    <div id="navbar-search" class="navbar-item">
      <Search></Search>
    </div>

    <!--  Friends button   -->
    <div class="navbar-item">
      <router-link id="new-conversation-button" class="navbar-icon" :to="{ name: 'friends' }" tag="button">
            <span class="icon">
              <img src="@/assets/icons/friends-grey.svg"/>
            </span>
      </router-link>
    </div>

    <!--   Settings button   -->
    <div class="navbar-item">
      <router-link class="navbar-icon" :to="{ name: 'settings' }" tag="button">
            <span class="icon">
              <img src="@/assets/icons/settings-grey.svg"/>
            </span>
      </router-link>
    </div>

    <!--  User icon dropdown  -->
    <div class="navbar-item">
      <div
          id="user-dropdown"
          class="dropdown is-right"
          @click="UserDropdownClick"
      >
        <div class="dropdown-trigger">
          <img
              v-if="imageExists"
              :src="userPictureUrl"
              alt="Avatar"
              class="avatar"
              slot="trigger"
          />

          <button v-else class="button navbar-icon" slot="trigger">
              <span class="icon">
                <i class="fas fa-user"></i>
              </span>
          </button>
        </div>
        <div class="dropdown-menu" id="dropdown-menu" role="menu">
          <div class="dropdown-content" v-if="connectedUser">
            <router-link to="/profile/edit">
              <a class="dropdown-item">{{ connectedUser.first_name }}
                {{ connectedUser.last_name }}</a
              >
            </router-link>
            <a class="dropdown-item" @click="Logout">Logout</a>
          </div>
        </div>
      </div>
    </div>
  </nav>
</template>

<script>
import ApiService from "@/service/api";
import Search from "./Search";

const api = new ApiService();
export default {
  components: {
    Search,
  },
  data() {
    return {
      imageExists: false,
      connectedUser: null,
    };
  },
  mounted() {
    api.getData("users/session").then((response) => {
      if (response.ok) {
        response.json().then((resp) => {
            const user = resp.Result;
            this.connectedUser = user;
            this.CheckUrlValidity();
        });
      }
    });
  },
  computed: {
    userPictureUrl() {
      return api.apiUrl + "/users/" + this.connectedUser.id + "/picture";
    },
  },
  methods: {
    Logout: function () {
      this.$store.dispatch("logout").then(() => {
        this.$router.push("/login");
      });
    },
    CheckUrlValidity() {
      api
          .headData("users/" + this.connectedUser.id + "/picture")
          .then((response) => {
            this.imageExists = response.ok;
          });
    },
    UserDropdownClick() {
      var element = document.getElementById("user-dropdown");

      if (element.classList.contains("is-active")) {
        element.classList.remove("is-active");
      } else {
        element.classList.add("is-active");
      }
    },
  },
};
</script>

<style>
#navbarBasicExample {
  display: flex;
}

#navbar-search {
  flex: 1;
  max-width: 600px;
  /*min-width: 50px;*/
  margin: 0 auto;
}

.navbar-icon {
  width: 40px;
  height: 40px;
  border-radius: 50% !important;
  background-color: white;
  border: 1px solid lightgrey;
  color: inherit;
  vertical-align: middle;
  position: relative;
  z-index: 2;
  cursor: pointer;
}
</style>

<style scoped>
.avatar {
  width: 40px !important;
  height: 40px !important;
  max-height: none !important;
  border-radius: 50%;
  cursor: pointer;
}

.dropdown {
  height: 40px;
}

nav {
  display: flex;
}
</style>