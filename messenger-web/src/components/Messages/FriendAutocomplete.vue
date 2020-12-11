<template>
  <div class="autocomplete-field">
    <input autocomplete="on" @keyup="Typing" v-model="searchTerm" class="input" type="text" @focus="InputFocusIn" @blur="InputFocusOut">
    <div v-show="resultHide == false" id="autocomplete-results">
      <div v-for="result in searchResult" :key="result.id" class="search-result">
        //<div v-on:click="OptionSelected">
          //{{item.first_name}} {{item.last_name}}
        //</div>
        <AutocompleteItem :clicked="OptionSelected" :item="result"></AutocompleteItem>
      </div>
    </div>
  </div>
</template>

<script>
import AutocompleteItem from "@/components/Messages/AutocompleteItem";

export default {
  name: "autocomplete",
  data(){
    return{
      searchTerm:"",
      resultHide: true
    }
  },
  components:{
    AutocompleteItem
  },
  props:["typing", "searchResult", "isLoading", "select"],
  methods:{
    OptionSelected(option){
      this.searchTerm = ""
      this.select(option)
    },
    Typing(){
      this.typing(this.searchTerm)
    },
    InputFocusIn(){
      this.resultHide = false
    },
    InputFocusOut(){
      this.resultHide = true
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
