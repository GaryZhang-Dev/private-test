<template></template>
<script>
import applicationUserManager from '../applicationusermanager'

export default {
  data() {
    return {}
  },
  async created() {
    debugger
    applicationUserManager.signoutRedirect()
  },
}
</script>
