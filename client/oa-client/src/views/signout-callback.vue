<template></template>

<script>
import { mapActions } from 'vuex'

export default {
  data() {
    return {}
  },
  methods: {
    ...mapActions(['handleLogOut']),
  },
  async created() {
    debugger
    await this.handleLogOut()
    this.$router.replace({ name: 'home' })
  },
}
</script>

<style>
</style>