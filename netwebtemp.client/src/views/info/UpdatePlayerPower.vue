<script setup lang="ts">
import { ref, shallowRef } from 'vue';
import { router } from '@/router';

import BaseBreadcrumb from '@/components/shared/BaseBreadcrumb.vue';
import UiParentCard from '@/components/shared/UiParentCard.vue';
import * as api from '@/api/index';

// 全域元件
import { useDialogStore } from '@/stores/dialog';
const dialog = useDialogStore();

// 頁面標題與麵包屑
const page = ref({ title: '更新戰力' });
const breadcrumbs = shallowRef([
  {
    title: '功能',
    disabled: false,
    href: '#'
  },
  {
    title: '更新戰力',
    disabled: true,
    href: '#'
  }
]);

interface PlayerData {
  playerId: string;
  name?: string;
  headquartersLv: number;
  tankPower: number;
  aircraftPower: number;
  missilePower: number;
  techSpecialForces: number;
  techSiegeToSeize: number;
  techHero: number;
  techDefenseFortifications: number;
  hasT10: boolean;
}

const data = ref<PlayerData>({
  playerId: '',
  name: '',
  headquartersLv: 0,
  tankPower: 0,
  aircraftPower: 0,
  missilePower: 0,
  techSpecialForces: 0,
  techSiegeToSeize: 0,
  techHero: 0,
  techDefenseFortifications: 0,
  hasT10: false
});

const showinfo = ref(false);
const isAdd = ref(false);

/**
 * 取得玩家詳細資料
 * @param id
 */
const getDetail = (id: string) => {
  api.APIGETDETAIL(id).then((response) => {
    if (response.status === 200) {
      data.value = response.data;
      showinfo.value = true;
      isAdd.value = false;
    } else if (response.status === 204) {
      dialog.openConfirm({
        message: `查無${id}玩家資料，是否要新建資料？`,
        onConfirm: () => {
          isAdd.value = true;
          showinfo.value = true;
        },
        onCancel: () => {
          console.log('取消');
        }
      });
    } else {
      data.value = response.data;
      showinfo.value = true;
      isAdd.value = false;
    }
  });
};

/**
 * 新建玩家資料
 * @param data
 */
const create = () => {
  api.APICREATEPLAYER(data.value).then((response) => {
    if (response.status === 200) {
      dialog.openConfirm({
        message: '完成新建玩家資料',
        onConfirm: () => {
          // 前往列表頁
          router.push('/');
        },
        showCancel: false
      });
    }
  });
};

/**
 * 更新玩家資料
 */
const update = () => {
  api.APIUPDATEPLAYER(data.value).then((response) => {
    console.log(response);
    // 前往列表頁
    router.push('/');
  });
};

const clear = () => {
  showinfo.value = false;
  data.value = {
    playerId: '',
    name: '',
    headquartersLv: 0,
    tankPower: 0,
    aircraftPower: 0,
    missilePower: 0,
    techSpecialForces: 0,
    techSiegeToSeize: 0,
    techHero: 0,
    techDefenseFortifications: 0,
    hasT10: false
  };
};

const playerIdRules = ref([(v: string) => !!v || '玩家ID為必填', (v: string) => (v && v.length <= 20) || '玩家ID長度不可超過20個字元']);

const step1Validate = () => {
  const valid = playerIdRules.value.every((rule) => rule(data.value.playerId) === true);
  if (valid) {
    getDetail(data.value.playerId);
  }
};
</script>

<template>
  <BaseBreadcrumb
    :title="page.title"
    :breadcrumbs="breadcrumbs"></BaseBreadcrumb>
  <v-row>
    <!-- 輸入玩家ID -->
    <v-col
      cols="12"
      md="12"
      v-if="!showinfo">
      <UiParentCard title="步驟一：輸入玩家ID">
        <v-row>
          <v-col
            cols="12"
            md="12"
            class="d-flex justify-center">
            <v-text-field
              label="玩家ID / PLAYER ID"
              :rules="playerIdRules"
              required
              v-model="data.playerId"></v-text-field>
          </v-col>
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
              @click="step1Validate()">
              下一步 / NEXT
            </v-btn>
          </v-col>
        </v-row>
      </UiParentCard>
    </v-col>
    <!-- 詳細資料 -->
    <v-col
      cols="12"
      md="12"
      v-else>
      <UiParentCard title="步驟二：建立更新戰力資料">
        <!-- 總部 -->
        <v-row>
          <v-col
            cols="12"
            md="3">
            <v-text-field
              label="玩家ID"
              type="number"
              :rules="[(v) => !isNaN(Number(v)) || '只能輸入數字']"
              v-model.number="data.playerId"
              required
              disabled></v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="3">
            <v-text-field
              label="玩家名稱 / Name"
              v-model="data.name"
              required
              :disabled="!isAdd"></v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="3">
            <v-number-input
              label="總部等級 / Headquarters Level"
              required
              inputmode="numeric"
              :precision="0"
              control-variant="hidden"
              :max="35"
              :min="1"
              v-model="data.headquartersLv"></v-number-input>
          </v-col>
          <v-col
            cols="12"
            md="3">
            <v-checkbox
              label="T10 軍隊 / T10 Army"
              v-model="data.hasT10"></v-checkbox>
          </v-col>
        </v-row>
        <!-- 三隊戰力 -->
        <v-row>
          <v-col
            cols="12"
            md="4">
            <v-number-input
              label="坦克戰力 / Tank Power"
              required
              :precision="2"
              control-variant="hidden"
              :max="200"
              :min="0"
              v-model="data.tankPower"></v-number-input>
          </v-col>
          <v-col
            cols="12"
            md="4">
            <v-number-input
              label="飛機戰力 / Aircraft Power"
              required
              :precision="2"
              control-variant="hidden"
              :max="200"
              :min="0"
              v-model="data.aircraftPower"></v-number-input>
          </v-col>
          <v-col
            cols="12"
            md="4">
            <v-number-input
              label="導彈戰力 / Missile Power"
              required
              :precision="2"
              control-variant="hidden"
              :max="200"
              :min="0"
              v-model="data.missilePower"></v-number-input>
          </v-col>
        </v-row>
        <!-- 科技 -->
        <v-row>
          <v-col
            cols="12"
            md="3">
            <v-number-input
              label="特種部隊 / Special Forces"
              required
              inputmode="numeric"
              :precision="0"
              control-variant="hidden"
              :max="100"
              :min="0"
              v-model="data.techSpecialForces"></v-number-input>
          </v-col>
          <v-col
            cols="12"
            md="3">
            <v-number-input
              label="攻城拔寨 / Siege to Seize"
              required
              inputmode="numeric"
              :precision="0"
              control-variant="hidden"
              :max="100"
              :min="0"
              v-model="data.techSiegeToSeize"></v-number-input>
          </v-col>
          <v-col
            cols="12"
            md="3">
            <v-number-input
              label="英雄科技 / Hero"
              required
              inputmode="numeric"
              :precision="0"
              control-variant="hidden"
              :max="100"
              :min="0"
              v-model="data.techHero"></v-number-input>
          </v-col>
          <v-col
            cols="12"
            md="3">
            <v-number-input
              label="防禦工作 / Defense Fortifications"
              required
              inputmode="numeric"
              :precision="0"
              control-variant="hidden"
              :max="100"
              :min="0"
              v-model="data.techDefenseFortifications"></v-number-input>
          </v-col>
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
              @click="isAdd ? create() : update()">
              提交 / SUBMIT
            </v-btn>
            <v-btn
              class="text-secondary mx-6"
              color="lightsecondary"
              rounded="sm"
              variant="flat"
              @click="clear()">
              清除 / CLEAR
            </v-btn>
          </v-col>
        </v-row>
      </UiParentCard>
    </v-col>
  </v-row>
</template>
