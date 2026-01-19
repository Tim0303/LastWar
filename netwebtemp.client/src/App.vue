<template>
  <RouterView></RouterView>

  <ConfirmDialog
    v-model="dialog.show"
    :title="dialog.title"
    :message="dialog.message"
    @confirm="dialog.callback?.onConfirm?.()"
    @cancel="dialog.callback?.onCancel?.()" />
</template>

<script setup lang="ts">
import { RouterView } from 'vue-router';
import ConfirmDialog from '@/components/shared/ConfirmDialog.vue';
import { useDialogStore } from '@/stores/dialog';
const dialog = useDialogStore();
</script>
