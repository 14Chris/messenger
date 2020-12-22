<template>
  <div class="field is-horizontal">
    <div class="field-body">
      <!-- Plus button -->
      <button class="icon-button">
                <span class="icon">
                  <img src="@/assets/icons/plus-grey.svg"/>
                </span>
      </button>

      <!-- GIF button -->
      <button class="icon-button" @click="ChangeGifDisplay">
                <span class="icon">
                  <img src="@/assets/icons/gif-file-grey.svg"/>
                </span>
      </button>

      <!-- Sticker button -->
      <button class="icon-button" @click="ChangeStickerDisplay">
                <span class="icon">
                  <img src="@/assets/icons/sticky-note-grey.svg"/>
                </span>
      </button>

      <!-- File button -->
      <button class="icon-button">
                <span class="icon">
                  <img src="@/assets/icons/paperclip-grey.svg"/>
                </span>
      </button>

      <!-- Text input -->
      <input
          class="send-message-input"
          v-model="message"
          type="text"
          :placeholder="$t('newMessagePlaceholder')"
          v-on:keyup.enter="SendNewMessage"
      />

      <!-- Smiley button -->
      <button class="icon-button">
                <span class="icon">
                  <img src="@/assets/icons/smile-grey.svg"/>
                </span>
      </button>

      <!-- Thumbs up button -->
      <button class="icon-button">
                <span class="icon">
                  <img src="@/assets/icons/thumbs-up-grey.svg"/>
                </span>
      </button>
    </div>
    <div id="gif-menu" class="">
      <div class="gif-search">
        <input class="input" v-model="gifsSearch" @keyup="SearchGifs">
      </div>
      <div class="gif-results">
        <div v-for="(gif, index) in gifs" :key="index" @click="GifClicked(gif.id)">
          <img :src="gif.images.fixed_width.url">
        </div>
      </div>
    </div>

    <div id="stickers-menu" class="">
      <div class="stickers-search">
        <input class="input" v-model="stickersSearch" @keyup="SearchStickers">
      </div>
      <div class="stickers-results">
        <div v-for="(sticker, index) in stickers" :key="index">
          <img :src="sticker.downsized_small.fixed_width.url">
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "SendMessageBar",
  data() {
    return {
      message: "",
      gifs:[],
      stickers:[],
      gifsSearch:"",
      stickersSearch:"",
    };
  },
  props:{
    MessageSubmit:{
      required: true
    }
  },
  methods:{
   SendNewMessage() {
      if (this.message.length > 0) {
        var model = {
            text: this.message,
        };

        this.MessageSubmit(model)
        this.message = ""
      }
    },
    ChangeGifDisplay(e){
      const gifMenu = document.getElementById('gif-menu')
      var buttonCoordinates = e.currentTarget.getBoundingClientRect()

      gifMenu.style.bottom = (window.innerHeight - buttonCoordinates.top) +'px'

      this.LoadGif()

      if(gifMenu.classList.contains("show")){
        gifMenu.classList.remove('show')
      }
      else{
        gifMenu.classList.add('show')
      }
    },
    ChangeStickerDisplay(e){
      const stickersMenu = document.getElementById('stickers-menu')
      var buttonCoordinates = e.currentTarget.getBoundingClientRect()

      stickersMenu.style.bottom = (window.innerHeight - buttonCoordinates.top) +'px'

      this.LoadStickers()

      if(stickersMenu.classList.contains("show")){
        stickersMenu.classList.remove('show')
      }
      else{
        stickersMenu.classList.add('show')
      }
    },
    LoadGif(){
      this.gifs = []
      fetch("https://api.giphy.com/v1/gifs/trending?api_key="+process.env.VUE_APP_GIPHY_API_KEY+"&limit=25&rating=g", {
        method: 'GET',
      })
       .then(response=>{
         console.log(response)
        if(response.ok){
          response.json().then(data=>{
            this.gifs = data.data
          })
        }
      })
    },
    SearchGifs(){
      if(this.gifsSearch.length > 0){
        this.gifs = []
        fetch("https://api.giphy.com/v1/gifs/search?api_key="+process.env.VUE_APP_GIPHY_API_KEY+"&q="+this.gifsSearch, {
          method: 'GET',
        })
            .then(response=>{
              if(response.ok){
                response.json().then(data=>{
                  this.gifs = data.data
                })
              }
            })
      }
      else{
        //Load trending gifs
        this.LoadGif()
      }
    },
    LoadMoreGifs(){

    },
    LoadStickers(){
      this.stickers = []
      fetch("https://api.giphy.com/v1/stickers/trending?api_key="+process.env.VUE_APP_GIPHY_API_KEY, {
        method: 'GET',
      })
          .then(response=>{
            if(response.ok){
              response.json().then(data=>{
                this.stickers = data.data
              })
            }
          })
    },
    SearchStickers(){
      if(this.stickersSearch.length > 0){
        this.stickers = []
        fetch("https://api.giphy.com/v1/stickers/search?api_key="+process.env.VUE_APP_GIPHY_API_KEY+"&q="+this.gifsSearch, {
          method: 'GET',
        })
            .then(response=>{
              if(response.ok){
                response.json().then(data=>{
                  this.stickers = data.data
                })
              }
            })
      }
      else{
        //Load trending stickers
        this.LoadStickers()
      }
    },
    LoadMoreStickers(){

    },
    GifClicked(gifId){
      var model = {
        gifId: gifId,
      };

      console.log("GIF Clicked:", model)
      //this.MessageSubmit(model)
    }
  }
}
</script>

<style scoped>
.send-message-input {
  margin-right: 15px;
  flex: 1;
  border-radius: 7px;
  background-color: #f2f2f2;
  border: 0;
  height: 40px;
  text-indent: 15px;
}

.send-message-input:focus {
  outline: none;
}


#gif-menu{
  display: none;
  position:absolute;
  width: 200px;
  height: 400px;
  z-index: 3;
  background-color: whitesmoke;
  overflow-y: scroll;
}

.show{
  display: block !important;
}

.gif-results{

}

#stickers-menu{
  display: none;
  position:absolute;
  width: 200px;
  height: 400px;
  z-index: 3;
  background-color: whitesmoke;
  overflow-y: scroll;
}

</style>

<i18n>
{
  "en": {
    "newMessagePlaceholder": "Type your message"
  },
  "fr": {
    "newMessagePlaceholder": "Taper votre message"
  }
}
</i18n>