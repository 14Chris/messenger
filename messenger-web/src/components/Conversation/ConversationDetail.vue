<template>
  <div>
    <div v-if="loading != null">
      <div v-if="loading == true">
        Loading...
      </div>
      <div v-else>
        <div v-if="conversationDetail != null">
          <Avatar v-if="conversationDetail.friends.length <= 1" class="conversation-detail-avatar" userId="1"></Avatar>
          <AvatarGroup v-else class="conversation-detail-avatars"
                       :friendsIds="conversationDetail.friends.map(f=>f.id)"></AvatarGroup>
          <div class="conversation-detail-name">
            <div v-if="conversationDetail.friends.length <= 1">
              <router-link class="conversation-detail-name-link" :to="{ name: 'profilePage', params: { id: conversationDetail.friends[0].id }}">
                {{ conversationDetail.name }}
              </router-link>
            </div>
            <div v-else>{{ conversationDetail.name }}</div>
          </div>

          <!--  Conversation call actions -->
          <div class="conversation-details-actions">
            <button class="conversation-details-actions-btn button is-primary is-light"
                    v-if="conversationDetail.friends.length <= 1">
              <img class="action-img conversation-details-actions-img" src="@/assets/icons/phone-blue.svg"/> Call
              {{ conversationDetail.friends[0].first_name }}
            </button>

            <button class="conversation-details-actions-btn button is-primary is-light" v-else>
              <img class="action-img conversation-details-actions-img" src="@/assets/icons/phone-blue.svg"/> Call Group
            </button>

            <button class="conversation-details-actions-btn button is-primary is-light">
              <img class="action-img conversation-details-actions-img" src="@/assets/icons/video-blue.svg"/>Video Chat
            </button>
          </div>

          <!--  List of group members -->
          <div v-if="conversationDetail.friends.length > 1" class="conversation-group-friends">
            <div>
              Friends
            </div>
            <div v-for="friend in conversationDetail.friends" :key="friend.id">
              <router-link class="conversation-detail-name-link" :to="{ name: 'profilePage', params: { id: friend.id }}">
              {{friend.first_name}} {{friend.last_name}}
              </router-link>
            </div>
            <hr>
          </div>
          <div>
            <!--  Search in conversation  -->
            <div class="conversation-details-options conversation-search">
              Search in conversation
              <img class="action-img action-img-right" src="@/assets/icons/search-grey.svg"/>
            </div>
            <hr>
            <!--  Change conversation color -->
            <div class="conversation-details-options conversation-change-color">
              Change color
              <div class="conversation-change-color-selector action-img-right">

              </div>
            </div>
            <hr>
            <!--  Change conversation default smiley -->
            <div class="conversation-details-options conversation-change-emoji">
              Change smiley
            </div>
            <hr>
          </div>
          <!--  Conversation shared photos -->
          <div class="conversation-detail-photos">
            <img class="action-img action-img-left" src="@/assets/icons/image-grey.svg"/>
            Shared photos
            <div class="conversation-detail-photos-list">
<!--              No shared photos-->
            </div>
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
  props: ["conversationId"],
  components: {Avatar, AvatarGroup},
  data() {
    return {
      conversationDetail: null,
      loading: null
    }
  },
  mounted() {
    console.log(this.conversationId)
    this.GetConversationDetail(this.conversationId)
  },
  methods: {
    GetConversationDetail(id) {
      this.loading = true;
      api
          .getData("conversation/" + id + "/detail")
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
.conversation-detail-avatar {
  height: 70px;
  width: 70px;
  margin: 0 auto 20px;
}

.conversation-detail-avatars {
  height: 75px;
  margin: 0 auto 20px;
}

.conversation-detail-name {
  margin: 0 auto 25px;
  width: fit-content;
  font-size: 1.2em;

}

.conversation-detail-name-link{
  color: #1f191a;
}

.conversation-detail-name-link:hover{
  text-decoration: underline;
}

.conversation-details-actions {
  margin: 0 auto 25px;
  /*width: fit-content;*/
  display: flex;
  flex-direction: row;
}

.conversation-details-actions-btn {
  margin-right: 5px;
}

.conversation-details-options {
  font-size: 15px;
  cursor: pointer;
  padding: 2px 5px;
  border-radius: 5px;
}

.conversation-details-options:hover {
  /*background-color: lightgrey;*/
}

.action-img {
  height: 20px !important;
}

.conversation-details-actions-img {
  margin-right: 10px;
}

.action-img-right {
  float: right;
}

.action-img-left {
  float: left;
  margin-right: 15px;
}

.conversation-change-color{
  display: flex;
  position:relative;
}

.conversation-change-color-selector {
  position: absolute;
  background-color: #349cfc;
  width: 15px;
  height: 15px;
  right:5px;
  top:50%;
  bottom:50%;
  border-radius: 50%;
  border: 3px solid white;
  margin: auto 0 !important;
}

.conversation-detail-photos{
  flex:1;
  font-size: 15px;
}

.conversation-detail-photos-list{
  flex:1;
}
</style>