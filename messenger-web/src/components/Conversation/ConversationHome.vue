<template>
  <div class="columns conv-home is-mobile">
    <div class="column column-home is-one-fifth">
      <ConversationsList></ConversationsList>
    </div>
    <div class="column is-narrow column-no-top-marging">
      <hr class="conv-part-separator"/>
    </div>
    <div class="column column-home column-no-top-marging">
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
import ConversationsList from "@/components/Conversation/ConversationsList";

export default {
  name: "ConversationHome",
  components: {
    ConversationsList
  },
}
</script>

<style scoped>
.columns{
  margin:0 !important;
}

.conv-home{
  height:100%;
}

.columns:last-child{
  margin-bottom: 0 !important;
}
</style>