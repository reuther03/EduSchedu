import './assets/styles/index.css';


import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { PiniaVuePlugin } from 'pinia'

const app = createApp(App)

app.use(router)
app.use(PiniaVuePlugin)

app.mount('#app')
