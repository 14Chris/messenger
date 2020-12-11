<template>
  <div @click="ItemClicked">
    {{item.first_name}} {{item.last_name}}
  </div>
</template>

<script>
export default {
  name: "AutocompleteItem",
  props:['clicked', 'item'],
  methods:{
    ItemClicked(){
      this.clicked(this.item)
    }
  }
}
</script>

<style scoped>

</style>