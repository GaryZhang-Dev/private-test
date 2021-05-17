<template></template>

<script>
import { Toast } from 'vant'
import applicationUserManager from '../applicationusermanager'

export default {
  data() {
    return {}
  },
  async created() {
    try {
      debugger
      this.$toast.loading({
        duration: 0,
        message: '授权认证中...',
        forbidClick: true,
      })
      let user = await applicationUserManager.signinRedirectCallback()
      Toast.clear()
      window.location.replace(user.state.return_uri)
    } catch (e) {
      console.log(e)
    }
  },
  methods: {},
}
</script>

<style>
</style>