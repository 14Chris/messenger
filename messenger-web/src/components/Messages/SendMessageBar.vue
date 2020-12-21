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
      <button class="icon-button">
                <span class="icon">
                  <img src="@/assets/icons/gif-file-grey.svg"/>
                </span>
      </button>

      <!-- Sticker button -->
      <button class="icon-button">
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
  </div>
</template>

<script>
export default {
  name: "SendMessageBar",
  data() {
    return {
      message: "",
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