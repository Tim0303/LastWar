<script setup lang="ts">
import { ref, shallowRef } from 'vue';

import BaseBreadcrumb from '@/components/shared/BaseBreadcrumb.vue';
import UiParentCard from '@/components/shared/UiParentCard.vue';

const page = ref({ title: 'Player Power List' });
const breadcrumbs = shallowRef([
  {
    title: 'Info',
    disabled: false,
    href: '#'
  },
  {
    title: 'Player Power List',
    disabled: true,
    href: '#'
  }
]);
const list = shallowRef([
  { name: 'Frozen yoghurt', calories: 159 },
  { name: 'Ice cream sandwich', calories: 237 },
  { name: 'Eclair', calories: 262 },
  { name: 'Cupcake', calories: 305 },
  { name: 'Gingerbread', calories: 356 }
]);
</script>

<template>
  <BaseBreadcrumb :title="page.title" :breadcrumbs="breadcrumbs"></BaseBreadcrumb>
  <v-row>
    <v-col cols="12" md="12">
      <UiParentCard title="List">
        <v-card variant="outlined">
          <v-data-table :items="list"></v-data-table>
        </v-card>
      </UiParentCard>
    </v-col>
  </v-row>
</template>
