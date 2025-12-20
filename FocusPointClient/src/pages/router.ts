import { createMemoryHistory, createRouter } from 'vue-router'

import HomeView from './HomeView.vue'
import SettingsView from './SettingsView.vue'

const routes = [
  { path: '/', component: HomeView },
  { path: '/settings', component: SettingsView },
]

export const router = createRouter({
  history: createMemoryHistory(),
  routes,
})