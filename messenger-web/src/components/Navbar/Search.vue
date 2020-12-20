<template>
  <div class="field">
    <p class="control has-icons-left">
      <input class="input is-expanded" type="text" :placeholder="$t('searchPlaceholder')">
      <span class="icon is-small is-left">
      <i class="fas fa-search"></i>
    </span>
    </p>
  </div>
</template>

<script>
export default {

}
</script>

<style>
  .field{
    width: 100%;
  }
</style>

<i18n>
{
  "en": {
    "searchPlaceholder": "Search"
  },
  "fr": {
    "searchPlaceholder": "Rechercher"
  }
}
</i18n>