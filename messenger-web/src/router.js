import VueRouter from 'vue-router'
import Login from './components/Login'
import Register from './components/Register'
import Home from './components/Home'
import store from './store'
// import MessageList from "@/components/Messages/MessageList";
import NewMessage from "@/components/Messages/NewMessage";
import Conversation from "@/components/Conversation/Conversation";
import ConversationHome from "@/components/Conversation/ConversationHome";
import Friends from "@/components/Friends/Friends";
import FriendRequests from "@/components/Friends/FriendRequests";
import FriendsList from "@/components/Friends/FriendsList";

const routes = [
    {
        path: '/',
        name: 'home',
        component: Home,
        redirect: '/conv',
        meta: {
            requiresAuth: true
        },
        children: [
            {
                path: 'conv',
                name: 'conv',
                component: ConversationHome,
                children: [
                    {
                        path: 'new',
                        name: 'newMessage',
                        component: NewMessage
                    },
                    {
                        path: ':id',
                        name: 'conversation',
                        component: Conversation

                    }
                ]
            },
            {
                path: 'friends',
                name: 'friends',
                component: Friends,
                children: [
                    {
                        path: '',
                        name: 'friendsList',
                        component: FriendsList,
                    },
                    {
                        path: 'requests',
                        name: 'friendsRequests',
                        component: FriendRequests

                    }
                ]
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