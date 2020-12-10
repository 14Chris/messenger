<template>
  <div>
    <div v-if="loading != null">
      <div v-if="loading == true">
        Loading...
      </div>
      <div v-else>
        <div v-if="conversationDetail != null">
          <div>
            <Avatar v-if="conversationDetail.friends.length <= 1" class="conversation-detail-avatars" userId="1"></Avatar>
            <AvatarGroup v-else class="conversation-detail-avatars" :friendsIds="conversationDetail.friends.map(f=>f.id)"></AvatarGroup>
          </div>
          <div class="conversation-detail-name">
            {{conversationDetail.name}}
          </div>
          <div class="conversation-details-actions">
            <button v-if="conversationDetail.friends.length <= 1">Call {{conversationDetail.friends[0].first_name}}</button>
            <button v-else >Call Group</button>
            <button>Video Chat</button>
          </div>
          <div v-if="conversationDetail.friends.length > 1" class="conversation-group-friends">
            Friends
          </div>
          <hr>
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
import AvatarGroup from "@/components/User/Avatar/AvatarGroup";
export default {
  props:["conversationId"],
  components:{Avatar, AvatarGroup},
  data(){
    return {
      conversationDetail:null,
      loading:null
    }
  },
  mounted() {
    console.log(this.conversationId)
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