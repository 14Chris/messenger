<template>
  <div class="autocomplete-field">
    <input autocomplete="on" @keyup="Typing" v-model="search" class="input" type="text" @focus="InputFocusIn">
    <div v-show="isOpen == true" id="autocomplete-results">
      <div  class="search-result">
        <ul>
          <li
              v-for="(result,i) in searchResult" :key="i"
              @click="OptionSelected(result)">
            {{result.first_name}} {{result.last_name}}
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script>

import eventBus from "@/eventBus";

export default {
  name: "autocomplete",
  data(){
    return{
      search:"",
      isOpen: false
    }
  },
  components:{
  },
  props:["typing", "searchResult", "isLoading"],
  mounted() {
    document.addEventListener('click', this.handleClickOutside)
  },
  destroyed() {
    document.removeEventListener('click', this.handleClickOutside)
  },
  methods:{
    OptionSelected(option){
      this.search = ""
      eventBus.$emit('friendselected', option)
    },
    Typing(){
      this.typing(this.search)
    },
    InputFocusIn(){
      this.isOpen = true
    },
    handleClickOutside(evt) {
      if (!this.$el.contains(evt.target)) {
        this.isOpen = false;
        this.arrowCounter = -1;
      }
    }
  }
}
</script>

<style scoped>

  .autocomplete-field{
    display: flex;
    flex-direction: row;
    position: relative;
  }

  #autocomplete-results{
    position: absolute;
    width: 100%;
    height: 200px;
    top:45px;
    z-index: 2;
    border-radius: 15px;
    display: flex;
    flex-direction: column;
    -moz-box-shadow:    3px 3px 5px 6px #ccc;
    -webkit-box-shadow: 3px 3px 5px 6px #ccc;
    box-shadow:         3px 3px 5px 6px #ccc;
    overflow: auto;
    padding: 5px;
  }

  .autocomplete-results-hidden{
    /*display: none !important;*/
  }

  .search-result {
    cursor: pointer;
    margin-top: 5px;
  }

  .search-result:hover {
    background-color: lightgrey;
  }


</style>
