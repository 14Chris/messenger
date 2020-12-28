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
    <div id="gif-menu" ref="gifsMenu">
      <div class="gif-search">
        <input class="input" v-model="gifsSearch" @keyup="SearchGifs">
      </div>
      <div class="gif-results">
        <div v-for="(gif, index) in gifs" :key="index" @click="GifClicked(gif.id)">
          <img :src="gif.images.fixed_width.url">
        </div>
      </div>
    </div>

    <div id="stickers-menu" ref="stickersMenu">
      <div class="stickers-search">
        <input class="input" v-model="stickersSearch" @keyup="SearchStickers">
      </div>
      <div class="stickers-results">
        <div v-for="(sticker, index) in stickers" :key="index" @click="StickerClicked(sticker.id)">
          <img :src="sticker.images.fixed_width.url">
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
      gifs: [],
      stickers: [],
      gifsSearch: "",
      stickersSearch: "",
    };
  },
  props: {
    MessageSubmit: {
      required: true
    }
  },
  created() {
    this.$nextTick(function () {
      document.getElementById("gif-menu").addEventListener('scroll', this.HandleGifScroll);
      document.getElementById("stickers-menu").addEventListener('scroll', this.HandleStickersScroll);
    })
  },
  methods: {
    SendNewMessage() {
      if (this.message.length > 0) {
        var model = {
          text: this.message,
        };

        this.MessageSubmit(model)
        this.message = ""
      }
    },
    ChangeGifDisplay(e) {
      const gifMenu = document.getElementById('gif-menu')
      var buttonCoordinates = e.currentTarget.getBoundingClientRect()

      gifMenu.style.bottom = (window.innerHeight - buttonCoordinates.top) + 'px'

      this.LoadGif()

      if (gifMenu.classList.contains("show")) {
        gifMenu.classList.remove('show')
      } else {
        gifMenu.classList.add('show')
      }
    },
    ChangeStickerDisplay(e) {
      const stickersMenu = document.getElementById('stickers-menu')
      var buttonCoordinates = e.currentTarget.getBoundingClientRect()

      stickersMenu.style.bottom = (window.innerHeight - buttonCoordinates.top) + 'px'

      this.LoadStickers()

      if (stickersMenu.classList.contains("show")) {
        stickersMenu.classList.remove('show')
      } else {
        stickersMenu.classList.add('show')
      }
    },
    LoadGif() {
      this.gifs = []
      fetch("https://api.giphy.com/v1/gifs/trending?limit=15&rating=g&api_key=" + process.env.VUE_APP_GIPHY_API_KEY, {
        method: 'GET',
      })
          .then(response => {
            console.log(response)
            if (response.ok) {
              response.json().then(data => {
                this.gifs = data.data
              })
            }
          })
    },
    async SearchGifs() {
      this.gifs = []
      if (this.gifsSearch.length > 0) {

        if (this.searchGifAvailabilityTimer) {
          clearTimeout(this.searchGifAvailabilityTimer);
          this.searchGifAvailabilityTimer = null;
        }

        this.searchGifAvailabilityTimer = setTimeout(() => {
          fetch("https://api.giphy.com/v1/gifs/search?api_key=" + process.env.VUE_APP_GIPHY_API_KEY + "&q=" + this.gifsSearch, {
            method: 'GET',
          })
              .then(response => {
                if (response.ok) {
                  response.json().then(data => {
                    this.gifs = data.data
                  })
                }
              })
        }, 500);

      } else {
        //Load trending gifs
        this.LoadGif()
      }
    },
    LoadMoreGifs() {
      //if is search
      if (this.gifsSearch.length > 0) {
        fetch("https://api.giphy.com/v1/gifs/search?api_key=" + process.env.VUE_APP_GIPHY_API_KEY + "&q=" + this.gifsSearch + "&offset=" + (this.gifs.length - 1), {
          method: 'GET',
        })
            .then(response => {
              if (response.ok) {
                response.json().then(data => {
                  this.gifs = this.gifs.concat(data.data)
                })
              }
            })
      } else {
        fetch("https://api.giphy.com/v1/gifs/trending?limit=15&rating=g&api_key=" + process.env.VUE_APP_GIPHY_API_KEY + "&offset=" + (this.gifs.length - 1), {
          method: 'GET',
        })
            .then(response => {
              if (response.ok) {
                response.json().then(data => {
                  this.gifs = this.gifs.concat(data.data)
                })
              }
            })
      }
    },
    LoadStickers() {
      this.stickers = []
      fetch("https://api.giphy.com/v1/stickers/trending?limit=15&api_key=" + process.env.VUE_APP_GIPHY_API_KEY, {
        method: 'GET',
      })
          .then(response => {
            if (response.ok) {
              response.json().then(data => {
                this.stickers = data.data
              })
            }
          })
    },
    async SearchStickers() {
      this.stickers = []
      if (this.stickersSearch.length > 0) {

        if (this.searchStickerAvailabilityTimer) {
          clearTimeout(this.searchStickerAvailabilityTimer);
          this.searchStickerAvailabilityTimer = null;
        }

        this.searchStickerAvailabilityTimer = setTimeout(() => {
          fetch("https://api.giphy.com/v1/stickers/search?api_key=" + process.env.VUE_APP_GIPHY_API_KEY + "&q=" + this.stickersSearch, {
            method: 'GET',
          })
              .then(response => {
                if (response.ok) {
                  response.json().then(data => {
                    this.stickers = data.data
                  })
                }
              })
        }, 500);


      } else {
        //Load trending stickers
        this.LoadStickers()
      }
    },
    LoadMoreStickers() {

      //if is search
      if (this.stickersSearch.length > 0) {
        fetch("https://api.giphy.com/v1/stickers/search?api_key=" + process.env.VUE_APP_GIPHY_API_KEY + "&q=" + this.stickersSearch + "&offset=" + (this.stickersSearch.length - 1), {
          method: 'GET',
        })
            .then(response => {
              if (response.ok) {
                response.json().then(data => {
                  this.stickers = this.stickers.concat(data.data)
                })
              }
            })
      } else {
        fetch("https://api.giphy.com/v1/stickers/trending?limit=15&api_key=" + process.env.VUE_APP_GIPHY_API_KEY + "&offset=" + (this.stickersSearch.length - 1), {
          method: 'GET',
        })
            .then(response => {
              if (response.ok) {
                response.json().then(data => {
                  this.stickers = this.stickers.concat(data.data)
                })
              }
            })
      }
    },
    GifClicked(gifId) {
      var model = {
        gifId: gifId,
      };

      this.MessageSubmit(model)
    },
  StickerClicked(stickerId) {
    var model = {
      stickerId: stickerId,
    };

    this.MessageSubmit(model)
  },
  HandleGifScroll() {
    let gifsMenu = this.$refs.gifsMenu

    //When scroll down ended
    if (gifsMenu.scrollHeight - gifsMenu.scrollTop == gifsMenu.clientHeight) {
      this.LoadMoreGifs()
    }
  },
  HandleStickersScroll() {
    let stickersMenu = this.$refs.stickersMenu
    //When scroll down ended
    if (stickersMenu.scrollHeight - stickersMenu.scrollTop == stickersMenu.clientHeight) {
      this.LoadMoreStickers()
    }
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


#gif-menu {
  display: none;
  position: absolute;
  width: 200px;
  height: 400px;
  z-index: 3;
  background-color: whitesmoke;
  overflow-y: scroll;
}

.show {
  display: block !important;
}

.gif-results {

}

#stickers-menu {
  display: none;
  position: absolute;
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