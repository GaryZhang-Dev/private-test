<template>
  <a-row>
    <a-col :span="8" :offset="8">
      <div class="example">
        <a-spin size="large" />
      </div>
    </a-col>
  </a-row>
</template>

<script>
import { GetClient } from '@/api/authService'
export default {
  async created() {
    debugger
    var response = await GetClient({ ReturnUrl: this.$route.query.ReturnUrl })
    if (
      response.clientId === 'myprivate.test.client' ||
      response.clientId === 'myprivate.test.oa.client'
    ) {
      debugger
      this.$router.push({
        name: 'login-password',
        query: { ...this.$route.query },
      })
    }
  },
}
</script>