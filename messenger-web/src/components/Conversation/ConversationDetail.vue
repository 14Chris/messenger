<template>
  <div>
    <div v-if="loading != null">
      <div v-if="loading == true">
        Loading...
      </div>
      <div v-else>
        <div v-if="conversationDetail != null">
          <div>
            <Avatar class="conversation-detail-avatars" userId="1"></Avatar>
          </div>
          <div class="conversation-detail-name">
            {{conversationDetail.name}}
          </div>
          <div class="conversation-details-actions">
            <button>Call</button>
            <button>Video chat</button>
          </div>
          <div>
            <div class="conversation-search">
              Search in conversation
            </div>
            <hr>
            <div class="conversation-change-color">
              Change color
            </div>
            <hr>
            <div class="conversation-change-emoji">
              Change smiley
            </div>
            <hr>
          </div>
          <div class="conversation-shared-photos">
            Shared photos
          </div>
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

import Avatar from "@/components/User/Avatar/Avatar";
export default {
  props:["conversationId"],
  components:{Avatar},
  data(){
    return {
      conversationDetail:null,
      loading:null
    }
  },
  mounted() {
    // this.GetConversationDetail(this.conversationId)
    this.conversationDetail = {
      name:"Corentin Fievet"
    }

    this.loading = false
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
  .conversation-detail-avatars{
    height: 75px;
    width: 75px;
    margin: 0 auto 15px;
  }

  .conversation-detail-name{
    margin: 0 auto 25px;
    width: fit-content;
    font-size: 1.2em;
  }

  .conversation-details-actions{
    margin: 0 auto 25px;
    width: fit-content;
  }
</style>