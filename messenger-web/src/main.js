import Vue from 'vue'
import VueRouter from 'vue-router'
import router from './router'
import App from './App.vue'
import store from './store'
import Vuelidate from 'vuelidate'

Vue.config.productionTip = false

import Axios from 'axios'
import i18n from './i18n'

Vue.prototype.$http = Axios;
const token = localStorage.getItem('token')
if (token) {
  Vue.prototype.$http.defaults.headers.common['Authorization'] = token
}

Vue.use(Vuelidate)
Vue.use(VueRouter)

new Vue({
  router,
  store,
  i18n,
  render: h => h(App)
}).$mount('#app')
