<script setup lang="ts">
import { ref, shallowRef } from 'vue';
import { formatDateTime } from '@/utils/formatDate';

import BaseBreadcrumb from '@/components/shared/BaseBreadcrumb.vue';
import UiParentCard from '@/components/shared/UiParentCard.vue';
import * as api from '@/api/index';

const page = ref({ title: '戰力清單' });

const breadcrumbs = shallowRef([
  {
    title: '功能',
    disabled: false,
    href: '#'
  },
  {
    title: '戰力清單',
    disabled: true,
    href: '#'
  }
]);

interface ListData {
  name: string;
  headquartersLv: number;
  tankPower: number;
  aircraftPower: number;
  missilePower: number;
  techSpecialForces: number;
  techSiegeToSeize: number;
  techHero: number;
  techDefenseFortifications: number;
  hasT10: boolean;
  updateTime: Date;
}

interface Param {
  name?: string | null;
}

const loading = ref<boolean>(false);

const params = ref<Param>({
  name: ''
});

const list = shallowRef<ListData[]>([]);

const headers = <object[]>[
  {
    title: '玩家名稱',
    align: 'start',
    sortable: false,
    key: 'name'
  },
  { title: '總部等級', key: 'headquartersLv' },
  { title: '坦克戰力', key: 'tankPower' },
  { title: '飛機戰力', key: 'aircraftPower' },
  { title: '導彈戰力', key: 'missilePower' },
  { title: 'T10 軍隊', key: 'hasT10' },
  { title: '特種部隊', key: 'techSpecialForces' },
  { title: '攻城拔寨', key: 'techSiegeToSeize' },
  { title: '英雄科技', key: 'techHero' },
  { title: '防禦工作', key: 'techDefenseFortifications' },
  { title: '更新時間', key: 'updateTime' }
];

/**
 * 取得玩家戰力清單
 */
const getList = () => {
  loading.value = true;
  api.APIGETPLAYERLIST(params.value).then((response) => {
    list.value = response.data;
  });
  loading.value = false;
};

/**
 * 清除搜尋條件
 */
const clear = () => {
  params.value.name = '';
  getList();
};

getList();
</script>

<template>
  <BaseBreadcrumb
    :title="page.title"
    :breadcrumbs="breadcrumbs"></BaseBreadcrumb>
  <v-row>
    <v-col
      cols="12"
      md="12">
      <UiParentCard title="Search">
        <v-row>
          <v-col
            cols="12"
            md="6">
            <v-text-field
              label="玩家名稱"
              v-model="params.name"></v-text-field>
          </v-col>
          <!-- <v-col
            cols="12"
            md="6">
            <v-text-field label="Power From"></v-text-field>
          </v-col> -->
        </v-row>
        <v-row>
          <v-col
            cols="12"
            md="12"
            class="d-flex justify-center">
            <v-btn
              class="text-secondary mx-6"
              color="lightsecondary"
              rounded="sm"
              variant="flat"
              @click="getList()">
              搜尋 / Search
            </v-btn>
            <v-btn
              class="text-secondary mx-6"
              color="lightsecondary"
              rounded="sm"
              variant="flat"
              @click="clear()">
              清除 / Clear
            </v-btn></v-col
          >
        </v-row>
      </UiParentCard>
    </v-col>
  </v-row>
  <v-row>
    <v-col
      cols="12"
      md="12">
      <UiParentCard title="Result">
        <v-card variant="outlined">
          <v-data-table
            :items="list"
            :headers="headers"
            loading-text="Loading... Please wait"
            :loading="loading">
            <template v-slot:item.tankPower="{ item }">
              <!-- 超過40紅字 -->
              <span :style="item.tankPower > 40 ? 'color: red;' : ''">{{ item.tankPower }}</span>
            </template>
            <template v-slot:item.aircraftPower="{ item }">
              <span :style="item.aircraftPower > 40 ? 'color: red;' : ''">{{ item.aircraftPower }}</span>
            </template>
            <template v-slot:item.missilePower="{ item }">
              <span :style="item.missilePower > 40 ? 'color: red;' : ''">{{ item.missilePower }}</span>
            </template>
            <template v-slot:item.hasT10="{ item }">
              <span v-if="item.hasT10">Y</span>
              <span v-else>N</span>
            </template>
            <template v-slot:item.updateTime="{ item }">
              <span>{{ formatDateTime(item.updateTime) }}</span>
            </template>
          </v-data-table>
        </v-card>
      </UiParentCard>
    </v-col>
  </v-row>
</template>
