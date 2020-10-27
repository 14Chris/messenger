import VueRouter from 'vue-router'
import Login from './components/Login'
import Register from './components/Register'
import Home from './components/Home'
import store from './store'
// import MessageList from "@/components/MessagesView/MessageList";
import NewMessage from "@/components/MessagesView/NewMessage";
import Conversation from "@/components/ConversationView/Conversation";

const routes = [
    {
        path: '/',
        name: 'home',
        component: Home,
        meta: {
            requiresAuth: true
        },
        children: [
            // {
            //     path: '',
            //     name: 'message',
            //     component: MessageList
            // },
            {
                path: 'new',
                name: 'newMessage',
                component: NewMessage
            },
            {
                path: 'conv/:id',
                name: 'conversation',
                component: Conversation

            }
        ]
    },
    {
        path: '/login',
        name: 'login',
        component: Login
    },
    {
        path: '/register',
        name: 'register',
        component: Register
    },
]

const router = new VueRouter({
    routes // short for `routes: routes`
})

router.beforeEach((to, from, next) => {
    // console.log("isLoggedIn: ",store.getters.isLoggedIn)
    if (to.matched.some(record => record.meta.requiresAuth)) {
        if (store.getters.isLoggedIn) {
            next()
            return
        }
        next('/login')
    } else {
        next()
    }
})

export default router