import Vue from 'vue'
import VueRouter from 'vue-router'
import router from './router'
import App from './App.vue'
import store from './store'
import Vuelidate from 'vuelidate'
import Buefy from 'buefy'
// import 'buefy/dist/buefy.css'



Vue.config.productionTip = false

import Axios from 'axios'

Vue.prototype.$http = Axios;
const token = localStorage.getItem('token')
if (token) {
  Vue.prototype.$http.defaults.headers.common['Authorization'] = token
}

Vue.use(Vuelidate)
Vue.use(VueRouter)

//Ui library
Vue.use(Buefy)


new Vue({
  router,
  store,
  render: h => h(App),
}).$mount('#app')
