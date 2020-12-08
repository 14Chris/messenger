<template>
  <div>
    <div v-if="loading ">
      <div v-if="loading == true">
        Loading...
      </div>
      <div  v-else>
        <div v-if="conversationDetail != null">

        </div>
        <div v-else>
          Error while loading conversation details
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ApiService from "@/service/api";

const api = new ApiService();
export default {
  props:["conversationId"],
  data(){
    return {
      conversationDetail:null,
      loading:null
    }
  },
  mounted() {
    this.GetConversationDetail(this.conversationId)
  },
  methods:{
    GetConversationDetail(id){
      this.loading = true;
      api
          .getData("conversation/" + id+"/detail")
          .then((response) => {
            if (response.ok == true) {
              response.json().then((data) => {
                if (data.ResponseType == 1) {
                  this.conversationDetail = data.Result;
                } else {
                  this.conversationDetail = null;
                }
              });
            }
          })
          .catch((err) => {
            console.log(err);
          })
          .finally(() => {
            this.loading = false;
          });
    }
  }
}
</script>

<style>

</style>