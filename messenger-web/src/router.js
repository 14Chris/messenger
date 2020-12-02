import VueRouter from 'vue-router'
import Login from './components/Login'
import Register from './components/Register'
import Home from './components/Home'
import store from './store'
import NewMessage from "@/components/Messages/NewMessage";
import Conversation from "@/components/Conversation/Conversation";
import ConversationHome from "@/components/Conversation/ConversationHome";
import Friends from "@/components/Friends/Friends";
import FriendRequests from "@/components/Friends/FriendRequests";
import FriendsList from "@/components/Friends/FriendsList";
import Profile from "@/components/User/Profile"
import EditProfile from "@/components/User/EditProfile"
import ResetPassword from "@/components/User/ResetPassword"
import ForgotPassword from "@/components/User/ForgotPassword"
import ActivateAccount from "@/components/User/ActivateAccount"
import Settings from "@/components/Settings/Settings"

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
            },
            {
                path: 'settings',
                name: 'settings',
                component: Settings,
            },
            {
                path: 'profile/edit',
                name: 'EditProfile',
                component: EditProfile,
            },
            {
                path: 'profile/:id',
                name: 'profilePage',
                component: Profile,
            },
            

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
    {
        path: '/reset_password/:token',
        name: 'resetPassword',
        component: ResetPassword
    },
    {
        path: '/forgot_password',
        name: 'forgotPassword',
        component: ForgotPassword
    },
    {
        path: '/activate_account/:token',
        name: 'activateAccount',
        component: ActivateAccount
    }
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