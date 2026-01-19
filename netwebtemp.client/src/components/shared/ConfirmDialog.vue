<template>
  <v-dialog
    v-model="dialogVisible"
    max-width="400">
    <v-card>
      <v-card-title>{{ title }}</v-card-title>
      <v-card-text>{{ message }}</v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn
          color="primary"
          @click="onConfirm"
          >確認</v-btn
        >
        <v-btn
          v-if="showCancel"
          color="secondary"
          @click="onCancel"
          >取消</v-btn
        >
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';

const props = defineProps<{
  modelValue: boolean;
  title?: string;
  message?: string;
  showCancel?: boolean;
}>();

const emit = defineEmits(['update:modelValue', 'confirm', 'cancel']);

const dialogVisible = ref(props.modelValue);
const showCancel = ref(props.showCancel ?? true);

watch(
  () => props.modelValue,
  (newVal) => {
    dialogVisible.value = newVal;
  }
);

watch(dialogVisible, (newVal) => {
  if (newVal !== props.modelValue) {
    emit('update:modelValue', newVal);
  }
});

function onConfirm() {
  emit('confirm');
  dialogVisible.value = false;
}

function onCancel() {
  emit('cancel');
  dialogVisible.value = false;
}
</script>
