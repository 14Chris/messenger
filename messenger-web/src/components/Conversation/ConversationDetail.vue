<template>
  <div>
    <div v-if="loading != null">
      <div v-if="loading == true">
        Loading...
      </div>
      <div v-else>
        <div v-if="conversationDetail != null">
            <Avatar v-if="conversationDetail.friends.length <= 1" class="conversation-detail-avatar" userId="1"></Avatar>
            <AvatarGroup v-else class="conversation-detail-avatars" :friendsIds="conversationDetail.friends.map(f=>f.id)"></AvatarGroup>
          <div class="conversation-detail-name">
            {{conversationDetail.name}}
          </div>
          <div class="conversation-details-actions">
            <button class="conversation-details-actions-btn button is-primary is-light" v-if="conversationDetail.friends.length <= 1"><img class="action-img conversation-details-actions-img" src="@/assets/icons/image-grey.svg"/> Call {{conversationDetail.friends[0].first_name}}</button>
            <button class="conversation-details-actions-btn button is-primary is-light" v-else><img class="action-img conversation-details-actions-img" src="@/assets/icons/image-grey.svg"/> Call Group</button>
            <button class="conversation-details-actions-btn button is-primary is-light">
              <img class="action-img conversation-details-actions-img" src="@/assets/icons/image-grey.svg"/>Video Chat</button>
          </div>
<!--          <div v-if="conversationDetail.friends.length > 1" class="conversation-group-friends">-->
<!--            Friends-->
<!--          </div>-->
<!--          <hr>-->
          <div>
            <div class="conversation-details-options conversation-search">
              Search in conversation
              <img class="action-img action-img-right" src="@/assets/icons/search-grey.svg"/>
            </div>
            <hr>
            <div class="conversation-details-options conversation-change-color">
              Change color
            </div>
            <hr>
            <div class="conversation-details-options conversation-change-emoji">
              Change smiley
            </div>
            <hr>
          </div>
          <div class="conversation-details-options conversation-shared-photos">
            <img class="action-img" src="@/assets/icons/image-grey.svg"/>
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
  .conversation-detail-avatar{
    height: 70px;
    width: 70px;
    margin: 0 auto;
  }

  .conversation-detail-avatars{
    height: 75px;
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

  .conversation-details-actions-btn{
    margin-right: 5px;
  }

  .conversation-details-options{
    font-size: 15px;
  }

  .action-img{
    width: 20px;
  }

  .conversation-details-actions-img{
    margin-right: 10px;
  }

  .action-img-right{
    float: right;
  }
</style>