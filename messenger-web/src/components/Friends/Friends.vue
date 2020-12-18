<template>
  <div class="container">
    <div class="tabs is-centered is-boxed is-medium">
      <ul>
        <router-link to="/friends">
          <li v-bind:class="[$route.fullPath == '/friends' ? 'is-active' : '']"><a>Friends</a></li>
        </router-link>
        <router-link to="/friends/requests">
          <li v-bind:class="[$route.fullPath == '/friends/requests' ? 'is-active' : '']"><a>Requests</a></li>
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