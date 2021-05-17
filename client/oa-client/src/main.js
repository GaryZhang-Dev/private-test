
import { createApp } from 'vue'
import App from './App.vue'
import Vant from 'vant';
import 'vant/lib/index.css';
import router from "@/router";
import mgr from "./applicationusermanager";

router.beforeEach(async (to, from, next) => {
  debugger
  let requiresAuth = to.matched.some((record) => record.meta.requiresAuth);
  if (requiresAuth) {
    let user = await mgr.getUser();
    if (user && user.access_token && !user.expired) {
      next();
    } else {
      await mgr.login();
    }
  } else {
    next();
  }
})
createApp(App).use(router).use(Vant).mount('#app')
