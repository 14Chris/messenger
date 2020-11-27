<template>
  <div>
      <div v-if="isActivate == null">
          Activating...
      </div>
      <div v-else-if="isActivate == false">
          An error occured while activating your account. Retry again.
      </div>
      <div v-else>
          Your account have been activated
      </div>
  </div>
</template>

<script>
// import ApiService from "@/service/api";

// var api = new ApiService();
export default {
    data(){
        return {
            isActivate:null,
            token:""
        }
    },
    mounted(){
        this.token = this.$route.params.token
        this.ActivateAccount()
    },
    methods:{
        ActivateAccount(){
            //TODO
        }
    }
}
</script>

<style>

</style>