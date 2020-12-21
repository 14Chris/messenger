<template>
  <div class="container">
      <div v-if="isActivate == null">
            <div class="loader-wrapper">
                <div class="loader is-loading"></div>
            </div>
      </div>
      <div v-else-if="isActivate == false">
          {{$t('activationError')}}
      </div>
      <div v-else>
        <div class="card">
          <div class="card-content">
          <h2 class="title is-2">{{$t('accountActivatedTitle')}}</h2>
            <p>{{$t('accountActivatedText')}}</p>
            <p>{{$t('loginText')}}</p>
            <router-link to="/login"><button class="button is-primary is-light">Log in</button></router-link>
        </div>
        </div>
      </div>
  </div>
</template>

<script>
import ApiService from "@/service/api";

var api = new ApiService();
export default {
    data(){
        return {
            isActivate: null,
            token: ""
        }
    },
    mounted(){
        this.token = this.$route.params.token
        this.ActivateAccount()
    },
    methods:{
        ActivateAccount(){
            api.getData("users/account_activation/"+this.token)
                .then(response => {
                  if (response.ok == true) {
                    this.isActivate = true
                  }
                  else{
                      this.isActivate = false
                  }
          })
          .catch(() => {
             this.isActivate = false
          })
        }
    }
}
</script>

<style scoped>
    .loader-wrapper {
    position: absolute;
    top: 0;
    left: 0;
    height: 100%;
    width: 100%;
    background: #fff;
    opacity: 0;
    z-index: -1;
    transition: opacity .3s;
    display: flex;
    justify-content: center;
    align-items: center;
    border-radius: 6px;
        opacity: 1;
      z-index: 1;

}

.loader {
            height: 100px;
            width: 100px;
        }
</style>

<i18n>
{
  "en": {
    "activationError": "An error occured while activating your account. Please try again.",
    "accountActivatedTitle": "Account activated",
    "accountActivatedText": "Congratulations, your account has been activated !",
    "loginText": "You can now log in :",
    "loginButton": "Log in"
  },
  "fr": {
    "activationError": "Une erreur est survenue lors de l'activation de votre compte. Merci de réessayer",
    "accountActivatedTitle": "Compte activé",
    "accountActivatedText": "Félicitations, votre compte a été activé !",
    "loginText": "VOus pouvez maintenant vous connecter",
    "loginButton": "Connexion"
  }
}
</i18n>