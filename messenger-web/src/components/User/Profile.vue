<template>
  <div class="container">
    
    <div v-if="isLoading">
      <p>{{$t('loadingText')}}</p>
    </div>
    <div v-else>
      <div v-if="userProfile != null">
        <h3 class="title is-3">{{ userProfile.first_name }} {{ userProfile.last_name }}</h3>
        <Avatar class="profile-avatar" :userId="userProfile.id"></Avatar>
        <div class="box">
          <h4 class="title is-4">{{$t('informationsTitle')}}</h4>
          <div class="field">
              <label class="label">{{$t('emailLabel')}}</label>
  
                <p class="control">
                  <input
                    class="input is-static"
                    type="email"
                    :value="userProfile.email"
                    readonly
                  />
                </p>
              </div>
        
        </div>
      </div>
      <div v-else>
        <p>{{$t('getProfileError')}}</p>
      </div>
    </div>
  </div>
</template>

<script>
import ApiService from "@/service/api";
import Avatar from "@/components/User/Avatar/Avatar";
const api = new ApiService();
export default {
  name: "Profile",
  components: {
    Avatar,
  },
  data() {
    return {
      userProfile: null,
      isLoading: false,
    };
  },
  mounted() {
    this.GetUserProfileById(this.$route.params.id);
  },
  methods: {
    GetUserProfileById(id) {
      this.isLoading = true;
      api
        .getData("users/" + id + "/profile")
        .then((response) => {
          if (response.ok == true) {
            response.json().then((data) => {
                this.userProfile = data.Result;

            });
          }
          else {
            this.userProfile = null;
          }
        })
        .finally(() => {
          this.isLoading = false;
        });
    },
  },
};
</script>

<style scoped>

.box {
  margin-top: 25px;
}

.profile-avatar{
  height: 200px;
  width: 200px;
  margin: 0 auto;
}
</style>


<i18n>
{
  "en": {
    "loadingText": "Loading...",
    "informationsTitle": "Informations",
    "emailLabel": "Email",
    "getProfileError": "An error occured while retrieving user profile"
  },
  "fr": {
    "loadingText": "Chargement...",
    "informationsTitle": "Informations",
    "emailLabel": "Email",
    "getProfileError": "Une erreur est apparue pendant la récupération du profil"
  }
}
</i18n>