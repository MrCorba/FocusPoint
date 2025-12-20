import { createMemoryHistory, createRouter } from 'vue-router'

import HomeView from './HomeView.vue'
import SettingsView from './SettingsView.vue'

const routes = [
  { path: '/', component: HomeView, meta: { title: 'Home' }  },
  { path: '/settings', component: SettingsView , meta: { title: 'Settings' } },
]

const router = createRouter({
  history: createMemoryHistory(),
  routes,
});

router.afterEach((to) => {
  const APP_TITLE = 'FocusPoint'
  const nearestWithTitle = to.matched.slice().reverse().find(r => r.meta && r.meta.title)
  const title = nearestWithTitle ? `${nearestWithTitle.meta.title} | ` : ''
  document.title = `${title}${APP_TITLE}`
})

export { router };
