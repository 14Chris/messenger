<template>
  <div class="conv-list-item" @click="OpenConversation">
    <div v-if="convId == conversationItem.id" class="current-conv-indicator"></div>
    <div class="conv-data">
      <Avatar v-if="conversationItem.friends_ids.length == 1" class="conv-avatar"
              :userId="conversationItem.friends_ids[0]"></Avatar>
      <Avatar v-else-if="conversationItem.friends_ids.length > 1" class="conv-avatar"
              :userId="conversationItem.friends_ids[0]"></Avatar>
      <div class="conv-text">
        <h1>{{ conversationItem.name }}</h1>
        <div class="last-message-text">
          <!--  If solo conversation  -->
          <span v-if="conversationItem.last_message_sender.id == $store.state.user.id">You : </span>
          <!--  If group conversation -->
          <span
              v-else-if="conversationItem.friends_ids.length > 1">{{ conversationItem.last_message_sender.first_name }} {{
              conversationItem.last_message_sender.last_name
            }} : </span>

          <span>{{ conversationItem.last_message }}</span></div>
        <div class="last-message-date">{{ MessageDate }}</div>
      </div>
      <div class="conv-option">
        <button class="icon-button conv-option-button" @click.prevent.stop="ConversationOptionsClick">
                <span class="icon">
                  <img src="@/assets/icons/ellipsis.svg"/>
                </span>
        </button>
      </div>
    </div>

    <ul :id="'menu' + conversationItem.id" class="menu" @clickout="ConversationOptionsLostFocus">
      <li class="menu-item" @click="ArchiveConversation">Archive</li>
    </ul>
  </div>
</template>

<script>
import Avatar from "@/components/User/Avatar/Avatar";
import moment from "moment";
import ApiService from "@/service/api";

const api = new ApiService();

export default {
  name: "FriendListItem",
  props: ["conversationItem"],
  components: {
    Avatar,
  },
  data() {
    return {
      convId: -1
    }
  },
  mounted() {
    this.UpdateConvId()


  },
  watch: {
    $route() {
      this.UpdateConvId()
    }
  },
  computed: {
    MessageDate() {
      var date = this.conversationItem.last_message_date;
      return moment(date).calendar();
    },
  },
  methods: {
    OpenConversation() {
      this.$router.push("/conv/" + this.conversationItem.id);
    },
    UpdateConvId() {
      if (this.$route.name == "conversation") {
        this.convId = this.$route.params.id
      }
    },
    ConversationOptionsClick(e){
      var buttonCoordinates = e.currentTarget.getBoundingClientRect()

      const menu = document.getElementById('menu'+this.conversationItem.id)
        menu.style.top = `${buttonCoordinates.top + 25}px`
        menu.style.left = `${buttonCoordinates.left}px`

        if(menu.classList.contains("show")){
          menu.classList.remove('show')
        }
        else{
          menu.classList.add('show')
        }

    },
    ConversationOptionsLostFocus(){
      const menu = document.getElementById('menu'+this.conversationItem.id)
      menu.classList.remove('show')
    },
    ArchiveConversation(){
      api
          .update("conversation/" + this.conversationItem.id+"/archive", null)
          .then((response) => {
            if (response.ok == true) {
              response.json().then((data) => {
                console.log(data)
              });
            }
          })
          .catch((err) => {
            console.log(err);
          })
    }
  },
};
</script>

<style scoped>
.conv-list-item {
  margin-bottom: 10px;
  cursor: pointer;
  overflow-y: hidden;
  display: flex;
  flex-direction: row;
  width: 100%;
}

.current-conv-indicator {
  margin-right: 15px;
  background-color: #349cfc;
  width: 3px;
}

.last-message-text {
  font-size: 0.8em;
  color: rgb(179, 179, 179);
  word-break: break-all;
}

.last-message-date {
  font-size: 0.8em;
  color: rgb(179, 179, 179);
}

.conv-data {
  display: flex;
  flex:1;
}

.conv-text {
  flex: 1;
}

.conv-avatar {
  height: 40px !important;
  width: 40px !important;
  margin: auto 0;
  margin-right: 15px !important;
}

/*Conversation option button*/
.conv-option-button {
  display: none;
  margin: 0;
  position: absolute;
  top: 50%;
  left: 50%;
  -ms-transform: translate(-50%, -50%);
  transform: translate(-50%, -50%);
}

.conv-option {
  width: 40px;
  position: relative;
}

.conv-data:hover .conv-option-button, .conv-data:hover .conv-option-button {
  display: block;
}

/* Conversation options context menu */
.menu {
  display: none;
  background-color: white;
  padding: 10px 0px;
  border-radius: 5px;
  box-shadow: 2px 2px 30px lightgrey;
  position: absolute;
  transform-origin: center;
  z-index: 1;
  opacity: 0;
  transform: scale(0);
  transition: transform 0.2s, opacity 0.2s;
  width: 120px;
}

.menu.show {
  opacity: 1;
  transform: scale(1);
  transform-origin: top left;
  display: block !important;
}

.menu-item {
  display: block;
  padding: 10px 15px;
  transition: 0.1s;
  color: #666;
  font-size: 0.9em;
}

.menu-item:hover {
  background-color: #eee;
  cursor: pointer;
}


</style>