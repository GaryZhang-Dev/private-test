<template>
  <div>
    <van-form @submit="Login">
      <van-cell-group>
        <van-cell>
          <van-field label="账号" v-model="command.username" />
        </van-cell>
        <van-cell>
          <van-field label="密码" type="password" v-model="command.password" autocomplete />
        </van-cell>
      </van-cell-group>
      <div style="margin:16px">
        <van-button type="primary" size="large" block round native-type="submit">确定</van-button>
      </div>
    </van-form>
    <div style="margin:16px">
      <van-button type="success" size="large" block round @click="onRegist">注册</van-button>
    </div>
  </div>
</template>

<script>
import { PasswordLogin, PhoneNumberLogin2 } from '@/api/authService'
export default {
  data() {
    return {
      command: {},
    }
  },
  methods: {
    async Login() {
      console.log(1111)
      var response = await PasswordLogin({
        ...this.command,
        returnUrl: this.$route.query.ReturnUrl,
        // ticket: res.ticket,
        code: this.$route.query.code,
      })
    },
    onRegist() {
      console.log(this.$route)
      this.$router.push({
        name: 'regist-user',
        query: {
          ReturnUrl: this.$route.query.ReturnUrl ?? this.$route.fullpath,
        },
      })
    },
  },
}
</script>