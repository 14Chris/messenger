<template>
  <div class="container">
    <div class="tabs is-centered is-boxed is-medium">
      <ul>
        <router-link to="/friends">
          <li v-bind:class="[$route.fullPath == '/friends' ? 'is-active' : '']"><a>{{$t('friendsListTitle')}}</a></li>
        </router-link>
        <router-link to="/friends/requests">
          <li v-bind:class="[$route.fullPath == '/friends/requests' ? 'is-active' : '']"><a>{{$t('friendsRequestsTitle')}}</a></li>
        </router-link>
      </ul>

    </div>
    <router-view></router-view>
  </div>
</template>

<script>
export default {
  name: "Friends"
}
</script>

<style scoped>

</style>

<i18n>
{
  "en": {
    "friendsListTitle": "Friends"
  },
  "fr": {
    "friendsRequestTitle": "Requests"
  }
}
</i18n>