<template>
  <div class="container">
    <h3 class="title is-3">{{$t('title')}}</h3>
    <div class="box">
      <h4 class="title is-4">
        {{$t('languageTitle')}}
        <div>
          <div class="field">
            <label class="label">{{ $t('changeLanguageLabel') }}</label>
            <div class="control">
          <div class="select">
            <select v-model="newLanguage">
              <option v-for="(lang, i) in langs" :key="`Lang${i}`" :value="lang">{{ lang }}</option>
            </select>
          </div>
          </div>
          </div>
          <button class="button is-primary" @click="ChangeLanguage">{{$t('changeLanguageButton')}}</button>
        </div>
      </h4>
    </div>
  </div>
</template>

<script>
export default {
  data () {
    return {
      langs: ['en', 'fr'],
      newLanguage:''
    }
  },
  mounted() {
    this.newLanguage = this.$store.getters.selectedLanguage
    console.log(this.$store.getters.selectedLanguage)
  },
  methods:{
    ChangeLanguage(){
      if(this.newLanguage != this.$store.getters.selectedLanguage){
        this.$store.dispatch("SetApplicationLanguage", this.newLanguage)
            .then(()=>{
              this.$i18n.locale = this.$store.getters.selectedLanguage
            })
      }
    }
  }
}
</script>

<style>

</style>

<i18n>
{
  "en": {
    "title": "Settings",
    "languageTitle": "Language",
    "changeLanguageButton": "Change",
    "changeLanguageLabel": "Choose a language"
  },
  "fr": {
    "title": "Paramètres",
    "languageTitle": "Langue",
    "changeLanguageButton": "Changer",
    "changeLanguageLabel": "Choisissez une langue"
  }
}
</i18n>