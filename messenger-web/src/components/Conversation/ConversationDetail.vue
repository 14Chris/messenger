<template>
  <div>
    <div v-if="loading != null">
      <div v-if="loading == true">
        {{$t('loadingText')}}
      </div>
      <div v-else>
        <div v-if="conversationDetail != null">
          <Avatar v-if="conversationDetail.friends.length <= 1" class="conversation-detail-avatar" :userId="conversationDetail.friends[0].id"></Avatar>
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
              <img class="action-img conversation-details-actions-img" src="@/assets/icons/phone-blue.svg"/> {{$t('callFriendButton')}}
              {{ conversationDetail.friends[0].first_name }}
            </button>

            <button class="conversation-details-actions-btn button is-primary is-light" v-else>
              <img class="action-img conversation-details-actions-img" src="@/assets/icons/phone-blue.svg"/> {{$t('callGroupButton')}}
            </button>

            <button class="conversation-details-actions-btn button is-primary is-light">
              <img class="action-img conversation-details-actions-img" src="@/assets/icons/video-blue.svg"/>{{$t('videoChatButton')}}
            </button>
          </div>

          <!--  List of group members -->
          <div v-if="conversationDetail.friends.length > 1" class="conversation-group-friends">
            <div>
              {{$t('friendsTitle')}}
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
              {{$t('searchConvTitle')}}
              <img class="action-img action-img-right" src="@/assets/icons/search-grey.svg"/>
            </div>
            <hr>
            <!--  Change conversation color -->
            <div class="conversation-details-options conversation-change-color">
              {{$t('changeColorTitle')}}
              <div class="conversation-change-color-selector action-img-right">

              </div>
            </div>
            <hr>
            <!--  Change conversation default smiley -->
            <div class="conversation-details-options conversation-change-emoji">
              {{$t('changeSmileyTitle')}}
            </div>
            <hr>
          </div>
          <!--  Conversation shared photos -->
          <div class="conversation-detail-photos">
            <img class="action-img action-img-left" src="@/assets/icons/image-grey.svg"/>
            {{$t('sharedPhotosTitle')}}
            <div class="conversation-detail-photos-list">
              <!--  No shared photos-->
            </div>
          </div>
        </div>
        <div v-else>
          {{$t('getConversationDetailError')}}
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

                  this.conversationDetail = data.Result;

              });
            }
            else {
              this.conversationDetail = null;
            }
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
  background-color: #38B09D;
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

<i18n>
{
  "en": {
    "loadingText": "Loading...",
    "getConversationDetailError": "Error while loading conversation details",
    "callGroupButton": "Call group",
    "callFriendButton": "Call ",
    "videoChatButton": "Video chat",
    "friendsTitle": "Friends",
    "searchConvTitle": "Search in conversation",
    "changeColorTitle": "Change conversation color",
    "changeSmileyTitle": "Change default smiley",
    "sharedPhotosTitle": "Shared photos"
  },
  "fr": {
    "loadingText": "Chargement...",
    "getConversationDetailError": "Une erreur est survenue lors du chargement des détails de la conversation",
    "callGroupButton": "Appeler le groupe",
    "callFriendButton": "Appeler ",
    "videoChatButton": "Appel vidéo",
    "friendsTitle": "Amis",
    "searchConvTitle": "Rechercher dans la conversation",
    "changeColorTitle": "Changer la couleur de la conversation",
    "changeSmileyTitle": "Changer le smiley par défaut de la conversation",
    "sharedPhotosTitle": "Photos partagées"
  }
}
</i18n>