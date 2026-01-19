import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import { router } from './router';
import vuetify from './plugins/vuetify';
import '@/scss/style.scss';
import { PerfectScrollbarPlugin } from 'vue3-perfect-scrollbar';
import VueApexCharts from 'vue3-apexcharts';
import VueTablerIcons from 'vue-tabler-icons';

// components
import BaseAlert from './components/shared/BaseAlert.vue';
import BaseDialog from './components/shared/BaseDialog.vue';

const app = createApp(App);
app.use(router);
app.use(PerfectScrollbarPlugin);
app.use(createPinia());
app.use(VueTablerIcons);
app.use(VueApexCharts);

// 全域註冊 components
app.component('BaseAlert', BaseAlert);
app.component('BaseDialog', BaseDialog);

app.use(vuetify).mount('#app');
