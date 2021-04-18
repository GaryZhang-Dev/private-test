import { createRouter, createWebHashHistory, createWebHistory } from "vue-router"
import HelloWorld from '@/components/HelloWorld'

const routes = [
  {
    path: '/',
    name: '/',
    component: HelloWorld,
    children: [
      {
        path: "regist-user",
        name: "regist-user",
        meta: {
          title: "注册",
        },
        component: () => import("@/views/regist-user.vue")
      },
      {
        path: "login-password",
        name: "login-password",
        meta: {
          title: "登录",
        },
        component: () => import("@/views/login-password.vue")
      },
    ]
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

// router.push.prototype.constructor = function push(location, onResolve, onReject) {
//   if (onResolve || onReject) return originalPush.call(this, location, onResolve, onReject)
//   return originalPush.call(this, location).catch(err => err)
// }
// const originalPush = router.push.prototype.constructor;

export default router