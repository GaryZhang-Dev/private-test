<template>
  <div>
    <van-form @submit="regist">
      <van-cell-group>
        <van-cell>
          <van-field label="账号" v-model="command.username" />
        </van-cell>
        <van-cell>
          <van-field label="密码" type="password" v-model="command.password" />
        </van-cell>
      </van-cell-group>
      <van-button type="primary" size="large" native-type="submit">注册</van-button>
    </van-form>
  </div>
</template>
<script>
import { RegisterUser, PhoneNumberLogin2 } from '@/api/authService'
export default {
  data() {
    return {
      command: {},
    }
  },
  methods: {
    async regist() {
      try {
        var response = await RegisterUser(this.command)
        this.$toast('注册成功')
        if (response.ReturnUrl) {
          window.location.replace(response.ReturnUrl)
        }
      } catch (error) {}
    },
  },
}
</script>
