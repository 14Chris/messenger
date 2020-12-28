<template>
  <div class="message-item">
    <div
        class="message-container"
        v-bind:class="[
        $store.state.user.id != message.sender_id
          ? 'position-left'
          : 'position-right',
      ]"
    >
      <template v-if="lastUserMessage && $store.state.user.id != message.sender_id">
        <Avatar class="message-avatar message-avatar-left" :userId="message.sender_id"></Avatar>
      </template>

      <!--If message is text-->
      <div v-if="message.text"
           v-bind:class="[$store.state.user.id != message.sender_id ? 'message-box-left': 'message-box-right']">
        <div class="message-text" v-bind:class="[$store.state.user.id != message.sender_id ? 'other' : 'sender',]">
          {{ message.text }}
        </div>
        <div class="message-date" v-if="lastUserMessage">
          {{ MessageDate }}
        </div>
      </div>

      <!--  If message is GIF    -->
      <div class="gif-box" v-if="message.gif_id"
           v-bind:class="[$store.state.user.id != message.sender_id ? 'message-box-left': 'message-box-right']">
        <div class="loading-box" v-if="gif == null" >
          <Loading class="loading-spinner"></Loading>
        </div>
        <div v-else>
          <img :src="gif.images.original.webp">
        </div>
      </div>

      <!--  If message is sticker    -->
      <div class="sticker-box" v-if="message.sticker_id"
           v-bind:class="[$store.state.user.id != message.sender_id ? 'message-box-left': 'message-box-right']">
        <div class="loading-box" v-if="sticker == null">
          <Loading class="loading-spinner"></Loading>
        </div>
        <div  v-else>
          <img :src="sticker.images.original.webp">
        </div>

      </div>
      <template v-if="lastUserMessage && $store.state.user.id == message.sender_id">
        <Avatar class="message-avatar message-avatar-right" :userId="message.sender_id"></Avatar>
      </template>
    </div>

  </div>
</template>

<script>
import Avatar from "@/components/User/Avatar/Avatar";
import moment from 'moment'
import Loading from "@/shared_components/Loading/Loading";

export default {
  name: "MessageItem",
  props: ["message", "lastUserMessage"],
  components: {
    Avatar,
    Loading
  },
  data() {
    return {
      gif: null,
      sticker: null
    }
  },
  mounted() {
    if(this.message.gif_id){
      this.GetGif()
    }
    else if(this.message.sticker_id){
      this.GetSticker()
    }
  },
  computed: {
    MessageDate() {
      var date = this.message.date
      return moment(date).locale(this.$i18n.locale).calendar()
    },
  },
  methods: {
    GetGif() {
      this.gifs = []
      fetch("https://api.giphy.com/v1/gifs/" + this.message.gif_id + "?api_key=" + process.env.VUE_APP_GIPHY_API_KEY, {
        method: 'GET',
      }).then(response => {
            if (response.ok) {
              response.json().then(data => {
                this.gif = data.data
              })
            }
          })
    },
    GetSticker() {
      this.gifs = []
      fetch("https://api.giphy.com/v1/gifs/" + this.message.sticker_id + "?api_key=" + process.env.VUE_APP_GIPHY_API_KEY, {
        method: 'GET',
      }).then(response => {
            if (response.ok) {
              response.json().then(data => {
                this.sticker = data.data
              })
            }
          })
    },
  }
};
</script>

<style scoped>
.message-item {
  margin-bottom: 7px;
  width: 100%;
}

.message-container {
  width: fit-content;
  max-width: 80%;
  display: flex;
  flex-direction: row;
  position: relative;
}

.message-text {
  padding: 20px;
  border-radius: 30px;
  margin-bottom: 0;
  font-family: Helvetica, Arial, sans-serif;
  word-wrap: anywhere !important;
}

.position-left {
  float: left;
}

.position-right {
  float: right;
}

.other {
  background-color: lightgrey;
  border-bottom-left-radius: 0px;
}

.sender {
  background-color: #349cfc;
  border-bottom-right-radius: 0px;
  color: white;
}

.message-date {
  font-size: 0.8em;
  color: lightgrey;
  width: fit-content;
  float: right;
}

.message-avatar-right {
  height: 50px;
  right: 5px;
}

.message-avatar-left {
  height: 50px;
  left: 5px;
}

.message-avatar {
  margin: auto 0;
  position: absolute;
  bottom: 0;
  width: 40px !important;
  height: 40px !important;

}

.message-box-left {
  padding-left: 70px;
}

.message-box-right {
  margin-right: 70px;
}

.loading-box{
  height: 300px;
  width: 300px;
}

.loading-spinner{
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 50%;
}

</style>