import { createApp } from 'vue'
import App from './App.vue'
import { router } from './pages/router'
import "./styles/main.scss"
import { library } from '@fortawesome/fontawesome-svg-core'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import { faHome, faCogs } from '@fortawesome/free-solid-svg-icons'

library.add(faHome, faCogs);

createApp(App).use(router)
.component('font-awesome-icon', FontAwesomeIcon)
.mount('#app')
