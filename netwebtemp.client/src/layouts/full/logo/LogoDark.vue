<template>
  <div class="logo">
    <RouterLink to="/">
      <v-btn
        class="text-secondary mx-12"
        color="lightsecondary"
        rounded="sm"
        variant="flat">
        <CatIcon size="25"></CatIcon> CAT 9
      </v-btn>
      <div class="text-h1"></div>
    </RouterLink>
  </div>
</template>
<script setup>
import { RouterLink } from 'vue-router';
import { CatIcon } from 'vue-tabler-icons';
</script>
