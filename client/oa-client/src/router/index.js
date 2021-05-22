import { createRouter, createWebHashHistory, createWebHistory } from "vue-router"
import HelloWorld from '@/components/HelloWorld.vue'

const routes = [
  {
    path: '/',
    name: '/',
    redirect: "/first-page",
    // component: HelloWorld,
  },
  {
    path: "/first-page",
    name: "first-page",
    meta: {
      title: "SEO测试",
      requiresAuth: true,
    },
    component: () => import("@/views/first-request.vue")
  },
  {
    path: "/signout",
    name: "signout",
    component: () => import("@/views/signout.vue"),
  },
  {
    path: "/signin-callback",
    name: "signin-callback",
    component: () => import("@/views/signin-callback.vue"),
  },
  {
    path: "/signout-callback",
    name: "signout-callback",
    component: () => import("@/views/signout-callback.vue"),
  },
];
routes.forEach((route) => {
  if (!route.path) {
    route.path = route.path || "/" + (route.name || "");
  }
});
const router = createRouter({
  history: createWebHistory(),
  base: process.env.BASE_URL,
  routes,
});

// router.push.prototype.constructor = function push(location, onResolve, onReject) {
//   if (onResolve || onReject) return originalPush.call(this, location, onResolve, onReject)
//   return originalPush.call(this, location).catch(err => err)
// }
// const originalPush = router.push.prototype.constructor;

export default router