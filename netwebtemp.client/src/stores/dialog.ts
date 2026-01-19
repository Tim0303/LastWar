import { defineStore } from 'pinia';

type ConfirmCallback = {
  onConfirm?: () => void;
  onCancel?: () => void;
};

interface DialogState {
  show: boolean;
  title?: string;
  message?: string;
  callback: ConfirmCallback | null;
  showCancel?: boolean;
}

export const useDialogStore = defineStore('dialog', {
  state: (): DialogState => ({
    show: false,
    title: 'Message',
    message: '',
    callback: null
  }),
  actions: {
    openConfirm({
      title,
      message,
      onConfirm,
      onCancel,
      showCancel = true
    }: {
      title?: string;
      message?: string;
      onConfirm?: () => void;
      onCancel?: () => void;
      showCancel?: boolean;
    }) {
      this.title = title;
      this.message = message;
      this.callback = { onConfirm, onCancel };
      this.showCancel = showCancel;
      this.show = true;
    },
    close() {
      this.show = false;
    }
  }
});
