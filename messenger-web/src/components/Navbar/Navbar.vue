<template>
  <nav class="navbar" role="navigation" aria-label="main navigation">
    <div class="navbar-brand">
      <a class="navbar-item" href="/">
        <img src="@/assets/chat_logo.png" alt="Chat app logo" />
      </a>

      <a
        role="button"
        class="navbar-burger burger"
        aria-label="menu"
        aria-expanded="false"
        data-target="navbarBasicExample"
      >
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
            <div class="dropdown-content">
              <router-link to="/profile/edit">
                <a class="dropdown-item"
                  >{{ connectedUser.first_name }}
                  {{ connectedUser.last_name }}</a
                >
              </router-link>
              <a class="dropdown-item" @click="logout">Logout</a>
            </div>
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
          if (resp.ResponseType == 1) {
            const user = resp.Result;
            this.connectedUser = user;
            this.CheckUrlValidity();
          }
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
    logout: function () {
      this.$store.dispatch("logout").then(() => {
        this.$router.push("/login");
      });
    },
    CheckUrlValidity() {
      api
        .headData("users/" + this.connectedUser.id + "/picture")
        .then((response) => {
          console.log(response);
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

#navbar-buttons {
  right: 10px;
  position: absolute;
}

#navbar-search {
  flex-grow: 1;
  max-width: 600px;
  min-width: 200px;
  margin: 0 auto;
}

.navbar-icon {
  border-radius: 50% !important;
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

.dropdown{
  height: 40px;
}
</style>