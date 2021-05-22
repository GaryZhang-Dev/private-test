<template>
  <div>
    <van-form @submit="TestAction">
      <van-cell-group>
        <!-- <van-cell>
          <van-field label="账号" v-model="command.username" />
        </van-cell>
        <van-cell>
          <van-field label="密码" type="password" v-model="command.password" autocomplete />
        </van-cell>-->
      </van-cell-group>
      <div style="margin:16px">
        <van-button type="primary" size="large" block round native-type="submit">测试</van-button>
      </div>
    </van-form>
  </div>
</template>

<script>
import { Test } from '@/api/accountService'
export default {
  data() {
    return {
      command: {},
    }
  },
  methods: {
    async TestAction() {
      console.log(1111)
      var response = await Test({
        returnUrl: this.$route.query.returnUrl,
        // ticket: res.ticket,
      })
      console.log(response)
    },
    // onRegist() {
    //   console.log(this.$route)
    //   this.$router.push({
    //     name: 'regist-user',
    //     query: {
    //       ReturnUrl: this.$route.query.ReturnUrl ?? this.$route.fullpath,
    //     },
    //   })
    // },
  },
}
</script>