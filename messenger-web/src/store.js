import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'
// import ApiService from './service/api'
import CreateWebSocket from './service/websocket'

// var api = new ApiService

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        status: '',
        token: localStorage.getItem('token'),
        user: {},
        chatWebsocket: null,
        conversations: []
    },
    mutations: {
        auth_request(state) {
            state.status = 'loading'
        },
        auth_success(state, { token, user }) {
            state.status = 'success'
            state.token = token
            state.user = user

        },
        auth_error(state) {
            state.status = 'error'
        },
        logout(state) {
            state.status = ''
            state.token = ''
            state.chatWebsocket = null
        },
        set_user(state, user) {
            state.user = user
        },
        create_websocket(state) {
            var websocket = CreateWebSocket(state.token)
            //Creation of chat websocket object
            state.chatWebsocket = websocket
        },
        add_conversation(state, conversations) {
            state.conversations = conversations
        }
    },
    actions: {
        login({ commit }, { token, user }) {
            return new Promise((resolve,) => {
                commit('auth_request')
                localStorage.setItem('token', token)
                axios.defaults.headers.common['Authorization'] = token
                commit('auth_success', { token, user })
                commit('create_websocket')
                resolve()

            })
        },
        logout({ commit }) {
            return new Promise((resolve) => {
                commit('logout')
                localStorage.removeItem('token')
                delete axios.defaults.headers.common['Authorization']
                resolve()
            })
        },
        setUserSession({ commit }, user) {
            return new Promise((resolve) => {
                commit('set_user', user)
                commit('create_websocket')
                resolve()
            })
        },
    },
    getters: {
        isLoggedIn(state) {
            return state.token != null && state.token != undefined
        },
        authStatus: state => state.status,
        connectedUser: state => state.user,
        chatWebsocket: state => state.chatWebsocket
    }
})

